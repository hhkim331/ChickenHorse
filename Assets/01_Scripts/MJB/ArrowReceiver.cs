using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowReceiver : MonoBehaviour
{
    //화살을 삭제한다.
    public void DestroyArrow()
    {
        Destroy(transform.parent.parent.gameObject);
    }
}