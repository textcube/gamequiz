using UnityEngine;
using System.Collections;

public class HpManager : MonoBehaviour {

    public UISlider hpBar, mpBar;

    public int hpMax = 100;
    public int mpMax = 100;
    int hp = 100;
    int mp = 100;

    public void InitHp()
    {
        SetHp(hpMax);
    }

    public void InitMp()
    {
        SetHp(mpMax);
    }

    public void DoDamageHp(int point)
    {
        SetHp(hp - point);
    }

    public void DoSaveHp(int point)
    {
        SetHp(hp + point);
    }

    public void DoSaveMp(int point)
    {
        SetMp(mp + point);
    }

    public void SetHp(int point)
    {
        hp = Mathf.Clamp(point, 0, hpMax);
        if (hpBar)
            hpBar.value = (float)hp / (float)hpMax;
    }

    public void SetMp(int point)
    {
        mp = Mathf.Clamp(point, 0, mpMax);
        if (mpBar)
            mpBar.value = (float)mp / (float)mpMax;
    }

}
