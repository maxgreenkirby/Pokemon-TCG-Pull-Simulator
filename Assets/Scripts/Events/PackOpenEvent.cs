public struct PackOpenEvent: IMainEvent
{
    public Pack Pack;

    public PackOpenEvent(Pack pack)
    {
        Pack = pack;
    }
}
