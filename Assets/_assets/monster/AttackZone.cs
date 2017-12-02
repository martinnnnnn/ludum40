using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackZone : MonoBehaviour
{

    public int Damage = 1;
    public float AttackDuration = 1f;
    private float currentAttackDuration;


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

        Hero hero = other.GetComponent<Hero>();
        if (hero)
        {
            hero.ReceiveDamage(Damage);
            AttackEnd();
        }
    }

    private void AttackEnd()
    {
        OnAttackEnd();
        gameObject.SetActive(false);
    }


}

