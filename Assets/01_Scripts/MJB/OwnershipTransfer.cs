using Cinemachine;
using Photon.Pun;
using UnityEngine;

//해당 플레이어를 클릭했을 때 권한 요청을 수행하여 움직이게 하고 싶다.
public class OwnershipTransfer : MonoBehaviourPun
{
    public bool hasPlayer;

    public void OnClick()
    {
        //이미 플레이어가 선택이 되어있으면 선택되지 않게 함수를 나간다.
        if (hasPlayer == true) return;

        //내가 클릭한 플레이어의 움직임을 활성화 한다.
        LobbyManager.instance.SelectPlayer(photonView);

    }

    //플레이어를 선택했는지 모든 컴퓨터에 동기화
    [PunRPC]
    public void CheckHasPlayer(bool has)
    {
        hasPlayer = has;
    }
}