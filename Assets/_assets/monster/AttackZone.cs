using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackZone : MonoBehaviour
{

    public int Damage = 1;
    public float AttackDuration = 1f;
    private float currentAttackDuration;

    void OnEnable()
    {
        currentAttackDuration = 0;
    }

    void Update()
    {
        currentAttackDuration += Time.deltaTime;
        if (currentAttackDuration >= AttackDuration)
        {
            Debug.Log("AttackEnd");
            gameObject.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Attacking something");

        Hero hero = other.GetComponent<Hero>();
        if (hero)
        {
            hero.ReceiveDamage(Damage);
            gameObject.SetActive(false);
        }
    }
}

