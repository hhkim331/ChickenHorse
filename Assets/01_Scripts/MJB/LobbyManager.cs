using Cinemachine;
using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyManager : MonoBehaviour
{
    public static LobbyManager instance;

    //나의 커서 photonView 담을 변수
    public PhotonView cursorPhotonView;

    public CinemachineVirtualCamera cinemachineVirtualCamera;

    public RPlayer rPlayer;

    public int myColorIndex;

    private void Awake()
    {
        instance = this;

        //컬러 범위를 byte로 random 값을 준다.
        SoundManager.Instance.PlayBGM("Room");
    }

    //해당 플레이어 선택
    public void SelectPlayer(PhotonView playerPhoton)
    {
        //선택한 플레이어의 포톤을 커서 포톤으로 넘긴다.
        playerPhoton.TransferOwnership(cursorPhotonView.Owner);
        //플레이어가 선택되면 커서 비활성화를 동기화한다.
        cursorPhotonView.RPC("SyncActive", RpcTarget.AllBuffered, false);
        //카메라 타깃 변경
        rPlayer = playerPhoton.GetComponent<RPlayer>();

        SetActivePlayer(true);
    }

    public void SetActivePlayer(bool isEnabled)
    {
        //플레이어를 가졌다라는 것을 알린다.
        rPlayer.GetComponent<OwnershipTransfer>().HasPlayer(isEnabled, cursorPhotonView.Owner.ActorNumber);
    }
}