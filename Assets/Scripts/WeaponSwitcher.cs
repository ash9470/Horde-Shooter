using System.Collections.Generic;
using UnityEngine;

public class WeaponSwitcher : MonoBehaviour
{
    public Weapon[] weapons;
    public Player player;

    public Transform parent;
    public WeaponSelector weaponSelector;
    public List<WeaponSelector> weaponSelectorsList;
    int current = 0;


    public Sprite selectedsprite;
    public Sprite deselectedsprite;

    public void StartGame()
    {
        SetUpUI();
        if (player == null) player = GameManager.Instance._player;
        if (weapons.Length > 0) SetWeapon(0);
    }


    public void SetUpUI()
    {
        if (weaponSelectorsList.Count < weapons.Length)
        {

            foreach (var weapon in weapons)
            {
                WeaponSelector weaponui = Instantiate(weaponSelector, parent);
                weaponui.Weapon = weapon;
                weaponui.GunName.text = weaponui.Weapon.weaponName;
                weaponui.GunIconImage.sprite = weapon.Weaponsprite;
                weaponSelectorsList.Add(weaponui);
            }
        }
    }



    void Update()
    {
        if (!GameManager.Instance.isLive)
            return;

        if (Input.GetKeyDown(KeyCode.Q))
        {
            int next = (current + 1) % weapons.Length;
            SetWeapon(next);
        }
    }

    public void SetWeapon(int idx)
    {
        if (idx < 0 || idx >= weapons.Length) return;
        current = idx;
        player.currentWeapon = weapons[current];
        player.Weaponsprite.sprite = weapons[current].Weaponsprite;
        UpdateSectioUI(current);
    }

    void UpdateSectioUI(int Id)
    {
        weaponSelectorsList.ForEach(x => x.SetSelection(deselectedsprite));
        weaponSelectorsList[Id].SetSelection(selectedsprite);
    }

}
