using Photon.Pun;
using TMPro;
using UnityEngine;

public class ChangeCursor : MonoBehaviourPun
{
    //텍스트 매쉬 프로를 사용하여 플레이어 닉네임을 넣는다.
    public TextMeshProUGUI playerNameText;

    private Color color;
    private int colorIndex;

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
            colorIndex = LobbyManager.instance.colorsIndex.Dequeue();
            photonView.RPC(nameof(SetPlayerColor), RpcTarget.AllBuffered, colorIndex);
        }
    }

    private void Update()
    {
        //내가 아니면 이 함수를 나간다.
        if (photonView.IsMine == false) return;
        //내꺼만 커서를 활성화 하고 실행한다.
        InitCursorPos();
        CursorEnable();
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

    public void CursorEnable()
    {
        //내것의 플레이어 일 때
        if (Input.GetKeyDown(KeyCode.Q))
        {
            //플레이어가 처음에 들어왔을 때 Q를 누르는 것을 방지한다.
            if (LobbyManager.instance.rPlayer == null) return;
            //커서 비활성화를 모든 컴퓨터에 동기화 한다..
            photonView.RPC("CursorDisable", RpcTarget.All, true);

            //플레이어를 비활성화 시킨다.
            LobbyManager.instance.SetActivePlayer(false);
        }
    }

    [PunRPC]
    public void CursorDisable(bool cursorDisable)
    {
        transform.GetChild(0).gameObject.SetActive(cursorDisable);
        //커서 포톤 뷰 자식의 자식의 2번째 애니메이터 컴포넌트를 가져온다.
        Animator animator = transform.GetChild(1).GetComponent<Animator>();
        animator.SetTrigger("explosion");
        //내가 아니라면
        if (photonView.IsMine)
        {
            //커서가 비활성화 되어있다면
            if (cursorDisable)
            {
                //커서 잠금을 끈다.
                Cursor.lockState = CursorLockMode.Confined;
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
        color = LobbyManager.instance.nickNameColors[index];
        playerNameText.color = color;

        PlayerData.instance.AddPlayerColor(photonView.Owner.ActorNumber, color);
    }

    private void OnApplicationFocus(bool focus)
    {
        Cursor.visible = !focus;
    }
}