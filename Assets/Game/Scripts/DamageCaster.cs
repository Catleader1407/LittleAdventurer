using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageCaster : MonoBehaviour
{
    private Collider damageCasterCollider;
    public int damage = 30;
    public string TargetTag;
    private List<Collider> damagedTargetList;
    private void Awake()
    {
        damageCasterCollider = GetComponent<Collider>();
        damageCasterCollider.enabled = false;
        damagedTargetList = new List<Collider>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == TargetTag && !damagedTargetList.Contains(other))
        {
            Character targetCC = other.GetComponent<Character>();
            if (targetCC != null)
            {
                
                targetCC.ApplyDamage(damage, transform.parent.position);
                if (!targetCC.isPlayer)
                {
                    Character playerCharacter = this.transform.parent.GetComponent<Character>();
                    string currentAnimation = playerCharacter.currentAnimation();
                    switch (currentAnimation)
                    {
                        case "LittleAdventurerAndie_ATTACK_01":
                            AudioManager.instance.PlaySound(AudioManager.instance.combo01Impact, 0.5f);
                            break;
                        case "LittleAdventurerAndie_ATTACK_02":
                            AudioManager.instance.PlaySound(AudioManager.instance.combo02Impact, 0.5f);
                            break;
                        case "LittleAdventurerAndie_ATTACK_03":
                            AudioManager.instance.PlaySound(AudioManager.instance.combo03Impact, 0.5f);
                            break;
                    }
                }
                PlayerVFXManager playerVFXManager = transform.parent.GetComponent<PlayerVFXManager>();
                if (playerVFXManager != null)
                {
                    RaycastHit hit;
                    Vector3 orignalPos = transform.position + (-damageCasterCollider.bounds.extents.z) * transform.forward;
                    bool isHit = Physics.BoxCast(orignalPos, damageCasterCollider.bounds.extents / 2, transform.forward, out hit, transform.rotation, damageCasterCollider.bounds.extents.z, 1 << 6); ;
                    if (isHit)
                    {
                        Debug.Log("is Hit");                        
                        playerVFXManager.playSlash(hit.point + new Vector3(0, 0.5f, 0));
                        
                        
                    }
                }

            }
            damagedTargetList.Add(other);
        }
    }
    public void enableDamageCaster()
    {
        damagedTargetList.Clear();
        damageCasterCollider.enabled = true;
    }
    public void disableDamageCaster()
    {
        damagedTargetList.Clear();
        damageCasterCollider.enabled = false;
    }
/*    private void OnDrawGizmos()
    {
        if (damageCasterCollider == null)
        {
            damageCasterCollider = GetComponent<Collider>();
        }
        RaycastHit hit;
        Vector3 orignalPos = transform.position + (-damageCasterCollider.bounds.extents.z) * transform.forward;
        bool isHit = Physics.BoxCast(orignalPos, damageCasterCollider.bounds.extents / 2, transform.forward, out hit, transform.rotation, damageCasterCollider.bounds.extents.z, 1 << 6); ;

        if (isHit)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(hit.point, 0.3f);
        }
    }*/
}
