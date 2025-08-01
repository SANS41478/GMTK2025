using Space.GlobalInterface.PipelineInterface;
using UnityEngine;
namespace Space.PipelineFramework.Simple.Example.Test
{

    public class TestContest : IPipelineContext
    {
        public string massage;
        public  int index;
    }
    public class TestPipelineStage : APipelineStage<TestPipelineStage,TestContest>
    {
        public int Priority {
            get;
        }
        private int index=0;
        public override void Execute(TestContest context)
        {
            index=context.index;
            string res = context.massage;
            Debug.Log($"{index}: {res}");
            index++;
            context.index = index;
        }
    }
}