using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KHHPlayerMain : MonoBehaviourPun
{
    public bool isActive = true;
    public bool IsActive { get { return isActive; } }
    bool isGoal = false;
    public bool IsGoal { get { return isGoal; } }
    [SerializeField] RPlayer rPlayer;
    [SerializeField] Animator animator;
    [SerializeField] SpriteRenderer[] spriteRenderers;

    bool rp = false;
    bool ani = false;

    private void Start()
    {
        if (photonView.IsMine == false)
        {
            GetComponent<Rigidbody>().isKinematic = true;
        }
    }

    public void Active(bool active, Vector3 pos)
    {
        if (photonView.IsMine)
        {
            if (active)
            {
                isActive = true;
                isGoal = false;
                animator.enabled = true;
                foreach (var spriteRenderer in spriteRenderers)
                {
                    spriteRenderer.color = Color.white;
                }
                GetComponent<Rigidbody>().velocity = Vector3.zero;
            }
            else
            {
                animator.enabled = false;
            }

            photonView.RPC(nameof(PMActRPC), RpcTarget.Others, active, pos);
        }
    }

    [PunRPC]
    void PMActRPC(bool active, Vector3 pos)
    {
        if (active)
        {
            isActive = true;
            isGoal = false;
            animator.enabled = true;
            foreach (var spriteRenderer in spriteRenderers)
            {
                spriteRenderer.color = Color.white;
            }
            GetComponent<Rigidbody>().velocity = Vector3.zero;
        }
        else
        {
            animator.enabled = false;
        }
    }

    public void ActiveMove()
    {
        photonView.RPC(nameof(PMAMRPC), RpcTarget.All);
    }

    [PunRPC]
    void PMAMRPC()
    {
        rPlayer.enabled = true;
    }

    public void Hit()
    {
        if (photonView.IsMine)
        photonView.RPC(nameof(PMHitRPC), RpcTarget.All);
    }

    [PunRPC]
    void PMHitRPC()
    {
        isActive = false;
        rPlayer.enabled = false;
        animator.enabled = false;
        foreach (var spriteRenderer in spriteRenderers)
        {
            spriteRenderer.color = Color.red;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (photonView.IsMine)
        {
            if (transform.position.y < -10)
            {
                photonView.RPC(nameof(PMDieRPC), RpcTarget.All);
            }
        }
        //else
        //{
        //    rPlayer.enabled = rp;
        //    animator.enabled = ani;
        //}
    }

    [PunRPC]
    void PMDieRPC()
    {
        isActive = false;
        rPlayer.enabled = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (photonView.IsMine == false) return;
        if (other.tag == "Goal")
        {
            photonView.RPC(nameof(PMGoalRPC), RpcTarget.All);
        }
    }

    [PunRPC]
    void PMGoalRPC()
    {
        if(isGoal) return;
        isActive = false;
        isGoal = true;
        rPlayer.enabled = false;
        MainGameManager.instance.ScoreMgr.GetScore(Point.PointType.Goal, photonView.Owner.ActorNumber);
    }
}
