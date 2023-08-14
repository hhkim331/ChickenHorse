using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class KHHPlayerMain : MonoBehaviour
{
    bool isActive = true;
    public bool IsActive { get { return isActive; } }
    bool isDie = false;
    public bool isGoal = false;
    [SerializeField] RPlayer rPlayer;
    [SerializeField] Animator animator;
    [SerializeField] SpriteRenderer[] spriteRenderers;

    public void ResetPlayer()
    {
        isActive = true;
        isDie = false;
        isGoal = false;
        animator.enabled = true;
        foreach (var spriteRenderer in spriteRenderers)
        {
            spriteRenderer.color = Color.white;
        }
    }

    public void ActiveMove()
    {
        rPlayer.enabled = true;
    }

    public void Hit()
    {
        isActive = false;
        rPlayer.enabled = false;
        animator.enabled = false;
        foreach (var spriteRenderer in spriteRenderers)
        {
            spriteRenderer.color = Color.red;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (SceneManager.GetActiveScene().name != "MainScene") return;
        if (transform.position.y < -10)
        {
            isActive = false;
            isDie = true;
            rPlayer.enabled = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (SceneManager.GetActiveScene().name != "MainScene") return;
        if (other.tag == "Goal")
        {
            isActive = false;
            isGoal = true;
            rPlayer.enabled = false;
            KHHGameManager.instance.ScoreMgr.GetScore(Point.PointType.Goal, gameObject);
        }
    }
}
