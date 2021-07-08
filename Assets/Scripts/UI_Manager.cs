using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Manager : MonoBehaviour
{
    private Text _ScoreText;
    private Player player;
    private int latestScore;

    [SerializeField]
    private Text _waveText;

    //sprites for lives
    [SerializeField]
    private Sprite[] _livesSprites;
    private Image _livesImage;
    private Sprite _livesImageTemp;
    
    [SerializeField]
    private Text _GameoverText;

    [SerializeField]
    private Text _restartText;

    [SerializeField]
    Image GlowingArrow;
    [SerializeField]
    Text BossDamageText;

    private GameManager _gameManager;
   
    // Start is called before the first frame update
    void Start()
    {
        _ScoreText = GameObject.Find("Canvas").GetComponentInChildren<Text>();
        player = GameObject.Find("Player").GetComponent<Player>();
        _livesImage = GameObject.Find("Canvas").GetComponentInChildren<Image>();

        _gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();


        _GameoverText.gameObject.SetActive(false);
        _restartText.gameObject.SetActive(false);
        
        /*_GameoverText = GameObject.Find("Canvas").GetComponentInChildren<Text>();

        if(_GameoverText.gameObject.tag == "GameOver") See later on how to make this work.
        {
            _GameoverText.gameObject.SetActive(false) ;
        }
        */

        if(player == null)
        {
            Debug.LogError("Player Null.");
        }
        _ScoreText.text = "Score: ";
    }

    // Update is called once per frame
    void Update()
    {
        latestScore = player.UpdateScore();
        _ScoreText.text = "Score: " + latestScore.ToString();
    }

    public void updateImage(int _lives)
    {
        _livesImageTemp = _livesSprites[_lives];
        _livesImage.sprite = _livesImageTemp;

        if(_lives == 0)
        {
            GameoverSequence();
        }
    }


    void GameoverSequence()
    {
        _waveText.enabled = false; 
        _GameoverText.gameObject.SetActive(true);
        StartCoroutine(flickerText());
        _restartText.gameObject.SetActive(true);
        _gameManager.gameOver();

    }
    IEnumerator flickerText()
    {
        while(true) { 
        _GameoverText.gameObject.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        _GameoverText.gameObject.SetActive(false);
        yield return new WaitForSeconds(0.5f);
        }
    }

    public void showWaveText(int value)
    {
        if(_waveText.name == "WaveText")
        {
            _waveText.enabled = true;
            int waveNumber = value + 1;
            _waveText.text = "Wave" + " " + waveNumber.ToString();
            _waveText.fontStyle.Equals(FontStyle.Italic);
            StartCoroutine(disableWaveText());
        }

    }

    IEnumerator disableWaveText()
    {
        yield return new WaitForSeconds(2.0f);
        _waveText.enabled = false;
    }

    public void ShowPlayerWinText()
    {
        _waveText.text = "You Won this bad looking game";
    }

    public void DisplayArrowAndText()
    {
        GlowingArrow.GetComponent<Image>().enabled = true;
        BossDamageText.GetComponent<Text>().enabled = true;
        StartCoroutine(CloseArrowAndText());
    }

    IEnumerator CloseArrowAndText()
    {
        yield return new WaitForSeconds(2.5f);
        GlowingArrow.GetComponent<Image>().enabled = false;
        BossDamageText.GetComponent<Text>().enabled = false;
    }

}
