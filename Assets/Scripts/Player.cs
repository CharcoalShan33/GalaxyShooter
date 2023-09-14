using System.Collections;
using UnityEngine;
using UnityEngine.Pool;

public class Player : MonoBehaviour
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

    [SerializeField]
    int maxAmmoStorage = 50;

    [Header("Player")]

    [SerializeField]
    private float _pSpeed = 7.0f;

    [SerializeField]
    private GameObject _missle;

    [SerializeField]
    GameObject shootPoint;

    [SerializeField]
    private GameObject tripleShotMissle;

    [SerializeField]
    private GameObject _cannon;

    [SerializeField]
    GameObject homingMissle;

    [SerializeField]
    private GameObject shieldVisual;



    /// <summary>
    /// This is where the power-up activity booleans are located.
    /// </summary>
    private bool _isTripleShotActive;


    private bool isSpeedBoostActive;

    private bool isShieldBoostActive;

    private bool isNewFireActive;


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


    [Header("Speeding")]

    //[SerializeField]
    //float _shiftSpeed = 0.0f;// First example
    [SerializeField]
    GameObject _thrusters;

    private float speedMulitiplier = 2.5f;
    /*
        // Second Example
        [SerializeField]
        int _chargeCount = 0;
        bool _isSpeedBoostOn = false;
        bool stopCharge = false;
    */



    [SerializeField]
    private float minSpeed = 5f;

    [SerializeField]
    private float maxSpeed = 10f;

    [SerializeField]
    float incrementSpeed = 0.0f;

    //bool isCharging;
  

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

        _thrusters.gameObject.transform.localScale = new Vector3(.18f, .18f, .18f);

        _leftHitShip.SetActive(false);
        _rightHitShip.SetActive(false);

        currentAmmo = maxAmmo;


        // effect.SetActive(false);

        _pSpeed = 7.0f;

        _uiManager.ChargeValue(minSpeed, maxSpeed, _pSpeed);



    }

    // Update is called once per frame
    void Update()
    {
        CalculateMovement();

        if (Input.GetKeyDown(KeyCode.Space) && Time.time > _timePassed)
        {
            FireLaser();
        }
        //SpeedUp();

        if (currentAmmo <= 0)
        {
            currentAmmo = 0;
        }

        if (Input.GetKeyDown(KeyCode.M) && Time.time > _timePassed)
        {
            FireMissle();
        }



        if (Input.GetKey(KeyCode.C))
        {

        }
       



        //incrementSpeed = Mathf.Clamp(incrementSpeed, .01f, .5f);

        //UpdateSpeed(incrementSpeed);
    }
   
    void UpdateSpeed(float deltaTime)
    {
        if (_pSpeed < maxSpeed && Input.GetKey(KeyCode.LeftShift))
        {
            _pSpeed += incrementSpeed * deltaTime;
            _pSpeed = Mathf.Clamp(_pSpeed, minSpeed, maxSpeed);

            _uiManager.Charge();
            //_pSpeed = Mathf.Clamp(minSpeed, _pSpeed, maxSpeed);

        }
        else if (_pSpeed > minSpeed && Input.GetKeyUp(KeyCode.LeftShift))
        {
            _pSpeed -= incrementSpeed * deltaTime;
            _pSpeed = Mathf.Clamp(_pSpeed, minSpeed, maxSpeed);
            _uiManager.CancelCharge();
            //_pSpeed = Mathf.Clamp(minSpeed, _pSpeed, maxSpeed);

            _pSpeed = minSpeed;
        }
    }





    void FireMissle()
    {
        isFiring = true;
        _timePassed = Time.time + _fireRate;
        if (currentAmmo > 0)
        {
            PlaySFXClip(_laserShot[0]);
            Instantiate(homingMissle, shootPoint.transform.position, transform.rotation);
        }
    }




    void FireLaser()
    {

        isFiring = true;

        int ammoClamp = Mathf.Clamp(currentAmmo, 0, maxAmmo);
        currentAmmo = ammoClamp;

        _timePassed = Time.time + _fireRate;


        //_audioSource.Play();

        currentAmmo--;

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

            _uiManager.UpdateAmmo(currentAmmo);
        }
        else if (currentAmmo <= 0 && isFiring)
        {
            Debug.Log("No Ammo");
            isFiring = false;

            PlaySFXClip(_laserShot[1]);

            _uiManager.UpdateAmmo(0);
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


            if (_shieldsAmount > 0)

            {
                _shieldsAmount -= 1;
                shieldColor = _shieldRend.color;
                shieldColor.a -= .33f;
                _shieldRend.color = shieldColor;

                _uiManager.UpdateShields(_shieldsAmount);
                _uiManager.SetShield(_shieldsAmount);
            }

            else if (_shieldsAmount <= 0)
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
            Destroy(this.gameObject);
            Debug.Log("I am Destroyed");
            _spawnManager.OnPlayerDeath();
        }
        else
        {
            Debug.Log("I am hit");
        }


    }

    public void NewFireActive()
    {
        tripleShotMissle.SetActive(false);
        _missle.SetActive(false);
        _cannon.SetActive(true);
        isNewFireActive = true;

        StartCoroutine(NewFire());

    }


    public void TripleShotActive()
    {
        tripleShotMissle.SetActive(true);
        _cannon.SetActive(false);
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
        _missle.SetActive(true);
        tripleShotMissle.SetActive(false);


    }
    IEnumerator NewFire()
    {

        yield return new WaitForSeconds(5f);
        isNewFireActive = false;
        _missle.SetActive(true);
        _cannon.SetActive(false);

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
        currentAmmo = maxAmmo;
        _uiManager.UpdateAmmo(maxAmmo);
        isFiring = true;
        PlaySFXClip(_laserShot[0]);
        //_audioSource.mute = false;
    }


    public void Negated()
    {

        int randomEffect = Random.Range(1, 4);

        switch (randomEffect)
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
                else if (!isShieldBoostActive)
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
                _uiManager.UpdateAmmo(currentAmmo);
                Debug.Log("Decrease ammo");
                break;
        }

    }

    IEnumerator SlowDown()
    {
        yield return new WaitForSeconds(5f);
        _pSpeed = minSpeed;
    }

    /*

        void SpeedUp()
        {
            if (Input.GetKeyDown(KeyCode.RightShift) && isSpeedBoostActive == false && _isSpeedBoostOn == false && !stopCharge)
            {

                //_pSpeed += _shiftSpeed;
                _isSpeedBoostOn = true;
                Debug.Log("Speed UP");



                _uiManager.Charge();

                _thrusters.gameObject.transform.localScale = new Vector3(.3f, .3f, .3f);


                if (_chargeCount > 2)
                {
                    StartCoroutine(StopAllCharge());

                }
            }

            else if (Input.GetKeyUp(KeyCode.RightShift) && _isSpeedBoostOn == true && !stopCharge)
            {


                Debug.Log("Speed Down");
                _isSpeedBoostOn = false;

                _uiManager.CancelCharge();

                _thrusters.gameObject.transform.localScale = new Vector3(.2f, .2f, .2f);

                _chargeCount++;




                if (_chargeCount > 2)
                {
                    StartCoroutine(StopAllCharge());

                }

            }
        }

        IEnumerator StopAllCharge()
        {
            stopCharge = true;

            _thrusters.gameObject.transform.localScale = new Vector3(.18f, .18f, .18f);
            _uiManager.StartCountTimer(5f);

            yield return new WaitForSeconds(5.1f);
            _uiManager.StopCountDown();
            _chargeCount = 0;
            stopCharge = false;


        }






        */






}
