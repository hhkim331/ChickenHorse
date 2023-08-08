using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SunCoinReceiver : MonoBehaviour
{
    public void DestorySunCoin()
    {
        Destroy(transform.parent.gameObject);
    }
}