using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hero : MonoBehaviour
{
    [Header("Movement")]
    public float Speed;

    private Rigidbody _body;
    private float XDirection;
    private float ZDirection;
    private bool _movable;


    [Header("Loot")]
    public int Life;
    public int Gold;
    public GameObject[] Armor;

    [Header("Throwing")]
    public ThrowerTarget Target;
    public float ReachTime = 2f;

    private int _maxLife;
    private List<GameObject> unactivatedArmor;
    private List<GameObject> activatedArmor;

    Thrower _thrower;

    public System.Action OnLootChange;

	void Start ()
    {
        _movable = true;
        _body = GetComponent<Rigidbody>();
        _thrower = GetComponent<Thrower>();
        Reset();
    }
	
	void Update ()
    {
        XDirection = Input.GetAxis("Horizontal");
        ZDirection = Input.GetAxis("Vertical");
    }

    private void FixedUpdate()
    {
        if (_movable)
        {
            _body.velocity = new Vector3(XDirection * Time.deltaTime * Speed, _body.velocity.y, ZDirection * Time.deltaTime * Speed);
        }
        else
        {
            _body.velocity = Vector3.zero;
        }

        if (XDirection != 0 && ZDirection != 0)
        {
            transform.eulerAngles = new Vector3(0, -Mathf.Atan2(XDirection, -ZDirection) * 180 / Mathf.PI, 0);
        }

        if (!_movable)
        {
            transform.LookAt(Target.transform.position);
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
        }
    }

    public void ThrowArmor()
    {
        if (Life > 0)
        {
            string armorname = ReceiveDamage(1);
            _thrower.Throw(armorname);
        }
    }

    public void ThrowGold()
    {
        if (Gold > 0)
        {
            Gold--;
            _thrower.Throw("gold");
        }
    }


    public void RemoveArmor()
    {
        if (Life > 0)
        {
            ReceiveDamage(1);
        }
    }

    public string ReceiveDamage(int value)
    {
        string result = "";

        while (Life > 0 && value > 0)
        {
            value--;
            Life--;

            var obj = activatedArmor[Random.Range(0, activatedArmor.Count)];
            obj.SetActive(false);
            unactivatedArmor.Add(obj);
            activatedArmor.Remove(obj);
            result = obj.name;
        }
        if (value > 0)
        {
            Death();
        }
        return result;
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

    public void Movable(bool movable)
    {
        _movable = movable;
    }
}
