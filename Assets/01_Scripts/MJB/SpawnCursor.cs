using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnCursor : MonoBehaviour
{
    //플레이어 커서 오브젝트를 가져온다.
    public GameObject playerCursor;

    private void Start()
    {
        //오브젝트를 인스턴스화 한다.
        PhotonNetwork.Instantiate(playerCursor.name, Vector3.zero, Quaternion.identity);
    }
}