using UnityEngine;
using PrimeTween;

public class Pack : MonoBehaviour
{
    private Sequence _floatSequence;
    private Vector3 _mouseDownPos;

    public void Float(float startDelay = 0)
    {
        _floatSequence = Tween.PositionY(transform, -0.05f, 2f, Ease.InOutCubic, startDelay: startDelay)
            .Chain(Tween.PositionY(transform, 0.05f, 2f, Ease.InOutCubic))
            .OnComplete(() => Float());
    }

    public void StopFloating()
    {
        _floatSequence.Stop();
    }

    private void SelectPack()
    {
        Debug.Log("Pack selected");
        MainEventHandler.AddToEventStream(new PackChooseEvent(pack: this));
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

    private void OnDestroy()
    {
        _floatSequence.Stop();
    }
}
