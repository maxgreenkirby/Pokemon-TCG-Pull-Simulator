using UnityEngine;

public class WorldCard : MonoBehaviour
{
    private Card _card;
    [SerializeField] private MeshRenderer _meshRenderer;
    [SerializeField] private Material[] _rarityMaterials;
    private Vector3 _mouseDownPos;

    public void Initialize(Card card)
    {
        _card = card;

        Debug.Log((int)card.Rarity);
        _meshRenderer.material = _rarityMaterials[(int)card.Rarity];
        _meshRenderer.material.SetTexture("_Image", card.Sprite.texture);
    }

    private void Update()
    {
        
    }

    public void OnMouseDown()
    {
        _mouseDownPos = Input.mousePosition;
    }

    public void OnMouseUp()
    {
        // Select card if player is not dragging
        if (Vector2.Distance(_mouseDownPos, Input.mousePosition) < 10)
        {
            MainEventHandler.AddToEventStream(new CardSelectEvent(card: this));
        }
    }
}
