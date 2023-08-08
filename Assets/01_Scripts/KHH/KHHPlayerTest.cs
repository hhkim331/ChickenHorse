using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KHHPlayerTest : MonoBehaviour
{
    public bool isActive = true;
    bool isDie = false;
    bool isGoal = false;

    public void ResetPlayer()
    {
        isActive = true;
        isDie = false;
        isGoal = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.y < -10)
        {
            isActive = false;
            isDie = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Goal")
        {
            isActive = false;
            isGoal = true;
            KHHGameManager.instance.ScoreMgr.GetScore(Point.PointType.Goal, gameObject);
        }
    }
}
