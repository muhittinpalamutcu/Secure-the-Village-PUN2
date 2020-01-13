using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;

public class Robot : MonoBehaviourPun
{

    public float MoveSpeed = 5f;
    public Rigidbody2D rb;
    public SpriteRenderer sprite;
    public PhotonView photonview;
    public Camera _MainCamera;
    Vector2 movement;
    Vector2 mousePos;
    float angle;
    
    public GameObject bulletPreFab;
    public Transform bulletSpawnPoint;
    public Text PlayerName;
    // public static Robot reference = null;
    public bool DisableInputs=false;

    public string myName;


    // Start is called before the first frame update
    void Awake()
    {
       // reference = this;
        _MainCamera = FindObjectOfType<Camera>();
        

        
        if (photonView.IsMine)
        {
            GameManager.instance.LocalPlayer = this.gameObject;
            PlayerName.text ="You:"+ PhotonNetwork.NickName;
            PlayerName.color = Color.green;

            myName = PhotonNetwork.NickName;
        }
        else
        {
            PlayerName.text = photonview.Owner.NickName;
            PlayerName.color = Color.red;
        }
        

    }

    // Update is called once per frame
    void Update()
    {
        if (photonView.IsMine && !DisableInputs)
        {
            CheckInput();
            
        }
        
    }

     void FixedUpdate()
    {
        if (photonView.IsMine)
        {
            rb.MovePosition(rb.position + movement * MoveSpeed * Time.fixedDeltaTime);
            Vector2 lookDir = mousePos - rb.position;
            //Atan2 is function that returns the angle between x-axis and y axis it just take x and y and it calculate angle of x and y position
            angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg - 90f;
           // this.GetComponent<PhotonView>().RPC("rotate", RpcTarget.AllBuffered,angle);
            photonview.RPC("rotate", RpcTarget.AllBuffered,angle);
        }
       
    }


    public  void CheckInput()
    {
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        mousePos = _MainCamera.ScreenToWorldPoint(Input.mousePosition);
        
        if (Input.GetButtonDown("Fire1"))
        {
            shot();
        }

    }

    
    private void shot()
    {
        GameObject bullet=PhotonNetwork.Instantiate(bulletPreFab.name, bulletSpawnPoint.position, bulletSpawnPoint.rotation);
      //  PhotonNetwork.Instantiate(bulletPreFab.name, new Vector2(bulletSpawnPoint.position.x, bulletSpawnPoint.position.y), Quaternion.identity, 0);
       /* Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        rb.AddForce(bulletSpawnPoint.up * bulletForce, ForceMode2D.Impulse);*/
        bullet.GetComponent<Bullet>().localPlayerObj = this.gameObject;
    }
    
   
    [PunRPC]
    private void rotate(float angle)
    {
        rb.rotation = angle;
    }

   
   
    

}
