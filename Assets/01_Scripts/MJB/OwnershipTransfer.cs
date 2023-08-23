using Photon.Pun;
using Photon.Realtime;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

//해당 플레이어를 클릭했을 때 권한 요청을 수행하여 움직이게 하고 싶다.
public class OwnershipTransfer : MonoBehaviourPun
{
    public Character characterData;
    public bool hasPlayer;

    bool changePlayer = false;
    bool newHasPlayer = false;
    int curOwnerNum = 0;

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
    }

    public void OnClick()
    {
        //이미 플레이어가 선택이 되어있으면 선택되지 않게 함수를 나간다.
        if (hasPlayer == true) return;

        //내가 클릭한 플레이어의 움직임을 활성화 한다.
        LobbyManager.instance.SelectPlayer(photonView);

        GetComponent<Rigidbody>().isKinematic = false;
    }
    public void HasPlayer(bool has, int ownerNum)
    {
        changePlayer = true;
        newHasPlayer = has;
        curOwnerNum = ownerNum;
    }

    //플레이어를 선택했는지 모든 컴퓨터에 동기화
    [PunRPC]
    public void CheckHasPlayer(bool has)
    {
        hasPlayer = has;
        if (has)
        {
            PlayerData.instance.AddPlayer(photonView.Owner.ActorNumber);
            PlayerData.instance.SelectCharacter(photonView.Owner.ActorNumber, characterData);
        }
        else
        {
            //PlayerData.instance.RemovePlayer(photonView.Owner.ActorNumber);
            PlayerData.instance.UnSelectCharacter(photonView.Owner.ActorNumber);

            GetComponent<Rigidbody>().isKinematic = true;
        }
    }
}