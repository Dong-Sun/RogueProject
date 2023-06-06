using UnityEngine;

[RequireComponent(typeof(Controller2D))]
public class Player : Character
{
    // �뽬 �ӵ�
    public float DashVelocity = 15f;
    // �뽬 �ð�
    public float DashTime = 0.1f;

    #region private field
    // ���߿����� ���ӵ� �ð�
    private float accelerationTimeAirborne = 0.2f;
    // ���鿡���� ���ӵ� �ð�
    private float accelerationTimeGrounded = 0.1f;
    // �̵� �ӵ�
    private float moveSpeed = 6;
    // ���� ���� Ƚ���� ī��Ʈ�ϴ� ����
    private int jumpCount;
    // ���� ���� �ްԵ� �߷�
    private float gravity;
    // ���� ���� �� ����Ǵ� �ӵ�
    private float jumpVelocity;
    // �����̴� �ӵ�
    private Vector3 velocity;
    // ���콺 ��ġ�� ���� �뽬 ������ ���� ����
    private Vector2 dashDirection;
    // �뽬 �ð��� üũ�ϴ� Ÿ�̸�
    private float dashTimer;
    // Mathf.Smooth �Լ����� ��������� ���̴� ����
    private float velocityXSmoothing;
    // ��Ʈ�ѷ�
    private Controller2D controller;
    // �ִϸ�����
    private PlayerAnimator playerAnimator;
    #endregion

    void Start()
    {
        controller = GetComponent<Controller2D>();
        playerAnimator = GetComponent<PlayerAnimator>();
        PhysicsInit();
    }

    void Update()
    {
        VerticalCollision();
        Jump();
        VelocityInput();
        InputGroup();
        Movement();
    }

    private void PhysicsInit()
    {
        jumpCount = Status.JumpCount;
        dashDirection = Vector2.zero;
        dashTimer = 0f;
        // ������ = �ʱ�ӵ� * �ð� + ���ӵ� * �ð�^2 * 1/2
        // �� ���� ��ü������ �ٲٸ�
        // ��������(jumpHeight) = �߷�(gravity) * �ɸ��ð�(timeToJumpApex)^2 * 1/2
        // �߷��� ������ ������ ������ �������� �����ϸ�
        // �߷� = (2 * ���� ����) / (�ְ� ���̿� �����ϴ� �ð��� ����)
        gravity = -(2 * Status.JumpHeight) / Mathf.Pow(Status.TimeToJumpApex, 2);

        // ���� �ӵ� = �߷� * �ְ� ���̿� �����ϴ� �ð�
        jumpVelocity = Mathf.Abs(gravity) * Status.TimeToJumpApex;
    }

    private void VerticalCollision()
    {
        if (controller.collisions.above || controller.collisions.below)
        {
            print(controller.collisions.below);
            if (controller.collisions.below)
            {
                if (playerAnimator.animationState == PlayerAniState.Jump)
                    playerAnimator.animationState = PlayerAniState.Idle;
                jumpCount = 2;
            }
            velocity.y = 0;
        }
        else
        {
            print(controller.collisions.below);
            if (playerAnimator.animationState != PlayerAniState.Jump
                && playerAnimator.animationState != PlayerAniState.Dash)
                playerAnimator.animationState = PlayerAniState.Jump;
        }
    }

    private void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            // �ϴ� ����
            if (Input.GetKey(KeyCode.S))
            {
                controller.DownJump();
            }
            // �Ϲ� ����
            else if (jumpCount > 0)
            {
                velocity.y = jumpVelocity;
                jumpCount--;
            }
        }
    }

    private void VelocityInput()
    {
        Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal"), 0);
        float targetVelocityX = input.x * moveSpeed;
        // �ε巯�� ���Ӱ� ������ ���� SmoothDamp �Լ�
        // Mathf.SmoothDamp(���簪, ��ǥ��, ref �ӵ�, �ð�)
        velocity.x = Mathf.SmoothDamp(velocity.x, targetVelocityX, ref velocityXSmoothing, (controller.collisions.below) ? accelerationTimeGrounded : accelerationTimeAirborne);
        velocity.y += gravity * Time.deltaTime;

        if (Mathf.Abs(input.x) > 0)
        {
            if (playerAnimator.animationState == PlayerAniState.Idle)
                playerAnimator.animationState = PlayerAniState.Run;
        }
        else
        {
            if (playerAnimator.animationState == PlayerAniState.Walk)
                playerAnimator.animationState = PlayerAniState.Idle;
        }
    }

    private void InputGroup()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            // Attack
        }
        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            // Dash
            dashDirection = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
            dashTimer = DashTime;
        }
        if (Input.GetKeyDown(KeyCode.Q))
        {
            // Skill 1
        }
        if (Input.GetKeyDown(KeyCode.W))
        {
            // Skill 2
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            // Skill 3
        }
    }

    private void Movement()
    {
        if (dashTimer > 0)
        {
            if (playerAnimator.animationState != PlayerAniState.Dash)
                playerAnimator.animationState = PlayerAniState.Dash;
            dashTimer -= Time.deltaTime;
            velocity = dashDirection.normalized * DashVelocity;
            controller.Dash(velocity * Time.deltaTime);
        }
        else
        {
            if (playerAnimator.animationState == PlayerAniState.Dash)
                playerAnimator.animationState = PlayerAniState.Idle;
            controller.Move(velocity * Time.deltaTime);
        }
    }
}