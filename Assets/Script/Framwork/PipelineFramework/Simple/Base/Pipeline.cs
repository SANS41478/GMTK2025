using System.Collections;
using Space.Collections;
using Space.GlobalInterface.PipelineInterface;
namespace Space.PipelineFramework.Simple
{
    /// <summary>
    /// 线性的pipeline 
    /// </summary>
    public class Pipeline<T> : IPipeline<T>  where T : IPipelineContext
    {
        SortedList<int , IPipelineStage<T>> pipelineStages = new SortedList<int , IPipelineStage<T>>();
        public void AddStage(IPipelineStage<T> stage)
        {
            pipelineStages.Add(stage.DefaultPriority, stage);
        }
        public void AddStage(IPipelineStage<T> stage, int priority)
        {
            pipelineStages.Add(priority, stage);
        }
        public void RemoveStage(IPipelineStage<T> stage)
        {
            pipelineStages.Remove(stage);
        }
        public void Execute(T context)
        {
            foreach (var pipelineStage in pipelineStages.GetSorted())
            {
                pipelineStage.Execute(context);
            }
        }
    }
}