using Space.GlobalInterface.EventInterface;
using UnityEngine;
public struct ClipePlayInfo : IEventData
{
    public int num;
    /// <summary>
    ///     是否循环
    /// </summary>
    public bool isCycles;
    public ClipePlayType playType;
    public Vector2Int clickPos;
    public bool creatMark;
    public bool keyDownMask;
    //其它参数
}

public enum ClipePlayType
{
    /// <summary>
    ///     播放某个片段
    /// </summary>
    Null,
    Play,
    Backword,
}