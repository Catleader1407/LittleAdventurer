using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    public int maxHealth;
    public int currentHealth;
    public float currentHealthPercentage
    {
        get
        {
            return (float)currentHealth/(float)maxHealth;
        }
    }
    private Character character;
    
    private void Awake()
    {
        currentHealth = maxHealth;
        character = GetComponent<Character>();
    }
    public void ApplyDamage(int damage)
    {
        currentHealth -= damage; 
        Debug.Log(gameObject.name + " took damage " + damage);
        Debug.Log(gameObject.name + " currentHealth: " + currentHealth);
        checkHealth();
    }
    public void checkHealth()
    {
        if (currentHealth <= 0)
        {
            character.SwitchStateTo(Character.CharacterState.Dead);
        }
    }
    public void addHealth(int value)
    {
        currentHealth += value;
        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }
    }
}
