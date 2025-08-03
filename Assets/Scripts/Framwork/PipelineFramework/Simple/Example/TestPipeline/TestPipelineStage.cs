using Space.GlobalInterface.PipelineInterface;
using UnityEngine;
namespace Space.PipelineFramework.Simple.Example.Test
{

    public class TestContest : IPipelineContext
    {
        public  int index;
        public string massage;
    }
    public class TestPipelineStage : APipelineStage<TestPipelineStage, TestContest>
    {
        private int index ;
        public int Priority {
            get;
        }
        public override void Execute(TestContest context)
        {
            index = context.index;
            string res = context.massage;
            Debug.Log($"{index}: {res}");
            index++;
            context.index = index;
        }
    }
}