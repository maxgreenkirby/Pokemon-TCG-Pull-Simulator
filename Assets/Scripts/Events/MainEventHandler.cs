using System;
using UniRx;

public static class MainEventHandler
{
    private static Action<IMainEvent> _eventStream = delegate { };

    public static IObservable<T> ListenForEventStream<T>() where T : IMainEvent
    {
        return Observable.FromEvent<IMainEvent>(
                h => _eventStream += h,
                h => _eventStream -= h).OfType<IMainEvent, T>();
    }

    public static bool AddToEventStream(IMainEvent event1)
    {
        if (_eventStream == null) return false;
        _eventStream(event1);
        return true;
    }
}

