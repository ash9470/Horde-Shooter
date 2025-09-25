using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WeaponSelector : MonoBehaviour
{
    public Weapon Weapon;
    public Image SelectedUI;
    public Image GunIconImage;
    public TMP_Text GunName;


    public void SetSelection(Sprite sprite)
    {
        SelectedUI.sprite = sprite;
    }
}
