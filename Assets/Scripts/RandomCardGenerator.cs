using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomCardGenerator : MonoBehaviour
{

    private CardGenerator _cardGenerator; // Reference To Card Generator Script

    private void Start()
    {
        _cardGenerator = FindObjectOfType<CardGenerator>();
    }

    //--------------- Method To Generate Card From Player A Pack Of Cards --------------------

    public Card GenerateRandomCardA()
    {
        return _cardGenerator.playerACardsPack.Dequeue();
    }

    //--------------- Method To Generate Card From Player B Pack Of Cards --------------------

    public Card GenerateRandomCardB()
    {
        return _cardGenerator.playerBCardsPack.Dequeue();
    }
}
