using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//나의 부모, 부모, 부모의 z회전 값을 가져와서 나의 회전 값에 더해준다.
//플레이어가 triggerenter 되었을 때 나의 부모의 부모로 놓고, exit 되었을 때 부모를 null한다.
public class FerrisPlatform : MonoBehaviour
{
    private Transform player;

    private void Update()
    {
        transform.Rotate(0, 0, -transform.parent.parent.parent.GetComponent<FerrisCenterRotation>().speed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.gameObject.transform.parent.SetParent(transform);
            player = other.gameObject.transform.parent;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.gameObject.transform.parent.SetParent(null);
            player = null;
        }
    }
}