using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;


public class GameManager : MonoBehaviourPun
{
    public GameObject playerPrefab;
    public GameObject healthBoostPrefab;
    public GameObject bombPrefab;
    public GameObject canvas;
    public Text pingrate;

    public Text spawnTimer;
    public GameObject respawnUI;

    public static GameManager instance = null;
    private float TimeAmount = 5f;
    private bool startRespawn;
    [HideInInspector]
    public GameObject LocalPlayer;

    public GameObject LeaveScren;

    public GameObject KillGotKilledFeedBox;

    public GameObject winScreen;
    public Text wintext;
    private float TimeHealthSpawn = 40f;
    private float bombTimer = 40f;
    


    void Awake()
    {
        instance = this;
        canvas.SetActive(true);

        SpawnHealthPickUp();
        SpawnBomb();
    }

    void Update()
    {

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ToggleLeaveScreen();
        }

        if (startRespawn)
        {
            StartRespawn();
        }
        pingrate.text = "NetworkPing: " + PhotonNetwork.GetPing();

        SpawnHealthTimer();
        SpawnBombTimer();
        
    }

    public void StartRespawn()
    {
        TimeAmount -= Time.deltaTime;
        spawnTimer.text = "Respawn in: " + TimeAmount.ToString("F0");

        if (TimeAmount <= 0)
        {
            respawnUI.SetActive(false);
            startRespawn = false;
            PlayerRelocation();
            LocalPlayer.GetComponent<Health>().EnableInputs();
            LocalPlayer.GetComponent<PhotonView>().RPC("Revive", RpcTarget.AllBuffered);
        }
    }

    public void ToggleLeaveScreen()
    {

        if (LeaveScren.activeSelf)
        {
            LeaveScren.SetActive(false);
        }
        else
        {
            LeaveScren.SetActive(true);
        }

    }

    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
        PhotonNetwork.LoadLevel(0);
    }


    public void PlayerRelocation()
    {
        float randomX = Random.Range(-20, 20);
        float randomY = Random.Range(-10, 10);
        LocalPlayer.transform.localPosition = new Vector2(randomX, randomY);
    }

   public void EnableRespawn()
    {
        TimeAmount = 5;
        startRespawn = true;
        respawnUI.SetActive(true);
    }


    public void SpawnPlayer()
    {
        float randomValue = Random.Range(-20, 20);
        float randomValue2 = Random.Range(-10, 10);
        //PhotonNetwork.Instantiate(playerPrefab.name, new Vector2(playerPrefab.transform.position.x * randomValue, playerPrefab.transform.position.y * randomValue), Quaternion.identity, 0);
        PhotonNetwork.Instantiate(playerPrefab.name, new Vector2(randomValue, randomValue2), Quaternion.identity, 0);
        canvas.SetActive(false);

    }

    public void SpawnHealthPickUp()
    {
        float randomValue = Random.Range(-20, 20);
        float randomValue2 = Random.Range(-10, 10);
        PhotonNetwork.Instantiate(healthBoostPrefab.name, new Vector2(randomValue, randomValue2), Quaternion.identity, 0);

    }

    public void SpawnHealthTimer()
    {
        TimeHealthSpawn -= Time.deltaTime;
        if (TimeHealthSpawn <= 0)
        {
            SpawnHealthPickUp();
            TimeHealthSpawn = 40f;

        }
    }


    public void SpawnBomb()
    {
        float randomValue = Random.Range(-20, 20);
        float randomValue2 = Random.Range(-10, 10);
        PhotonNetwork.Instantiate(bombPrefab.name, new Vector2(randomValue, randomValue2), Quaternion.identity, 0);

    }

    public void SpawnBombTimer()
    {
        bombTimer -= Time.deltaTime;
        if (bombTimer <= 0)
        {
            SpawnBomb();
            bombTimer = 40f;

        }
    }



    public void WinScreen(string name)
    {
        winScreen.SetActive(true);
        wintext.text = name;

    }

 
}
