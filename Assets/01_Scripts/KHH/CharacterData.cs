using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Character
{
    public enum CharacterType
    {
        Horse,
        Chicken,
    }
    public CharacterType characterType;
    public Sprite characterImage;
    public string characterName;
    public GameObject characterPrefab;
}

[CreateAssetMenu(fileName = "CharacterData", menuName = "Scriptable Object/CharacterData")]
public class CharacterData : ScriptableObject
{
    public List<Character> characterList;

    public Character GetCharaterData(Character.CharacterType type)
    {
        for (int i = 0; i < characterList.Count; i++)
        {
            if (characterList[i].characterType == type)
            {
                return characterList[i];
            }
        }
        return null;
    }
}
