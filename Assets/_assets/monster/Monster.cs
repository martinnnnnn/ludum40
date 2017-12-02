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

    private bool isRecognized = false;


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

        AttackZone.GetComponent<AttackZone>().OnAttackEnd += StartFollowAfterAttack;
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
        if (isRecognized)
            return;

        if (obj.GetComponent<Hero>())
        {
            _hero = obj;
            _agent.isStopped = true;
        }
    }

    public void OnEndHearing(GameObject obj)
    {
        if (isRecognized)
            return;
        if (obj.GetComponent<Hero>() && currentRecognitionTime < RecognitionTime)
        {
            _agent.isStopped = false;
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
        if (!patrol)
            return;

        if (!_agent.pathPending)
        {
            if (_agent.remainingDistance <= _agent.stoppingDistance)
            {
                if (!_agent.hasPath || _agent.velocity.sqrMagnitude == 0f)
                {
                    currentPathPoint = (currentPathPoint + 1) % PatrolPoints.Length;
                    _agent.destination = _startingPosition + PatrolPoints[currentPathPoint];
                }
            }
        }
    }

    private void Recognition()
    {
        if (isRecognized)
            return;

        if (!_hero)
            return;

        currentRecognitionTime += Time.deltaTime;
        if (currentRecognitionTime >= RecognitionTime)
        {
            isRecognized = true;
            patrol = false;
            follow = true;
            _agent.isStopped = false;
        }
    }

    private void FollowHero()
    {
        if (follow)
        {
            Debug.Log("followplayer" + _agent.isStopped);
            _agent.destination = _hero.transform.position;

            if (currentAttackCooldown > 0)
            {
                currentAttackCooldown -= Time.deltaTime;
            }

            if (_agent.remainingDistance < DistanceToAttack && currentAttackCooldown <= 0)
            {
                Debug.Log("stop to attack");
                _agent.isStopped = true;
                currentAttackCooldown = AttackCooldown;
                Invoke("Attack", 0.5f);
                // TODO : change attack -> start anim and use anim callback to call attack
            }
        }
    }

    private void Attack()
    {
        transform.LookAt(_hero.transform.position);
        Debug.Log("attack");
        AttackZone.SetActive(true);
    }

    private void StartFollowAfterAttack()
    {
        _agent.isStopped = false;
    }
}