using DG.Tweening.Core;
using Photon.Pun;
using UnityEngine;

//
public class ChangeCursor : MonoBehaviourPun
{
    public GameObject player;

    private void Awake()
    {
        Cursor.lockState = CursorLockMode.Confined;
        transform.GetChild(0).gameObject.SetActive(true);
    }

    private void Update()
    {
        InitCursorPos();
        CursorEnable();
    }

    private void InitCursorPos()
    {
        //마우스 커서의 위치 x, y 값을 가져온다
        if (photonView.IsMine)
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            // 커서의 위치는 마우스 커서의 위치에서 플레이어의 이미지 커서 위치를 더한다.
            transform.position = mousePos;
        }
    }

    public void CursorDisable()
    {
        //나 자신을 끈다.
        transform.GetChild(0).gameObject.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void CursorEnable()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            //플레이어의 컨트롤러를 끈다. // 플레이어의 컨트롤러 작업
            //player.GetComponent<RPlayer>().enabled = false;
            //나 자신을 켠다.
            transform.GetChild(0).gameObject.SetActive(true);
            //커서를 게임 뷰에 나가지 못하게 한다.
            Cursor.lockState = CursorLockMode.Confined;
        }
    }
}