using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using UnityEngine.SceneManagement;

public class UiHandler : MonoBehaviour
{
    public static UiHandler Instance;
    [SerializeField]
    private GameObject _mainMenu;
    [SerializeField]
    private GameObject _gameWon;
    [SerializeField]
    private GameObject _game;

    public TextMeshProUGUI cardNumberA;
    public TextMeshProUGUI cardNumberB;

    private GameLogic _gameLogic;
    [SerializeField] TextMeshProUGUI winnerText; 

    [SerializeField] TextMeshProUGUI _playerACountText;
    [SerializeField] TextMeshProUGUI _playerBCountText;

    [SerializeField] Button turnBtnA;
    [SerializeField] Button turnBtnB;


    [SerializeField] Image playerACardImg;
    [SerializeField] Image playerBCardImg;

    [SerializeField] TextMeshProUGUI placedCardsCountText;

    [SerializeField] GameObject _cardMatchMessage;

    private CardGenerator _cardGen;

    private const string turnA = "Player A Turn";
    private const string turnB = "Player B Turn";

    [SerializeField] TextMeshProUGUI turnText;

    private Image btnImgA;
    private Image btnImgB;

    [SerializeField]
    private GameObject _playerCollectCards;
    [SerializeField]
    private GameObject _opponentCollectCards;

    public static event Action DisenableCard;

    private GameObject _cardAnimation;

    internal bool _matchFound;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }
    private void Start()
    {
        _cardGen = FindObjectOfType<CardGenerator>();
        _gameLogic = FindObjectOfType<GameLogic>();

        btnImgA = turnBtnA.GetComponent<Image>();  
        btnImgB = turnBtnB.GetComponent<Image>();   

        setCountText();  
        setPlacedCardsCountText(); 
    }


    public void setCardDetails(player cardPlayer)
    {
        if (cardPlayer.Equals(player.A))
            cardNumberA.text = _gameLogic.cardA.ToString();
        else
            cardNumberB.text = _gameLogic.cardB.ToString();
    }


    public void btnHandler(bool turn)
    {
        turnBtnA.interactable = !turn;
        turnBtnB.interactable = turn;
        _matchFound = false;
    }
    public void btnTurnOff()
    {
        turnBtnA.interactable = false;
        turnBtnB.interactable = false;
    }


    public void setTurnText(player playerTurn)
    {
        turnText.text = playerTurn.Equals(player.A) ? turnA : turnB;
    }


    public void GameEndText(player winner)
    {

        winnerText.text = $"Player {winner} Wins !";
        Image img = winner.Equals(player.A) ? playerBCardImg : playerACardImg;
        _gameWon.SetActive(true);
        img.enabled = false;
        disableButtons();
        Time.timeScale = 0;

    }


    public void disableButtons()
    {
        turnBtnA.gameObject.SetActive(false);
        turnBtnB.gameObject.SetActive(false);
    }


    public void setCardSprite(Card card, player pl)
    {

        Sprite cardsprite = card.cardSprite;

        if (pl.Equals(player.A))
        {
            playerACardImg.sprite = cardsprite;
        }
        else
        {
            playerBCardImg.sprite = cardsprite;
        }

    }


    public void setCountText()
    {
        _playerACountText.text =  _cardGen.playerACardsPack.Count.ToString();
        _playerBCountText.text =  _cardGen.playerBCardsPack.Count.ToString();
    }


    public void setPlacedCardsCountText()
    {
        placedCardsCountText.text = "Placed Cards : " + _cardGen.placedCards.Count;
    }


    public void disableMessage()
    {
        _cardMatchMessage.gameObject.SetActive(false);
        DisenableCard?.Invoke();
        _playerCollectCards.SetActive(false);
        _opponentCollectCards.SetActive(false);
    }


    public void enableMessage()
    {
        _cardMatchMessage.gameObject.SetActive(true);
    }


    public void callDisable(float time)
    {
        Invoke(nameof(disableMessage), time);
    }
    public void StartGame()
    {
        _gameWon.SetActive(false);
        _mainMenu.SetActive(false);
        _game.SetActive(true);
        Time.timeScale = 1f;
    }
    public void Mainmenu()
    {
        _gameWon.SetActive(false);
        _game.SetActive(false);
        _mainMenu.SetActive(true);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    public void CollectCards(bool player)
    {
        _cardAnimation = player ? _playerCollectCards : _opponentCollectCards;
        Invoke(nameof(ActiveAnimation), 2f);
    }
    public void ActiveAnimation()
    {
        _cardAnimation.SetActive(true);
    }
    public void ExitApp()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#elif UNITY_ANDROID
            Application.Quit();
#endif
    }
}
