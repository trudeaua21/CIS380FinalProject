using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

// Source Code based on https://github.com/Brackeys/RPG-Tutorial

public class EnemyController : MonoBehaviour
{
    private const float BASE_SPEED = 3f;
    private const float ATTACK_MOVE_SPEED = 1f;
    private float SwingTime = 1f;

    private float iFrames;
    private float CurrentSpeed;
    private float lastTime;
    private float AttackLength;

    private bool isAttacking;
    private bool isWalking;
    private bool isDying;
    private bool isResting;
    private bool isTakingDamage;
    private bool isDead;

    private Vector3 lastPosition;
    private Animator animator;
    //Controls how far the enemy can see
    public float visionRadius = 10f;

    public GameObject hitBox;
    Transform target;
    NavMeshAgent agent;
    CharacterCombat combat;

    CharacterStats stats;

    Rigidbody body;
    // Start is called before the first frame update
    void Start()
    {
        target = PlayerManager.instance.Player.transform;
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        combat = GetComponent<CharacterCombat>();
        body = GetComponent<Rigidbody>();
        stats = GetComponent<CharacterStats>();
        lastPosition = transform.position;
        lastTime = Time.deltaTime;
        iFrames = 3f;
        isDead = false;
    }

    // Update is called once per frame
    void Update()
    {
        //Update All animation statuses
        AnimatorUpdater();

        //Decrease Iframes by the amount of time passed
        if(iFrames > -1){
            iFrames -= Time.deltaTime;
        }

        //Decrease the swing time by the amount of time passed
        if(SwingTime > -1){
            iFrames -= Time.deltaTime;
        }

        //
        if(SwingTime > 0 && SwingTime < 0.5f){
            hitBox.SetActive(true);
        } else if (SwingTime < 0f){
            hitBox.SetActive(false);
        }

        //Speed Calculation
        SetCurrentSpeed(lastPosition, transform.position);
        //Set speed in animator to control if the walking animation
        animator.SetFloat("speed", CurrentSpeed);

        if((!isTakingDamage || !isDying) && !isDead){
            if(isAttacking){
                agent.speed = ATTACK_MOVE_SPEED;
            } else {
                agent.speed = BASE_SPEED;
            }
                
            //Distance to the player
            float distance = Vector3.Distance(target.position, transform.position);

            //Determine if player is in vision radius
            if(distance <= visionRadius)
            {
                //Move to player
                agent.SetDestination(target.position);
                
                //If the player is in stopping distance face it and attack
                if(distance <= agent.stoppingDistance & !isAttacking)
                {
                    FaceTarget();
                    hitBox.SetActive(true);
                    Attack_1();
                } 
                
            }
            
        }
    }

    private void OnTriggerEnter(Collider other){
            if (other.gameObject.CompareTag("Sword") && !isTakingDamage)
            {
                if(!isDead && iFrames < 0)
                {
                    combat.TakeDamage(target.GetComponent<CharacterStats>());
                    animator.Play("Base Layer.Armature|TakeDamage", 0, .25f);
                }
                iFrames = 2.0f;
            }  
    }

    void Attack_1()
    {
        CharacterStats targetStats = target.GetComponent<CharacterStats>();
        if(targetStats != null && combat.attackCooldown <= 0f)
        {
            animator.Play("Base Layer.Armature|Attack_1", 0, .25f);
            agent.speed = 1f;
        }
    }

    void FaceTarget()
    {
        Vector3 direction = (target.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, direction.y, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 10f);
    }

    private void AnimatorUpdater(){

        if(animator.GetCurrentAnimatorStateInfo(0).IsName("Base Layer.Armature|Attack_1")){
            isAttacking = true;
        } else {
            isAttacking = false;
        }

        if(animator.GetCurrentAnimatorStateInfo(0).IsName("Base Layer.Armature|TakeDamage")){
            isTakingDamage = true;
            agent.speed = 0f;
        } else {
            isTakingDamage = false;
        }

        if(animator.GetCurrentAnimatorStateInfo(0).IsName("Base Layer.Armature|Walk_Cycle_1")){
            isWalking = true;
        } else {
            isWalking = false;
        }

        if(animator.GetCurrentAnimatorStateInfo(0).IsName("Base Layer.Armature|Die")){
            isDying = true;
            isDead = true;
            agent.speed = 0f;
        } else {
            isDying = false;
        }

        if(animator.GetCurrentAnimatorStateInfo(0).IsName("Base Layer.Armature|Rest_1")){
            isResting = true;
        } else {
            isResting = false;
        }

    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, visionRadius);
    }

    private void SetCurrentSpeed(Vector3 last, Vector3 current){
        double distance = Math.Sqrt(Math.Pow((current.x - last.x), 2) + Math.Pow((current.z - last.z) , 2));
        CurrentSpeed = (float)distance / lastTime;
        lastTime = Time.deltaTime;
        lastPosition = transform.position;
    }
}
