using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum LootType
{
    ARMOR,
    GOLD
}



public class Loot : MonoBehaviour, IReset
{


    public LootType Type;
    public int Value;

    void Start ()
    {
		
	}
	
	void Update ()
    {
		
	}

    public void Reset()
    {
        GetComponent<Renderer>().enabled = true;
        GetComponent<Collider>().enabled = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        Hero hero = other.GetComponent<Hero>();
        if (hero)
        {
            hero.ReceiveLoot(this);
            GetComponent<Renderer>().enabled = false;
            GetComponent<Collider>().enabled = false;
        }
    }
}
