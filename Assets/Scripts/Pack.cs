using UnityEngine;
using PrimeTween;
using UniRx;

public class Pack : MonoBehaviour
{
    [SerializeField] private MeshRenderer _meshRenderer;
    [SerializeField] private Material _tearMaterial;
    private Sequence _floatSequence;
    private Vector3 _mouseDownPos;

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

    public void OpenPack()
    {
        MainEventHandler.AddToEventStream(new PackOpenEvent(pack: this));

        Tween.PositionY(transform, -300f, 1f, Ease.OutBack);  //.OnComplete(() => { Destroy(gameObject); });
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
