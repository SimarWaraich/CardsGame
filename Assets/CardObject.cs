using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace Bhabbhi
{
    public class CardObject : MonoBehaviour
    {
        public Image CardImage;
        Card Card;
        UserControl Handler;

        private void Start()
        {
            //GetComponent<Button>().onClick.AddListener(OnCardClick);
        }

        public void OnCardClick()
        {
            print("Card clicked");
            if (Handler != null)
            {
                Handler.OnCardPressed(this);
            }
        }

        public void Initialize(Card card, UserControl userControl = null)
        {
            Card = card;
            CardImage.sprite = Card.CardImage;
            Handler = userControl;
        }

        public Card GetCard()
        {
            return Card;
        }
    }
}
