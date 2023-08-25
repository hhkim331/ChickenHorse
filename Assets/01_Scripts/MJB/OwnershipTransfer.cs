using Photon.Pun;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

//해당 플레이어를 클릭했을 때 권한 요청을 수행하여 움직이게 하고 싶다.
public class OwnershipTransfer : MonoBehaviourPun
{
    public Character characterData;
    public bool hasPlayer;

    private Transform characterText;

    public RectTransform rectTransform;

    private bool changePlayer = false;
    private bool newHasPlayer = false;
    private int curOwnerNum = 0;

    private void Start()
    {
        curOwnerNum = photonView.Owner.ActorNumber;
    }

    private void Update()
    {
        if (changePlayer && photonView.Owner.ActorNumber == curOwnerNum)
        {
            changePlayer = false;
            photonView.RPC("CheckHasPlayer", RpcTarget.AllBuffered, newHasPlayer);
        }

        //rect localSacle을 나의 localsacle로 변경한다.
        rectTransform.localScale = transform.localScale;
    }

    public void OnClick()
    {
        //이미 플레이어가 선택이 되어있으면 선택되지 않게 함수를 나간다.
        if (hasPlayer == true) return;

        //내가 클릭한 플레이어의 움직임을 활성화 한다.
        LobbyManager.instance.SelectPlayer(photonView);

        characterText = photonView.transform.GetChild(3);

        GetComponent<Rigidbody>().isKinematic = false;
    }

    public void HasPlayer(bool has, int ownerNum)
    {
        if (has)
        {
            changePlayer = true;
            newHasPlayer = has;
            curOwnerNum = ownerNum;
        }
        else
        {
            photonView.RPC(nameof(CheckHasPlayer), RpcTarget.All, false);
        }
    }

    //플레이어를 선택했는지 모든 컴퓨터에 동기화
    [PunRPC]
    public void CheckHasPlayer(bool has)
    {
        hasPlayer = has;
        //들어온 플레이어의 player를 가져갔는지 동기화한다.
        GetComponent<RPlayer>().enabled = has;
        //플레이어의 4번째 child의 canvas를 킨다.
        characterText.gameObject.SetActive(has);
        characterText.GetChild(1).GetComponent<TextMeshProUGUI>().text = photonView.Owner.NickName;

        if (has)
        {
            PlayerData.instance.AddPlayer(photonView.Owner.ActorNumber);
            PlayerData.instance.SelectCharacter(photonView.Owner.ActorNumber, characterData);
        }
        else
        {
            PlayerData.instance.UnSelectCharacter(photonView.Owner.ActorNumber);
            GetComponent<Rigidbody>().isKinematic = true;
        }
    }
}