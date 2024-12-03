public struct PackDragEvent : IMainEvent
{
    public Pack Pack;
    public bool IsDragging;

    public PackDragEvent(Pack pack, bool isDragging)
    {
        Pack = pack;
        IsDragging = isDragging;
    }
}