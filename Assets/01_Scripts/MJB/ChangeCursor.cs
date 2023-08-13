using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeCursor : MonoBehaviour
{
    //커서 이미지를 가져온다.
    [SerializeField] private Texture2D cursorImage;

    public GameObject player;

    private void Awake()
    {
        //커서를 게임 뷰에 나가지 못하게 한다.
        Cursor.lockState = CursorLockMode.Confined;
    }

    //시작하기 전에 커서 이미지를 가져와서 변경한다.
    private void Start()
    {
        Vector2 cursorPosition = new Vector2(cursorImage.width / 2, cursorImage.height / 2);
        Cursor.SetCursor(cursorImage, cursorPosition, CursorMode.ForceSoftware);
    }

    private void Update()
    {
        CursorEnable();
    }

    public void CursorDisable()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void CursorEnable()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            //플레이어의 컨트롤러를 끈다. // 플레이어의 컨트롤러 작업
            player.GetComponent<TestController>().enabled = false;
            Cursor.visible = true;
            //커서를 게임 뷰에 나가지 못하게 한다.
            Cursor.lockState = CursorLockMode.Confined;
        }
    }
}