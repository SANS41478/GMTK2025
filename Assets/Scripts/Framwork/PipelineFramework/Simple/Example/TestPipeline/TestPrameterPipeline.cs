using Space.GlobalInterface.PipelineInterface;
using UnityEngine;
namespace Space.PipelineFramework.Simple.Example.Test
{
    /// <summary>
    ///     此处用了Attribute 可以通过ReflectionPipeLineFactory调用
    ///     也可也用DictionaryFactory 看喜好选择
    /// </summary>
    public class TestParamsPipeline : APipelineStage<TestParamsPipeline, TestContest>
    {
        private string addedMassage = "";
        private int index ;
        public override IPipelineStage<TestContest> SetParams(params object[] parameters)
        {
            TestParamsPipelineCreateInfo info = parameters[0]  as TestParamsPipelineCreateInfo;
            DefaultPriority = info.DefaultPriority;
            addedMassage = info.AddedMassage;
            return base.SetParams(parameters);
        }
        public override void Execute(TestContest context)
        {
            index = context.index;
            string res = context.massage;
            Debug.Log($"{index}: {res}  ---- {addedMassage}");
            context.index++;
        }
        public class TestParamsPipelineCreateInfo
        {
            public string AddedMassage;
            public int DefaultPriority;
        }
    }
}