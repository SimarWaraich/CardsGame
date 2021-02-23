using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
namespace Bhabbhi
{
    public class GameManager : MonoBehaviour
    {
        public List<Card> Deck = new List<Card>(52);
        public DeckDatabase deckDatabase;
        public List<Player> Players;
        public int IndexOfPlayerWithTurn;
        public int randomNo;
        public Transform CardsHolder;
        public GameObject CardPrefab;
        Dictionary<int, Card> CardsOnTable = new Dictionary<int, Card>();

        List<Player> Standings = new List<Player>();

        public SuitsEnum CurrentSuit = SuitsEnum.Undefined;
        public static GameManager Instance;

        private void Awake()
        {
            if (Instance != null)
                Destroy(Instance);
            Instance = this;

        }

        public void StartGame()
        {
            Standings.Clear();
            CardsOnTable.Clear();
            AssignNumberToPlayers();
            Deck = deckDatabase.cardDecks[0].Cards;
            Deck = Deck.OrderBy((arg) => UnityEngine.Random.value).ToList();
            DistributeCards();
        }

        private void AssignNumberToPlayers()
        {
            for (int i = 0; i < Players.Count; i++)
            {
                Players[i].PlayerNumber = i;
            }
            var random = randomNo = UnityEngine.Random.Range(0, Players.Count);
            Players[random].IsMine = true;

            for (int i = 0; i < Players.Count; i++)
            {
                if (random >= Players.Count)
                {
                    random = 0;
                }
                if (i == 0)
                {
                    Players[random].GetComponent<RectTransform>().anchorMin = new Vector2(0.5f, 0);
                    Players[random].GetComponent<RectTransform>().anchorMax = new Vector2(0.5f, 0);
                    Players[random].GetComponent<RectTransform>().pivot = new Vector2(0.5f, 0);
                }
                else if (i == 1)
                {
                    Players[random].GetComponent<RectTransform>().anchorMin = new Vector2(1, 0.5f);
                    Players[random].GetComponent<RectTransform>().anchorMax = new Vector2(1, 0.5f);
                    Players[random].GetComponent<RectTransform>().pivot = new Vector2(1, 0.5f);
                }
                else if (i == 2)
                {
                    Players[random].GetComponent<RectTransform>().anchorMin = new Vector2(0.5f, 1);
                    Players[random].GetComponent<RectTransform>().anchorMax = new Vector2(0.5f, 1);
                    Players[random].GetComponent<RectTransform>().pivot = new Vector2(0.5f, 1);
                }
                else if (i == 3)
                {
                    Players[random].GetComponent<RectTransform>().anchorMin = new Vector2(0, 0.5f);
                    Players[random].GetComponent<RectTransform>().anchorMax = new Vector2(0, 0.5f);
                    Players[random].GetComponent<RectTransform>().pivot = new Vector2(0, 0.5f);
                }

                //Players[random].GameStarted();
                random++;

            }
        }

        void DistributeCards()
        {
            for (int i = 0; i < Deck.Count; i++)
            {
                var value = i % Players.Count;
                Deck[i].SetName();
                Players[value].MyHandOfCards.Add(Deck[i]);
            }
            IndexOfPlayerWithTurn = FindPlayerWithAceOfSpade();
            StartCoroutine(GiveTurnToPlayer(Players[IndexOfPlayerWithTurn], true));
            Debug.LogError("Player with Ghadi is " + IndexOfPlayerWithTurn);
            Deck.Clear();
            Players[randomNo].ShowUserCards();
        }

        public void ThrowACard(Card card, int playerNumber)
        {
            var cardGO = Instantiate(CardPrefab, CardsHolder).GetComponent<CardObject>();
            cardGO.Initialize(card);
            cardGO.transform.eulerAngles = new Vector3(0, 0, UnityEngine.Random.Range(-90f, 90f));
            //cardGO.transform. = new Vector3(0, 0, UnityEngine.Random.Range(-90f, 90f));

            if (CurrentSuit == SuitsEnum.Undefined)
            {
                CurrentSuit = card.Suit;
                Debug.LogError("New Saar Started " + card.Name);
                OnStartOfANewSaar();
            }
            else if (CheckForThola(card))
            {
                Debug.LogError("Tholla is given " + card.Name);
                StartCoroutine(GiveThollaToPlayer(card));
                return;
            }

            CardsOnTable.Add(playerNumber, card);

            if (HasAllPlayerPlayedThereCards())
            {
                StartCoroutine(FoldTheCardsOnTable());
            }
            else
            {
                ChangeTurnToNextPlayer();
            }
        }

        private bool HasAllPlayerPlayedThereCards()
        {
            for (int i = 0; i < Players.Count; i++)
            {
                for (int x = 0; x < CardsOnTable.Count; x++)
                {
                    if (!CardsOnTable.ContainsKey(Players[i].PlayerNumber) && Players[i].playerState != Player.States.Finished)
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        public void PlayerCardsFinished(Player player)
        {
            Standings.Add(player);
        }

        IEnumerator GiveThollaToPlayer(Card card)
        {
            IndexOfPlayerWithTurn = CheckPlayerWithHighestCard();
            yield return new WaitForSeconds(1);
            //Players[playerWithTurn].MyHandOfCards.Add(card);
            for (int i = 0; i < CardsHolder.childCount; i++)
            {
                var cardGo = CardsHolder.GetChild(i).GetComponent<CardObject>();
                Players[IndexOfPlayerWithTurn].AddTholaCards(cardGo.GetCard());
                Destroy(cardGo.gameObject, 0.2f);
            }
            print("Tholla is added to Player" + IndexOfPlayerWithTurn);
            CardsOnTable.Clear();
            CurrentSuit = SuitsEnum.Undefined;
            StartCoroutine(GiveTurnToPlayer(Players[IndexOfPlayerWithTurn]));
        }

        IEnumerator FoldTheCardsOnTable()
        {
            IndexOfPlayerWithTurn = CheckPlayerWithHighestCard();
            StartCoroutine(GiveTurnToPlayer(Players[IndexOfPlayerWithTurn]));
            yield return new WaitForSeconds(1);
            for (int i = 0; i < CardsHolder.childCount; i++)
            {
                Destroy(CardsHolder.GetChild(i).gameObject, 0.2f);
            }
            Deck.AddRange(CardsOnTable.Values);
            CardsOnTable.Clear();
            CurrentSuit = SuitsEnum.Undefined;
        }

        private void OnStartOfANewSaar()
        {
        }

        private int CheckPlayerWithHighestCard()
        {
            var max = CardsOnTable.FirstOrDefault();
            foreach (var kvp in CardsOnTable)
            {
                if (kvp.Value.Value > max.Value.Value)
                    max = kvp;
            }
            return max.Key;
        }

        private bool CheckForThola(Card card)
        {
            if (CurrentSuit != card.Suit && CardsOnTable.Count > 0)
            {
                return true;
            }
            return false;
        }

        public void ChangeTurnToNextPlayer()
        {
            if (CheckIfAllPlayersHaveFinishedOrQuited())
            {

            }

            IndexOfPlayerWithTurn++;

            if (IndexOfPlayerWithTurn >= Players.Count)
                IndexOfPlayerWithTurn = 0;

            StartCoroutine(GiveTurnToPlayer(Players[IndexOfPlayerWithTurn]));
        }

        private bool CheckIfAllPlayersHaveFinishedOrQuited()
        {
            int playersActive = 4;
            for (int i = 0; i < Players.Count; i++)
            {
                if (Players[i].playerState == Player.States.Finished)
                {
                    playersActive--;
                }
            }
            return (playersActive < 2);
        }

        IEnumerator GiveTurnToPlayer(Player player, bool isFirstMove = false)
        {
            if (Standings.Contains(player))
            {

            }
            yield return new WaitForSeconds(3f);

            if (isFirstMove)
                player.MakeFirstMove();

            player.StartMyTurn();
        }

        int FindPlayerWithAceOfSpade()
        {
            for (int i = 0; i < Players.Count; i++)
            {
                if (Players[i].FindAceOfSpade() != null)
                    return i;
            }
            return -1;
        }
    }
}