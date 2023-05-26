using UnityEngine;

[RequireComponent(typeof(CharacterController2D))]
public class Player : MonoBehaviour
{
    public int JumpCount = 2;
    // ���� ����
    public float jumpHeight = 4;
    // �ְ� ���̿� �����ϴ� �ð�
    public float timeToJumpApex = 0.4f;

    public float DashVelocity = 15f;

    public float DashTime = 0.1f;

    // ���߿����� ���ӵ� �ð�
    float accelerationTimeAirborne = 0.2f;
    // ���鿡���� ���ӵ� �ð�
    float accelerationTimeGrounded = 0.1f;
    // �̵� �ӵ�
    float moveSpeed = 6;

    int jumpCount;

    // ���� ���� �ްԵ� �߷�
    float gravity;
    // ���� ���� �� ����Ǵ� �ӵ�
    float jumpVelocity;
    // �����̴� �ӵ�
    Vector3 velocity;

    Vector2 dashDirection;
    float dashTimer;

    float velocityXSmoothing;

    CharacterController2D controller;

    void Start()
    {
        controller = GetComponent<CharacterController2D>();
        jumpCount = JumpCount;
        dashDirection = Vector2.zero;
        dashTimer = 0f;
        // ������ = �ʱ�ӵ� * �ð� + ���ӵ� * �ð�^2 * 1/2
        // �� ���� ��ü������ �ٲٸ�
        // ��������(jumpHeight) = �߷�(gravity) * �ɸ��ð�(timeToJumpApex)^2 * 1/2
        // �߷��� ������ ������ ������ �������� �����ϸ�
        // �߷� = (2 * ���� ����) / (�ְ� ���̿� �����ϴ� �ð��� ����)
        gravity = -(2 * jumpHeight) / Mathf.Pow(timeToJumpApex, 2);

        // ���� �ӵ� = �߷� * �ְ� ���̿� �����ϴ� �ð�
        jumpVelocity = Mathf.Abs(gravity) * timeToJumpApex;

        print("Gravity: " + gravity + "  Jump Velocity: " + jumpVelocity);
    }

    void Update()
    {
        //print(dashVelocity);
        if (controller.collisions.above)
            velocity.y = 0;

        if (StayFloor())
        {
            velocity.y = 0;
            jumpCount = 2;
        }

        Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        // �����̽��� �Է�, �ٴڿ� ��������� ����
        if (Input.GetKeyDown(KeyCode.Space) && jumpCount > 0)
        {
            //if (Input.GetKey(KeyCode.S))
            //{
            //    controller.DownJump();
            //}
            //else
            //{
            velocity.y = jumpVelocity;
            jumpCount--;
            //}
        }

        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            dashDirection = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
            dashTimer = DashTime;
        }

        float targetVelocityX = input.x * moveSpeed;
        // �ε巯�� ���Ӱ� ������ ���� SmoothDamp �Լ�
        // Mathf.SmoothDamp(���簪, ��ǥ��, ref �ӵ�, �ð�)
        velocity.x = Mathf.SmoothDamp(velocity.x, targetVelocityX, ref velocityXSmoothing, (controller.collisions.below) ? accelerationTimeGrounded : accelerationTimeAirborne);
        velocity.y += gravity * Time.deltaTime;

        if (dashTimer > 0)
        {
            dashTimer -= Time.deltaTime;
            velocity = dashDirection.normalized * DashVelocity;
            controller.Dash(velocity * Time.deltaTime);
        }
        else
            controller.Move(velocity * Time.deltaTime);
    }

    bool StayFloor()
    {
        return controller.collisions.below || controller.collisions.floor || controller.collisions.climbingSlope || controller.collisions.descendingSlope;
    }
}