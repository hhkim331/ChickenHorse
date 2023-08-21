using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInfo : MonoBehaviour
{
    [SerializeField] Image characterImage;
    [SerializeField] TextMeshProUGUI nickNameText;
    [SerializeField] TextMeshProUGUI characterNameText;

    Character character;
    public Character Character { get { return character; } }
    public RectTransform pointParent;

    public void Set(string nickName, Character character)
    {
        this.character = character;
        characterImage.sprite = character.characterImage;
        nickNameText.text = nickName;
        characterNameText.text = character.characterName;
    }
}
