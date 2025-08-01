using Space.GlobalInterface.PipelineInterface;
namespace Space.PipelineFramework
{
    public static class GlobalPipelineStageFactory
    {
        public static IPipelineStageFactory Instance = FrameworkFactory.GetInstance<IPipelineStageFactory>();
    }
}