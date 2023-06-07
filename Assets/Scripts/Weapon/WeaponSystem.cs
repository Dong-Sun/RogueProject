using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "Weapon Data", menuName = "Scriptable Object/WeaponData", order = int.MaxValue)]
public class WeaponData : ScriptableObject
{
    /// <summary>
    /// ���̾� ����ũ ����
    /// </summary>
    public LayerMask LayerName;
    /// <summary>
    /// ���� �̸�
    /// </summary>
    public string WeaponName;
    /// <summary>
    /// ���ݷ�
    /// </summary>
    public int Damage;
    /// <summary>
    /// ���ݼӵ�
    /// </summary>
    public float AttackSpeed;
    /// <summary>
    /// ��Ÿ� ���� ����
    /// </summary>
    public Vector2 Range;
    /// <summary>
    /// ��Ÿ� ��ġ ����
    /// </summary>
    public Transform RangeP;
    /// <summary>
    /// ġ��Ÿ Ȯ��
    /// </summary>
    public int CriticalHitChance;
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

public class WeaponSystem : MonoBehaviour
{
    
}
