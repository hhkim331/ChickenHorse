using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//나의 부모, 부모, 부모의 z회전 값을 가져와서 나의 회전 값에 더해준다.
public class FerrisPlatform : MonoBehaviour
{
    private void Update()
    {
        transform.Rotate(0, 0, -transform.parent.parent.parent.GetComponent<FerrisCenterRotation>().speed * Time.deltaTime);
    }
}