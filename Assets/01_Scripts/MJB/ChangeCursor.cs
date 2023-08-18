using Photon.Pun;
using UnityEngine;

public class ChangeCursor : MonoBehaviourPun
{
    private void Awake()
    {
        Cursor.lockState = CursorLockMode.Confined;
        transform.GetChild(0).gameObject.SetActive(true);

        //내가 아니면
        if (photonView.IsMine)
        {
            //커서 포톤을 가진다.
            LobbyManager.instance.cursorPhotonView = photonView;
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
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            //커서 비활성화를 모든 컴퓨터에 동기화 한다..
            photonView.RPC("CursorDisable", RpcTarget.All, true);
            LobbyManager.instance.SetActivePlayer(false);
        }
    }

    [PunRPC]
    public void CursorDisable(bool cursorDisable)
    {
        transform.GetChild(0).gameObject.SetActive(cursorDisable);
        //내가 아니라면
        if (photonView.IsMine)
        {
            //커서가 비활성화 되어있다면
            if (cursorDisable)
            {
                //커서 잠금을 끈다.
                Cursor.lockState = CursorLockMode.Confined;
            }
            //커서가 비활성화 되어있지 않다면
            else
            {
                //커서 잠금을 한다.
                Cursor.lockState = CursorLockMode.Locked;
            }
        }
    }
}