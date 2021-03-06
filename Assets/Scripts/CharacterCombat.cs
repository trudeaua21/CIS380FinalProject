using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Source Code based on https://github.com/Brackeys/RPG-Tutorial

[RequireComponent(typeof(CharacterStats))]
public class CharacterCombat : MonoBehaviour
{
    public float attackResetTime = 3.0f;
    public float attackSpeed = 1f;
    public float attackCooldown = 0f;
    public float attackDelay = 0.6f;

    public event System.Action OnAttack;

    CharacterStats myStats;

    private void Start()
    {
        myStats = GetComponent<CharacterStats>();
    }

    private void Update()
    {
        attackCooldown -= Time.deltaTime;
    }
    public void Attack(CharacterStats targetStats)
    {
        if(attackCooldown <= 0f)
        {
            StartCoroutine(DoDamage(targetStats, attackDelay));

            if(OnAttack != null)
            {
                OnAttack();
            }
            attackCooldown = attackResetTime;
        }
        //targetStats.TakeDamage(myStats.damage.GetValue());
    }

    public void TakeDamage(CharacterStats playerStats){
         Debug.Log (transform.name + " swings for " + myStats.damage.GetValue () + " damage");
         myStats.TakeDamage(playerStats.damage.GetValue());
     }

    IEnumerator DoDamage (CharacterStats stats, float delay)
    {
        yield return new WaitForSeconds(delay);
        stats.TakeDamage(myStats.damage.GetValue());
    }
}
