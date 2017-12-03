using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackZone : MonoBehaviour
{

    public int Damage = 1;
    public float AttackDuration = 1f;
    private float currentAttackDuration;

    public bool IsHero;

    public System.Action OnAttackEnd;

    void OnEnable()
    {
        currentAttackDuration = 0;
    }

    void Update()
    {
        currentAttackDuration += Time.deltaTime;
        if (currentAttackDuration >= AttackDuration)
        {
            AttackEnd();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!IsHero)
        {
            Hero hero = other.GetComponent<Hero>();
            if (hero)
            {
                AttackEnd();
                hero.ReceiveDamage(Damage);
            }
        }
        else
        {
            Monster monster = other.GetComponent<Monster>();
            if (monster)
            {
                AttackEnd();
                monster.ReceiveDamage(Damage);
            }
        }
    }

    private void AttackEnd()
    {
        if (OnAttackEnd != null)
        {
            OnAttackEnd.Invoke();
        }
        gameObject.SetActive(false);
    }


}

