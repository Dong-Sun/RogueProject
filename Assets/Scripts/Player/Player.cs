using UnityEngine;

[RequireComponent(typeof(Controller2D))]
public class Player : MonoBehaviour
{
    // ���� ����
    public float jumpHeight = 4;
    // �ְ� ���̿� �����ϴ� �ð�
    public float timeToJumpApex = 0.4f;
    // ���߿����� ���ӵ� �ð�
    float accelerationTimeAirborne = 0.2f;
    // ���鿡���� ���ӵ� �ð�
    float accelerationTimeGrounded = 0.1f;
    // �̵� �ӵ�
    float moveSpeed = 6;

    // ���� ���� �ްԵ� �߷�
    float gravity;
    // ���� ���� �� ����Ǵ� �ӵ�
    float jumpVelocity;
    // �����̴� �ӵ�
    Vector3 velocity;

    float velocityXSmoothing;

    Controller2D controller;

    void Start()
    {
        controller = GetComponent<Controller2D>();

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
        // ����, �Ʒ��� �浹�� �ӵ��� 0���� �ʱ�ȭ
        if (controller.collisions.above || controller.collisions.below)
        {
            velocity.y = 0;
        }

        Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        // �����̽��� �Է�, �ٴڿ� ��������� ����
        if (Input.GetKeyDown(KeyCode.Space) && controller.collisions.below)
        {
            velocity.y = jumpVelocity;
        }

        float targetVelocityX = input.x * moveSpeed;
        // �ε巯�� ���Ӱ� ������ ���� SmoothDamp �Լ�
        // Mathf.SmoothDamp(���簪, ��ǥ��, ref �ӵ�, �ð�)
        velocity.x = Mathf.SmoothDamp(velocity.x, targetVelocityX, ref velocityXSmoothing, (controller.collisions.below) ? accelerationTimeGrounded : accelerationTimeAirborne);
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }
}