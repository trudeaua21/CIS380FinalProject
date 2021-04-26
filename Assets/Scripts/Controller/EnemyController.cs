using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

// Source Code based on https://github.com/Brackeys/RPG-Tutorial

public class EnemyController : MonoBehaviour
{
    private const float BASE_SPEED = 3;

    private float iFrames;
    private float CurrentSpeed;
    private float lastTime;

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
        AnimatorUpdater();
        iFrames -= Time.deltaTime;
        SetCurrentSpeed(lastPosition, transform.position);
        animator.SetFloat("speed", CurrentSpeed);

        if((!isTakingDamage || !isAttacking || !isDying) && !isDead){
                
                float distance = Vector3.Distance(target.position, transform.position);

                if(distance <= visionRadius)
                {
                    agent.SetDestination(target.position);
                    UnityEngine.AI.NavMeshHit hit;
                    if(!agent.Raycast(target.position, out hit)){
                        if(distance <= agent.stoppingDistance)
                        {
                            Attack_1();
                            FaceTarget();
                        } 
                        else if(combat.attackCooldown <= 2)
                        {
                            agent.speed = BASE_SPEED;
                        }
                    }
                }
            
        }
    }

    private void OnTriggerEnter(Collider other){
        if(iFrames < 0){
            if (other.gameObject.CompareTag("Sword"))
            {
                Debug.Log("Sword do be hitting");
                combat.TakeDamage(target.GetComponent<CharacterStats>());
                iFrames = 1.5f;
                if(stats.currentHealth > 0){
                    animator.Play("Base Layer.Armature|TakeDamage", 0, .25f);
                }
            }
            agent.speed = 0f;
        }
         else {
            Debug.Log("Iframes worked");
        }
        Debug.Log("Iframes: " + iFrames);
    }

    void Attack_1()
    {
        CharacterStats targetStats = target.GetComponent<CharacterStats>();
        if(targetStats != null && combat.attackCooldown <= 0f)
        {
            combat.Attack(targetStats);
            animator.Play("Base Layer.Armature|Attack_1", 0, .25f);
            agent.speed = 1f;
        }
    }

    void FaceTarget()
    {
        Vector3 direction = (target.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, direction.y, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
    }

    private void AnimatorUpdater(){
        if(animator.GetCurrentAnimatorStateInfo(0).IsName("Base Layer.Armature|Attack_1")){
            isAttacking = true;
        } else {
            isAttacking = false;
        }

        if(animator.GetCurrentAnimatorStateInfo(0).IsName("Base Layer.Armature|TakeDamage")){
            isTakingDamage = true;
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
        } else {
            isDying = false;
        }

        if(animator.GetCurrentAnimatorStateInfo(0).IsName("Base Layer.Armature|Die")){
            isDying = true;
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
