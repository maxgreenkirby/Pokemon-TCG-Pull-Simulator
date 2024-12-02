public struct CardSelectEvent: IMainEvent
{
    public WorldCard Card;

    public CardSelectEvent(WorldCard card)
    {
        Card = card;
    }
}
