using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Dweiss;

public class Hero : MonoBehaviour
{
    [Header("Movement")]
    public float Speed;

    private Rigidbody _body;
    private float XDirection;
    private float ZDirection;


    [Header("Loot")]
    public int Life;
    public int Gold;
    public GameObject[] Armor;

    private List<GameObject> unactivatedArmor;
    private List<GameObject> activatedArmor;


    private int _maxLife;

	void Start ()
    {
        _body = GetComponent<Rigidbody>();

        Reset();
    }
	
	void Update ()
    {
        XDirection = Input.GetAxis("Horizontal");
        ZDirection = Input.GetAxis("Vertical");

       

        if (Input.GetButtonDown("Fire1"))
        {
            RemoveArmor();
        }
    }

    private void FixedUpdate()
    {
        _body.velocity = new Vector3(XDirection * Time.deltaTime * Speed, _body.velocity.y, ZDirection * Time.deltaTime * Speed);

        if (XDirection != 0 && ZDirection != 0)
        {
            transform.eulerAngles = new Vector3(0, -Mathf.Atan2(XDirection, -ZDirection) * 180 / Mathf.PI, 0);
        }
    }

    public void ReceiveLoot(Loot loot)
    {
        switch(loot.Type)
        {
            case LootType.GOLD:
                Gold += loot.Value;
                break;
            case LootType.ARMOR:
                GetNewArmor();
                break;
        }
    }

    public void GetNewArmor()
    {
        if (Life < _maxLife)
        {
            Life++;

            var obj = unactivatedArmor[Random.Range(0, unactivatedArmor.Count)];
            obj.SetActive(true);
            activatedArmor.Add(obj);
            unactivatedArmor.Remove(obj);

            //int rand = Random.Range(0, Armor.Length);
            //while (Armor[rand].activeSelf)
            //{
            //    rand = Random.Range(0, Armor.Length);
            //}
            //Armor[rand].SetActive(true);
        }
        
    }

    public void RemoveArmor()
    {
        if (Life > 0)
        {
            ReceiveDamage(1);
        }
    }

    public void ReceiveDamage(int value)
    {
        while (Life > 0 && value > 0)
        {
            value--;
            Life--;

            var obj = activatedArmor[Random.Range(0, activatedArmor.Count)];
            obj.SetActive(false);
            unactivatedArmor.Add(obj);
            activatedArmor.Remove(obj);

            //int rand = Random.Range(0, Armor.Length);
            //while (!Armor[rand].activeSelf)
            //{
            //    rand = Random.Range(0, Armor.Length);
            //}
            //Armor[rand].SetActive(false);
        }
        if (value > 0)
        {
            Death();
        }
    }

    private void Death()
    {
        Debug.Log("I'm dead !");
    }

    private void Reset()
    {
        Gold = 0;
        Life = 0;
        _maxLife = Armor.Length;
        unactivatedArmor = new List<GameObject>();
        activatedArmor = new List<GameObject>();
        foreach (var obj in Armor)
        {
            obj.SetActive(false);
            unactivatedArmor.Add(obj);
        }
    }

    //private void OnTriggerEnter(Collider other)
    //{
    //    Monster monster = other.GetComponent<Monster>();
    //    if (monster)
    //    {
    //        monster.OnStartHearing(gameObject);
    //    }
    //}

    //private void OnTriggerExit(Collider other)
    //{
    //    Monster monster = other.GetComponent<Monster>();
    //    if (monster)
    //    {
    //        monster.OnEndHearing(gameObject);
    //    }
    //}
}
