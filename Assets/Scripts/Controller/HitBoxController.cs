using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitBoxController : MonoBehaviour
{
    // Start is called before the first frame update

    public GameObject hitBox;
    CharacterStats myStats;
    CharacterStats targetStats;
    CharacterCombat combat;
    Transform target;

    void Start(){
        myStats = GetComponent<CharacterStats>();
        target = PlayerManager.instance.Player.transform;
    }

    private void OnTriggerEnter(Collider other){
        CharacterStats targetStats = target.GetComponent<CharacterStats>();
        if(targetStats != null && combat.attackCooldown <= 0f)
        {
            combat.Attack(targetStats);
            Debug.Log("I took damage");
        }
    }
    
    // Update is called once per frame
    void Update()
    {
        
    }
}
