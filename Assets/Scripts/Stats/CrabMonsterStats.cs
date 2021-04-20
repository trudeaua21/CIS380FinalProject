using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Source Code based on https://github.com/Brackeys/RPG-Tutorial

public class CrabMonsterStats : CharacterStats
{
    public override void Die()
    {
        base.Die();

        //Add Death animation
        Destroy(gameObject);

        //Add item drops?
    }
}
