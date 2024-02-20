using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy02Shoot : MonoBehaviour
{
    public Transform shootingPoint;
    public GameObject damageOrb;
    private Character characterController;

    private void Awake()
    {
        characterController = GetComponent<Character>();
    }
    private void Update()
    {
        characterController.RotateToTarget();
    }
    public void shootTheDamageOrb()
    {
        Instantiate(damageOrb, shootingPoint.position, Quaternion.LookRotation(shootingPoint.forward));
    }
    
}
