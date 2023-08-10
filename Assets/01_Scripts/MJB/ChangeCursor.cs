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
        Cursor.SetCursor(cursorImage, Vector2.zero, CursorMode.ForceSoftware);
    }
}