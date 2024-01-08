using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardChanger : MonoBehaviour
{
    private bool _placeCard;
    private bool _playerType;
    private GameLogic _gameLogic;
    [SerializeField]
    private Sprite _defaultImage;
    [SerializeField]
    private Image _image;
    [SerializeField]
    private Animator _animation;
    private readonly string _playerCard = "PlayerCard";
    private readonly string _opponentCard = "OpponentCard";
    void Start()
    {
        _gameLogic = FindObjectOfType<GameLogic>();
    }
    private void OnEnable()
    {
        UiHandler.DisenableCard += DeactivateCard;
    }
    void Update()
    {
        if (gameObject.transform.localScale.x > 0 && _placeCard)
        {
            _placeCard = false;
            if (_playerType)
                _gameLogic.setRandomCardA();
            else
                _gameLogic.setRandomCardB();
        }
    }
    public void GetCard(bool playerType)
    {
        if (!_gameLogic.checkCardsFinish())
        {
            transform.SetAsLastSibling();
            _animation.Rebind();
            _image.enabled = true;
            _image.sprite = _defaultImage;
            _playerType = playerType;
            _placeCard = true;
        }
        else
        {
            DeactivateCard();
            _gameLogic.endGame();
        }
    }
    public void DeactivateCard()
    {
        _image.enabled = false;
    }
    private void OnDisable()
    {
        UiHandler.DisenableCard -= DeactivateCard;
    }
}
