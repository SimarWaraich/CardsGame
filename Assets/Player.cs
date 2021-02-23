using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
namespace Bhabbhi
{
    public class Player : MonoBehaviour
    {
        public bool IsMine;
        public States playerState = States.Waiting;
        public Image TimerImage;

        GameManager gameManager;
        public string PlayerName;
        public int PlayerNumber;
        public List<Card> MyHandOfCards = new List<Card>();
        RectTransform mTransform;
        private float TotalSecondsToMakeAMove = 60;
        private float Timer;
        bool IsTimerRunning;

        public UnityAction OnMyTurnStarted;
        public UnityAction OnMyTurnOver;

        public Text NameText { get; private set; }

        private void Awake()
        {
            mTransform = GetComponent<RectTransform>();
            gameManager = FindObjectOfType<GameManager>();
            print("players are initialised");
        }

        private void Start()
        {
            NameText = GetComponentInChildren<Text>();
            NameText.text = PlayerName;
            if (IsMine)
            {
                gameObject.GetComponent<UserControl>().enabled = true;
                NameText.text = "You";
                print("This is me player" + PlayerName);
                mTransform.anchoredPosition = new Vector2(0, 60);
            }
            else
            {
                Destroy(gameObject.GetComponent<UserControl>());
                gameObject.AddComponent<AIControl>();
            }
            TimerImage.fillAmount = 0;
        }

        public Card FindACardToMakeAMove(SuitsEnum suit)
        {
            if (suit == SuitsEnum.Undefined)
            {
                return MyHandOfCards[UnityEngine.Random.Range(0, MyHandOfCards.Count)];
            }
            foreach (var item in MyHandOfCards)
            {
                if (item.Suit == suit)
                    return item;
            }
            print("No card found to Move");

            return null;
        }
        public Card FindACardToGiveThollu(SuitsEnum suit)
        {
            foreach (var item in MyHandOfCards)
            {
                if (item.Suit != suit)
                {
                    var max = MyHandOfCards[0];
                    foreach (var card in MyHandOfCards)
                    {
                        if (card.Value > max.Value)
                            max = card;
                    }
                    return max;
                }
            }
            print("No card found to give thollu");
            return null;
        }

        public void ShowUserCards()
        {
            gameObject.GetComponent<UserControl>().CreateCards();
        }

        public Card FindAceOfSpade()
        {
            foreach (var item in MyHandOfCards)
            {
                if (item.Suit == SuitsEnum.Spade && item.Value == ValuesEnum.Ace)
                    return item;
            }
            return null;
        }

        public void StartMyTurn()
        {
            if (playerState == States.Finished || playerState == States.Finished)
            {
                gameManager.ChangeTurnToNextPlayer();
                return;
            }
            print("My turn has started " + PlayerName);
            Timer = TotalSecondsToMakeAMove;
            IsTimerRunning = true;
            playerState = States.MyTurn;
            TimerImage.fillAmount = 1;
            OnMyTurnStarted?.Invoke();
        }

        private void Update()
        {
            if (IsTimerRunning)
            {
                Timer -= Time.deltaTime;
                TimerImage.fillAmount = Timer / TotalSecondsToMakeAMove;
                if (TimerImage.fillAmount < 0.25)
                {
                    TimerImage.color = Color.red;
                }
                else if (TimerImage.fillAmount < 0.5)
                    TimerImage.color = Color.yellow;
                else
                    TimerImage.color = Color.green;

                if (Timer <= 0)
                {
                    OnTimerUp();
                }
            }
        }

        private void OnTimerUp()
        {
            IsTimerRunning = false;
            TimerImage.fillAmount = 0;
            OnMyTurnOver?.Invoke();
            playerState = States.Finished;            //if a player does not play his card Quit it.
            gameManager.ChangeTurnToNextPlayer();
        }

        public void MakeFirstMove()
        {
            //FindAceOfSpade
        }

        public void PlayMyCard(Card card)
        {
            print("Card thrown by " + PlayerName + " Card is " + card.Name);
            MyHandOfCards.Remove(card);
            gameManager.ThrowACard(card, PlayerNumber);
            TimerImage.fillAmount = 0;
            playerState = States.Waiting;
            IsTimerRunning = false;

            if (MyHandOfCards.Count == 0)
            {
                playerState = States.Finished;
                gameManager.PlayerCardsFinished(this);
            }
        }

        public void AddTholaCards(Card card)
        {
            MyHandOfCards.Add(card);
            if (IsMine)
            {
                GetComponent<UserControl>().CreateCard(card);
            }

        }

        public enum States { Waiting, MyTurn, Finished }
    }
}
