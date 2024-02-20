using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PickUp : MonoBehaviour
{
    public enum PickUpType
    {
        Heal, Coin
    }
    public PickUpType type;
    public int value = 20;
    public ParticleSystem collectVFX;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            other.gameObject.GetComponent<Character>().pickUpItem(this);
            if (collectVFX != null)
            {
                Instantiate(collectVFX,transform.position, Quaternion.identity);
            }
            Destroy(this.gameObject);
        }
    }
}
