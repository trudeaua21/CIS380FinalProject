using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Source Code based on https://github.com/Brackeys/RPG-Tutorial

public class CrabMonsterStats : MonoBehaviour
{
    // Health
	public int maxHealth = 100;
	public int currentHealth { get; private set; }

	public Stat damage;
	public Stat armor;

	// Set current health to max health
	// when starting the game.
	void Awake()
	{
		currentHealth = maxHealth;
	}

	// Damage the character
	public void TakeDamage(int damage)
	{
		// Subtract the armor value
		damage -= armor.GetValue();
		damage = Mathf.Clamp(damage, 0, int.MaxValue);

		// Damage the character
		currentHealth -= damage;
		Debug.Log(transform.name + " takes " + damage + " damage.");

		// If health reaches zero
		if (currentHealth <= 0)
		{
			Die();
		}
	}

	public virtual void Die()
	{
		Debug.Log(transform.name + " died.");
	}
}
