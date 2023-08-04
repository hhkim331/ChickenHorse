using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TotalStageObjectData", menuName = "Scriptable Object/TotalStageObjectData")]
public class TotalStageObjectData : ScriptableObject
{
    public List<GameObject> stageObjectDataList = new List<GameObject>();
}
