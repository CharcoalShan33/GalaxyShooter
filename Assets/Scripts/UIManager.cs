using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Unity.Mathematics;


public class UIManager : MonoBehaviour
{
    [SerializeField]
    private TMP_Text _score_Text;

    [SerializeField]
    private Image _livesImage;

    [SerializeField]
    private Sprite[] lifeSprites;

    [SerializeField]
    private TMP_Text _gameOverText;

    [SerializeField]
    private TMP_Text _restartText;
    private GameManager gameManager;

    [Header("Cool-Down")]
    [SerializeField]
    TMP_Text _timeText;

    [SerializeField]
    Slider _cFiller;

    [SerializeField]
    float coolDownSeconds;
    float maxCoolDown = 5f;

    [Header("Thruster")]


    [SerializeField]
    Slider _chargeBar;

    [SerializeField]
    Image thrustFill;

    [SerializeField]
    private Gradient thrustGrade;

    [SerializeField]
    TMP_Text _thrustText;

    [Header("Reload")]
    // current ammo text
    [SerializeField]
    TMP_Text _ammoText;

    [Header("Shield")]

    [SerializeField]
    Slider shieldSlider;

    [SerializeField]
    TMP_Text shieldText;

    [SerializeField]
    private Gradient shieldGrade;

    [SerializeField]
    Image shieldFill;
    private Player play;

    [Header("Magnet")]

    [SerializeField]
    Slider visualizer;

    [SerializeField]
    Image magnetFill;

    [SerializeField]
    private Gradient magnetGrade;

    [SerializeField]
    TMP_Text _magnetText;


    // Start is called before the first frame update
    void Start()
    {
        _score_Text.text = 0000.ToString();

        shieldText.text = 0.ToString();

        play = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();

        _gameOverText.gameObject.SetActive(false);

        _restartText.gameObject.SetActive(false);

        gameManager = GameObject.Find("game_manager").GetComponent<GameManager>();

        shieldSlider.gameObject.SetActive(false);

        _cFiller.gameObject.SetActive(false);
        maxCoolDown = _cFiller.maxValue;
        _cFiller.value = maxCoolDown;
        //UpdateAmmo();

    }

    // Update is called once per frame
    void Update()
    {

        UpdateAmmo();

        _timeText.text = coolDownSeconds.ToString("0.0");

        if (coolDownSeconds > 0)
        {
            coolDownSeconds -= Time.deltaTime;
            _cFiller.value = CountDown();
        }
        else if (coolDownSeconds <= 0)
        {
            coolDownSeconds = 0;
            _cFiller.value = 0;
        }

    }
    float CountDown()
    {
        return maxCoolDown - coolDownSeconds;
    }

    public void UpdateShields(int currentShield)
    {

        shieldText.text = currentShield.ToString();
    }

    public void UpdateMagnetCharge(float magnet)
    {

        _magnetText.text = Mathf.RoundToInt(magnet) + "%";
    }


    public void UpdateScore(int playerScore)
    {
        _score_Text.text = playerScore.ToString();
    }

    public void UpdateLives(int currentLives)
    {
        _livesImage.sprite = lifeSprites[currentLives];

        if (currentLives == 0)
        {
            GameOverSequence();
        }

    }
    public void UpdateAmmo()
    {
        _ammoText.text = "Ammo: " + play.currentAmmo + " / " + play.maxAmmo + " | " + " Storage: " + play.currentReserve + " / " + play.maxReserve;

        if (play.currentAmmo <= 5 && play.currentAmmo > 0)
        {
            StartCoroutine(Blink());
        }

        else if (play.currentAmmo > 5)
        {
            _ammoText.color = Color.white;

        }

    }
    //_ammoText.text = $"{play.currentAmmo} / {play.maxAmmo} | {play.currentReserve} / {play.maxReserve}";
    //_ammoText.text = reload.ToString();
    //_currentValue = reload;
    //_ammoText.text = "Ammo: " + _currentValue + "/" + _maxValue;



    IEnumerator Blink()
    {
        while (true)
        {
            _ammoText.color = Color.yellow;
            yield return new WaitForSeconds(.1f);
            _ammoText.color = Color.white;
            yield return new WaitForSeconds(.1f);
            if (play.currentAmmo <= 0)
            {
                yield return StartCoroutine(StopBlink());
                yield break;
            }
        }


    }
    IEnumerator StopBlink()
    {
        yield return new WaitForSeconds(.5f);

        _ammoText.color = Color.red;

    }

    void GameOverSequence()
    {
        gameManager.GameOver();
        _gameOverText.gameObject.SetActive(true);
        StartCoroutine(GameOverFlicker());
        _restartText.gameObject.SetActive(true);
    }

    IEnumerator GameOverFlicker()
    {
        while (true)
        {
            _gameOverText.text = "GAME OVER";
            yield return new WaitForSeconds(.5f);
            _gameOverText.text = "";
            yield return new WaitForSeconds(.5f);
        }
    }

    public void SetMaxShield(int shieldNum)
    {
        shieldSlider.maxValue = shieldNum;
        shieldSlider.value = shieldNum;
        shieldFill.color = shieldGrade.Evaluate(shieldNum);
        shieldSlider.gameObject.SetActive(true);
    }

    public void SetShield(int shieldNum)
    {
        shieldSlider.value = shieldNum;
        shieldFill.color = shieldGrade.Evaluate(shieldSlider.normalizedValue);

        if (shieldNum <= 0)
        {
            shieldSlider.gameObject.SetActive(false);
        }
    }

    public void StartCountTimer(float count)
    {
        coolDownSeconds = count;
        _cFiller.gameObject.SetActive(true);
        _chargeBar.gameObject.SetActive(false);
        if (count <= 0)
        {
            StopCountDown();
        }
    }
    public void StopCountDown()
    {
        _cFiller.gameObject.SetActive(false);
        _chargeBar.gameObject.SetActive(true);
    }


    public void UseMagnet()
    {
        visualizer.gameObject.SetActive(true);
        play.isMagnetActive = true;

    }

    public void DeactivateMagnet()
    {
        visualizer.gameObject.SetActive(false);
        play.isMagnetActive = false;
    }
}
