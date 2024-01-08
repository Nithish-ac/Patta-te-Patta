using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum player
{
    A,
    B
};


public class GameLogic : MonoBehaviour
{
    public bool turn;

    public Card cardA; 
    public Card cardB; 

    // UiHandler Script Reference
    private UiHandler _uiRef;

    // Reference to RandomCardGenerator Script
    private RandomCardGenerator _randomCardGen;

    // Refernce To Card Generator Script
    private CardGenerator _cardGen;

    // player Enum Variable Representing Winner
    private player winner;
    private bool _rewardedPlayer;

    private void Start()
    {
        _uiRef = UiHandler.Instance;
        _randomCardGen = FindObjectOfType<RandomCardGenerator>(); // Getting Reference To RandomCardGenerator Script
        _cardGen = FindObjectOfType<CardGenerator>(); // Getting Reference To CardGenerator Script

    }

    #region Player Turn Handler

    //------------------- Handling Player Turns -----------------------

    private void Update()
    {
        if (!_uiRef._matchFound)
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
        _uiRef._matchFound = true;
        _uiRef.btnTurnOff();
        if (toReward.Equals(player.A))
        {
            foreach (Card placed in _cardGen.placedCards)   //  Adding All Placed Cards To Player A Cards Pack
            {
                _cardGen.playerACardsPack.Enqueue(placed);
            }
            _uiRef.CollectCards(true);
            _rewardedPlayer = true;
        }
        else
        {
            foreach (Card placed in _cardGen.placedCards)   // Adding All Placed Cards To Player B Cards Pack
            {
                _cardGen.playerBCardsPack.Enqueue(placed);
            }
            _uiRef.CollectCards(false);
            _rewardedPlayer = false;
        }
        Invoke(nameof(postRewardHandler),1f); // handling After Reward Changes
    }

    //------------ Method To Handle Updations n All After Rewarding Player -----------

    void postRewardHandler()
    {
        _uiRef.enableMessage(); // showing Cards Matched Message
        _cardGen.placedCards.Clear(); // clearing Placed Cards List
        _uiRef.setCountText(); // updating Cards Count Text Of Both The PLayers
        _uiRef.setPlacedCardsCountText(); // updating Placed Cards Count Text
        _uiRef.callDisable(2f); // calling Method To Hide Cards Matched Text Message
        Invoke(nameof(TurnPlayerOn), 2f);
    }
    void TurnPlayerOn()
    {
        _uiRef.btnHandler(_rewardedPlayer);
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

    public void endGame()
    {
        _uiRef.GameEndText(winner);
    }


}
