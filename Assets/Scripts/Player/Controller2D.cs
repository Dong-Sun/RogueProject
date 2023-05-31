using System.Collections;
using UnityEngine;


public class Controller2D : RaycastController
{

    float maxClimbAngle = 80f;
    float maxDescendAngle = 80f;

    public CollisionInfo collisions;

    public override void Start()
    {
        base.Start();
    }

    public void DownJump()
    {
        UpdateRaycastOrigins();
        for (int i = 0; i < verticalRayCount; i++)
        {
            Vector2 rayOrigin = raycastOrigins.bottomLeft;

            rayOrigin += Vector2.right * (verticalRaySpacing * i);

            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.down, skinWidth * 7f, collisionMask);

            if (hit && !hit.transform.CompareTag("Wall"))
            {
                StartCoroutine(SwitchLayer(hit.transform.gameObject));
            }
        }
    }

    public void Dash(Vector3 velocity)
    {
        UpdateRaycastOrigins();

        // ���� ������ ����
        collisions.Reset();
        collisions.velocityOld = velocity;

        // ��翡 ���� �߷� ����
        if (velocity.y < 0)
        {
            DescendSlope(ref velocity);
        }
        // x�� ������ �浹 üũ
        if (velocity.x != 0)
        {
            HorizontalCollisions(ref velocity);
        }
        // y�� ������ �浹 üũ
        if (velocity.y != 0)
        {
            VerticalCollisions(ref velocity);
        }
        if (!collisions.verticalWall)
            velocity.y = collisions.velocityOld.y;
        if (!collisions.horizontalWall)
            velocity.x = collisions.velocityOld.x;
        // ���� ������
        transform.Translate(velocity);
    }

    public void Move(Vector3 velocity)
    {
        UpdateRaycastOrigins();

        // ���� ������ ����
        collisions.Reset();
        collisions.velocityOld = velocity;

        // ��翡 ���� �߷� ����
        if (velocity.y < 0)
        {
            DescendSlope(ref velocity);
        }
        // x�� ������ �浹 üũ
        if (velocity.x != 0)
        {
            HorizontalCollisions(ref velocity);
        }
        // y�� ������ �浹 üũ
        if (velocity.y != 0)
        {
            VerticalCollisions(ref velocity);
        }

        // ���� ������
        transform.Translate(velocity);
    }

    // ���� �浹 üũ
    void HorizontalCollisions(ref Vector3 velocity)
    {
        // x�� �̵� ����
        float directionX = Mathf.Sign(velocity.x);

        // Ray�� ���� = �ӵ� + ��Ų�β�
        float rayLength = Mathf.Abs(velocity.x) + skinWidth;

        // Raycast
        for (int i = 0; i < horizontalRayCount; i++)
        {
            // �̵� ���⿡ ���� Ray ������
            Vector2 rayOrigin = (directionX == -1) ? raycastOrigins.bottomLeft : raycastOrigins.bottomRight;

            // ������ ���� ������ ���� Ray �������� �̵�
            rayOrigin += Vector2.up * (horizontalRaySpacing * i);

            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.right * directionX, rayLength, collisionMask);

            Debug.DrawRay(rayOrigin, Vector2.right * directionX * rayLength, Color.red);

            // ���� �浹(���� ������ ��)
            if (hit && !hit.transform.CompareTag("Floor"))
            {
                if (hit.transform.CompareTag("Wall"))
                    collisions.horizontalWall = true;
                // ��������� �浹 ������ ���� ����(�鿡 ����) ������ ������ ���� ��簢 ���
                float slopeAngle = Vector2.Angle(hit.normal, Vector2.up);

                // i == 0�� ���� �ٴ� Ray�� �ǹ�, ��簢�� �ִ� ��簢���� ���� ���
                if (i == 0 && slopeAngle <= maxClimbAngle)
                {
                    // ���� ��縦 �������� ���� �̿��ٸ�
                    if (collisions.descendingSlope)
                    {
                        // ��縦 �������� ������ �ƴϰ� ��
                        collisions.descendingSlope = false;
                        // �����ߴ� ���� �ӵ��� ����
                        velocity = collisions.velocityOld;
                    }

                    // ��縦 ������ �����ϴ� �Ÿ�
                    float distanceToSlopeStart = 0;

                    // ���� ����� ������ ���� �������� ��簢�� �ٸ��ٸ�
                    if (slopeAngle != collisions.slopeAngleOld)
                    {
                        // ������ �Ÿ�
                        distanceToSlopeStart = hit.distance - skinWidth;

                        // x�� �ӵ��� ����
                        velocity.x -= distanceToSlopeStart * directionX;
                    }

                    // ��縦 ������ ���� �ӵ��� ����
                    ClimbSlope(ref velocity, slopeAngle);
                    // distanceToSlopeStart ������ ���� ���θ�ŭ ����
                    velocity.x += distanceToSlopeStart * directionX;
                }

                // ���� ��縦 ������ ���� �ƴϰų�, ��簢�� �ִ� ���� �� �ִ� ��簢 ���� ũ��
                if ((!collisions.climbingSlope || slopeAngle > maxClimbAngle) && !hit.transform.CompareTag("Stair"))
                {
                    // x�� �ӵ� ����(����� �Ÿ� ���)
                    velocity.x = (hit.distance - skinWidth) * directionX;
                    rayLength = hit.distance;

                    // ��縦 ������ ���̿��ٸ�
                    if (collisions.climbingSlope)
                    {
                        // y�� �ӵ� ����
                        velocity.y = Mathf.Tan(collisions.slopeAngle * Mathf.Deg2Rad) * Mathf.Abs(velocity.x);
                    }

                    // �̵� ���⿡ ���� �浹 üũ
                    collisions.left = directionX == -1;
                    collisions.right = directionX == 1;
                }
            }
        }
    }

    // ���� �浹 üũ
    void VerticalCollisions(ref Vector3 velocity)
    {
        // y�� �̵� ����
        float directionY = Mathf.Sign(velocity.y);

        // Ray�� ���� = �ӵ� + ��Ų�β�
        float rayLength = Mathf.Abs(velocity.y) + skinWidth;

        // Raycast
        for (int i = 0; i < verticalRayCount; i++)
        {
            // �̵� ���⿡ ���� Ray ������
            Vector2 rayOrigin = (directionY == -1) ? raycastOrigins.bottomLeft : raycastOrigins.topLeft;

            // ������ ���� ������ ���� Ray �������� �̵�
            rayOrigin += Vector2.right * (verticalRaySpacing * i + velocity.x);

            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.up * directionY, rayLength, collisionMask);
            Debug.DrawRay(rayOrigin, Vector2.up * directionY * rayLength, Color.red);

            // ���� �浹
            if (hit)
            {
                if (directionY != 1)
                {
                    if (hit.transform.CompareTag("Wall") || hit.transform.CompareTag("Floor"))
                    {
                        if (hit.transform.CompareTag("Wall"))
                            collisions.verticalWall = true;
                        // y�� �ӵ� ����(����� �Ÿ� ���)
                        velocity.y = (hit.distance - skinWidth) * directionY;
                        rayLength = hit.distance;

                        // ��� �ö󰡴� ��
                        if (collisions.climbingSlope)
                        {
                            // x�� �ӵ� ����
                            velocity.x = velocity.y / Mathf.Tan(collisions.slopeAngle * Mathf.Deg2Rad) * Mathf.Sign(velocity.x);
                        }

                        // �̵� ���⿡ ���� �� �Ʒ� �浹 üũ
                        collisions.below = true;
                    }
                    else if (hit.transform.CompareTag("Stair"))
                    {
                        if(i == 0)
                        {
                            hit = Physics2D.Raycast(rayOrigin, Vector2.left, rayLength, collisionMask);
                            if (hit && Vector2.Angle(hit.normal, Vector2.up) <= 45f)
                            {
                                // y�� �ӵ� ����(����� �Ÿ� ���)
                                velocity.y = (hit.distance - skinWidth) * directionY;
                                rayLength = hit.distance;

                                // ��� �ö󰡴� ��
                                if (collisions.climbingSlope)
                                {
                                    // x�� �ӵ� ����
                                    velocity.x = velocity.y / Mathf.Tan(collisions.slopeAngle * Mathf.Deg2Rad) * Mathf.Sign(velocity.x);
                                }

                                // �̵� ���⿡ ���� �� �Ʒ� �浹 üũ
                                collisions.below = true;
                            }
                        }
                        else
                        {
                            hit = Physics2D.Raycast(rayOrigin, Vector2.right, rayLength, collisionMask);
                            if (hit && Vector2.Angle(hit.normal, Vector2.up) <= 45f)
                            {
                                // y�� �ӵ� ����(����� �Ÿ� ���)
                                velocity.y = (hit.distance - skinWidth) * directionY;
                                rayLength = hit.distance;

                                // ��� �ö󰡴� ��
                                if (collisions.climbingSlope)
                                {
                                    // x�� �ӵ� ����
                                    velocity.x = velocity.y / Mathf.Tan(collisions.slopeAngle * Mathf.Deg2Rad) * Mathf.Sign(velocity.x);
                                }

                                // �̵� ���⿡ ���� �� �Ʒ� �浹 üũ
                                collisions.below = true;
                            }
                        }
                    }
                }
                else if(hit.transform.CompareTag("Wall"))
                {
                    collisions.verticalWall = true;
                    // y�� �ӵ� ����(����� �Ÿ� ���)
                    velocity.y = (hit.distance - skinWidth) * directionY;
                    rayLength = hit.distance;

                    // ��� �ö󰡴� ��
                    if (collisions.climbingSlope)
                    {
                        // x�� �ӵ� ����
                        velocity.x = velocity.y / Mathf.Tan(collisions.slopeAngle * Mathf.Deg2Rad) * Mathf.Sign(velocity.x);
                    }

                    // �̵� ���⿡ ���� �� �Ʒ� �浹 üũ
                    collisions.above = true;

                }
            }

        }

        // ��� �ö󰡴� ��
        if (collisions.climbingSlope)
        {
            // x�� �̵� ����
            float directionX = Mathf.Sign(velocity.x);

            // Ray�� ����
            rayLength = Mathf.Abs(velocity.x) + skinWidth;

            // ���⿡ ���� Ray ������
            Vector2 rayOrigin = ((directionX == -1) ? raycastOrigins.bottomLeft : raycastOrigins.bottomRight) + Vector2.up * velocity.y;
            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.right * directionX, rayLength, collisionMask);

            // ���� �浹
            if (hit)
            {
                // ���� ���
                float slopeAngle = Vector2.Angle(hit.normal, Vector2.up);

                // ���� ������ ���� ������ �ٸ��ٸ�
                if (slopeAngle != collisions.slopeAngle)
                {
                    // x�� �ӵ��� ���� ���� ����
                    velocity.x = (hit.distance - skinWidth) * directionX;
                    collisions.slopeAngle = slopeAngle;
                }
            }
        }
    }

    // ���� �ö� �� �ӵ� ����
    void ClimbSlope(ref Vector3 velocity, float slopeAngle)
    {
        // �̵� �Ÿ�
        float moveDistance = Mathf.Abs(velocity.x);

        // y�� �̵� �ӵ�
        float climbVelocityY = Mathf.Sin(slopeAngle * Mathf.Deg2Rad) * moveDistance;

        // ���� y�� �̵� �ӵ��� ��簢���� ���� ���� y�� �̵� �ӵ� ��
        if (velocity.y <= climbVelocityY)
        {
            // ���� �ӵ��� ���� �����
            velocity.y = climbVelocityY;

            // ���� ����� �ӵ��� ���� x�� �̵� �ӵ� ����
            velocity.x = Mathf.Cos(slopeAngle * Mathf.Deg2Rad) * moveDistance * Mathf.Sign(velocity.x);

            // �ٴ� �浹 üũ, ���� �ö󰡴� ��, ��簢 ����
            collisions.below = true;
            collisions.climbingSlope = true;
            collisions.slopeAngle = slopeAngle;
        }
    }

    // ���� ������ �� �ӵ� ����
    void DescendSlope(ref Vector3 velocity)
    {
        // x�� �̵� ����
        float directionX = Mathf.Sign(velocity.x);

        // raycast �������� �����ϱ� ���� ���׿�����(�̵� ������ �ݴ� �ʿ� Ray)
        Vector2 rayOrigin = (directionX == -1) ? raycastOrigins.bottomRight : raycastOrigins.bottomLeft;
        RaycastHit2D hit = Physics2D.Raycast(rayOrigin, -Vector2.up, Mathf.Infinity, collisionMask);

        // ���� �浹
        if (hit)
        {
            // ��������� �浹 ������ ���� ����(�鿡 ����) ������ ������ ���� ��簢 ���
            float slopeAngle = Vector2.Angle(hit.normal, Vector2.up);

            // �ش� ��簡 ������ �� �ִ� �������� Ȯ��
            if (slopeAngle != 0 && slopeAngle <= maxDescendAngle)
            {
                // ��� ����� �����̴� ������ ��ġ���� Ȯ��
                if (Mathf.Sign(hit.normal.x) == directionX)
                {
                    // x�� �̵� �ӵ� * Tan(��簢)�ϸ� ��簢 ��ŭ �밢�� �̵��� x�� �̵��Ÿ��� ����
                    // �ش� ��ü������ �Ÿ� <= x�� �̵� �ӵ� * Tan(��簢) 
                    if (hit.distance - skinWidth <= Mathf.Tan(slopeAngle * Mathf.Deg2Rad) * Mathf.Abs(velocity.x))
                    {
                        // �̵��Ÿ�
                        float moveDistance = Mathf.Abs(velocity.x);

                        // �̵��Ÿ��� ���� �������� �ӵ�
                        float descendVelocityY = Mathf.Sin(slopeAngle * Mathf.Deg2Rad) * moveDistance;

                        // x�� �̵� �ӵ� ���� = cos(��簢) * �̵��Ÿ� * �̵� ����
                        velocity.x = Mathf.Cos(slopeAngle * Mathf.Deg2Rad) * moveDistance * Mathf.Sign(velocity.x);
                        velocity.y -= descendVelocityY;

                        // ��簢, �������� ��, �ٴ� ���� ����
                        collisions.slopeAngle = slopeAngle;
                        collisions.descendingSlope = true;
                        collisions.below = true;
                    }
                }
            }
        }
    }

    IEnumerator SwitchLayer(GameObject g)
    {
        g.layer = 0;
        yield return new WaitForSeconds(0.3f);
        g.layer = 6;
    }

    // �浹 ����
    public struct CollisionInfo
    {
        // �����¿� �浹 ����
        public bool above, below;
        public bool left, right;

        // ���������� ���� ����
        public bool climbingSlope;
        public bool descendingSlope;

        public bool verticalWall;
        public bool horizontalWall;

        // ��簢
        public float slopeAngle, slopeAngleOld;
        public Vector3 velocityOld;

        public void Reset()
        {
            above = below = false;
            left = right = false;
            verticalWall = false;
            horizontalWall = false;
            climbingSlope = false;
            descendingSlope = false;

            slopeAngleOld = slopeAngle;
            slopeAngle = 0;
        }
    }
}