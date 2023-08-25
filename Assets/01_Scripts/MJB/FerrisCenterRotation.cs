using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FerrisCenterRotation : ObjectScript
{
    //회전 속도
    public float speed = 90f;

    public override void ResetObject()
    {
        transform.rotation = Quaternion.identity;
    }

    private void Update()
    {
        if (!active) return;
        //나를 z축으로 회전시킨다.
        transform.Rotate(0, 0, speed * Time.deltaTime);
    }
}