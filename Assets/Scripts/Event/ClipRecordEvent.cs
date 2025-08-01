using Space.GlobalInterface.EventInterface;
namespace Event
{
    public struct ClipRecordEvent : IEventData
    {
        /// <summary>
        ///     true 为开始录制 false为结束录制
        /// </summary>
        public bool recordModel;
    }
}