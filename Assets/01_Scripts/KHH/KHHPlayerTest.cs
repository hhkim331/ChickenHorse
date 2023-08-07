using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KHHPlayerTest : MonoBehaviour
{
    public bool isDie = false;

    public void ResetPlayer()
    {
        isDie = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(transform.position.y < -10)
        {
            isDie = true;
        }
    }
}
