using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;

public class Health : MonoBehaviourPun
{
    public Image fillImage;
    public float health = 1;
    public Rigidbody2D rb;
    public SpriteRenderer sr;
    public BoxCollider2D collider;
    public GameObject playerCanvas;
    public Robot playerScript;
    [HideInInspector]
    public static Health refHealth = null;
    [HideInInspector]
    public float sum = 0f;
    public GameObject KillGotKilledText;
  

    private float Timex = 10f;
    public bool StartLoadScene;




    void Awake()
    {
        refHealth = this;
    
    }

    void Update()
    {
        if (StartLoadScene)
        {
            startReturningLobby();
        }

        Debug.Log("Sum from update function: "+sum);
      
    }




    public void startReturningLobby()
    {
        Timex -= Time.deltaTime;

        if (Timex <= 0)
        {

            StartLoadScene = false;

            this.GetComponent<PhotonView>().RPC("Endgame", RpcTarget.AllBuffered);


        }
    }




    public void CheckHealth()
    {
        if (photonView.IsMine && health <= 0)
        {
            GameManager.instance.EnableRespawn();
            playerScript.DisableInputs = true;
            this.GetComponent<PhotonView>().RPC("death", RpcTarget.AllBuffered);
        }
    }


    public void EnableInputs()
    {
        playerScript.DisableInputs = false;
    }

    [PunRPC]
    public void death()
    {
        rb.gravityScale = 0;
        collider.enabled = false; ;
        sr.enabled = false;
        playerCanvas.SetActive(false);
    }

    [PunRPC]
    public void Revive()
    {
        rb.gravityScale = 0;
        collider.enabled = true; ;
        sr.enabled = true;
        playerCanvas.SetActive(true);
        fillImage.fillAmount = 1;
        health = 1;
    }


    [PunRPC]
    public void HealthUpdate(float damage)
    {

        fillImage.fillAmount -= damage;
        health = fillImage.fillAmount;
        if (health == 0)
        {
            rb.gravityScale = 0;
            collider.enabled = false;
            sr.enabled = false;
            playerCanvas.SetActive(false);
        }

        CheckHealth();
    }

    [PunRPC]
    public void HealthBoost(float health2)
    {
        if (health < 1) {
           
                fillImage.fillAmount += health2;
                health = fillImage.fillAmount;
       
        }
    }

    [PunRPC]
    void Endgame()
    {
        PhotonNetwork.LeaveRoom();
        PhotonNetwork.LoadLevel(0);
    }



    [PunRPC]
    void callWin(string name)
    {
        GameManager.instance.WinScreen(name);
    }


    [PunRPC]
    public void EnableLobby(bool check)
    {
        Timex = 10;
        StartLoadScene = check;
    }

    [PunRPC]
    void KillCounter(string name)
    {
        float x = 1f;
        sum = sum + x;



        GameObject go = Instantiate(KillGotKilledText, new Vector2(0, 0), Quaternion.identity);
        go.transform.SetParent(GameManager.instance.KillGotKilledFeedBox.transform, false);

        go.GetComponent<Text>().text = "Kill: " + sum;
        Destroy(go, 7);




        if (sum == 10)
        {
            this.GetComponent<PhotonView>().RPC("EnableLobby", RpcTarget.AllBuffered, true);
            this.GetComponent<PhotonView>().RPC("callWin", RpcTarget.AllBuffered, name);

        }
    }

    [PunRPC]
    void deathCounter()
    {

    }

}
