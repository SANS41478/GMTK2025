namespace Script.GlobalInterface.BuffInterface
{
    public interface IBuff
    {
        string BuffId { get; }
        void OnApply(IBuffContext context);
        void OnRemove(IBuffContext context);
    }
}