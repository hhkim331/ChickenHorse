using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class KHHPlayerMain : MonoBehaviourPun
{
    public bool isActive = false;
    public bool IsActive { get { return isActive; } }
    bool isGoal = false;
    public bool IsGoal { get { return isGoal; } }
    [SerializeField] RPlayer rPlayer;
    [SerializeField] Animator animator;
    //[SerializeField] SpriteRenderer spriteRenderer;

    bool rp = false;
    bool ani = false;

    [SerializeField] Transform canvasTransform;
    [SerializeField] RectTransform nameBackBoardRT;
    [SerializeField] TextMeshProUGUI nameText;

    private void Start()
    {
        if (photonView.IsMine == false)
        {
            GetComponent<Rigidbody>().isKinematic = true;
        }

        nameText.text = photonView.Owner.NickName;
        nameBackBoardRT.sizeDelta = new Vector2(nameText.preferredWidth, nameText.preferredHeight);
    }

    public void Active(bool active, Vector3 pos)
    {
        if (photonView.IsMine)
        {
            if (active)
            {
                isActive = true;
                isGoal = false;
                animator.Rebind();
                animator.enabled = true;
                GetComponent<Rigidbody>().velocity = Vector3.zero;
                transform.position = pos;
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
            transform.position = pos;
            animator.Rebind();
            animator.enabled = true;
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
        if (isActive == false) return;

        if (photonView.IsMine)
            photonView.RPC(nameof(PMHitRPC), RpcTarget.All);
    }

    [PunRPC]
    void PMHitRPC()
    {
        isActive = false;
        rPlayer.enabled = false;
        //animator.enabled = false;

        animator.SetTrigger("Dead");
    }

    // Update is called once per frame
    void Update()
    {
        if (photonView.IsMine)
        {
            if (!isActive) return;

            if (transform.position.y < -10)
            {
                if (isActive) photonView.RPC(nameof(PMDieRPC), RpcTarget.All);
            }
        }

        canvasTransform.localScale = transform.localScale;
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
        animator.SetTrigger("Dead");
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
        if (isGoal) return;
        isActive = false;
        isGoal = true;
        rPlayer.enabled = false;
        MainGameManager.instance.ScoreMgr.GetScore(Point.PointType.Goal, photonView.Owner.ActorNumber);

        animator.SetTrigger("Goal");
    }
}
