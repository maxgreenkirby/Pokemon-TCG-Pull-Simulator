using UnityEngine;

[CreateAssetMenu(fileName = "Card", menuName = "Scriptable Objects/Card")]
public class Card : ScriptableObject
{
    public int ID;
    public string Name;
    public int HP;
    public ERarity Rarity;
    public EEffectType EffectType;
    public Sprite Sprite;
}



