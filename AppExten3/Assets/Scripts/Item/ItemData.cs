using UnityEngine;

[CreateAssetMenu(fileName = "ItemData", menuName = "Scriptable Objects/ItemData")]
public class ItemData : ScriptableObject
{
    public int itemID;
    public string Name;
    public string Description;
    public Sprite Icon;

    public bool isWeapon;
    public bool isHeal;

    public int damage;
    public int healAmount;
}
