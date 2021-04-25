using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

// Source Code based on https://github.com/Brackeys/RPG-Tutorial

public class EnemyController : MonoBehaviour
{
    private const float BASE_SPEED = 3;

    private float CurrentSpeed;

    private float lastTime;

    private Vector3 lastPosition;
    private Animator animator;
    //Controls how far the enemy can see
    public float visionRadius = 10f;

    Transform target;
    NavMeshAgent agent;
    CharacterCombat combat;

    Rigidbody body;
    // Start is called before the first frame update
    void Start()
    {
        target = PlayerManager.instance.Player.transform;
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        combat = GetComponent<CharacterCombat>();
        body = GetComponent<Rigidbody>();
        lastPosition = transform.position;
        lastTime = Time.deltaTime;
    }

    // Update is called once per frame
    void Update()
    {
        SetCurrentSpeed(lastPosition, transform.position);
        animator.SetFloat("speed", CurrentSpeed);
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

    private void OnTriggerEnter(Collider other){
        Debug.Log("Sword do be hitting");
        if (other.gameObject.CompareTag("Sword"))
        {
            combat.TakeDamage(target.GetComponent<CharacterStats>());
        }
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
