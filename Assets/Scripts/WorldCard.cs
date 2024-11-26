using UnityEngine;

public class WorldCard : MonoBehaviour
{
    private Card _card;
    [SerializeField] private MeshRenderer _meshRenderer;
    [SerializeField] private Material[] _rarityMaterials;

    public void Initialize(Card card)
    {
        _card = card;

        _meshRenderer.material = _rarityMaterials[(int)card.Rarity];
        _meshRenderer.material.SetTexture("_Image", card.Sprite.texture);
    }
}
