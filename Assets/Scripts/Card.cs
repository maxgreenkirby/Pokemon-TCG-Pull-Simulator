using UnityEngine;

[CreateAssetMenu(fileName = "Card", menuName = "Scriptable Objects/Card")]
public class Card : ScriptableObject
{
    public string Name;
    public int HP;
    public ERarity Rarity;
    public ECardType CardType;
    public EEffectType EffectType;
    public Sprite Sprite;
}



