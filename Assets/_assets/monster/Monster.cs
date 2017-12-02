using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Monster : MonoBehaviour {


    public Vector3[] PatrolPoints;
    private bool patrol;
    private int currentPathPoint;
    private Vector3 _startingPosition;

    private NavMeshAgent _agent;


    public float RecognitionTime = 3;
    public float DistanceToAttack = 2;
    public float AttackCooldown = 2;
    public GameObject AttackZone;

    private float currentAttackCooldown = 0;
    private float currentRecognitionTime = 0;
    private GameObject _hero;
    private bool follow = false;

    void Start()
    {
        _startingPosition = transform.position;
        _agent = GetComponent<NavMeshAgent>();
        AttackZone.SetActive(false);
        patrol = false;
        currentPathPoint = -1;
        if (PatrolPoints.Length > 0)
        {
            patrol = true;
        }

        //_agent.destination = goal.position;
    }


    void Update()
    {
        Patrol();
        Recognition();
        FollowHero();
    }

    public void OnStartHearing(GameObject obj)
    {
        if (obj.GetComponent<Hero>())
        {
            _hero = obj;
        }
    }

    public void OnEndHearing(GameObject obj)
    {
        if (obj.GetComponent<Hero>() && currentRecognitionTime < RecognitionTime)
        {
            _hero = null;
        }
    }

    void OnDrawGizmosSelected()
    {
        foreach (var point in PatrolPoints)
        {
            Gizmos.color = Color.red;
            if (_startingPosition != Vector3.zero)
            {
                Gizmos.DrawWireSphere((_startingPosition + point), 0.2f);
            }
            else
            {
                Gizmos.DrawWireSphere((transform.position + point), 0.2f);
            }
        }
    }

    private void Patrol()
    {

        if (!_agent.pathPending && patrol)
        {
            if (_agent.remainingDistance <= _agent.stoppingDistance)
            {
                if (!_agent.hasPath || _agent.velocity.sqrMagnitude == 0f)
                {
                    currentPathPoint = (currentPathPoint + 1) % PatrolPoints.Length;
                    Debug.Log("new point " + (_startingPosition + PatrolPoints[currentPathPoint]));
                    _agent.destination = _startingPosition + PatrolPoints[currentPathPoint];
                }
            }
        }

        //if (patrol && !_agent.hasPath)
        //{
        //    currentPathPoint = (currentPathPoint + 1) % PatrolPoints.Length;
        //    _agent.destination = _startingPosition + PatrolPoints[currentPathPoint];
        //}
    }

    private void Recognition()
    {
        if (_hero)
        {
            currentRecognitionTime += Time.deltaTime;
            if (currentRecognitionTime >= RecognitionTime)
            {
                patrol = false;
                follow = true;
            }
        }
    }

    private void FollowHero()
    {
        if (follow)
        {
            if (currentAttackCooldown > 0)
            {
                currentAttackCooldown -= Time.deltaTime;
            }

            
            if (_agent.remainingDistance < DistanceToAttack && currentAttackCooldown <= 0)
            {
                _agent.destination = transform.position;
                currentAttackCooldown = AttackCooldown;
                // TODO : change attack -> start anim and use anim callback to call attack
                Invoke("Attack", 0.5f);
                //Attack();
            }
            else
            {
                _agent.destination = _hero.transform.position;
            }
        }
    }

    private void Attack()
    {
        AttackZone.SetActive(true);
    }
}