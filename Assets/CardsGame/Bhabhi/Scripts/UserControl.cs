using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
namespace Bhabbhi
{
    public class UserControl : MonoBehaviour
    {
        Player myPlayer;
        public Transform CardsContainer;
        public GameObject CardPrefab;

        private void Awake()
        {
            myPlayer = GetComponent<Player>();
            CardsContainer = GameObject.Find("CardsContainer").transform;
        }

        public void OnCardPressed(CardObject cardGO)
        {
            if (myPlayer.playerState == Player.States.MyTurn)
            {
                myPlayer.PlayMyCard(cardGO.GetCard());
                Destroy(cardGO.gameObject, 0.1f);
            }
        }

        private void Start()
        {
            myPlayer.OnMyTurnStarted += OnMyTurnStarted;
            myPlayer.OnMyTurnOver += OnMyTurnOver;
        }

        private void OnMyTurnStarted()
        {
            // Turn ON card Interactions.
        }

        void OnMyTurnOver()
        {
            // Turn OFF card Interactions.
        }


        public void CreateCards()
        {
            if (myPlayer.MyHandOfCards == null)
                Debug.LogError("Cards have not been initialised");
            myPlayer.MyHandOfCards = myPlayer.MyHandOfCards.OrderBy((arg) => arg.Suit).ThenBy((arg) => arg.Value).ToList();
            print("Cards created");
            for (int i = 0; i < myPlayer.MyHandOfCards.Count; i++)
            {
                var card = Instantiate(CardPrefab, CardsContainer).GetComponent<CardObject>();
                card.Initialize(myPlayer.MyHandOfCards[i], this);
            }
        }

        public void CreateCard(Card card)
        {
            myPlayer.MyHandOfCards = myPlayer.MyHandOfCards.OrderBy((arg) => arg.Suit).ThenBy((arg) => arg.Value).ToList();
            var cardGo = Instantiate(CardPrefab, CardsContainer).GetComponent<CardObject>();
            cardGo.Initialize(card, this);
        }
    }

}
