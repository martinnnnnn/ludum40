﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public enum MonsterState
{
    IDLE,
    PATROL,
    RECOGNITION,
    HEARING,
    FOLLOW
}

public class Monster : MonoBehaviour {


    public Vector3[] PatrolPoints;

    private int currentPathPoint;
    private Vector3 _startingPosition;

    private NavMeshAgent _agent;


    public float RecognitionTime = 3;
    public float DistanceToAttack = 2;
    public float AttackCooldown = 2;
    public GameObject AttackZone;

    public float HearDuration = 5f;

    private float currentAttackCooldown = 0;
    private float currentRecognitionTime = 0;
    private Hero _hero;
    private SoundEmiter _emiter;

    MonsterState State;

    void OnEnable()
    {
        _startingPosition = transform.position;
        _agent = GetComponent<NavMeshAgent>();
        AttackZone.SetActive(false);
        currentPathPoint = -1;

        State = MonsterState.IDLE;
        if (PatrolPoints.Length > 0)
        {
            State = MonsterState.PATROL;
        }

        AttackZone.GetComponent<AttackZone>().OnAttackEnd += StartFollowAfterAttack;
        GetComponent<MeshRenderer>().material.color = Color.blue;
        _hero = FindObjectOfType<LevelHandler>().Hero;
        if (_hero) _emiter = _hero.GetComponentInChildren<SoundEmiter>();
    }

    private void Start()
    {
        _startingPosition = transform.position;
        _agent = GetComponent<NavMeshAgent>();
        AttackZone.SetActive(false);
        currentPathPoint = -1;

        State = MonsterState.IDLE;
        if (PatrolPoints.Length > 0)
        {
            State = MonsterState.PATROL;
        }

        AttackZone.GetComponent<AttackZone>().OnAttackEnd += StartFollowAfterAttack;
        GetComponent<MeshRenderer>().material.color = Color.blue;
        _hero = FindObjectOfType<LevelHandler>().Hero;
        _emiter = _hero.GetComponentInChildren<SoundEmiter>();
    }

    public void HearObject(Throwable obj)
    {
        if (Vector3.Distance(obj.transform.position, transform.position) < obj.SoundRadius)
        {
            State = MonsterState.HEARING;
            _agent.destination = obj.transform.position;
        }
    }

    void Update()
    {
        ListenHero();
        switch (State)
        {
            case MonsterState.IDLE:
                break;
            case MonsterState.PATROL:
                Patrol();
                break;
            case MonsterState.RECOGNITION:
                Recognize();
                break;
            case MonsterState.HEARING:
                Hear();
                break;
            case MonsterState.FOLLOW:
                FollowHero();
                break;
        }
    }

    public void ListenHero()
    {
        if (State == MonsterState.FOLLOW || State == MonsterState.HEARING)
            return;
        

        if (Vector3.Distance(_emiter.transform.position,transform.position) < _emiter.SoundRadius && _emiter.IsEmitingSound)
        {
            State = MonsterState.RECOGNITION;
            GetComponent<MeshRenderer>().material.color = Color.yellow;
            _agent.isStopped = true;
        }
        else
        {
            if (currentRecognitionTime < RecognitionTime)
            {
                State = MonsterState.PATROL;
                GetComponent<MeshRenderer>().material.color = Color.blue;
                _agent.isStopped = false;
                currentRecognitionTime = 0;
            }
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

    private void Recognize()
    {
        currentRecognitionTime += Time.deltaTime;
        if (currentRecognitionTime >= RecognitionTime)
        {
            State = MonsterState.FOLLOW;
            _agent.isStopped = false;
            GetComponent<MeshRenderer>().material.color = Color.red;
        }
    }

    private void Hear()
    {
        if (!_agent.pathPending)
        {
            if (_agent.remainingDistance <= _agent.stoppingDistance)
            {
                if (!_agent.hasPath || _agent.velocity.sqrMagnitude == 0f)
                {
                    _agent.isStopped = true;
                    Invoke("BackToPatrol", HearDuration);
                }
            }
        }
    }

    private void BackToPatrol()
    {
        _agent.isStopped = false;
        if (PatrolPoints.Length == 0)
        {
            State = MonsterState.IDLE;
            _agent.destination = _startingPosition;
        }
        else
        {
            State = MonsterState.PATROL;
        }
    }

    private void FollowHero()
    {
        _agent.destination = _hero.transform.position;

        if (currentAttackCooldown > 0)
        {
            currentAttackCooldown -= Time.deltaTime;
        }

        if (_agent.remainingDistance < DistanceToAttack && currentAttackCooldown <= 0)
        {
            _agent.isStopped = true;
            currentAttackCooldown = AttackCooldown;
            Invoke("Attack", 0.5f);
            // TODO : change attack -> start anim and use anim callback to call attack
        }
    }

    private void Attack()
    {
        transform.LookAt(_hero.transform.position);
        AttackZone.SetActive(true);
    }

    private void StartFollowAfterAttack()
    {
        _agent.isStopped = false;
    }
}