// using System.Collections.Generic;
// using Space.GlobalInterface.PipelineInterface;
// namespace Space.PipelineFramework.Simple.Example.DamagePipeLine
// {
//
//     /// <summary>
//     /// 封装了一层具有默认管线的管道
//     /// ShareData{
//     ///   "Damage" int  伤害，由伤害计算写入
//     ///   "DamageCastor" Player 伤害产生者
//     ///   "DamageGetter" Player 受到伤害的人
//     ///   "DamageTags" Ienumrator string的可迭代对象 伤害的tags
//     ///   "DamageBooomRate" float 暴击的概率
//     ///   ""
//     /// }
//     /// 基础伤害计算(写入伤害数值)----->应用伤害类型（如物理、火焰）----->暴击判断(伤害翻倍)------>
//     /// 伤害抗性(免疫)------>伤害抗性(格挡)------>伤害产生(造成伤害)
//     /// </summary>
//     public class DamagePipeLine : IPipeline
//     {
//         IPipeline pipeline;
//         public DamagePipeLine()
//         {
//             var factory = GlobalPipelineStageFactory.Instance;
//             pipeline = FrameworkFactory.GetInstance<IPipeline>();
//             pipeline.AddStage(factory.CreatePipelineStage(""));
//         }
//         public void AddStage(IPipelineStage stage)
//         {
//             pipeline.AddStage(stage);
//         }
//         public void AddStage(IPipelineStage stage, int priority)
//         {
//             pipeline.AddStage(stage, priority);
//         }
//         public void RemoveStage(IPipelineStage stage)
//         {
//             pipeline.RemoveStage(stage);
//         }
//         public void Execute(IPipelineContext context)
//         {
//             pipeline.Execute(context);   
//         }
//     }
// }