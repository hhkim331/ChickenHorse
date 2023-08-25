using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ObjectScript : MonoBehaviour
{
    public bool active = false;
    public abstract void ResetObject();
}
