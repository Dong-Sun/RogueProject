using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Weapon : MonoBehaviour, IWeapon
{
    //���� ���ݷ�
    [SerializeField] private int exsDamage;
    //���� ���ݼӵ�
    [SerializeField] private float exsAttackSpeed;
    //���� ��Ÿ�
    [SerializeField] private Bounds exsRange;
    //���� ġ��Ÿ Ȯ��
    [SerializeField] private float exsCriticalHitChance;
    //���� ������
    [SerializeField] private int exsCriticalHitDamage;
    //���� ������
    [SerializeField] private float exsPenetrate;
    //���� �Ӽ�
    [SerializeField] private WeaponType exsProperty;

    public int Damage
    {
        get => exsDamage;
        set => exsDamage = value;
    }

    public float AttackSpeed
    {
        get => exsAttackSpeed;
        set => exsAttackSpeed = value;
    }

    //��Ÿ��� ���� ���������� ����
    public Bounds Range
    {
        get => exsRange;
        set => exsRange = value;
    }

    public float CriticalHitChance
    {
        get => exsCriticalHitChance;
        set => exsCriticalHitChance = value;
    }

    public int CriticalHitDamage
    {
        get => exsCriticalHitDamage;
        set => exsCriticalHitDamage = value;
    }

    public float Penetrate
    {
        get => exsPenetrate;
        set => exsPenetrate = value;
    }

    public WeaponType WType
    {
        get => exsProperty;
        set => exsProperty = value;
    }
}
