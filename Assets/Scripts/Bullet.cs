using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class Bullet : MonoBehaviourPun
{
    public float MoveSpeed = 40f;
    public float DestroyTime = 2f;
    public GameObject localPlayerObj;
    public GameObject hitEffect;
    public float bulletDamage = 0.10f;

    public string killerName;

     void Start()
    {
        if (photonView.IsMine)
        {
            killerName = localPlayerObj.GetComponent<Robot>().myName;
        }
    }





    void Update()
    {
        /*
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        rb.AddForce(bulletSpawnPoint.up * bulletForce, ForceMode2D.Impulse);*/
        transform.Translate(Vector2.up * MoveSpeed * Time.deltaTime);
    }


    IEnumerator destroyBullet()
    {
        yield return new WaitForSeconds(DestroyTime);
        this.GetComponent<PhotonView>().RPC("Destroy", RpcTarget.AllBuffered);

    }

    [PunRPC]

    void Destroy()
    {
        Destroy(this.gameObject);
    }


     void OnTriggerEnter2D(Collider2D collision)
    {


        if (collision.gameObject.tag == "Wall")
        {
            
            GameObject effect = Instantiate(hitEffect, transform.position, Quaternion.identity);
            Destroy(effect, 1f); 
            this.GetComponent<PhotonView>().RPC("Destroy", RpcTarget.AllBuffered);
        } 
    

        if (!photonView.IsMine)
        {
            return;
        }

        PhotonView target = collision.gameObject.GetComponent<PhotonView>();


        if (target != null && (!target.IsMine || target.IsSceneView))
        {
            if (target.tag == "Player")
            {
                GameObject effect = Instantiate(hitEffect, transform.position, Quaternion.identity);
                Destroy(effect, 1f);
                target.RPC("HealthUpdate", RpcTarget.AllBuffered,bulletDamage);

                if (target.GetComponent<Health>().health <= 0)
                {
                    this.GetComponent<PhotonView>().RPC("Destroy", RpcTarget.AllBuffered);
                    Player Gotkilled = target.Owner;
                    target.RPC("deathCounter", Gotkilled);
                    //targer.Owner.NickName
                    target.RPC("KillCounter", localPlayerObj.GetComponent<PhotonView>().Owner, killerName);
                }
            }

            
            this.GetComponent<PhotonView>().RPC("Destroy", RpcTarget.AllBuffered);

        }
    }
}
