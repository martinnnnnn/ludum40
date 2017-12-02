using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundEmiter : MonoBehaviour
{

    private GameObject _hero;

    private void Start()
    {
        _hero = GetComponentInParent<Hero>().gameObject;
    }

    private void OnTriggerEnter(Collider other)
    {
        Monster monster = other.GetComponent<Monster>();
        if (monster)
        {
            monster.OnStartHearing(_hero);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Monster monster = other.GetComponent<Monster>();
        if (monster)
        {
            monster.OnEndHearing(_hero);
        }
    }
}
