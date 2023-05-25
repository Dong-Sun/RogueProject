using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct WeaponStruct
{
    /// <summary>
    /// ���ݷ�
    /// </summary>
    public int Damage;
    /// <summary>
    /// ���ݼӵ�
    /// </summary>
    public float AttackSpeed;
    /// <summary>
    /// ��Ÿ�
    /// </summary>
    public Bounds Range;
    public Vector2 PointA;
    public Vector2 PointB;
    /// <summary>
    /// ġ��Ÿ Ȯ��
    /// </summary>
    public float CriticalHitChance;
    /// <summary>
    /// ġ��Ÿ ������
    /// </summary>
    public int CriticalHitDamage;
    /// <summary>
    /// ������
    /// </summary>
    public float Penetrate;
    /// <summary>
    /// ��� ������ �ۼ�Ʈ
    /// </summary>
    public float Shock;
    /// <summary>
    /// ���� ������ �ۼ�Ʈ
    /// </summary>
    public float Pierce;
    /// <summary>
    /// ���� ������ �ۼ�Ʈ
    /// </summary>
    public float Cut;
}

public abstract class Weapon : MonoBehaviour
{
}
