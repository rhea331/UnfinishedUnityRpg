using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageTextController : MonoBehaviour {
    private static DamageText damageText;
    private static GameObject canvas;

    public static void Initialize()
    {
        canvas = GameObject.FindGameObjectWithTag("UI");
        damageText = Resources.Load<DamageText>("Prefabs/Combat/UI/PopUpTextParent");
    }

    public static void CreateDamageText(int damage, float x, float y)
    {
        DamageText instance = Instantiate(damageText);
        Vector2 screenPosition = Camera.main.WorldToScreenPoint(new Vector2(x, y));
        instance.transform.SetParent(canvas.transform, false);
        instance.transform.position = screenPosition;
        if (damage < 0) //Healing
        {
            instance.SetColor(Color.green);
            instance.SetText((-damage).ToString());
        }
        else
        {
            instance.SetText(damage.ToString());
        }

    }

    public static void Finish()
    {
        Resources.UnloadAsset(damageText);
    }
}

