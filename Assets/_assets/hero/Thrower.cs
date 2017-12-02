using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Polycrime;

public class Thrower : MonoBehaviour
{
    public GameObject Target;
    public GameObject Gold;
    public GameObject ThrowPoint;

    private Hero _hero;

    private void Start()
    {
        _hero = GetComponent<Hero>(); ;
    }

    public void Throw(string itemName)
    {
        GameObject objToThrow = null;
        

        if (itemName == "gold")
        {
            objToThrow = Instantiate(Gold);
        }
        else
        {
            foreach(var armor in _hero.Armor)
            {
                if (armor.name == itemName)
                {
                    objToThrow = Instantiate(armor, ThrowPoint.transform.position, new Quaternion());
                    break;
                }
            }
        }
   
        if (!objToThrow)
            return;

        objToThrow.SetActive(true);
        objToThrow.AddComponent<Rigidbody>();
        objToThrow.AddComponent<Throwable>();
        objToThrow.GetComponent<Collider>().enabled = true;

        Vector3 velocity = TrajectoryMath.CalculateVelocity(objToThrow.GetComponent<Collider>().bounds.center, Target.transform.position, _hero.ReachTime);

        var objectInterface = objToThrow.GetComponent(typeof(IPropelBehavior)) as IPropelBehavior;

        if (objectInterface != null)
        {
            objectInterface.React(velocity);
        }
    }

}
