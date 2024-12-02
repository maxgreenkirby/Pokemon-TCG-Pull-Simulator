public struct PackSelectEvent: IMainEvent
{
    public Pack Pack;

    public PackSelectEvent(Pack pack)
    {
        Pack = pack;
    }
}
