using System.Collections;
using UnityEngine;
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

    //[SerializeField]
    // int maxAmmoStorage = 50;

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

    [SerializeField]
    private GameObject shieldVisual;

    [Header("Speeding")]

    [SerializeField]
    GameObject _thrusters;

    [SerializeField]
    private float minSpeed = 5f;

    [SerializeField]
    private float maxSpeed = 10f;

    private float speedMulitiplier = 2.5f;

    //[serializeField] private GameObject effect;



    // Start is called before the first frame update

    void Start()
    {
        //_spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();
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

        _uiManager.UpdateAmmo(maxAmmo);



        // effect.SetActive(false);
        _pSpeed = 7.0f;

        currentAmmo = maxAmmo;
    }

    // Update is called once per frame
    void Update()
    {
        CalculateMovement();

        if (Input.GetKeyDown(KeyCode.Space) && Time.time > _timePassed)
        {
            FireLaser();
        }

        if (currentAmmo <= 0)
        {
            currentAmmo = 0;
        }

        if (Input.GetKeyDown(KeyCode.M) && Time.time > _timePassed)
        {
            FireMissle();
        }

        if (Input.GetKeyDown(KeyCode.C))
        {
            AttractMagnet();
        }
    }

    void AttractMagnet()
    {
        //foreach;
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

        currentAmmo--;

        if (currentAmmo > 0)
        {
            PlaySFXClip(_laserShot[0]);

            GameObject bullet1 = PoolManager.Instance.RetrieveObject("Laser");
            _missle = bullet1;
            if (bullet1 != null)
            {
                bullet1.transform.position = shootPoint.transform.position;
                bullet1.transform.rotation = shootPoint.transform.rotation;
                bullet1.SetActive(true);
            }


            //Instantiate(_missle, shootPoint.transform.position, Quaternion.identity);

            if (_isTripleShotActive == true)
            {
                GameObject bullet2 = PoolManager.Instance.RetrieveObject("Laser1");
                tripleShotMissle = bullet2;
                if (bullet2 != null)
                {
                    bullet2.transform.position = shootPoint.transform.position;
                    bullet2.transform.rotation = shootPoint.transform.rotation;
                    bullet2.SetActive(true);
                    bullet1.SetActive(false);
                }
                //Instantiate(tripleShotMissle, shootPoint.transform.position, Quaternion.identity);
            }

            if (isNewFireActive == true)
            {

                GameObject bullet3 = PoolManager.Instance.RetrieveObject("Laser2");
                _cannon = bullet3;
                if (bullet3 != null)
                {
                    bullet3.transform.position = shootPoint.transform.position;
                    bullet3.transform.rotation = shootPoint.transform.rotation;
                    bullet3.SetActive(true);
                    bullet1.SetActive(false);
                }

                //Instantiate(_cannon, shootPoint.transform.position, Quaternion.identity);
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
        //tripleShotMissle.SetActive(false);
        //_missle.SetActive(false);
        _cannon.SetActive(true);
        isNewFireActive = true;
        StartCoroutine(NewFire());
    }

    public void TripleShotActive()
    {
        tripleShotMissle.SetActive(true);
        //_cannon.SetActive(false);
        //_missle.SetActive(false);
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
        //_missle.SetActive(true);
    }
    IEnumerator NewFire()
    {
        yield return new WaitForSeconds(5f);
        isNewFireActive = false;
        _cannon.SetActive(false);
        //_missle.SetActive(true);
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
        //PlaySFXClip(_laserShot[0]);
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
}
