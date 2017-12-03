using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hero : MonoBehaviour, IReset
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

    [Header("Attack")]
    public float AttackCooldown = 3;
    public GameObject AttackZone;

    private float currentAttackCooldown = 0;

    private int _maxLife;
    private List<GameObject> unactivatedArmor;
    [HideInInspector]
    public List<GameObject> ActivatedArmor;

    Thrower _thrower;

    public System.Action OnLootChange;

    [HideInInspector]
    public bool _dead;

    private Vector3 _startingPosition;

	void Start ()
    {
        _startingPosition = transform.position;
        _movable = true;
        _body = GetComponent<Rigidbody>();
        _thrower = GetComponent<Thrower>();
        Reset();
        AttackZone.SetActive(false);
    }

    public void Reset()
    {
        _dead = false;
        transform.position = _startingPosition;
        _body.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
        Gold = 0;
        Life = 0;
        _maxLife = Armor.Length;
        unactivatedArmor = new List<GameObject>();
        ActivatedArmor = new List<GameObject>();
        foreach (var obj in Armor)
        {
            obj.SetActive(false);
            unactivatedArmor.Add(obj);
        }
    }

    void Update ()
    {
        if (_dead)
        {
            return;
        }
        XDirection = Input.GetAxis("Horizontal");
        ZDirection = Input.GetAxis("Vertical");

        Attack();
    }

    private void FixedUpdate()
    {
        if (_movable && (Mathf.Abs(XDirection) > 0.1f || Mathf.Abs(ZDirection) > 0.1f))
        {
            _body.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ; 
            _body.velocity = new Vector3(XDirection * Time.deltaTime * Speed, _body.velocity.y, ZDirection * Time.deltaTime * Speed);
            transform.eulerAngles = new Vector3(0, -Mathf.Atan2(XDirection, -ZDirection) * 180 / Mathf.PI, 0);
        }
        else
        {
            _body.velocity = Vector3.zero;
            _body.constraints = RigidbodyConstraints.FreezeAll; 
        }

        //if (Mathf.Abs(XDirection) > 0.1f || Mathf.Abs(ZDirection) > 0.1f)
        //{
        //    transform.eulerAngles = new Vector3(0, -Mathf.Atan2(XDirection, -ZDirection) * 180 / Mathf.PI, 0);
        //}

        if (!_movable)
        {
            var LootAtPosition = new Vector3(Target.transform.position.x, transform.position.y, Target.transform.position.z);
            transform.LookAt(LootAtPosition);
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
        OnLootChange();
    }

    public void GetNewArmor()
    {
        if (Life < _maxLife)
        {
            Life++;

            var obj = unactivatedArmor[Random.Range(0, unactivatedArmor.Count)];
            obj.SetActive(true);
            ActivatedArmor.Add(obj);
            unactivatedArmor.Remove(obj);
        }
        OnLootChange();
    }

    public void ThrowArmor(float reachtime)
    {
        if (Life > 0)
        {
            string armorname = ReceiveDamage(1);
            _thrower.Throw(armorname, reachtime);
        }
        OnLootChange();
    }

    public void ThrowGold(float reachtime)
    {
        if (Gold > 0)
        {
            Gold--;
            _thrower.Throw("gold", reachtime);
        }
        OnLootChange();
    }


    public void RemoveArmor()
    {
        if (Life > 0)
        {
            ReceiveDamage(1);
        }
        OnLootChange();
    }

    public string ReceiveDamage(int value)
    {
        string result = "";

        while (Life > 0 && value > 0)
        {
            value--;
            Life--;

            var obj = ActivatedArmor[Random.Range(0, ActivatedArmor.Count)];
            obj.SetActive(false);
            unactivatedArmor.Add(obj);
            ActivatedArmor.Remove(obj);
            result = obj.name;
        }
        if (value > 0)
        {
            Death();
        }
        OnLootChange();
        return result;
    }

    private void Death()
    {
        _dead = true;
        _body.velocity = Vector3.zero;
        _body.constraints = RigidbodyConstraints.FreezeAll;
        FindObjectOfType<UIHandler>().ShowUI();
    }



    public void Movable(bool movable)
    {
        _movable = movable;
    }

    private void Attack()
    {
        if (currentAttackCooldown > 0)
        {
            currentAttackCooldown -= Time.deltaTime;
        }
        if (Input.GetButtonDown("Attack") && currentAttackCooldown <= 0)
        {
            currentAttackCooldown = AttackCooldown;

            AttackZone.SetActive(true);
        }
    }

}
