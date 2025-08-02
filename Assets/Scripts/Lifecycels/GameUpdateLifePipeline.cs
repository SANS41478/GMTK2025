using Space.GlobalInterface.Lifecycle;
namespace Lifecycels
{
    /// <summary>
    ///     游戏帧的更新
    /// </summary>
    public enum GameUpdateLifePipeline
    {
        
        /// <summary>
        ///     拿走方块
        /// </summary>
        TakeCube = 5,
        /// <summary>
        ///     影像移动判断
        /// </summary>
        ClipMoveCharge = 10,
        /// <summary>
        ///     影像移动
        /// </summary>
        ClipMove = 15,

        /// <summary>
        ///     录制开始结束倒反倍速暂停等
        /// </summary>
        ClipAction = 25,
        /// <summary>
        ///     玩家移动判断
        /// </summary>
        PlayerMoveCharge = 30,
        /// <summary>
        ///     玩家移动
        /// </summary>
        PlayerMove = 35,

        
        /// <summary>
        ///     排挤
        /// </summary>
        ClipPush = 40,
        /// <summary>
        ///     释放影像
        /// </summary>
        PlayClip = 50,
        /// <summary>
        ///     放置方块
        /// </summary>
        PutCube = 60,

        Refresh = 65,

    }

    /// <summary>
    ///     暂时只写一个
    ///     后面有重复再拆吧
    /// </summary>
    public interface IGameLifecycleAction : ILifecycleSubscriber
    {
        public void Update(ILifecycleManager.UpdateContext ctx);
    }
    public interface IPlayerMoveCharge : ILifecycleSubscriber
    {
        public void Update(ILifecycleManager.UpdateContext ctx);
    }
    public interface IPlayerMove : ILifecycleSubscriber
    {
        public void Update(ILifecycleManager.UpdateContext ctx);
    }
    public interface IClipMoveCharge : ILifecycleSubscriber
    {
        public void Update(ILifecycleManager.UpdateContext ctx);
    }
    public interface IClipMove : ILifecycleSubscriber
    {
        public void Update(ILifecycleManager.UpdateContext ctx);
    }
    public interface IClipAction : ILifecycleSubscriber
    {
        public void Update(ILifecycleManager.UpdateContext ctx);
    }
    public interface ITakeCube : ILifecycleSubscriber
    {
        public void Update(ILifecycleManager.UpdateContext ctx);
    }
    public interface IPlayClip : ILifecycleSubscriber
    {
        public void Update(ILifecycleManager.UpdateContext ctx);
    }
    public interface IPutCube : ILifecycleSubscriber
    {
        public void Update(ILifecycleManager.UpdateContext ctx);
    }
    public interface IClipPush : ILifecycleSubscriber
    {
        public void Update(ILifecycleManager.UpdateContext ctx);
    }

    public interface IInputRefresh : ILifecycleSubscriber
    {
        public void Refresh();
    }


}