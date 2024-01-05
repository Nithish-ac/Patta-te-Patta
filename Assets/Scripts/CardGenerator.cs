using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//----------- Script To Generate Pack Of Cards And Handle Shuffling And Distribution Of Cards ---------------

public class CardGenerator : MonoBehaviour
{

    public List<Card> cardsPack = new List<Card>();  // List Of Deck Of Cards
    public Queue<Card> playerACardsPack = new Queue<Card>();  // Queue Of Pack Of Cards In Player A Hand
    public Queue<Card> playerBCardsPack = new Queue<Card>();  // Queue Of Pack Of Cards In PLayer B Hand

    public List<Card> placedCards = new List<Card>();  // List Of Placed Cards

    public Sprite[] cardSprites;  // Array Of Sprites To Store Loaded Sprites

    private void Awake()
    {
        LoadSprites();
        GenerateCards();
        shuffleCardsPack();
        distributeCardsPack();

    }

    #region Card Operations

    //------------ Method to generate Pack of Cards -------------------

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


    //---------------- Method To Shuffle Cards ------------------

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

    //------------ Method To Distribute Cards ---------------

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

    //------------ Method To Load Card Sprites ---------------

    void LoadSprites()
    {
        cardSprites = Resources.LoadAll<Sprite>("Icons"); // Getting Sprites[] from Icons Folder in Resources
    }

    #endregion


    #region  Some Debugging Methods

    //-----------------------------Methods For Debugging-----------------------------------------

    //------------- Method To Display Pack Of Cards For Debugging ---------------

    void displayCardsPack()
    {
        foreach (Card card in cardsPack)
            print(card);
    }

    //------------- Method To Display Pack of Cards In Player A Hand For Debugging --------------

    void displayCardPackA()
    {
        print("Cards Count : " + playerACardsPack.Count);
        foreach (Card card in playerACardsPack)
            print(card);
    }

    //------------- Method To Display Pack of Cards In Player B Hand For Debugging --------------

    void displayCardPackB()
    {
        print("Cards Count : " + playerBCardsPack.Count);
        foreach (Card card in playerBCardsPack)
            print(card);
    }


    //---------------------------------------------------------------------------------------------------

    #endregion


}


#region Card Implementation

//-----------------Enumeration Representing Suit Of Card-------------------

public enum Suit
{
    club = 1,
    diamond = 2,
    heart = 3,
    spade = 4

}

//-----------------Enumeration Representing Rank Or Number Of Card---------------

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


//-----------------Class Representing A Card ---------------------

public class Card
{
    // Instance Variables

    public Rank rank { get; }
    public Suit suit { get; }
    public Sprite cardSprite { get; }

    // Constructor

    public Card(Rank rank, Suit suit, Sprite cardSprite)
    {
        this.rank = rank;
        this.suit = suit;
        this.cardSprite = cardSprite;
    }

    // Overriding To String() Method Of Object Class

    public override string ToString()
    {
        return $"{rank} : {suit}";
    }

}


#endregion
