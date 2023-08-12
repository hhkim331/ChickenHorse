using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeCursor : MonoBehaviour
{
    //커서 이미지를 가져온다.
    [SerializeField] private Texture2D cursorImage;

    public GameObject player;

    //시작할때 커서 이미지를 가져와서 변경한다.
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
            Cursor.lockState = CursorLockMode.None;
        }
    }
}