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

    // Ui Buttons To Place Cards For Both Players
    [SerializeField] Button turnBtnA;
    [SerializeField] Button turnBtnB;

    // Ui Image Components Representing Card Images Of Both Players

    [SerializeField] Image playerACardImg;
    [SerializeField] Image playerBCardImg;

    // Ui Text Component To Display Placed Cards Count
    [SerializeField] TextMeshProUGUI placedCardsCountText;

    // Ui Text Component To Display Message On Card Matching
    [SerializeField] GameObject _cardMatchMessage;

    // Refernce To Card Generator Script
    private CardGenerator _cardGen;

    // String Literals For Turn Text
    private const string turnA = "Player A Turn";
    private const string turnB = "Player B Turn";

    // Ui TextMeshPro Component To Display Turn 
    [SerializeField] TextMeshProUGUI turnText;

    // References To Image Component of Player Turn Buttons
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

        btnImgA = turnBtnA.GetComponent<Image>();  // Getting Image Component Of Player A Turn Btn
        btnImgB = turnBtnB.GetComponent<Image>();   // Getting Image Component Of Player B Turn Btn

        setCountText();  // setting Card Count Text Of Both The Players
        setPlacedCardsCountText(); // setting Placed Cards Count Text 
    }

    //----------- Method To Set Card Details On Ui --------------

    public void setCardDetails(player cardPlayer)
    {
        if (cardPlayer.Equals(player.A))
            cardNumberA.text = _gameLogic.cardA.ToString();
        else
            cardNumberB.text = _gameLogic.cardB.ToString();
    }

    //---------------- Method To Handle Turn Btns BG And Interactability ----------------------

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
        Debug.Log("btn off");
    }

    //----------------- Method To Set Text To Represent Player Turn ----------------------

    public void setTurnText(player playerTurn)
    {
        turnText.text = playerTurn.Equals(player.A) ? turnA : turnB;
    }

    // --------- Method To Show Game End Messages ------------

    public void GameEndText(player winner)
    {

        winnerText.text = $"Player {winner} Wins !";
        Image img = winner.Equals(player.A) ? playerBCardImg : playerACardImg;
        _gameWon.SetActive(true);
        img.enabled = false;
        disableButtons();
        Time.timeScale = 0;

    }

    //------------- Methods To Disable Turn Buttons ------------------

    public void disableButtons()
    {
        turnBtnA.gameObject.SetActive(false);
        turnBtnB.gameObject.SetActive(false);
    }

    //----------- Method To Set Card Sprite Image On Ui ------------

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

    //------------- Method To Set Card Count Text Of Both The Players ---------------

    public void setCountText()
    {
        _playerACountText.text =  _cardGen.playerACardsPack.Count.ToString();
        _playerBCountText.text =  _cardGen.playerBCardsPack.Count.ToString();
    }

    //---------------- Method To Set Placed Cards Count Text ------------------

    public void setPlacedCardsCountText()
    {
        placedCardsCountText.text = "Placed Cards : " + _cardGen.placedCards.Count;
    }

    //------------ Method To Hide Card Match Message Text -----------------

    public void disableMessage()
    {
        _cardMatchMessage.gameObject.SetActive(false);
        DisenableCard?.Invoke();
        _playerCollectCards.SetActive(false);
        _opponentCollectCards.SetActive(false);
    }

    //--------------- Method To Show Card Match Text On Rank Matching ----------------

    public void enableMessage()
    {
        _cardMatchMessage.gameObject.SetActive(true);
    }

    //-------------- Method To Hide Cards Matched Text After Particular Period Of Time -------------

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
        Debug.Log("Collect Cards");
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
