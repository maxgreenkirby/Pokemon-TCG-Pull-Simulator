using UnityEngine;
using PrimeTween;
using UnityEngine.EventSystems;

public class Pack : MonoBehaviour, IPointerClickHandler
{
    public void Float(float startDelay = 0)
    {
        Tween.PositionY(transform, -0.05f, 2f, Ease.InOutCubic, startDelay: startDelay)
            .Chain(Tween.PositionY(transform, 0.05f, 2f, Ease.InOutCubic))
            .OnComplete(() => Float());
    }

    private void SelectPack()
    {

    }

    public void OnPointerClick(PointerEventData eventData)
    {
        SelectPack();
        // MainEventHandler.AddToEventStream<PackOpenEvent>(new PackOpenEvent());
    }
}
