using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvTest : MonoBehaviour
{
    [SerializeField]
    TestSkill[] SkillInv = new TestSkill[4];
    int a = 0;
    public Dictionary<string, TestSkill> SkillDic;
    TestSkill Chronos = new TestSkill("ũ�γ뽺", "�ð��� �����մϴ�.");

    void Update()
    {
        if (a < 4)
        {
            if (Input.GetKeyDown(KeyCode.A))
            {
                Add(Chronos, a);
                a++;
            }
        }
        else
        {
            a = 0;
        }
    }

    public void Add(TestSkill Skill, int i)
    {
        SkillInv[i] = Skill;
        SkillInv[i].Show();
    }
}
