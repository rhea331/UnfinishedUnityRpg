using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeroRow : MonoBehaviour {

    public Text NameText;
    public Text HPText;
    public Text MPText;
    public Image ChargeImage;
	

    public void SetName (string name)
    {
        NameText.text = name;
    }
    public void SetHP(int currentHP, int maxHP)
    {
        HPText.text = currentHP.ToString() + " / " + maxHP.ToString();
    }

    public void SetMP (int currentMP, int maxMP)
    {
        MPText.text = currentMP.ToString() + " / " + maxMP.ToString();
    }

    public void SetCharge(float currentFill, int maxFill)
    {
        if (currentFill <= 0)
        {
            ChargeImage.fillAmount = 0;
        }
        else
        {
            ChargeImage.fillAmount = currentFill / maxFill;
        }

    }
}
