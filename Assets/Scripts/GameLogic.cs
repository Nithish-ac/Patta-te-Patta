using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// Enum Representing player

public enum player
{
    A,
    B
};


public class GameLogic : MonoBehaviour
{
    // Boolean Representing turn of player
    public bool turn;

    public Card cardA; // Current Card Placed By A
    public Card cardB; // Current Card Placed By B

    // UiHandler Script Reference
    private UiHandler _uiRef;

    // Reference to RandomCardGenerator Script
    private RandomCardGenerator _randomCardGen;

    // Refernce To Card Generator Script
    private CardGenerator _cardGen;

    // player Enum Variable Representing Winner
    private player winner;



    private void Start()
    {
        _uiRef = FindObjectOfType<UiHandler>(); // Getting Reference To UiHandler Script
        _randomCardGen = FindObjectOfType<RandomCardGenerator>(); // Getting Reference To RandomCardGenerator Script
        _cardGen = FindObjectOfType<CardGenerator>(); // Getting Reference To CardGenerator Script

    }

    #region Player Turn Handler

    //------------------- Handling Player Turns -----------------------

    private void Update()
    {
        if (!turn)
        {
            _uiRef.setTurnText(player.A);
            _uiRef.btnHandler(turn);

        }
        else
        {
            _uiRef.setTurnText(player.B);
            _uiRef.btnHandler(turn);

        }

    }

    #endregion

    #region Player Card Generator On Turn

    //-------------- Method To Pick And Place Card From Player A Pack Of Cards ---------------

    public void setRandomCardA()
    {
        cardA = _randomCardGen.GenerateRandomCardA();
        cardSettings(cardA, player.A);


        AddToPlacedCards(cardA);
        _uiRef.setPlacedCardsCountText();

        _uiRef.setCountText();

        if (checkSameRank())
        {
            rewardCards(player.A);
        }
        else
            turn = !turn;

        if (checkCardsFinish())
            endGame();


    }

    //-------------- Method To Pick And Place Card From Player B Pack Of Cards ---------------

    public void setRandomCardB()
    {
        cardB = _randomCardGen.GenerateRandomCardB();
        cardSettings(cardB, player.B);

        AddToPlacedCards(cardB);
        _uiRef.setPlacedCardsCountText();

        _uiRef.setCountText();

        if (checkSameRank())
        {
            rewardCards(player.B);
        }
        else
            turn = !turn;



        if (checkCardsFinish())
            endGame();

    }

    #endregion

    //------------- Method To Handle Setting of  Card Sprite And Details --------------

    void cardSettings(Card card, player pl)
    {
        _uiRef.setCardSprite(card, pl);
        _uiRef.setCardDetails(pl);
    }


    //----------------- Method To Check Whether Rank Is Same Or Not On Placing Of Card By Player ------------------

    bool checkSameRank()
    {
        return cardA != null && cardB != null && cardA.rank.ToString() == cardB.rank.ToString();
    }

    //------------------ Method To Reward Cards To Respective PLayer On Rank Matching -------------------

    void rewardCards(player toReward)
    {
        if (toReward.Equals(player.A))
        {
            foreach (Card placed in _cardGen.placedCards)   //  Adding All Placed Cards To Player A Cards Pack
            {
                _cardGen.playerACardsPack.Enqueue(placed);

            }

        }
        else
        {
            foreach (Card placed in _cardGen.placedCards)   // Adding All Placed Cards To Player B Cards Pack
            {
                _cardGen.playerBCardsPack.Enqueue(placed);

            }

        }


        postRewardHandler(); // handling After Reward Changes

    }

    //------------ Method To Handle Updations n All After Rewarding Player -----------

    void postRewardHandler()
    {
        _uiRef.enableMessage(); // showing Cards Matched Message
        _cardGen.placedCards.Clear(); // clearing Placed Cards List
        _uiRef.setCountText(); // updating Cards Count Text Of Both The PLayers
        _uiRef.setPlacedCardsCountText(); // updating Placed Cards Count Text
        _uiRef.callDisable(2f); // calling Method To Hide Cards Matched Text Message

    }

    //----------- Method To Check Game End And Decide Winner --------------

    public bool checkCardsFinish()
    {
        if (_cardGen.playerACardsPack.Count == 0)
        {
            winner = player.B;
            return true;
        }
        else
             if (_cardGen.playerBCardsPack.Count == 0)
        {
            winner = player.A;
            return true;
        }

        return false;
    }

    //------------ Method To Add Card To Pack Of Placed Cards -------------

    void AddToPlacedCards(Card cardToAdd)
    {
        _cardGen.placedCards.Add(cardToAdd);
    }

    //----------- Method To Handle End Of Game ------------

    void endGame()
    {
        _uiRef.GameEndText(winner);
    }


}
