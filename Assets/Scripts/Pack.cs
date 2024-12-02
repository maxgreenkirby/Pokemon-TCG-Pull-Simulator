using UnityEngine;
using PrimeTween;
using UniRx;
using System.Collections.Generic;

public class Pack : MonoBehaviour
{
    [Header("Setup")]
    [SerializeField] private MeshRenderer _meshRenderer;
    [SerializeField] private Material _tearMaterial;
    private Sequence _floatSequence;
    private Vector3 _mouseDownPos;

    [Header("Cards")]
    [SerializeField] private List<WorldCard> _cards;

    private void Awake()
    {
        MainEventHandler.ListenForEventStream<PackChooseEvent>().Subscribe(OnPackChooseEvent).AddTo(this);
    }

    private void OnPackChooseEvent(PackChooseEvent packOpenEvent)
    {
        _floatSequence.Stop();
    }
    
    private void SelectPack()
    {
        MainEventHandler.AddToEventStream(new PackChooseEvent(pack: this));
    }

    public void SetTearMaterial()
    {
        Material[] materials = _meshRenderer.materials;
        materials[1] = _tearMaterial;
        _meshRenderer.materials = materials;
    }

    public void AddCard(WorldCard card)
    {
        _cards.Add(card);
    }

    public void OpenPack()
    {
        MainEventHandler.AddToEventStream(new PackOpenEvent(pack: this));

        Tween.LocalPositionY(transform, -1f, 1.75f, Ease.OutQuart);  //.OnComplete(() => { Destroy(gameObject); });

        // Animate the cards out of the pack
        for (int i = 0; i < _cards.Count; i++)
        {
            Tween.LocalPositionY(_cards[i].transform, 0, 0.5f, 1.75f, Ease.OutQuart, startDelay: 0.1f * i);
        }
    }

    public void OnMouseDown()
    {
        _mouseDownPos = Input.mousePosition;
    }

    public void OnMouseUp()
    {
        // Select pack if player is not dragging
        if (Vector2.Distance(_mouseDownPos, Input.mousePosition) < 10)
        {
            SelectPack();
        }
    }

    public void Float(float startDelay = 0)
    {
        _floatSequence = Tween.PositionY(transform, -0.05f, 2f, Ease.InOutCubic, startDelay: startDelay)
            .Chain(Tween.PositionY(transform, 0.05f, 2f, Ease.InOutCubic))
            .OnComplete(() => Float());
    }
}
