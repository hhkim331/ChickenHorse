﻿using Photon.Pun;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;

//해당 플레이어를 클릭했을 때 권한 요청을 수행하여 움직이게 하고 싶다.
public class OwnershipTransfer : MonoBehaviourPun
{
    public Character characterData;
    public bool hasPlayer;

    private Transform characterText;

    public RectTransform rectTransform;

    public TextMeshProUGUI textMeshProUGUI;

    private bool changePlayer = false;
    private bool newHasPlayer = false;
    private int curOwnerNum = -1;

    private void Start()
    {
        curOwnerNum = PlayerData.instance.GetCurCharacterPlayer(characterData.characterType);
        if (curOwnerNum != -1)
        {
            if (PhotonNetwork.LocalPlayer.ActorNumber == curOwnerNum)
            {
                photonView.TransferOwnership(PhotonNetwork.CurrentRoom.GetPlayer(curOwnerNum));
                LobbyManager.instance.rPlayer = GetComponent<RPlayer>();
                changePlayer = true;
                newHasPlayer = true;
            }
        }

        //curOwnerNum = photonView.Owner.ActorNumber;
    }

    private void Update()
    {
        if (changePlayer && photonView.Owner.ActorNumber == curOwnerNum)
        {
            changePlayer = false;
            GetComponent<Rigidbody>().isKinematic = false;
            LobbyManager.instance.cinemachineVirtualCamera.Follow = transform;
            photonView.RPC("CheckHasPlayer", RpcTarget.AllBuffered, curOwnerNum, photonView.Owner.NickName, newHasPlayer, PlayerData.instance.PlayerColorDic[curOwnerNum]);
        }

        //rect localSacle을 나의 localsacle로 변경한다.
        rectTransform.localScale = transform.localScale;
    }

    public void OnClick()
    {
        //이미 플레이어가 선택이 되어있으면 선택되지 않게 함수를 나간다.
        if (hasPlayer == true) return;
        if (LobbyManager.instance.rPlayer != null) return;

        //내가 클릭한 플레이어의 움직임을 활성화 한다.
        LobbyManager.instance.SelectPlayer(photonView);
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
            photonView.RPC(nameof(CheckHasPlayer), RpcTarget.AllBuffered, photonView.Owner.ActorNumber, photonView.Owner.NickName, false, LobbyManager.instance.myColorIndex);
        }
    }

    //플레이어를 선택했는지 모든 컴퓨터에 동기화
    [PunRPC]
    public void CheckHasPlayer(int actorNum, string actorNick, bool has, int colorIndex)
    {
        hasPlayer = has;
        //들어온 플레이어의 player를 가져갔는지 동기화한다.
        GetComponent<RPlayer>().enabled = has;
        //플레이어의 4번째 child의 canvas를 킨다.
        photonView.transform.GetChild(3).gameObject.SetActive(has);
        textMeshProUGUI.text = actorNick;
        textMeshProUGUI.color = PlayerData.instance.nickNameColors[colorIndex];

        if (has)
        {
            PlayerData.instance.AddPlayer(actorNum);
            PlayerData.instance.SelectCharacter(actorNum, characterData);
        }
        else
        {
            PlayerData.instance.UnSelectCharacter(actorNum);
            GetComponent<Rigidbody>().isKinematic = true;
        }
    }
}