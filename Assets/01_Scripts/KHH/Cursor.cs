using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cursor : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        float h = Input.GetAxisRaw("Horizontal"); // -1, 0, 1
        float v = Input.GetAxisRaw("Vertical");   // -1, 0, 1

        Vector3 dir = new Vector3(h, v, 0);
        dir.Normalize();

        transform.position += dir * Time.deltaTime * 50;
    }
}
