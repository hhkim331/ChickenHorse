using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RPlayerWallJump : MonoBehaviour
{
    Rigidbody rb;

    Vector3 dir;

    public float speed = 5;
    public float jumpPower = 5;

    //최대 점프 시간
    public float maxJumpTime = 1.0f;

    //최대 점프 높이
    public float maxJumpHeight = 3.0f;

    //점프를 한 현재 시간
    public float currentTime;

    //점프의 높이 체크
    public bool jumpTime = true;

    // jump의 높이를 제한하기 위한 addForce에 들어가는 power의 횟수 제한
    int fCount = 3;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        // 만약에 벽을 만나면
        // 작동을 멈추고 싶다.
        if (IsWall() == false) return;

        float h = Input.GetAxis("Horizontal");

        dir = Vector3.right * h;
        dir.Normalize();

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            //rb.AddForce(Vector3.forward * 1);            
            //rb.velocity = Vector3.forward * 2;
            rb.AddForce(new Vector3(1, 0, 0), ForceMode.VelocityChange);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            rb.AddForce(new Vector3(-2, 0, 0), ForceMode.VelocityChange);
        }
        // 점프 입력을 받으면 점프 시작

        if (Input.GetKey(KeyCode.Space) && IsWall())
        {
            jumpTime = true;
            currentTime = 0;
            jumpPower = 2;
        }

        // 점프 입력을 떼면 점프 중지
        if (Input.GetKeyUp(KeyCode.Space))
        {
            jumpTime = false;
            fCount = 3;
        }
    }



    // 벽을 확인하자
    // 벽이 있으면, 바닥에서 실행하는 코드를 끄자
   
    
    
    
    private void FixedUpdate()
    {


        //만약에 땅에 닿아있다면
        if (IsWall() == true)
        {
            //현재 속도
            Vector3 currVelocity = rb.velocity;
            //움직여야 하는 속도
            Vector3 targetVelocity = dir * speed;

            rb.AddForce(targetVelocity - currVelocity, ForceMode.VelocityChange);

        }
        else
        {
            //현재 속도
            Vector3 currVelocity = rb.velocity;
            //움직여야 하는 속도
            Vector3 targetVelocity = dir * speed * 1.0f;

            // 평행으로 움직이는 y의 값과 점프하고 내려올 때의 y 값을 같게 하자.
            targetVelocity.x = currVelocity.x;

            rb.AddForce(targetVelocity - currVelocity, ForceMode.VelocityChange);
        }

        if (jumpTime)
        {

            if (fCount > 0)
            {
                rb.AddForce(new Vector3(-jumpPower,0 , 0), ForceMode.VelocityChange);
                fCount--;
            }

            //jumpTime = false;
        }

        // 중력
        rb.AddForce(new Vector3(9.81f,0,0));



    }

    public bool IsWall()
    {
        Ray ray = new Ray(transform.position, Vector3.right);

        return Physics.Raycast(ray, 1.02f);
    }
}