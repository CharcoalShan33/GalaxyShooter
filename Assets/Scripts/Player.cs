using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

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
    private float _pSpeed = 7.0f;

    [SerializeField]
    private GameObject _missle;

    SpriteRenderer spriteRend;

    [Header("Weapons")]
    [SerializeField]
    GameObject shootPoint;

    [SerializeField]
    private GameObject tripleShotMissle;

    [SerializeField]
    private GameObject _cannon;

    [SerializeField]
    GameObject homingMissle;

    [Header("Active Functions")]

    private bool _isTripleShotActive;

    private bool isSpeedBoostActive;

    private bool isShieldBoostActive;

    private bool isNewFireActive;

    [SerializeField] GameObject explodeObject;


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

    [SerializeField]
    GameObject visualizer;

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
    GameObject _thrusters;

    [SerializeField]
    int chargeCount = 1;

    bool _isThrustBoostOn;

    bool _stopCharge;

    [SerializeField]
    float _shiftSpeed = 3.0f;

    private float minSpeed = 7f;
    // private float maxSpeed = 10f;


    // for the speed powerup
    private float speedMulitiplier = 1.5f;

    //[serializeField] private GameObject effect;

    // Start is called before the first frame update

    void Start()
    {
        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();
        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();
        _audioSource = GetComponent<AudioSource>();
        _shieldRend = transform.Find("Shield").GetComponentInChildren<SpriteRenderer>();

        if (_uiManager == null)
        {
            Debug.LogError("UI Manager is Null! Add a UI component.");
        }
        if (_spawnManager == null)
        {
            Debug.LogError("Spawn Manager is NULL! Add a spawn manager component.");
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



        _thrusters.gameObject.transform.localScale = new Vector3(.18f, .18f, .18f);

        _leftHitShip.SetActive(false);
        _rightHitShip.SetActive(false);
        // effect.SetActive(false);
        _pSpeed = minSpeed;

        currentAmmo = maxAmmo;

    }

    // Update is called once per frame
    void Update()
    {
        CalculateMovement();
        SpeedUp();

        if (Input.GetKeyDown(KeyCode.Space) && Time.time > _timePassed)
        {
            FireLaser();
        }

        if (currentAmmo <= 0)
        {
            currentAmmo = 0;
        }

        

        if (Input.GetKeyDown(KeyCode.C))
        {
            AttractMagnet();
        }

        if (Input.GetKeyDown(KeyCode.N) && currentAmmo <= 0 && currentReserve > 0)
        {

            RefillAmmo();
        }


        if (currentReserve <= 0)
        {

            currentReserve = 0;

        }

    }

    public void SpeedUp()
    {
        if (Input.GetKeyDown(KeyCode.RightShift) && !isSpeedBoostActive && !_isThrustBoostOn && !_stopCharge)
        {
            _pSpeed += _shiftSpeed;

            _isThrustBoostOn = true;

            _uiManager.Charge();
            _thrusters.gameObject.transform.localScale = new Vector3(.3f, .3f, 0f);

            if (chargeCount > 2)
            {

                StartCoroutine(StopAllCharge());
            }

        }
        else if (Input.GetKeyUp(KeyCode.RightShift) && _isThrustBoostOn && !_stopCharge)
        {
            _pSpeed -= _shiftSpeed;

            _isThrustBoostOn = false;
            _uiManager.CancelCharge();

            _thrusters.gameObject.transform.localScale = new Vector3(.2f, .2f, 0f);

            chargeCount++;
            if (chargeCount > 2)
            {
               
                StartCoroutine(StopAllCharge());
            }
        }


    }

    IEnumerator StopAllCharge()
    {
        _stopCharge = true;
        _thrusters.gameObject.transform.localScale = new Vector3(.17f, .17f, 0f);
        _uiManager.StartCountTimer(5);
        yield return new WaitForSeconds(5.1f);
        _uiManager.StopCountDown();
        chargeCount = 0;
        _stopCharge = false;

    }

    void AttractMagnet()
    {
        //foreach;
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

            Instantiate(_missle, shootPoint.transform.position, Quaternion.identity);

            if (_isTripleShotActive == true)
            {

                Instantiate(tripleShotMissle, shootPoint.transform.position, Quaternion.identity);
            }

            if (isNewFireActive == true)
            {
                Instantiate(_cannon, shootPoint.transform.position, Quaternion.identity);
            }

           
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


        lives--;

        if (lives == 2)
        {

            _rightHitShip.SetActive(true);
        }
        if (lives == 1)
        {

            _leftHitShip.SetActive(true);
        }

        _uiManager.UpdateLives(lives);

        if (lives <= 0)
        {
            lives = 0;

            Instantiate(explodeObject, transform.position, Quaternion.identity);

            gameObject.SetActive(false);
            Destroy(this.gameObject, 2.3f);
            Debug.Log("I am Destroyed");
            _spawnManager.OnPlayerDeath();
        }

    }

    public void NewFireActive()
    {
       
       _missle.SetActive(false);
        _cannon.SetActive(true);
        isNewFireActive = true;
        StartCoroutine(NewFire());
    }

    public void TripleShotActive()
    {
        tripleShotMissle.SetActive(true);
        //_cannon.SetActive(false);
        _missle.SetActive(false);
        _isTripleShotActive = true;
        StartCoroutine(TripleShotCountDown());
    }

    public void SpeedBoostActive()
    {
        isSpeedBoostActive = true;
        _pSpeed *= speedMulitiplier;
        StartCoroutine(SpeedCountDown());
        //effect.SetActive(true);
    }

    public void ShieldBoostActive()
    {

        isShieldBoostActive = true;
        _uiManager.UpdateShields(maxShieldAmount);
        _uiManager.SetMaxShield(maxShieldAmount);
        _shieldRend.color = new Color(1f, 1f, 1f);
        shieldVisual.SetActive(true);
    }

    IEnumerator TripleShotCountDown()
    {
        yield return new WaitForSeconds(5f);
        _isTripleShotActive = false;
        tripleShotMissle.SetActive(false);
        _missle.SetActive(true);
    }
    IEnumerator NewFire()
    {
        yield return new WaitForSeconds(5f);
        isNewFireActive = false;
        _cannon.SetActive(false);
        _missle.SetActive(true);
    }

    IEnumerator SpeedCountDown()
    {
        yield return new WaitForSeconds(5f);
        _pSpeed /= speedMulitiplier;
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


    
}