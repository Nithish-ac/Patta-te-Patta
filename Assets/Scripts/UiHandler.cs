using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


//-------------- Script To Handle Ui Elements And Events ----------------

public class UiHandler : MonoBehaviour
{

    // Ui TextMeshPro Components To Display Genarted Card Rank And Suit
    public TextMeshProUGUI cardNumberA;
    public TextMeshProUGUI cardNumberB;

    private GameLogic _gameLogic;
    //[SerializeField] TurnHandler _turnRef; // Reference To TurnHandler Script
    [SerializeField] TextMeshProUGUI winnerText; // winner Ui Text Component

    // Ui Text Components To Display Player Cards Count 

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
    [SerializeField] TextMeshProUGUI _cardMatchMessage;

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

    // Default BG Color For Button
    private Color defaultBtnColor;

    private void Start()
    {
        _cardGen = FindObjectOfType<CardGenerator>();
        _gameLogic = FindObjectOfType<GameLogic>();

        btnImgA = turnBtnA.GetComponent<Image>();  // Getting Image Component Of Player A Turn Btn
        btnImgB = turnBtnB.GetComponent<Image>();   // Getting Image Component Of Player B Turn Btn
        defaultBtnColor = new Color(221f, 220f, 201f, 255f); // Setting Default Btn Color

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

        btnImgA.color = turn ? defaultBtnColor : Color.yellow;
        btnImgB.color = turn ? Color.yellow : defaultBtnColor;
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
        _playerACountText.text = "Cards : " + _cardGen.playerACardsPack.Count;
        _playerBCountText.text = "Cards : " + _cardGen.playerBCardsPack.Count;
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

}
