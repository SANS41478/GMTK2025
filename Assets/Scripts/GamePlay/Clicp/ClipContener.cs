using System.Collections.Generic;
using GamePlay;
/// <summary>
///     录像容器
/// </summary>
public class ClipContener
{
    public Dictionary<string, IList<IList<IMoveEventData>>> dataDict = new Dictionary<string, IList<IList<IMoveEventData>>>();
    public List<IRecordAble> IRecordAblesList = new List<IRecordAble>();
}