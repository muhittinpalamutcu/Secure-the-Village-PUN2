using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
public class HealthPickUp : MonoBehaviourPun
{


    public float healthBonus = 0.2f;

    private void Awake()
    {
        
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        /*
        if(col.gameObject.tag == "Player") { 
        this.GetComponent<PhotonView>().RPC("HealthBoost", RpcTarget.AllBuffered, healthBonus);
        this.GetComponent<PhotonView>().RPC("DestroyBonusHealth", RpcTarget.AllBuffered);
        }*/

        if (col.gameObject.tag == "Player" || col.gameObject.tag=="Wall")
        {
            DestroyBonusHealth();
        }
       
        PhotonView target = col.gameObject.GetComponent<PhotonView>();


        if (target != null && (!target.IsMine || target.IsSceneView))
        {
            if (target.tag == "Player")
            {
                target.RPC("HealthBoost", RpcTarget.AllBuffered, healthBonus);
            }


        }
        

    }

    [PunRPC]
    void DestroyBonusHealth()
    {
        Destroy(this.gameObject);
    }


}
