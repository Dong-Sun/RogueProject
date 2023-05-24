using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum WeaponType
{
    Shock,
    Penetrate,
    Cut
}

public interface IWeapon
{
    /// <summary>
    /// ���ݷ�
    /// </summary>
    int Damage { get; set; }

    /// <summary>
    /// ���ݼӵ�
    /// </summary>
    float AttackSpeed { get; set; }

    /// <summary>
    /// ��Ÿ�
    /// </summary>
    Bounds Range { get; set; }

    /// <summary>
    /// ġ��Ÿ Ȯ��
    /// </summary>
    float CriticalHitChance { get; set; }

    /// <summary>
    /// ������
    /// </summary>
    int CriticalHitDamage { get; set; }

    /// <summary>
    /// ������
    /// </summary>
    float Penetrate { get; set; }

    /// <summary>
    /// ����Ÿ��
    /// </summary>
    WeaponType WType { get; set; }
}
