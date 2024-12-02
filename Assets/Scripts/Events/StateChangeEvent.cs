public struct StateChangeEvent : IMainEvent
{
    public EState State;

    public StateChangeEvent(EState state)
    {
        State = state;
    }
}
