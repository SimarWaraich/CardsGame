using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Bhabbhi;

public class PlayerInjector : MonoBehaviour
{
    public GameManager gameManager;
    public GameObject playerPrefab;
    public Transform playerContainer;


    void Start()
    {
        var NumberOfPlayers = 4; //Random.Range(2, 5);

        for (int i = 0; i < NumberOfPlayers; i++)
        {
            var player = Instantiate(playerPrefab, playerContainer).GetComponent<Player>();
            player.gameObject.name = "Player " + i;
            player.PlayerName = "Player " + i;//"Guest" + Random.Range(1111, 9999);
            gameManager.Players.Add(player);
        }
        gameManager.StartGame();

    }
}
