using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FillChargeBar : MonoBehaviour {

    private Image image;

	// Use this for initialization
	void Start () {
        image = GetComponent<Image>();
	}

    public void SetFill(float fillAmount)
    {
        image.fillAmount = fillAmount;
    }

    public void IncreaseFill(float fillAmount)
    {
        image.fillAmount += fillAmount;
    }

    public float GetFill()
    {
        return image.fillAmount;
    }
}
