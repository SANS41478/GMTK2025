// using Space.GlobalInterface.PipelineInterface;
// namespace Space.PipelineFramework.Simple.Example.DamagePipeLine
// {
//     /// <summary>
//     /// 基础伤害管道
//     /// </summary>
//     public class BaseDamageCalculate : APipelineStage<BaseDamageCalculate>
//     {
//         public override void Execute(IPipelineContext context)
//         {
//             Player player = context.GetSharedData<Player>("DamageCaster");
//             int damage= player.GetSkillDamage();
//             context.SetSharedData("Damage", damage);
//         }
//     }
// }