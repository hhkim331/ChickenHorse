using Photon.Pun;
using TMPro;
using UnityEngine;

public class ChangeCursor : MonoBehaviourPun
{
    //텍스트 매쉬 프로를 사용하여 플레이어 닉네임을 넣는다.
    public TextMeshProUGUI playerNameText;

    //커서의 Text2D 이미지를 가져온다.
    public SpriteRenderer cursorImage;

    //explosion 색깔 변경을 위한 이미지를 가져온다.
    public SpriteRenderer explosionImage;

    private Color color;
    private int colorIndex = -1;

    private void Awake()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Confined;
        transform.GetChild(0).gameObject.SetActive(true);

        //내가 아니면
        if (photonView.IsMine)
        {
            //커서 포톤을 가진다.
            LobbyManager.instance.cursorPhotonView = photonView;

            photonView.RPC(nameof(SetPlayerName), RpcTarget.AllBuffered, photonView.Owner.NickName);
        }

        //내가 마스터 클라이언트라면 색깔을 정해준다.
        if (PhotonNetwork.LocalPlayer.IsMasterClient)
        {
            if (PlayerData.instance.PlayerColorDic.ContainsKey(photonView.Owner.ActorNumber))
            {
                colorIndex = PlayerData.instance.PlayerColorDic[photonView.Owner.ActorNumber];
                if (colorIndex == -1)
                    colorIndex = PlayerData.instance.colorsIndex.Dequeue();
                photonView.RPC(nameof(SetPlayerColor), RpcTarget.AllBuffered, colorIndex);
                photonView.RPC("SyncActive", RpcTarget.AllBuffered, false);
            }
            else
            {
                colorIndex = PlayerData.instance.colorsIndex.Dequeue();
                photonView.RPC(nameof(SetPlayerColor), RpcTarget.AllBuffered, colorIndex);
            }
        }
    }

    private void Update()
    {
        //내가 아니면 이 함수를 나간다.
        if (photonView.IsMine == false) return;
        //내꺼만 커서를 활성화 하고 실행한다.
        InitCursorPos();
        ActiveCursor();
    }

    private void InitCursorPos()
    {
        if (photonView.IsMine)
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            // 커서의 위치는 마우스 커서의 위치에서 플레이어의 이미지 커서 위치를 더한다.
            transform.position = mousePos;
        }
    }

    public void ActiveCursor()
    {
        //내것의 플레이어 일 때
        if (Input.GetKeyDown(KeyCode.Q))
        {
            //플레이어가 처음에 들어왔을 때 Q를 누르는 것을 방지한다.
            if (LobbyManager.instance.rPlayer == null) return;
            //커서 비활성화를 모든 컴퓨터에 동기화 한다..
            photonView.RPC(nameof(SyncActive), RpcTarget.AllBuffered, true);

            //플레이어를 비활성화 시킨다.
            LobbyManager.instance.SetActivePlayer(false);
        }
    }

    [PunRPC]
    public void SyncActive(bool isActive)
    {
        transform.GetChild(0).gameObject.SetActive(isActive);
        //커서 포톤 뷰 자식의 자식의 2번째 애니메이터 컴포넌트를 가져온다.
        Animator animator = transform.GetChild(1).GetComponent<Animator>();

        //내가 아니라면
        if (photonView.IsMine)
        {
            //커서가 활성화 되어있다면
            if (isActive)
            {
                //커서 잠금을 끈다.
                Cursor.lockState = CursorLockMode.Confined;
            }
            else
            {
                animator.SetTrigger("explosion");
                SoundManager.Instance.PlaySFX("explosion");
            }
        }
    }

    //캐릭터 닉네임을 동기화 하자
    [PunRPC]
    public void SetPlayerName(string playerName)
    {
        playerNameText.text = playerName;
    }

    [PunRPC]
    public void SetPlayerColor(int index)
    {
        if (photonView.IsMine)
        {
            LobbyManager.instance.myColorIndex = index;
        }

        colorIndex = index;
        color = PlayerData.instance.nickNameColors[index];
        playerNameText.color = color;
        //커서 텍스쳐 색깔을 변경한다.
        cursorImage.color = color;
        //폭발 이미지 색깔을 변경한다.
        explosionImage.color = color;

        PlayerData.instance.AddPlayerColor(photonView.Owner.ActorNumber, index);
    }

    private void OnApplicationFocus(bool focus)
    {
        Cursor.visible = !focus;
    }
}