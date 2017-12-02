using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundEmiter : MonoBehaviour
{

    public float InitialRadius = 2;
    public float ArmorRaduis = 2;
    public float GoldRadius = 1;

    private float currentRadius;

    private Hero _hero;
    

    private void Start()
    {
        _hero = GetComponentInParent<Hero>();
        _hero.OnLootChange += OnRadiusChange;
    }

    void OnRadiusChange()
    {
        currentRadius = InitialRadius;
        currentRadius += ArmorRaduis * _hero.Armor.Length;
        currentRadius += GoldRadius * _hero.Gold;
    }
    

    private void OnTriggerEnter(Collider other)
    {
        Monster monster = other.GetComponent<Monster>();
        if (monster)
        {
            monster.OnStartHearing(_hero.gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Monster monster = other.GetComponent<Monster>();
        if (monster)
        {
            monster.OnEndHearing(_hero.gameObject);
        }
    }
}
