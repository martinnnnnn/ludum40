using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelHandler : MonoBehaviour
{
    [HideInInspector]
    public Monster[] Monsters;
    private GameObject[] _monstersCpy;

    [HideInInspector]
    public Loot[] Loots;
    private GameObject[] _lootsCpy;

    [HideInInspector]
    public Hero Hero;
    private GameObject _heroCpy;

    void Start()
    {
        Monsters = FindObjectsOfType<Monster>();
        _monstersCpy = new GameObject[Monsters.Length];
        for (int i = 0; i < Monsters.Length; i++)
        {
            _monstersCpy[i] = Instantiate(Monsters[i].gameObject, Monsters[i].transform.position, Monsters[i].transform.rotation);
            _monstersCpy[i].SetActive(false);
            _monstersCpy[i].transform.parent = transform;
        }

        Loots = FindObjectsOfType<Loot>();
        _lootsCpy = new GameObject[Loots.Length];
        for (int i = 0; i < Loots.Length; i++)
        {
            _lootsCpy[i] = Instantiate(Loots[i].gameObject, Loots[i].transform.position, Loots[i].transform.rotation);
            _lootsCpy[i].SetActive(false);
            _lootsCpy[i].transform.parent = transform;
        }

        Hero = FindObjectOfType<Hero>();
        _heroCpy = Instantiate(Hero.gameObject, Hero.transform.position, Hero.transform.rotation);
        _heroCpy.SetActive(false);
        _heroCpy.transform.parent = transform;

    }

    public void Reset()
    {
        for (int i = 0; i < Monsters.Length; i++)
        {
            Destroy(Monsters[i].gameObject);
            Monsters[i] = Instantiate(_monstersCpy[i], _monstersCpy[i].transform.position, _monstersCpy[i].transform.rotation).GetComponent<Monster>();
            Monsters[i].gameObject.SetActive(true);
        }
        for (int i = 0; i < Loots.Length; i++)
        {
            Destroy(Loots[i].gameObject);
            Loots[i] = Instantiate(_lootsCpy[i], _lootsCpy[i].transform.position, _lootsCpy[i].transform.rotation).GetComponent<Loot>();
            Loots[i].gameObject.SetActive(true);
        }
        Destroy(Hero.gameObject);
        Hero = Instantiate(_heroCpy, _heroCpy.transform.position, _heroCpy.transform.rotation).GetComponent<Hero>();
        Hero.gameObject.SetActive(true);


        FindObjectOfType<CameraHandler>().Hero = Hero.gameObject;

        //foreach (Monster monster in Monsters)
        //{
        //    Destroy(monster.gameObject);
        //}
        //foreach (Loot loot in Loots)
        //{
        //    Destroy(loot.gameObject);
        //}
        //Destroy(Hero);

        //foreach (GameObject obj in _monstersCpy)
        //{
        //    Instantiate(obj, obj.transform.position, obj.transform.rotation).SetActive(true);

        //}
        //foreach (GameObject obj in _lootsCpy)
        //{
        //    Instantiate(obj, obj.transform.position, obj.transform.rotation).SetActive(true);
        //}
        //Instantiate(_heroCpy, _heroCpy.transform.position, _heroCpy.transform.rotation).SetActive(true);
    }

    void Update()
    {

    }
}
