#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.ProjectWindowCallback;
using ShaderKeywordFilter = UnityEditor.ShaderKeywordFilter;
#endif
using System;
using UnityEngine.Scripting.APIUpdating;
using UnityEngine.Assertions;
using UnityEngine.Experimental.Rendering;

namespace UnityEngine.Rendering.MotoyincLab
{
    [Serializable, ReloadGroup, ExcludeFromPreset]
    public class MotoyincLabRendererData : ScriptableRendererData
    {
        public PostProcessData postProcessData = null;
        
        protected override ScriptableRenderer Create()
        {
            // Renderer并不存在RenderData里，虽然是由RenderData创建的
            ReloadAllNullProperties();
            return new MotoyincLabRenderer(this);
        }
        
        // 创建Asset文件
#if UNITY_EDITOR
        internal class CreateMotoyincLabRendererAsset : EndNameEditAction
        {
            public override void Action(int instanceId, string pathName, string resourceFile)
            {
                var instance = MotoyincLabRenderPipelineAsset.CreateRendererData(pathName, RendererType.MotoyincLabRenderer, false) as MotoyincLabRendererData;
                Selection.activeObject = instance;
            }
        }
    
        [MenuItem("Assets/Create/MotoyincLabRP/MotoyincLabRP RendererData")]
        static void CreateMotoyincLabRendererData()
        {
            ProjectWindowUtil.StartNameEditingIfProjectWindowExists(0, CreateInstance<CreateMotoyincLabRendererAsset>(), "new MotoyincLabRP RendererData.asset", null, null);
        }
#endif
        private void ReloadAllNullProperties()
        {
#if UNITY_EDITOR
            ResourceReloader.TryReloadAllNullIn(this, MotoyincLabRenderPipelineAsset.packagePath);

            if (postProcessData != null)
                ResourceReloader.TryReloadAllNullIn(postProcessData, MotoyincLabRenderPipelineAsset.packagePath);
#endif
        }

    }
}
