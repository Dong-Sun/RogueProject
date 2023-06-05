using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct WeaponInfo
{
    /// <summary>
    /// ���̾� ����ũ ����
    /// </summary>
    public LayerMask LayerName;
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
    public List<WeaponInfo> weaponInfos;

    [ContextMenu("To Json Data")]
    void WeaponStatSave()
    {
        string json = JsonUtility.ToJson(weaponInfos);

        string FileName = "WeaponInfo";
        string path = Application.dataPath + "/" + FileName + ".Json";

        File.WriteAllText(path, json);
    }
}
