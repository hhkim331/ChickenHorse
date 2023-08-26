using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerData : MonoBehaviour
{
    public static PlayerData instance;

    public Color[] nickNameColors;
    public Queue<int> colorsIndex = new Queue<int>();

    //플레이어가 선택한 캐릭터 정보
    //string : 플레이어 활동 번호
    //Character : 캐릭터 정보
    Dictionary<int, Character> playerCharacterDic = new Dictionary<int, Character>();
    public Dictionary<int, Character> PlayerCharacterDic { get { return playerCharacterDic; } }
    Dictionary<int, int> playerColorDic = new Dictionary<int, int>();
    public Dictionary<int, int> PlayerColorDic { get { return playerColorDic; } }

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

    private void Start()
    {
        //colorIndex에  nickNameColors의 길이만큼의 숫자를 넣는다.
        for (int i = 0; i < nickNameColors.Length; i++)
        {
            colorsIndex.Enqueue(i);
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

    public void AddPlayerColor(int actorNum, int colorIdx)
    {
        Debug.Log("AddPlayerColor:" + actorNum);
        playerColorDic[actorNum] = colorIdx;
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

    public int GetCurCharacterPlayer(Character.CharacterType type)
    {
        foreach (var item in playerCharacterDic)
        {
            if (item.Value != null && item.Value.characterType == type)
            {
                return item.Key;
            }
        }
        return -1;
    }

    public Color GetCurPlayerColor(int actorNum)
    {
        return nickNameColors[playerColorDic[actorNum]];
    }
}
