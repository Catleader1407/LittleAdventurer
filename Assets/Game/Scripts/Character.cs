using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UIElements;


public class Character : MonoBehaviour
{
    private CharacterController characterController;
    public float moveSpeed = 5f;
    private Vector3 movementVelocity;
    private PlayerInput playerInput;
    private float verticalVelocity;
    public float gravity = -9.8f;

    private Animator animator;

    public bool isPlayer = true;

    private UnityEngine.AI.NavMeshAgent navMeshAgent;
    private Transform TargetPlayer;
    //Health
    private Health health;
    private DamageCaster damageCaster;
    //Player Slides
    private float attackStartTime;
    public float attackSlideDuration = 0.4f;
    public float attackSlideSpeed = 0.06f;

    private Vector3 impactOnCharacter;

    public bool isInvincible;
    public float invincibleDuration = 0f;

    private float attackAnimationDuration;
    public float slideSpeed = 9f;
    public int coin;

    private float currentSpawnTime;
    public float spawnDuration = 2f;
    //State Machine
    public enum CharacterState
    {
        Normal, Attacking, Dead, BeingHit, Slide, Spawn
    }

    public CharacterState currentState;
    private MaterialPropertyBlock materialPropertyBlock;
    private SkinnedMeshRenderer skinnedMeshRenderer;
    public GameObject itemToDrop;
    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        health = GetComponent<Health>();
        damageCaster = GetComponentInChildren<DamageCaster>();
        
        skinnedMeshRenderer = GetComponentInChildren<SkinnedMeshRenderer>();
        materialPropertyBlock = new MaterialPropertyBlock();
        skinnedMeshRenderer.GetPropertyBlock(materialPropertyBlock);

        if (!isPlayer)
        {
            navMeshAgent = GetComponent<UnityEngine.AI.NavMeshAgent>();
            TargetPlayer = GameObject.FindWithTag("Player").transform;
            navMeshAgent.speed= moveSpeed;
            SwitchStateTo(CharacterState.Spawn);
        }
        else
        {
            playerInput = GetComponent<PlayerInput>();
        }
    }
    private  void CalculateEnemyMovement()
    {
        if (Vector3.Distance(TargetPlayer.position,transform.position) >= navMeshAgent.stoppingDistance)
        {
            navMeshAgent.SetDestination(TargetPlayer.position);
            animator.SetFloat("Speed", 0.2f);
        }
        else
        {
            navMeshAgent.SetDestination(transform.position);
            animator.SetFloat("Speed", 0f);
            SwitchStateTo(CharacterState.Attacking);
        }
    }
    private void CalculatePlayerMovement()
    {
        if (playerInput.MouseButtonDown && characterController.isGrounded)
        {
            SwitchStateTo(CharacterState.Attacking);
            return;
        }
        else if (playerInput.spaceKeyDown && characterController.isGrounded)
        {
            SwitchStateTo(CharacterState.Slide);
            return;
        }
        movementVelocity.Set(playerInput.HorizontalInput,0f,playerInput.VerticalInput);
        movementVelocity.Normalize();
        movementVelocity = Quaternion.Euler(0, -45f, 0) * movementVelocity;

        animator.SetFloat("Speed", movementVelocity.magnitude);
        movementVelocity *= moveSpeed * Time.deltaTime;
        if (movementVelocity != Vector3.zero)
        {
            transform.rotation = Quaternion.LookRotation(movementVelocity);
        }
        animator.SetBool("AirBorne", !characterController.isGrounded);
    }
    private void FixedUpdate()
    {
        switch (currentState)
        {
            case CharacterState.Normal:
                if (isPlayer)
                {
                    CalculatePlayerMovement();
                }
                else
                {
                    CalculateEnemyMovement();
                }
                break;
            case CharacterState.Attacking:
                if (isPlayer)
                {                   
                    if (Time.time < attackStartTime + attackSlideDuration)
                    {
                        float timePassed = Time.time - attackStartTime;
                        float lerpTime = timePassed / attackSlideDuration;
                        movementVelocity = Vector3.Lerp(transform.forward * attackSlideSpeed, Vector3.zero, lerpTime);
                    }
                    if (playerInput.MouseButtonDown && characterController.isGrounded)
                    {
                        
                        string currentClipName = animator.GetCurrentAnimatorClipInfo(0)[0].clip.name;
                        attackAnimationDuration = animator.GetCurrentAnimatorStateInfo(0).normalizedTime;
                        
                        if (currentClipName != "LittleAdventurerAndie_ATTACK_03" && attackAnimationDuration > 0.5f && attackAnimationDuration < 0.7f)
                        {
                            playerInput.MouseButtonDown = false;
                            SwitchStateTo(CharacterState.Attacking);
                            CalculatePlayerMovement();
                        }
                    }
                }
                break;
            case CharacterState.Dead:
                return;
            case CharacterState.BeingHit:
                break;
            case CharacterState.Slide:
                movementVelocity = transform.forward * slideSpeed * Time.deltaTime;
                break;
            case CharacterState.Spawn:
                currentSpawnTime -= Time.deltaTime;
                if (currentSpawnTime < 0)
                {
                    SwitchStateTo(CharacterState.Normal);
                }
                break;
        }
        if (impactOnCharacter.magnitude > 0.2f)
        {
            movementVelocity = impactOnCharacter * Time.deltaTime;
        }
        impactOnCharacter = Vector3.Lerp(impactOnCharacter, Vector3.zero, Time.deltaTime * 5);

        if (isPlayer) 
        {
            if (!characterController.isGrounded)
            {
                verticalVelocity = gravity;
            }
            else
            {
                verticalVelocity = gravity * 0.3f;
            }
            movementVelocity += verticalVelocity * Vector3.up * Time.deltaTime;
            characterController.Move(movementVelocity);
            movementVelocity = Vector3.zero;

        } else
        {
            if (currentState != CharacterState.Normal)
            {
                characterController.Move(movementVelocity);
                movementVelocity = Vector3.zero;
            }
        }
    }

    public void SwitchStateTo(CharacterState newState)
    {

        //Clear cache
        if (isPlayer)
        {
            playerInput.clearCache();
        }
      

        //Exiting state
        switch (currentState)
        {
            case CharacterState.Normal:
                break;
            case CharacterState.Attacking:
                if (damageCaster != null)
                {
                    disableDamageCaster();
                }
                if (isPlayer)
                {
                    GetComponent<PlayerVFXManager>().stopBlade();
                }
                break;
            case CharacterState.Dead:
                return;
            case CharacterState.BeingHit:                
                break;
            case CharacterState.Slide:
                break;
            case CharacterState.Spawn:
                isInvincible = false;
                break;
        }
        //Entering State
        switch (newState)
        {
            case CharacterState.Normal:
                break;
            case CharacterState.Attacking:
                //Rotate to attack player after finishing attacking
                 if (!isPlayer)
                {
                    RotateToTarget();
                    /*Quaternion newRotation = Quaternion.LookRotation(TargetPlayer.position - transform.position);
                    transform.rotation = newRotation;*/
                }
                animator.SetTrigger("Attack");
                if (isPlayer)
                {
                    attackStartTime = Time.time;
                }
                break;
            case CharacterState.Dead:
                characterController.enabled = false;
                animator.SetTrigger("Dead");                
                StartCoroutine(materialDissolve());
                if (!isPlayer)
                {
                    SkinnedMeshRenderer mesh = GetComponentInChildren<SkinnedMeshRenderer>();
                    mesh.gameObject.layer = 0;
                }
                break;
            case CharacterState.BeingHit:
                animator.SetTrigger("BeingHit");
                if (isPlayer)
                {
                    isInvincible = true;
                    StartCoroutine(DelayCancelInvincible());
                }
                break;
            case CharacterState.Slide:
                animator.SetTrigger("Slide");
                break;
            case CharacterState.Spawn:
                isInvincible = true;
                currentSpawnTime = spawnDuration;
                StartCoroutine(materialAppear());
                break;
        }

        Debug.Log(currentState + " switch to " + newState);
        currentState = newState;

    }
    public void attackAnimationEnds()
    {
        SwitchStateTo(CharacterState.Normal);
    }
    public void beingHitAnimationEnds()
    {
        SwitchStateTo(CharacterState.Normal);
    }
    public void slideAnimationEnds()
    {
        SwitchStateTo(CharacterState.Normal);
    }
    public void ApplyDamage(int damage, Vector3 attackerPos = new Vector3())
    {
        if (isInvincible)
        {
            return;
        }
        if (health != null)
        {
            health.ApplyDamage(damage);
        }
        if (!isPlayer)
        {
            GetComponent<EnemyVFXManager>().playBeingHitVFX(attackerPos);
        }
        StartCoroutine(materialBlink());
        if (isPlayer)
        {
            SwitchStateTo(CharacterState.BeingHit);
            addImpact(attackerPos, 10f);
        } else
        {
            addImpact(attackerPos, 3f);
        }
    }
    IEnumerator DelayCancelInvincible()
    {
        yield return new WaitForSeconds(2);
        isInvincible = false;

    }
    private void addImpact(Vector3 attackerPos, float force)
    {
        Vector3 impactDir = transform.position - attackerPos;
        impactDir.Normalize();
        impactDir.y = 0;
        impactOnCharacter = impactDir * force;
    }
    public void enableDamageCaster()
    {
        damageCaster.enableDamageCaster();
    }
    public void disableDamageCaster()
    {
        damageCaster.disableDamageCaster();
    }
    IEnumerator materialBlink()
    {
        materialPropertyBlock.SetFloat("_blink", 0.4f);
        skinnedMeshRenderer.SetPropertyBlock(materialPropertyBlock);
        yield return new WaitForSeconds(0.2f);

        materialPropertyBlock.SetFloat("_blink", 0f);
        skinnedMeshRenderer.SetPropertyBlock(materialPropertyBlock);
    }
    IEnumerator materialDissolve()
    {        
        yield return new WaitForSeconds(2);
        AudioManager.instance.PlaySound(AudioManager.instance.dead, 0.5f);
        float dissolveTimeDuration = 2f;
        float currentDissolveTime = 0;
        float dissolveHeight_start = 20f;
        float dissolveHeight_target = -10f;
        float dissolveHeight;
        materialPropertyBlock.SetFloat("_enableDissolve", 1f);
        skinnedMeshRenderer.SetPropertyBlock(materialPropertyBlock); 
        
        while (currentDissolveTime < dissolveTimeDuration)
        {
            currentDissolveTime += Time.deltaTime;
            dissolveHeight = Mathf.Lerp(dissolveHeight_start, dissolveHeight_target, currentDissolveTime / dissolveTimeDuration);
            materialPropertyBlock.SetFloat("_dissolve_height", dissolveHeight);
            skinnedMeshRenderer.SetPropertyBlock(materialPropertyBlock);
            yield return null;
        }
        dropItem();
    }
    IEnumerator materialAppear()
    {
        AudioManager.instance.PlaySound(AudioManager.instance.appear,0.5f);
        float dissolveTimeDuration = spawnDuration;
        float currentDissolveTime = 0;
        float dissolveHeight_start = -10f;
        float dissolveHeight_target = 20f;
        float dissolveHeight;
        materialPropertyBlock.SetFloat("_enableDissolve", 1f);
        skinnedMeshRenderer.SetPropertyBlock(materialPropertyBlock);

        while (currentDissolveTime < dissolveTimeDuration)
        {
            currentDissolveTime += Time.deltaTime;
            dissolveHeight = Mathf.Lerp(dissolveHeight_start, dissolveHeight_target, currentDissolveTime / dissolveTimeDuration);
            materialPropertyBlock.SetFloat("_dissolve_height", dissolveHeight);
            skinnedMeshRenderer.SetPropertyBlock(materialPropertyBlock);
            yield return null;
        }
        materialPropertyBlock.SetFloat("_enableDissolve", 0f);
        skinnedMeshRenderer.SetPropertyBlock(materialPropertyBlock);
    }
    public void dropItem()
    {
        if (itemToDrop != null)
        {
            Instantiate(itemToDrop, transform.position, Quaternion.identity);
        }
    }
    public void pickUpItem(PickUp item)
    {
        switch(item.type)
        {
            case PickUp.PickUpType.Heal:
                AudioManager.instance.PlaySound(AudioManager.instance.pickUpHeal, 0.5f);
                addHealth(item.value);
                break;
            case PickUp.PickUpType.Coin:
                AudioManager.instance.PlaySound(AudioManager.instance.pickUpCoin, 0.5f);
                addCoin(item.value);
                break;
        }
    }
    public void addHealth(int value)
    {
        health.addHealth(value);
        GetComponent<PlayerVFXManager>().playHeal();
    }
    public void addCoin(int value)
    {
        coin += value;
    }
    public void RotateToTarget()
    {
        if (currentState != CharacterState.Dead)
        {
            transform.LookAt(TargetPlayer, Vector3.up);
        }
    }
    public void playSoundNPC01()
    {
        AudioManager.instance.playSoundNPC01();
    }
    public void playSoundNPC02()
    {
        AudioManager.instance.playSoundNPC02();
    }
    public string currentAnimation()
    {
        return animator.GetCurrentAnimatorClipInfo(0)[0].clip.name;
    }
}
