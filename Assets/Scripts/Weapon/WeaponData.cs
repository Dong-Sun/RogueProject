using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "Weapon Data", menuName = "Scriptable Object/WeaponData", order = int.MaxValue)]
public class WeaponData : ScriptableObject
{
    public LayerMask LayerName; //���̾� ����ũ ����(���� ��Ÿ����� ���Դ��� Ȯ���ϱ� ����)

    public string WeaponName; //���� �̸� ����
    
    public int Damage; //���ݷ� ����
    
    public float AttackSpeed; //���� �ӵ� ����
    
    public Vector2 Range; //��Ÿ� ���� ����
    
    public int CriticalHitChance; //ġ��Ÿ Ȯ�� ����
    
    public int CriticalHitDamage; //ġ��Ÿ ������ ����
    
    public float Penetrate; //������ ����
    
    public float Shock; // ��� ������ ����

    public float Pierce; //���� ������ ����
   
    public float Cut; //���� ������ ����
}