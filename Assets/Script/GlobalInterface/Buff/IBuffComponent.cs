using System.Collections.Generic;
namespace Script.GlobalInterface.BuffInterface
{
    /// <summary>
    /// buff组件的接口，基础的一些函数
    /// 获得位于这个组件内的buff
    /// 负责存储buff和管理生命周期
    /// </summary>
    public interface IBuffComponent
    {
        void AddBuff(string buffId);
        void RemoveBuff(string buffId);
        void Update(float deltaTime);
        // 查询接口
        bool HasBuff(string buffId);
        IEnumerable<IBuff> GetBuffs();
        // 扩展接口（小规模返回null）
    }
}