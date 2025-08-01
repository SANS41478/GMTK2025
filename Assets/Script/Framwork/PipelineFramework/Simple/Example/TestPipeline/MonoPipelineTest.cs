// using System;
// using Space.GlobalInterface.PipelineInterface;
// using UnityEngine;
// namespace Space.PipelineFramework.Simple.Example.Test
// {
//     public class MonoPipelineTest : MonoBehaviour
//     {
//         private IPipeline pipeline;
//         private IPipelineContext pipelineContext;
//         private void Awake()
//         {
//             IPipelineFactory factory = GlobalPipelineStageFactory.Instance;
//             pipeline = FrameworkFactory.GetInstance<IPipeline>();
//             pipeline.AddStage(factory.CreatePipelineStage("Example/Test/TestParamsPipeline",new TestParamsPipeline.TestParamsPipelineCreateInfo()
//             {
//                 AddedMassage = "我是第一个信息，我的优先度是-1",
//                 DefaultPriority = -1
//             }));
//             pipeline.AddStage(factory.CreatePipelineStage("Example/Test/TestParamsPipeline",new TestParamsPipeline.TestParamsPipelineCreateInfo()
//             {
//                 AddedMassage = "我是第一个信息，我的优先度是1",
//                 DefaultPriority = 1
//             }));
//             pipeline.AddStage(factory.CreatePipelineStage("Example/Test/TestStage"));
//             pipeline.AddStage(factory.CreatePipelineStage("Example/Test/TestStage"));
//             pipeline.AddStage(factory.CreatePipelineStage("Example/Test/TestStage"));
//             pipeline.AddStage(factory.CreatePipelineStage("Example/Test/TestStage"));
//             pipelineContext = FrameworkFactory.GetInstance<IPipelineContext>();
//         }
//
//
//         private float timer = 0;
//         private int count = 0;
//         private void Update()
//         {
//             timer-=Time.deltaTime;
//             if (timer<0)
//             {
//                 pipelineContext.SetSharedData("TestCount", $"{count++}次管道测试");
//                 pipeline.Execute(pipelineContext);
//                 timer = 3f;
//                 //一般情况下这里可以保存一下需要的数据
//                 //使用后记得清除上下文
//                 pipelineContext.Clear();
//             }
//         }
//     }
// }