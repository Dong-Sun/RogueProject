using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IItem
{
    /// <summary>
    /// ������ ���Ȯ��
    /// </summary>
    bool ItemUse { get; set; }

    /// <summary>
    /// ���� ����
    /// </summary>
    bool DungeonInOut { get; set; }
}

