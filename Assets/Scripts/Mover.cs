using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mover : MonoBehaviour
{    private Rigidbody2D rig;

    [SerializeField] bool canRotate;
    [SerializeField] bool canMove;
    float _speed;
    [SerializeField] float defaultSpeed = 1.5f;

    float _rotateSpeed;
    [SerializeField] float defaultRotation = 45f;
    // Start is called before the first frame update
    void Start()
    {
        
        rig = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        defaultSpeed = Mathf.Clamp(defaultSpeed, 0f, 100f);
        defaultRotation = Mathf.Clamp(defaultRotation, -90f, 90f);
        if (canMove)
        {
            _speed = defaultSpeed;
            rig.velocity = _speed * Time.deltaTime * Vector3.down;
        }

        if (canRotate)
        {
            _rotateSpeed = defaultRotation;
            rig.angularVelocity = _rotateSpeed;
        }
        else
        {
            if (!canMove)
            {
                _speed = 0f;
                rig.velocity = Vector3.zero;
            }

            if (!canRotate)
            {
                _rotateSpeed = 0f;
                rig.angularVelocity = 0f;
            }
        }
    }
}
