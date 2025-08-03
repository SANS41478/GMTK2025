using System.Collections;
using System.Collections.Generic;
using Space.GlobalInterface.EventInterface;
using UnityEngine;

public struct ClipePlayInfo : IEventData
{
    public int num;
    /// <summary>
    /// 是否循环
    /// </summary>
    public bool isCycles;
    public ClipePlayType playType;

    //其它参数
}

public enum ClipePlayType
{
    /// <summary>
    /// 播放某个片段
    /// </summary>
    Play,
    Backword
}