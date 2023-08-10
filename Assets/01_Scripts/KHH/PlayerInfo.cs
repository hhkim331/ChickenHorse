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

    public void Set(Character character)
    {
        this.character = character;
        characterImage.sprite = character.characterImage;
        nickNameText.text = "내 닉네임";
        characterNameText.text = character.characterName;
    }
}
