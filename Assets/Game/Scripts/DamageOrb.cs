using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageOrb : MonoBehaviour
{
    public float speed = 2f;
    public int damage = 10;
    public ParticleSystem hitVFX;
    private Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        rb.MovePosition(transform.position + transform.forward * speed * Time.deltaTime);
    }
    private void OnTriggerEnter(Collider other)
    {
        Character characterController = other.GetComponent<Character>();
        if (characterController != null && characterController.isPlayer)
        {
            AudioManager.instance.PlaySound(AudioManager.instance.playerHitRange, 0.5f);
            characterController.ApplyDamage(damage,transform.position);
        }
        Instantiate(hitVFX,transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
