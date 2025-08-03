using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class GlobalMaskFeature : ScriptableRendererFeature
{
    class GlobalMaskPass : ScriptableRenderPass
    {
        static readonly string kProfilerTag = "Global Mask Pass";
        static readonly int BlendModeID = Shader.PropertyToID("_BlendMode");

        Mesh            fullscreenMesh;
        Material        maskMaterial;
        GlobalMaskSettings settings;

        public GlobalMaskPass(Material material, GlobalMaskSettings settings)
        {
            maskMaterial    = material;
            this.settings   = settings;
            renderPassEvent = settings.passEvent;
            fullscreenMesh  = GenerateFullscreenMesh();
        }

        public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
        {
            if (maskMaterial == null || !maskMaterial.shader.isSupported)
                return;

            var cmd = CommandBufferPool.Get(kProfilerTag);
            
            var renderer = renderingData.cameraData.renderer;
            var cameraTarget = renderer.cameraColorTarget;

            // 创建变换矩阵：缩放 + 平移
            Matrix4x4 transform = Matrix4x4.TRS(
                new Vector3(settings.maskOffset.x, settings.maskOffset.y, 0), // 位置偏移
                Quaternion.identity,                                          // 无旋转
                new Vector3(settings.maskScale.x, settings.maskScale.y, 1)   // 缩放
            );

            // 检查当前材质是否有足够的pass
            int passCount = maskMaterial.passCount;
            
            // 根据材质的 _BlendMode 属性选择 shader pass (0 for Normal, 1 for Multiply)
            int shaderPass = 0; // 默认使用第一个pass
            
            if (passCount > 1)
            {
                // 只有当材质有多个pass时才尝试切换
                shaderPass = maskMaterial.GetFloat(BlendModeID) < 0.5f ? 0 : 1;
            }

            // 直接在当前渲染目标上绘制遮罩
            cmd.SetRenderTarget(cameraTarget);
            cmd.DrawMesh(fullscreenMesh, transform, maskMaterial, 0, shaderPass);

            context.ExecuteCommandBuffer(cmd);
            CommandBufferPool.Release(cmd);
        }

        public void Dispose()
        {
            if (fullscreenMesh != null)
            {
                if (Application.isPlaying)
                    Object.Destroy(fullscreenMesh);
                else
                    Object.DestroyImmediate(fullscreenMesh);
                fullscreenMesh = null;
            }
        }

        Mesh GenerateFullscreenMesh()
        {
            var mesh = new Mesh { name = "Fullscreen Quad" };
            mesh.SetVertices(new[]
            {
                new Vector3(-1, -1, 0),
                new Vector3( 1, -1, 0),
                new Vector3(-1,  1, 0),
                new Vector3( 1,  1, 0),
            });
            mesh.SetUVs(0, new[]
            {
                new Vector2(0, 0),
                new Vector2(1, 0),
                new Vector2(0, 1),
                new Vector2(1, 1),
            });
            mesh.SetIndices(new[] { 0, 2, 1, 2, 3, 1 }, MeshTopology.Triangles, 0);
            mesh.UploadMeshData(true);
            return mesh;
        }
    }

    [System.Serializable]
    public class GlobalMaskSettings
    {
        public RenderPassEvent passEvent = RenderPassEvent.AfterRendering + 1;
        public Material        maskMaterial = null;
        [Header("Mask Transform")]
        [Tooltip("遮罩缩放 (1,1) = 铺满屏幕，值越大遮罩越大")]
        public Vector2         maskScale = Vector2.one;
        [Tooltip("遮罩偏移 屏幕空间坐标")]
        public Vector2         maskOffset = Vector2.zero;
    }

    public GlobalMaskSettings settings = new GlobalMaskSettings();
    GlobalMaskPass maskPass;

    public override void Create()
    {
        if (settings.maskMaterial == null)
        {
            Debug.LogError("GlobalMaskFeature: 未指定 Mask Material");
            return;
        }
        
        // 确保材质使用的是正确的着色器
        if (!settings.maskMaterial.shader.name.Contains("GlobalMask"))
        {
            Debug.LogWarning("GlobalMaskFeature: 指定的材质不使用GlobalMask着色器！当前使用: " + settings.maskMaterial.shader.name);
        }
        
        maskPass = new GlobalMaskPass(settings.maskMaterial, settings)
        {
            renderPassEvent = settings.passEvent
        };
    }

    public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
    {
        if (maskPass == null)
            return;
        renderer.EnqueuePass(maskPass);
    }

    protected override void Dispose(bool disposing)
    {
        maskPass?.Dispose();
        maskPass = null;
    }
}
