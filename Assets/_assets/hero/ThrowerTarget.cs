using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Polycrime;

// TODO : Sound managment
// TODO : fog of war
// TODO : monster attack
// TODO : hero attack
// TODO : throw gold

public class ThrowerTarget : MonoBehaviour
{
    private Hero _hero;
    private Rigidbody _body;
    private LineRenderer _lr;
    private MeshRenderer _renderer;
    private bool _pressed;

    public float Speed;
    public float MaximalDistance;

    private float XDirection;
    private float ZDirection;

    private void Start()
    {
        _hero = GameObject.Find("Hero").GetComponent<Hero>();
        _lr = GetComponent<LineRenderer>();
        _renderer = GetComponent<MeshRenderer>();
        _body = GetComponent<Rigidbody>();
        _renderer.enabled = false;
    }

    private void Update()
    {
        XDirection = Input.GetAxis("Horizontal");
        ZDirection = Input.GetAxis("Vertical");

        if (Input.GetButtonDown("Fire1"))
        {
            _renderer.enabled = true;
            _pressed = true;
            _hero.Movable(false);

            transform.position = new Vector3(_hero.transform.position.x + 10, _hero.transform.position.y, _hero.transform.position.z);
        }

        if (Input.GetButtonUp("Fire1"))
        {
            _renderer.enabled = false;
            _pressed = false;
            _hero.Movable(true);
        }

        if (_pressed)
        {
            _lr.enabled = true;
            Render(_hero.transform.position, transform.position, _hero.ReachTime, Color.blue);
            // TODO : change renderer to fx

            // if input then throw
            if (Input.GetButtonDown("Fire2"))
            {
                _hero.ThrowArmor();
            }
            if (Input.GetButtonDown("Fire3"))
            {
                _hero.ThrowGold();
            }
        }
        else
        {
            _lr.enabled = false;
        }
    }

    private void FixedUpdate()
    {
        if (_pressed)
        {
            if ((Mathf.Abs(XDirection) > 0.2f || Mathf.Abs(ZDirection) > 0.2f))
            {
                _body.velocity = new Vector3(XDirection * Time.deltaTime * Speed, _body.velocity.y, ZDirection * Time.deltaTime * Speed);
            }
            else
            {
                _body.velocity = Vector3.zero;
            }
            if (Vector3.Distance(_hero.transform.position, transform.position) >= MaximalDistance)
            {
                Debug.Log("hello");
                var allowedPos = transform.position - _hero.transform.position;
                allowedPos = Vector3.ClampMagnitude(allowedPos, MaximalDistance);
                transform.position = _hero.transform.position + allowedPos;

                //transform.position = transform.position - _hero.transform.position;
            }
        }
    }

    public void Render(Vector3 startPoint, Vector3 endPoint, float time, Color color)
    {
        Vector3 initialVelocity = TrajectoryMath.CalculateVelocity(startPoint, endPoint, time);
        float deltaTime = time / initialVelocity.magnitude;
        int drawSteps = (int)(initialVelocity.magnitude - 0.5f);
        Vector3 currentPosition = startPoint;
        Vector3 previousPosition = currentPosition;
        Gizmos.color = color;

        List<Vector3> positions = new List<Vector3>();
        positions.Add(startPoint);

        if (IsParabolicVelocity(initialVelocity))
        {
            for (int i = 0; i < drawSteps; i++)
            {
                currentPosition += (initialVelocity * deltaTime) + (0.5f * Physics.gravity * deltaTime * deltaTime);
                initialVelocity += Physics.gravity * deltaTime;
                positions.Add(currentPosition);

                previousPosition = currentPosition;
            }
        }
        else
        {
            Debug.Log("error");
        }
        positions.Add(endPoint);
        _lr.positionCount = positions.Count;
        _lr.SetPositions(positions.ToArray());
    }

   

    private static bool IsParabolicVelocity(Vector3 velocity)
    {
        return !(velocity.x == 0 && velocity.z == 0);
    }
}
