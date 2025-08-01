using UnityEngine;
namespace Space.PipelineFramework.Simple.Example.DamagePipeLine
{
    /// <summary>
    /// 随便写一点了比较潦草
    /// </summary>
    public class Player
    {
        public int Level=1;
        /// <summary>
        /// 直接简单实现了
        /// </summary>
        public int GetSkillDamage()
        {
            return (Level + Random.Range(1,10)) * 3;
        }
        public void costDamage(int damage)
        {
            Debug.Log($"被造成了: {damage}伤害");
        }
    }
}