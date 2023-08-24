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
    [SerializeField] Animator explosionAni;
    //[SerializeField] SpriteRenderer spriteRenderer;

    //bool rp = false;
    //bool ani = false;

    [SerializeField] Transform canvasTransform;
    [SerializeField] RectTransform nameBackBoardRT;
    [SerializeField] TextMeshProUGUI nameText;

    private void Start()
    {
        nameText.text = photonView.Owner.NickName;
        nameBackBoardRT.sizeDelta = new Vector2(nameText.preferredWidth, nameText.preferredHeight);

        if (photonView.IsMine)
            animator.transform.localPosition = new Vector3(0, -0.95f, -0.1f);
        else
            animator.transform.localPosition = new Vector3(0, -0.95f, -photonView.Owner.ActorNumber * 0.001f);
    }

    public void Active(bool active, Vector3 pos)
    {
        if (photonView.IsMine)
        {
            if (active)
            {
                isActive = true;
                isGoal = false;
                animator.gameObject.SetActive(true);
                animator.Rebind();
                animator.enabled = true;
                canvasTransform.gameObject.SetActive(true);
                GetComponent<Rigidbody>().isKinematic = false;
                transform.position = pos;
                explosionAni.SetTrigger("explosion");
            }
            else
            {
                animator.enabled = false;
                animator.gameObject.SetActive(false);
                canvasTransform.gameObject.SetActive(false);
                GetComponent<Rigidbody>().isKinematic = true;
                transform.position = pos;
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
            animator.gameObject.SetActive(true);
            animator.Rebind();
            animator.enabled = true;
            canvasTransform.gameObject.SetActive(true);
            GetComponent<Rigidbody>().velocity = Vector3.zero;
            explosionAni.SetTrigger("explosion");
        }
        else
        {
            animator.enabled = false;
            animator.gameObject.SetActive(false);
            canvasTransform.gameObject.SetActive(false);
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
        isActive = false;
        if (photonView.IsMine)
            photonView.RPC(nameof(PMDieRPC), RpcTarget.All);
    }

    // Update is called once per frame
    void Update()
    {
        if (photonView.IsMine)
        {
            if (!isActive) return;

            if (transform.position.y < -10)
            {
                isActive = false;
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
        MainGameManager.instance.AnyPlayerDie();
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
