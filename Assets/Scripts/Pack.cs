using UnityEngine;
using PrimeTween;

public class Pack : MonoBehaviour
{
    private Sequence _floatSequence;

    public void Float(float startDelay = 0)
    {
        _floatSequence = Tween.PositionY(transform, -0.05f, 2f, Ease.InOutCubic, startDelay: startDelay)
            .Chain(Tween.PositionY(transform, 0.05f, 2f, Ease.InOutCubic))
            .OnComplete(() => Float());
    }

    private void SelectPack()
    {
        Debug.Log("Pack selected");
        MainEventHandler.AddToEventStream(new PackChooseEvent(pack: this));
    }

    public void OnMouseDown()
    {
        SelectPack();
    }

    private void OnDestroy()
    {
        _floatSequence.Stop();
    }
}
