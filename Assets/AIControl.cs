using UnityEngine;
namespace Bhabbhi
{
    public class AIControl : MonoBehaviour
    {
        Player myPlayer;

        void Start()
        {
            myPlayer = GetComponent<Player>();
            myPlayer.OnMyTurnStarted += OnMyTurnStarted;
            myPlayer.OnMyTurnOver += OnMyTurnOver;
        }

        private void OnMyTurnOver()
        {

        }

        private void OnMyTurnStarted()
        {
            print("AiTurn");
            Invoke("MakeAMove", 2f);
        }

        void MakeAMove()
        {
            Card card = myPlayer.FindAceOfSpade();
            if (card == null)
            {
                card = myPlayer.FindACardToMakeAMove(GameManager.Instance.CurrentSuit);// Play a card of matching suit or start a new saar
            }
            if (card == null)
            {
                print("Player " + myPlayer.PlayerName + "Giving thollu");
                card = myPlayer.FindACardToGiveThollu(GameManager.Instance.CurrentSuit);//Give Thollu
            }
            myPlayer.PlayMyCard(card);
        }
    }
}
