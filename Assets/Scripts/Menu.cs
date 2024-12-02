using UnityEngine;
using UniRx;

public class Menu : MonoBehaviour
{
    [SerializeField] private RectTransform _mainMenuPanel;

    private void Awake()
    {
        MainEventHandler.ListenForEventStream<StateChangeEvent>().Subscribe(OnStateChange).AddTo(this);
    }

    private void OnStateChange(StateChangeEvent stateChangeEvent)
    {
        bool isMenu = stateChangeEvent.State == EState.Menu;
        _mainMenuPanel.gameObject.SetActive(isMenu);
    }
}
