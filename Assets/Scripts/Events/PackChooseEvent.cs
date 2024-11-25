public struct PackChooseEvent: IMainEvent
{
    public Pack Pack;

    public PackChooseEvent(Pack pack)
    {
        Pack = pack;
    }
}
