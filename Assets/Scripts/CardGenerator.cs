using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardGenerator : MonoBehaviour
{

    public List<Card> cardsPack = new List<Card>();  
    public Queue<Card> playerACardsPack = new Queue<Card>();  
    public Queue<Card> playerBCardsPack = new Queue<Card>();  

    public List<Card> placedCards = new List<Card>();  

    public Sprite[] cardSprites; 

    private void Awake()
    {
        LoadSprites();
        GenerateCards();
        shuffleCardsPack();
        distributeCardsPack();

    }

    #region Card Operations


    void GenerateCards()
    {
        int k = 0;
        for (int i = 1; i <= 4; i++)
        {
            for (int j = 1; j <= 13; j++)
            {
                Card card = new Card((Rank)j, (Suit)i, cardSprites[k++]);
                cardsPack.Add(card);
            }
        }
    }



    void shuffleCardsPack()
    {
        int packSize = cardsPack.Count;
        while (packSize > 1)
        {
            packSize--;
            int n = Random.Range(0, packSize + 1);
            Card temp = cardsPack[n];
            cardsPack[n] = cardsPack[packSize];
            cardsPack[packSize] = temp;
        }
    }


    void distributeCardsPack()
    {

        for (int i = 0; i < cardsPack.Count; i++)
        {

            if (i % 2 == 0)
                playerACardsPack.Enqueue(cardsPack[i]);
            else
                playerBCardsPack.Enqueue(cardsPack[i]);

        }


    }


    void LoadSprites()
    {
        cardSprites = Resources.LoadAll<Sprite>("Icons"); 
    }

    #endregion


    #region  Some Debugging Methods



    void displayCardsPack()
    {
        foreach (Card card in cardsPack)
            print(card);
    }


    void displayCardPackA()
    {
        print("Cards Count : " + playerACardsPack.Count);
        foreach (Card card in playerACardsPack)
            print(card);
    }


    void displayCardPackB()
    {
        print("Cards Count : " + playerBCardsPack.Count);
        foreach (Card card in playerBCardsPack)
            print(card);
    }



    #endregion


}


#region Card Implementation


public enum Suit
{
    club = 1,
    diamond = 2,
    heart = 3,
    spade = 4

}


public enum Rank
{
    one = 1,
    two = 2,
    three = 3,
    four = 4,
    five = 5,
    six = 6,
    seven = 7,
    eight = 8,
    nine = 9,
    ten = 10,
    jack = 11,
    queen = 12,
    king = 13,
}



public class Card
{

    public Rank rank { get; }
    public Suit suit { get; }
    public Sprite cardSprite { get; }


    public Card(Rank rank, Suit suit, Sprite cardSprite)
    {
        this.rank = rank;
        this.suit = suit;
        this.cardSprite = cardSprite;
    }


    public override string ToString()
    {
        return $"{rank} : {suit}";
    }

}


#endregion
