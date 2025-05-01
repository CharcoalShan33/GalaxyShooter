using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

using Vector3 = UnityEngine.Vector3;
using Vector2 = UnityEngine.Vector2;
using Unity.Mathematics;
using Unity.VisualScripting;
using TMPro;
using UnityEngine.Assertions.Must;



public class Player : MonoBehaviour
{
    [Header("Shooting")]

    [SerializeField]
    private float _fireRate = .5f;

    private float _timePassed;
    public int currentAmmo, maxAmmo = 15;
    // current Ammo is the hold amount of the mags
    // Max Ammo is the mag amount
    public int currentReserve, maxReserve = 50;
    // Max Ammo is the number that will be reloaded and taken from the reserves.
    // Max Reserve is how many can be stored fully.

    bool isFiring;

    [Header("Player")]

    // default speed
    [SerializeField]
    float _pSpeed = 7.0f;
    [SerializeField]
    GameObject shootPoint;

    [SerializeField]
    GameObject grenadePoint;

    SpriteRenderer spriteRend;

    [Header("Weapons")]
    
    [SerializeField]
    private GameObject _tripleShot;

    [SerializeField]
    private GameObject _cannon; // still using it.

    [SerializeField]
    GameObject _grenade;

    [SerializeField]
    GameObject _laser; // default laser

    // Missile Here

    [Header("Active Functions")]

    private bool _isTripleShotActive;

    private bool isSpeedBoostActive;

    private bool isShieldBoostActive;

    private bool isNewFireActive; // new weapon

    [SerializeField] GameObject explodeObject;


    public bool rareWeaponActive;


    [Header("UI Elements")]

    [SerializeField]
    private int lives;

    [SerializeField]
    private int _score;

    private UIManager _uiManager;

    private SpawnManager _spawnManager;


    [Header("Effects")]

    [SerializeField]
    private GameObject _leftHitShip;

    [SerializeField]
    private GameObject _rightHitShip;

    [Header("Magnet")]

    public bool isMagnetActive = false;

    [SerializeField]
    float magnetRadius;


    [Header("Sound")]

    [SerializeField]
    private AudioClip[] _laserShot;

    private AudioSource _audioSource;

    [Header("Shield")]

    [SerializeField]
    int _shieldsAmount;

    [SerializeField]
    int maxShieldAmount = 3;

    SpriteRenderer _shieldRend;

    Color shieldColor;

    [SerializeField]
    private GameObject shieldVisual;

    [Header("Speeding")]

    [SerializeField]
    float fuelAmount;

    [SerializeField]
    float fuelGainSpeed = 2.0f;

    [SerializeField]
    float fuelDecreaseSpeed = 2.0f;

    [SerializeField]
    GameObject _thrusters;

    private float minSpeed = 7f; // default speed
    private float maxSpeed = 12f; // maximum speed.

    // for the speed powerup
    private float speedMulitiplier = 1.5f;

    // Vector3 minGrowth = new(.2f, .2f, 0.0f);
    Vector3 maxGrowth = new(.3f, .3f, 0.0f);

    Vector3 defaultThrust = new(.17f, .17f, 0.0f);

    [Header("SpawnTime")]
    public float spawnTime = 2f;
    public Vector3 currentPosition;

    [SerializeField] float invincibleTime;

   //[Header("Other")]
    GameManager _gm;

    void Start()
    {
        this.gameObject.tag = "Player";
        currentPosition = transform.position;

        _laser.SetActive(true);

        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();
        _audioSource = GetComponent<AudioSource>();
        _shieldRend = transform.Find("Shield").GetComponentInChildren<SpriteRenderer>();
        _gm = GameObject.Find("game_manager").GetComponent<GameManager>();

        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();
        if (_uiManager == null)
        {
            Debug.LogError("UI Manager is Null! Add a UI component.");
        }
        if (_spawnManager == null)
        {
            Debug.LogError("SpawnManager is NULL! Add a spawn manager component.");
        }

        if (_gm == null)
        {
            Debug.LogError("Game Manager is NULL! Add a game manager component.");
        }

        if (_audioSource == null)
        {
            Debug.LogError("AudioSource is not found! Add an audio source component.");
        }

        if (_shieldRend == null)
        {
            Debug.LogError("The Renderer is not found! Find the child component.");
            Debug.Log("Null. Check if component has a parent");
        }

        spriteRend = GetComponent<SpriteRenderer>();
        if (spriteRend == null)
        {
            Debug.LogError("The renderer is not found! Add a SpriteRenderer source component.");
        }
        _thrusters.gameObject.transform.localScale = defaultThrust;
        _leftHitShip.SetActive(false);
        _rightHitShip.SetActive(false);
        _pSpeed = minSpeed;

        currentAmmo = maxAmmo;
        _pSpeed = Mathf.Ceil(_pSpeed);
    }

    // Update is called once per frame
    void Update()
    {
        CalculateMovement();
        //SpeedUp();
        if (Input.GetKeyDown(KeyCode.Space) && Time.time > _timePassed)
        {
            FireLaser();
        }
        if (currentAmmo <= 0)
        {
            currentAmmo = 0;
        }
        if (Input.GetKey(KeyCode.C))
        {
            AttractMagnet();
        }
        if (Input.GetKeyUp(KeyCode.C) && isMagnetActive)
        {
            StopMagnet();
        }

        if (Input.GetKeyDown(KeyCode.N) && currentAmmo <= 0 && currentReserve > 0)
        {
            RefillAmmo();
        }

        if (Input.GetKeyDown(KeyCode.V))
        {
           // Missiles

        }

        if (Input.GetKeyDown(KeyCode.J))
        {


        // Grenade
        }
        if (currentReserve <= 0)
        {
            currentReserve = 0;
        }

        if (_pSpeed <= 7f)
        {
            _pSpeed = 7f;
        }
        if (_pSpeed >= maxSpeed)
        {
            _pSpeed = maxSpeed;
        }


        if (fuelAmount <= 0f)
        {
            fuelAmount = 0f;
        }

    }

    public void AttractMagnet()
    {
        isMagnetActive = true;
        //foreach;
        _uiManager.UseMagnet();
    }
    public void StopMagnet()
    {
        isMagnetActive = false;
        //foreach;
        _uiManager.DeactivateMagnet();
    }


    void FireLaser()
    {
        isFiring = true;

        int ammoClamp = Mathf.Clamp(currentAmmo, 0, maxAmmo);
        currentAmmo = ammoClamp;

        _timePassed = Time.time + _fireRate;

        currentAmmo--;
        _uiManager.UpdateAmmo();

        if (currentAmmo > 0)
        {
            PlaySFXClip(_laserShot[0]);


            Instantiate(_laser, shootPoint.transform.position, Quaternion.identity);

            if (_isTripleShotActive == true)
            {

                Instantiate(_tripleShot, shootPoint.transform.position, Quaternion.identity);
            }
            //

        }
        else if (currentAmmo <= 0 && isFiring)
        {
            Debug.Log("No Ammo");
            isFiring = false;

            PlaySFXClip(_laserShot[1]);
        }

    }




    void Shake()
    {
        CameraShake camShake = GameObject.Find("ShakeManager").GetComponent<CameraShake>();
        camShake.Tremor(.55f, 25f, .5f, 2);
        // magnitude, frequency, duration, shake ID
    }

    public void PlaySFXClip(AudioClip clip)
    {
        _audioSource.PlayOneShot(clip);
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

    public void TakeDamage()
    {
        while (isShieldBoostActive == true)
        {

            _shieldsAmount -= 1;
            shieldColor = _shieldRend.color;
            shieldColor.a -= .33f;
            _shieldRend.color = shieldColor;

            _uiManager.UpdateShields(_shieldsAmount);
            _uiManager.SetShield(_shieldsAmount);

            if (_shieldsAmount <= 0)
            {
                shieldVisual.SetActive(false);

                isShieldBoostActive = false;
                return;
            }
            return;

        }
        Shake();

        // respawn
        lives--;

        StartCoroutine(Respawn());

        _uiManager.UpdateLives(lives);

        if (lives <= 0)
        {
            lives = 0;
            
           // StopAllCoroutines();
            //StopCoroutine(Respawn());
            Instantiate(explodeObject, transform.position, Quaternion.identity);

            gameObject.SetActive(false);
            Destroy(this.gameObject, 2.3f);
            _spawnManager.OnPlayerDeath();
        }

    }

    private IEnumerator Respawn()
    {

        Collider2D collide = gameObject.GetComponent<Collider2D>();
        collide.enabled = false;
        isFiring = false;
        spriteRend.enabled = false;
        _thrusters.SetActive(false);
        if (lives == 2)
        {


            _rightHitShip.SetActive(false);
        }
        if (lives == 1)
        {

            _rightHitShip.SetActive(false);
            _leftHitShip.SetActive(false);
        }
        gameObject.layer = 7;

        yield return new WaitForSeconds(spawnTime);

        currentPosition = transform.position;

        for (float i = 0; i < invincibleTime; i += 0.1f)
        {

            spriteRend.enabled = !spriteRend.enabled;
            yield return new WaitForSeconds(0.1f);
        }

        if (lives == 2)
        {
            _rightHitShip.SetActive(true);
        }
        if (lives == 1)
        {
            _rightHitShip.SetActive(true);
            _leftHitShip.SetActive(true);
        }
        collide.enabled = true;
        spriteRend.enabled = true;
        isFiring = true;
        _thrusters.SetActive(true);
        gameObject.layer = 6;

    }

    public void NewFireActive()
    {

        _laser.SetActive(false);
        _cannon.SetActive(true);
        isNewFireActive = true;
        StartCoroutine(NewFire());
    }

    public void TripleShotActive()
    {
        _tripleShot.SetActive(true);
        //_cannon.SetActive(false);
        _laser.SetActive(false);
        _isTripleShotActive = true;
        StartCoroutine(TripleShotCountDown());
    }

    public void SpeedBoostActive()
    {
        isSpeedBoostActive = true;
        _pSpeed *= speedMulitiplier;
        _thrusters.transform.localScale = maxGrowth;
        StartCoroutine(SpeedCountDown());
        //effect.SetActive(true);
    }

    public void ShieldBoostActive() // setting max amount of shields
    {

        isShieldBoostActive = true;
        _uiManager.UpdateShields(maxShieldAmount);
        _uiManager.SetMaxShield(maxShieldAmount);
        _shieldRend.color = new Color(1f, 1f, 1f);
        shieldVisual.SetActive(true);
    }

    public void BombFire()
    {
        //rareWeaponActive = true;
        _laser.SetActive(false);
        _grenade.SetActive(true);

        currentAmmo = 10;
        maxAmmo = 10;
        currentReserve = 30;
        maxReserve = 30;

    }

    IEnumerator TripleShotCountDown()
    {
        yield return new WaitForSeconds(5f);
        _isTripleShotActive = false;
        _tripleShot.SetActive(false);
        _laser.SetActive(true);
    }
    IEnumerator NewFire()
    {
        yield return new WaitForSeconds(5f);
        isNewFireActive = false;
        _cannon.SetActive(false);
        _laser.SetActive(true);
    }

    IEnumerator SpeedCountDown()
    {
        yield return new WaitForSeconds(5f);
        _pSpeed /= speedMulitiplier;
        _thrusters.transform.localScale = defaultThrust;

        isSpeedBoostActive = false;
        // effect.SetActive(false);
    }

    public void AddScore(int points)
    {
        _score += points;
        _uiManager.UpdateScore(_score);
        Debug.Log("Add Score");
    }

    public void AddLives()
    {
        if (lives < 3)
        {
            lives++;
            _leftHitShip.SetActive(false);
            _rightHitShip.SetActive(false);
        }

        else
        {
            lives = 3;
        }
        _uiManager.UpdateLives(lives);

    }
    public void AddShieldStrength(int shields)
    {
        _shieldsAmount = shields;

    }

    public void RefillAmmo()
    {
        int reloadAmt = currentAmmo;

        if (currentReserve >= (maxAmmo - reloadAmt))
        {
            currentAmmo += (maxAmmo - reloadAmt);

            currentReserve -= (maxAmmo - reloadAmt);

        }

        else
        {
            currentAmmo = currentReserve;
            currentReserve = 0;
        }

        _uiManager.UpdateAmmo();


        isFiring = true;
        PlaySFXClip(_laserShot[2]);
        _audioSource.mute = false;
    }

    public void AddAmmo(int amount)
    {
        currentReserve += amount;
        if (currentReserve > maxReserve)
        {
            currentReserve = maxReserve;
        }



    }

    public void AddBombs(int amount) /// amount of bombs
    {

    }

    public void AddMissiles(int amount) // amount of missiles
    {

    }
    public void Negated()
    {

        int randomValue = Random.Range(1, 4);

        switch (randomValue)
        {
            case 1:
                _pSpeed = 2;

                StartCoroutine(SlowDown());
                Debug.Log("Slow");
                break;

            case 2:
                if (isShieldBoostActive && _shieldsAmount > 0)
                {

                    _shieldsAmount -= 1;
                    shieldColor = _shieldRend.color;
                    shieldColor.a -= .33f;
                    _shieldRend.color = shieldColor;
                    _uiManager.SetMaxShield(_shieldsAmount);
                    _uiManager.SetShield(_shieldsAmount);
                    _uiManager.UpdateShields(_shieldsAmount);
                    Debug.Log("Decrease Shield");

                }
                if (!isShieldBoostActive)
                {
                    if (_score > 0)
                    {
                        _score -= 10;
                        _uiManager.UpdateScore(_score);
                        Debug.Log("Lower score");
                    }
                }
                break;

            case 3:

                currentAmmo -= 5;
                currentReserve -= 5;
                _uiManager.UpdateAmmo();
                Debug.Log("Decrease ammo");
                break;
        }
    }

    IEnumerator SlowDown()
    {
        yield return new WaitForSeconds(5f);
        _pSpeed = minSpeed;
    }

    public void Reset()
    {
        lives = 3;
        invincibleTime = 3f;
        spawnTime = 3f;
        currentAmmo = maxAmmo;
        currentReserve = maxReserve;
    }

    public void InstantDeath()
    {

        if (lives > 0)
        {
            _uiManager.UpdateLives(0);
            if (isShieldBoostActive == true)
            {
                isShieldBoostActive = false;
            }
            lives = 0;

            Instantiate(explodeObject, transform.position, Quaternion.identity);

            gameObject.SetActive(false);
            Destroy(this.gameObject, 2.3f);
            _gm.GameOver();
            _spawnManager.OnPlayerDeath();
         
            //StopAllCoroutines();
        }



    }
}