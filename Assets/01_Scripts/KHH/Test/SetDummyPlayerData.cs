using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetDummyPlayerData : MonoBehaviourPun
{
    public Character[] characters;

    bool check = false;
    int charater = 0;

    // Start is called before the first frame update
    void Start()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
    }

    // Update is called once per frame
    void Update()
    {
        Room room = PhotonNetwork.CurrentRoom;
        if (room.PlayerCount == 2 && !check)
        {
            check = true;

            foreach(var player in PhotonNetwork.CurrentRoom.Players)
            {
                PlayerData.instance.AddPlayer(player.Value.ActorNumber);
                PlayerData.instance.SelectCharacter(player.Value.ActorNumber, characters[charater]);
                charater++;
            }

            //if (photonView.Owner.IsMasterClient)
            //{
            //    StartCoroutine(enumerator());
            //}
        }
    }

    //IEnumerator enumerator()
    //{
    //    yield return new WaitForSeconds(3f);
    //    PhotonNetwork.LoadLevel("KHH_Test");
    //}
}
