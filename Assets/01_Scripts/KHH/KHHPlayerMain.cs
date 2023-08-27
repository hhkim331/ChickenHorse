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

    Rigidbody rB;
    [SerializeField] Collider bodyCol;

    //bool rp = false;
    //bool ani = false;

    [SerializeField] Transform canvasTransform;
    [SerializeField] RectTransform nameBackBoardRT;
    [SerializeField] TextMeshProUGUI nameText;

    private void Awake()
    {
        rB = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        nameText.color = PlayerData.instance.GetCurPlayerColor(photonView.Owner.ActorNumber);
        nameText.text = photonView.Owner.NickName;
        nameBackBoardRT.sizeDelta = new Vector2(nameText.preferredWidth, nameText.preferredHeight);

        if (photonView.IsMine)
        {
            animator.transform.localPosition = new Vector3(0, -0.95f, -0.1f);
        }
        else
        {
            animator.transform.localPosition = new Vector3(0, -0.95f, -photonView.Owner.ActorNumber * 0.001f);
            rB.constraints = RigidbodyConstraints.FreezeAll;
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
                animator.gameObject.SetActive(true);
                animator.Rebind();
                animator.enabled = true;
                canvasTransform.gameObject.SetActive(true);
                rB.isKinematic = false;
                bodyCol.isTrigger = false;
                explosionAni.SetTrigger("explosion");

                foreach (Transform transform in GetComponentsInChildren<Transform>())
                    transform.gameObject.layer = LayerMask.NameToLayer("Player");
            }
            else
            {
                animator.enabled = false;
                animator.gameObject.SetActive(false);
                canvasTransform.gameObject.SetActive(false);
                rB.isKinematic = true;
            }
            transform.position = pos;

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
            animator.gameObject.SetActive(true);
            animator.Rebind();
            animator.enabled = true;
            canvasTransform.gameObject.SetActive(true);
            rB.isKinematic = false;
            bodyCol.isTrigger = false;
            explosionAni.SetTrigger("explosion");

            foreach (Transform transform in GetComponentsInChildren<Transform>())
                transform.gameObject.layer = LayerMask.NameToLayer("Player");
        }
        else
        {
            animator.enabled = false;
            animator.gameObject.SetActive(false);
            canvasTransform.gameObject.SetActive(false);
            rB.isKinematic = true;
        }
        transform.position = pos;
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

    public void Hit(int killerNum)
    {
        if (isActive == false) return;
        isActive = false;
        if (photonView.IsMine)
            photonView.RPC(nameof(PMDieRPC), RpcTarget.All, killerNum);
    }

    // Update is called once per frame
    void Update()
    {
        if (photonView.IsMine)
        {
            if (!isActive) return;

            if (transform.position.y < -10)
            {
                if (isActive) photonView.RPC(nameof(PMDieRPC), RpcTarget.All, -1);
                isActive = false;
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
    void PMDieRPC(int killerNum)
    {
        isActive = false;
        rPlayer.enabled = false;
        bodyCol.isTrigger = true;
        animator.SetTrigger("Dead");
        MainGameManager.instance.AnyPlayerDie();
        if (killerNum != -1 && photonView.Owner.ActorNumber != killerNum)
            MainGameManager.instance.ScoreMgr.AddScore(Point.PointType.Trap, killerNum);
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
        MainGameManager.instance.ScoreMgr.AddScore(Point.PointType.Goal, photonView.Owner.ActorNumber);

        animator.SetTrigger("Goal");
    }
}
