namespace Space.GlobalInterface.Lifecycle
{
    /// <summary>
    /// 生命周期组件内的Stage
    /// 标记接口
    /// 实际上通过增减这个来实现生命周期的调控
    /// 但是具体如何调控由子类实现，接口不关心
    /// </summary>
    public interface ILifecyclePhase
    {
        public string PhaseName { get; }

    }
}