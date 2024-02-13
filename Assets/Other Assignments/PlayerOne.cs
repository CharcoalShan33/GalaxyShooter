using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerOne : MonoBehaviour
{
    [Header("Shooting")]

    [SerializeField]
    private float _fireRate = .5f;

    private float _timePassed;


    [SerializeField]
    int maxAmmo = 15;

    [SerializeField]
    int currentAmmo = 0;

    bool isFiring;

    //[SerializeField]
    // int maxAmmoStorage = 50;

    [Header("Player")]

    [SerializeField]
    private float _pSpeed = 7.0f;

    [SerializeField]
    private GameObject _missle;

    [SerializeField]
    GameObject shootPoint;


    private Color color;

    private void Start()
    {
        _pSpeed = 7.0f;

        currentAmmo = maxAmmo;

        
    }

    private void Update()
    {
        CalculateMovement();

        if (Input.GetKeyDown(KeyCode.Space) && Time.time > _timePassed)
        {
            FireLaser();

            //Ray2D ray = new(transform.position, transform.up);
            // Debug.DrawRay(ray.origin, ray.direction * 6);

           

        }

        if (currentAmmo <= 0)
        {
            currentAmmo = 0;
        }



    }


    void FireLaser()
    {

        isFiring = true;

        int ammoClamp = Mathf.Clamp(currentAmmo, 0, maxAmmo);
        currentAmmo = ammoClamp;

        _timePassed = Time.time + _fireRate;

        currentAmmo--;

        if (currentAmmo > 0)
        {
            Instantiate(_missle, shootPoint.transform.position, Quaternion.identity);
        }
        else if (currentAmmo <= 0 && isFiring)
        {
            Debug.Log("No Ammo");
            isFiring = false;
        }
    }

    void CalculateMovement()
    {
        float hInput = Input.GetAxis("Horizontal");
        float vInput = Input.GetAxis("Vertical");
        Vector2 direction = new Vector2(hInput, vInput);
        transform.position = new Vector2(transform.position.x, Mathf.Clamp(transform.position.y, -7f, 7f));
        transform.position = new Vector2(Mathf.Clamp(transform.position.x, -14f, 14f), transform.position.y);
        transform.Translate(direction * _pSpeed * Time.deltaTime);
    }




    ///The Test Code///

    
}