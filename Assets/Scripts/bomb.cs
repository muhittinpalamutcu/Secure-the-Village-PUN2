using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class bomb : MonoBehaviourPun
{

    public float damage = 0.10f;
    public GameObject hitEffect;

    void OnTriggerEnter2D(Collider2D col)
    {
        /*
        if(col.gameObject.tag == "Player") { 
        this.GetComponent<PhotonView>().RPC("HealthBoost", RpcTarget.AllBuffered, healthBonus);
        this.GetComponent<PhotonView>().RPC("DestroyBonusHealth", RpcTarget.AllBuffered);
        }*/


        if (col.gameObject.tag == "Player" || col.gameObject.tag == "Wall")
        {
            DestroyBomb();
        }


      

        PhotonView target = col.gameObject.GetComponent<PhotonView>();


        if (target != null && (!target.IsMine || target.IsSceneView))
        {
            if (target.tag == "Player")
            {
                GameObject effect = Instantiate(hitEffect, transform.position, Quaternion.identity);
                Destroy(effect, 1f);
                target.RPC("HealthUpdate", RpcTarget.AllBuffered, damage);
            }


        }


    }

    [PunRPC]
    void DestroyBomb()
    {
        Destroy(this.gameObject);
    }


}
