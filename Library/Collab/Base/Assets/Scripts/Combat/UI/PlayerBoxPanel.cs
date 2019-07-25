using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerBoxPanel : MonoBehaviour {

    public Image PlayerFace;
    public Button PhysicalAttackButton;
    public Button MagicalAttackButton;
    public Button ItemsButton;
    public Button FleeButton;

    public void EnablePlayerBox(PlayerStateMachine PSM, CanvasController canvas)
    {
        this.gameObject.SetActive(true);
        SetImage(PSM.GetComponent<SpriteRenderer>().sprite);
        PhysicalAttackButton.onClick.AddListener(canvas.PhysicalAttackSelection);
        MagicalAttackButton.onClick.AddListener(canvas.MagicAttackSelection);
        //ItemsButton.onClick.AddListener(canvas.ItemsSelection);
        //FleeButton.onClick.AddListener(canvas.FleeSelection);
    }

    public void SetImage(Sprite image)
    {
        PlayerFace.sprite = image;
    }

    public void DisablePlayerBox()
    {
        this.gameObject.SetActive(false);

    }

    public void SetButtons(PlayerStateMachine PSM)
    {

    }

    public void RemoveListeners()
    {
        PhysicalAttackButton.onClick.RemoveAllListeners();
        MagicalAttackButton.onClick.RemoveAllListeners();
        ItemsButton.onClick.RemoveAllListeners();
        FleeButton.onClick.RemoveAllListeners();
    }
}
