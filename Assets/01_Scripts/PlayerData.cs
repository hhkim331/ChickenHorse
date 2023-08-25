﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerData : MonoBehaviour
{
    public static PlayerData instance;

    //플레이어가 선택한 캐릭터 정보
    //string : 플레이어 활동 번호
    //Character : 캐릭터 정보
    Dictionary<int, Character> playerCharacterDic = new Dictionary<int, Character>();
    public Dictionary<int, Character> PlayerCharacterDic { get { return playerCharacterDic; } }
    Dictionary<int, Color> playerColorDic = new Dictionary<int, Color>();
    public Dictionary<int, Color> PlayerColorDic { get { return playerColorDic; } }

    //플레이어 색상 정보
    //Dictionary<int, Color> playerColorDic = new Dictionary<int, Color>();

    //싱글톤
    public void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    //// Start is called before the first frame update
    //void Start()
    //{

    //}

    //// Update is called once per frame
    //void Update()
    //{

    //}

    public void AddPlayer(int actorNum)
    {
        Debug.Log("AddPlayer:" + actorNum);
        playerCharacterDic[actorNum] = null;
    }

    public void AddPlayerColor(int actorNum, Color color)
    {
        Debug.Log("AddPlayerColor:" + actorNum);
        playerColorDic[actorNum] = color;
    }

    //public void RemovePlayer(int actorNum)
    //{
    //    Debug.Log("RemovePlayer");
    //    playerCharacterDic.Remove(actorNum);
    //}

    public void SelectCharacter(int actorNum, Character character)
    {
        playerCharacterDic[actorNum] = character;
    }

    public void UnSelectCharacter(int actorNum)
    {
        playerCharacterDic[actorNum] = null;
    }
}
