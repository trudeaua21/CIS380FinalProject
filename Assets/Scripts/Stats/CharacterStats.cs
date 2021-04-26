using UnityEngine;
using UnityEngine.UI;
/* Base class that player and enemies can derive from to include stats. */
// Source Code based on https://github.com/Brackeys/RPG-Tutorial

public class CharacterStats : MonoBehaviour
{

	// Health
	public float maxHealth = 100;
	public float currentHealth { get; private set; }

	public Stat damage;
	public Stat armor;
	public Text healthBar;
	Animator animator;

	// Set current health to max health
	// when starting the game.
	void Awake()
	{
		currentHealth = maxHealth;
		animator = GetComponent<Animator>();
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

		animator.SetFloat("health", currentHealth);
		// If health reaches zero
		if (currentHealth <= 0)
		{
			Die();
		}
	}
	void Update()
	{
		if (healthBar != null) { 
		healthBar.text = "Health= " + currentHealth + "/" + maxHealth;
		}
    }
    public virtual void Die()
	{
		// Die in some way
		// This method is meant to be overwritten
		Debug.Log(transform.name + " died.");

	}

}
