using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeCursor : MonoBehaviour
{
    //커서 이미지를 가져온다.
    [SerializeField] private Texture2D cursorImage;

    //시작할때 커서 이미지를 가져와서 변경한다.
    private void Start()
    {
        Vector2 cursorPosition = new Vector2(cursorImage.width / 2, cursorImage.height / 2);
        Cursor.SetCursor(cursorImage, cursorPosition, CursorMode.ForceSoftware);
    }
}