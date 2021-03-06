using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitBoxController : MonoBehaviour
{
    // Start is called before the first frame update

    public GameObject hitBox;
    CharacterCombat combat;
    Transform target;

    void Start(){
        combat = GetComponent<CharacterCombat>();
        target = PlayerManager.instance.Player.transform;
    }

    private void OnTriggerEnter(Collider other){
        if(hitBox.activeSelf){
            CharacterStats targetStats = target.GetComponent<CharacterStats>();
            combat.Attack(targetStats);
        }
        
    }
    
    // Update is called once per frame
    void Update()
    {
        
    }
}
