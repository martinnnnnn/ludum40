using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum LootType
{
    ARMOR,
    GOLD
}



public class Loot : MonoBehaviour
{


    public LootType Type;
    public int Value;

    void Start ()
    {
		
	}
	
	void Update ()
    {
		
	}

    private void OnTriggerEnter(Collider other)
    {
        Hero hero = other.GetComponent<Hero>();
        if (hero)
        {
            hero.ReceiveLoot(this);
            Destroy(gameObject);
        }
    }
}
