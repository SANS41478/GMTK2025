using System.Collections;
using System.Collections.Generic;
using Space.GlobalInterface.EventInterface;
using UnityEngine;

public struct ClipePlayInfo : IEventData
{
    public int num;
    public ClipePlayType playType;
    //其它参数
}

public enum ClipePlayType
{
    /// <summary>
    /// 播放某个片段
    /// </summary>
    Play,
    /// <summary>
    /// 删除某个片段
    /// </summary>
    Delete,
    /// <summary>
    /// TODO: 停止播放某个片段
    /// </summary>
    Stop,
    Choice
}