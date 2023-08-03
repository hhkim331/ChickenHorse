using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RPlayerMove : MonoBehaviour
{

    // �ӷ�
    public float speed = 5;

    public int maxJumps = 1;
    private int jumpCount = 0;
    private bool isJumping = false;

    // character controller ���� ����
    CharacterController cc;

    // �����Ŀ�
    float jumpPower = 5;
    // �߷�
    float gravity = -9.81f;
    // y �ӷ�
    float yVelocity;

    // �� ���� �ð� ���� �߰�
    private float wallJumpTime = 0.4f;

    // �� ����
    public bool isWall = false;

    Vector3 dir;
    public float currentTime;
    public float MoveTime;



    // Start is called before the first frame update
    void Start()
    {
        //character controller ��������
        cc = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {

        // W,A,S,D Ű�� ������ �յ��¿�� �����̰� �ʹ�.

        // 1. ������� �Է��� ����.
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        // 2. ������ �����.
        //�¿�
        Vector3 dirH = transform.right * h;
        //���Ʒ�
        Vector3 dirV = transform.up * v;
        //����
        Vector3 dir = dirH + dirV;

        //Vector3 dir = h * Vector3.right + v * Vector3.up;

        dir.Normalize();

        Vector3 velocity = dir * speed;

        //���࿡ ���� ����ִٸ�
        if (cc.isGrounded == true)
        {
            //yVelocity�� 0���� ����
            yVelocity = 0;

            jumpCount = 0;
        }

        // �����̽��ٸ� ������ �����Ѵ�.
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (jumpCount < maxJumps)
            {
                //yVelocity�� jumpPower �� ����
                yVelocity = jumpPower;
                jumpCount++;
            }
        }

        //yVelocity �� �߷¸�ŭ ���ҽ�Ű��
        yVelocity += gravity * Time.deltaTime;

        //yVelocity ���� dir�� y ���� ����
        dir.y = yVelocity;

        //�� �������� ��������
        //transform.position += velocity * Time.deltaTime;
        cc.Move(dir * speed * Time.deltaTime);

        if (cc.isGrounded)
        {
            isWall = false;
        }
    }



    // ������

    private float wallJumpPower = 5f;
    private float wallJumpSpeed = 3f;
    private void UpdateWallJump()
    {
        {
            // ���� �پ� �ִ��� Ȯ���� Raycast ����
            // ���� ��ġ 
            Vector3 raycast = transform.position + Vector3.up * 0.1f;

            // �浹 �˻� �Ÿ� 
            float castDistance = 0.5f;

            // �ٴ� ���̾� ����ũ ����
            LayerMask groundLayerMask = LayerMask.GetMask("Ground");

            // Raycast �Ǵ� SphereCast�� ����Ͽ� �浹 �˻縦 �����մϴ�.
            bool isTouchingWall = Physics.Raycast(raycast, transform.forward, castDistance, groundLayerMask);

            // ���� �پ� ������ �� ���� ������ �����մϴ�.
            if (isTouchingWall)
            {
                // �̵� ������ ������ �Լ��� �����մϴ�.
                if (dir.magnitude == 0)
                {
                    return;
                }

                // ������ �ð��� �ӵ��� �ʱ�ȭ �մϴ�.
                float wallJumpTimer = 0f;
                float currentWallJumpSpeed = wallJumpSpeed;

                // ������ ������ ����մϴ�. �⺻ �̵� ������ �ݴ� �������� 2�踸ŭ �̵��մϴ�.
                Vector3 newdir = -dir.normalized * 2;

                // charactercontroller�� �̿��ؼ� ������������ �̵��մϴ�.
                // ���ο� �������� �÷��̾ ȸ���մϴ�.
                if (newdir.magnitude == 0) return;
                else
                {
                    transform.forward = newdir;
                }
            }
        }
    }
}



