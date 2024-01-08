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

    private UiHandler _uiRef;

    private RandomCardGenerator _randomCardGen;

    private CardGenerator _cardGen;

    private player winner;
    private bool _rewardedPlayer;

    private void Start()
    {
        _uiRef = UiHandler.Instance;
        _randomCardGen = FindObjectOfType<RandomCardGenerator>();
        _cardGen = FindObjectOfType<CardGenerator>(); 
    }

    #region Player Turn Handler


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


    void cardSettings(Card card, player pl)
    {
        _uiRef.setCardSprite(card, pl);
        _uiRef.setCardDetails(pl);
    }



    bool checkSameRank()
    {
        return cardA != null && cardB != null && cardA.rank.ToString() == cardB.rank.ToString();
    }


    void rewardCards(player toReward)
    {
        _uiRef._matchFound = true;
        _uiRef.btnTurnOff();
        if (toReward.Equals(player.A))
        {
            foreach (Card placed in _cardGen.placedCards)  
            {
                _cardGen.playerACardsPack.Enqueue(placed);
            }
            _uiRef.CollectCards(true);
            _rewardedPlayer = true;
        }
        else
        {
            foreach (Card placed in _cardGen.placedCards)  
            {
                _cardGen.playerBCardsPack.Enqueue(placed);
            }
            _uiRef.CollectCards(false);
            _rewardedPlayer = false;
        }
        Invoke(nameof(postRewardHandler),1f); 
    }


    void postRewardHandler()
    {
        _uiRef.enableMessage(); 
        _cardGen.placedCards.Clear(); 
        _uiRef.setCountText(); 
        _uiRef.setPlacedCardsCountText(); 
        _uiRef.callDisable(2f); 
        Invoke(nameof(TurnPlayerOn), 2f);
    }
    void TurnPlayerOn()
    {
        _uiRef.btnHandler(_rewardedPlayer);
    }

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


    void AddToPlacedCards(Card cardToAdd)
    {
        _cardGen.placedCards.Add(cardToAdd);
    }


    public void endGame()
    {
        _uiRef.GameEndText(winner);
    }


}
