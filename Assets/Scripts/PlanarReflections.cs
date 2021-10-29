using System;
using UnityEngine.Experimental.Rendering;
using UnityEngine.Serialization;
//no math pack for 2020 yet
//using Unity.Mathematics; 
// Copied from BoatAttack Water System

namespace UnityEngine.Rendering.Universal
{

    [ExecuteAlways]
    [ExecuteInEditMode]
    public class PlanarReflections : MonoBehaviour
    {
        [Header("Dithering")]
        [Tooltip("URP Settings/ForwardRenderer/Dither")]
        [SerializeField] private ScriptableRendererFeature m_ditherRenderFeature;
        [Tooltip("반사에서도 프랍에 가려진 캐릭터를 디더링 상태로 그립니다")]
        [SerializeField] private bool m_drawDithering = true;
        
        [Serializable]
        public enum ResolutionMulltiplier
        {
            Full,
            Half,
            Quarter,
            Octa
        }

        [Serializable]
        public class PlanarReflectionSettings
        {
            public ResolutionMulltiplier m_ResolutionMultiplier = ResolutionMulltiplier.Half;
            public float m_ClipPlaneOffset = 0.07f;
            public LayerMask m_ReflectLayers = -1;
            public bool m_Shadows;
        }

        private bool _needToRefreshReflectionResolution;

        [SerializeField]
        public PlanarReflectionSettings m_settings = new PlanarReflectionSettings();

        public GameObject target;
        [FormerlySerializedAs("camOffset")] public float m_planeOffset;

        private static Camera _reflectionCamera;
        public RenderTexture _reflectionTexture;
        
        private readonly int _planarReflectionTextureId = Shader.PropertyToID("_PlanarReflectionTexture");
        private MeshRenderer _meshRenderer = default;

        //private int2 _oldReflectionTextureSize;
        public void RefreshReflectionResolution()
        {
            _needToRefreshReflectionResolution = true;
        }

        
        // public static event Action<ScriptableRenderContext, Camera> BeginPlanarReflections;

        private void OnEnable()
        {
            _needToRefreshReflectionResolution = true;
            RenderPipelineManager.beginCameraRendering += ExecutePlanarReflections;
            
            if (_meshRenderer == null)
            {
                _meshRenderer = GetComponent<MeshRenderer>();
            }
        }

        // Cleanup all the objects we possibly have created
        private void OnDisable()
        {
            Cleanup();
        }

        private void OnDestroy()
        {
            Cleanup();
        }

        private void Cleanup()
        {
            //Debug.Log("Cleanup");
            
            RenderPipelineManager.beginCameraRendering -= ExecutePlanarReflections;

            if(_reflectionCamera)
            {
                _reflectionCamera.targetTexture = null;
                SafeDestroy(_reflectionCamera.gameObject);
            }
            if (_reflectionTexture)
            {
                RenderTexture.ReleaseTemporary(_reflectionTexture);
            }
        }

        private static void SafeDestroy(Object obj)
        {
            if (Application.isEditor)
            {
                DestroyImmediate(obj);
            }
            else
            {
                Destroy(obj);
            }
        }

        private void UpdateCamera(Camera src, Camera dest)
        {
            if (dest == null) return;

            dest.CopyFrom(src);
            dest.useOcclusionCulling = false;
            if (dest.gameObject.TryGetComponent(out UniversalAdditionalCameraData camData))
            {
                camData.renderShadows = m_settings.m_Shadows; // turn off shadows for the reflection camera
            }
        }

        private void UpdateReflectionCamera(Camera realCamera)
        {
            //Debug.Log("UpdateReflectionCamera. _reflectionCamera = " + _reflectionCamera);
            
            if (_reflectionCamera == null)
                _reflectionCamera = CreateMirrorObjects();

            // find out the reflection plane: position and normal in world space
            Vector3 pos = Vector3.zero;
            Vector3 normal = Vector3.up;
            if (target != null)
            {
                pos = target.transform.position + Vector3.up * m_planeOffset;
                normal = target.transform.up;
            }

            UpdateCamera(realCamera, _reflectionCamera);

            // Render reflection
            // Reflect camera around reflection plane
            var d = -Vector3.Dot(normal, pos) - m_settings.m_ClipPlaneOffset;
            var reflectionPlane = new Vector4(normal.x, normal.y, normal.z, d);

            var reflection = Matrix4x4.identity;
            reflection *= Matrix4x4.Scale(new Vector3(1, -1, 1));

            CalculateReflectionMatrix(ref reflection, reflectionPlane);
            var oldPosition = realCamera.transform.position - new Vector3(0, pos.y * 2, 0);
            var newPosition = ReflectPosition(oldPosition);
            _reflectionCamera.transform.forward = Vector3.Scale(realCamera.transform.forward, new Vector3(1, -1, 1));
            _reflectionCamera.worldToCameraMatrix = realCamera.worldToCameraMatrix * reflection;

            // Setup oblique projection matrix so that near plane is our reflection
            // plane. This way we clip everything below/above it for free.
            var clipPlane = CameraSpacePlane(_reflectionCamera, pos - Vector3.up * 0.1f, normal, 1.0f);
            var projection = realCamera.CalculateObliqueMatrix(clipPlane);
            _reflectionCamera.projectionMatrix = projection;
            _reflectionCamera.cullingMask = m_settings.m_ReflectLayers; // never render water layer
            _reflectionCamera.transform.position = newPosition;
            
            //Debug.Log("Refl cam pos : " + _reflectionCamera.transform.position + " dir : " + _reflectionCamera.transform.forward);
            
        }

        // Calculates reflection matrix around the given plane
        private static void CalculateReflectionMatrix(ref Matrix4x4 reflectionMat, Vector4 plane)
        {
            reflectionMat.m00 = (1F - 2F * plane[0] * plane[0]);
            reflectionMat.m01 = (-2F * plane[0] * plane[1]);
            reflectionMat.m02 = (-2F * plane[0] * plane[2]);
            reflectionMat.m03 = (-2F * plane[3] * plane[0]);

            reflectionMat.m10 = (-2F * plane[1] * plane[0]);
            reflectionMat.m11 = (1F - 2F * plane[1] * plane[1]);
            reflectionMat.m12 = (-2F * plane[1] * plane[2]);
            reflectionMat.m13 = (-2F * plane[3] * plane[1]);

            reflectionMat.m20 = (-2F * plane[2] * plane[0]);
            reflectionMat.m21 = (-2F * plane[2] * plane[1]);
            reflectionMat.m22 = (1F - 2F * plane[2] * plane[2]);
            reflectionMat.m23 = (-2F * plane[3] * plane[2]);

            reflectionMat.m30 = 0F;
            reflectionMat.m31 = 0F;
            reflectionMat.m32 = 0F;
            reflectionMat.m33 = 1F;
        }

        private static Vector3 ReflectPosition(Vector3 pos)
        {
            var newPos = new Vector3(pos.x, -pos.y, pos.z);
            return newPos;
        }

        private float GetScaleValue()
        {
            switch(m_settings.m_ResolutionMultiplier)
            {
                case ResolutionMulltiplier.Full:
                    return 1f;
                case ResolutionMulltiplier.Half:
                    return 0.5f;
                case ResolutionMulltiplier.Quarter:
                    return 0.25f;
                case ResolutionMulltiplier.Octa:
                    return 0.125f;
                default:
                    return 0.5f; // default to half res
            }
        }

        // Given position/normal of the plane, calculates plane in camera space.
        private Vector4 CameraSpacePlane(Camera cam, Vector3 pos, Vector3 normal, float sideSign)
        {
            var offsetPos = pos + normal * m_settings.m_ClipPlaneOffset;
            var m = cam.worldToCameraMatrix;
            var cameraPosition = m.MultiplyPoint(offsetPos);
            var cameraNormal = m.MultiplyVector(normal).normalized * sideSign;
            return new Vector4(cameraNormal.x, cameraNormal.y, cameraNormal.z, -Vector3.Dot(cameraPosition, cameraNormal));
        }

        private Camera CreateMirrorObjects()
        {
            //Debug.Log("CreateMirrorObjects");

            var go = new GameObject("Planar Reflections");
            var reflectionCamera = go.AddComponent<Camera>();
            var cameraData = go.AddComponent(typeof(UniversalAdditionalCameraData)) as UniversalAdditionalCameraData;

#if UNITY_EDITOR
            Assertions.Assert.IsNotNull(cameraData);
#endif

            cameraData.requiresColorOption = CameraOverrideOption.Off;
            cameraData.requiresDepthOption = CameraOverrideOption.Off;
            //cameraData.SetRenderer(1);

            var t = transform;
            reflectionCamera.transform.SetPositionAndRotation(t.position, t.rotation);
            reflectionCamera.depth = -10;
            reflectionCamera.enabled = false;
            
            go.hideFlags = HideFlags.HideAndDontSave;

            return reflectionCamera;
        }

        private void PlanarReflectionTexture(Camera cam)
        {
            if (_reflectionTexture == null || _needToRefreshReflectionResolution)
            {
                var res = ReflectionResolution(cam, UniversalRenderPipeline.asset.renderScale);
                const bool useHdr10 = true;
                const RenderTextureFormat hdrFormat = useHdr10 ? RenderTextureFormat.RGB111110Float : RenderTextureFormat.DefaultHDR;
                _reflectionTexture = RenderTexture.GetTemporary((int)res.x, (int)res.y, 16,
                    GraphicsFormatUtility.GetGraphicsFormat(hdrFormat, true));
                
                Texture tex = _meshRenderer.sharedMaterial.GetTexture(_planarReflectionTextureId);
                if (!ReferenceEquals(tex, _reflectionTexture))
                {
                    _meshRenderer.sharedMaterial.SetTexture(_planarReflectionTextureId, _reflectionTexture);
                }
                
                _needToRefreshReflectionResolution = false;
            }
            _reflectionCamera.targetTexture =  _reflectionTexture;
        }

        private Vector2 ReflectionResolution(Camera cam, float scale)
        {
            var x = (int)(cam.pixelWidth * scale * GetScaleValue());
            var y = (int)(cam.pixelHeight * scale * GetScaleValue());
            return new Vector2(x, y);
        }

        private void ExecutePlanarReflections(ScriptableRenderContext context, Camera camera)
        {
            // we dont want to render planar reflections in reflections or previews
            if (camera.cameraType == CameraType.Reflection || camera.cameraType == CameraType.Preview)
                return;

            UpdateReflectionCamera(camera); // create reflected camera
            PlanarReflectionTexture(camera); // create and assign RenderTexture

            var data = new PlanarReflectionSettingData(); // save quality settings and lower them for the planar reflections
            data.Set(); // set quality settings

            // 디더링 끄고 RT에 그리고 다시 켜기
            if (!m_drawDithering && m_ditherRenderFeature != null)
            {
                m_ditherRenderFeature.SetActive(false);
            }
            
            UniversalRenderPipeline.RenderSingleCamera(context, _reflectionCamera); // render planar reflections
            
            if (!m_drawDithering && m_ditherRenderFeature != null)
            {
                m_ditherRenderFeature.SetActive(true);
            }

            data.Restore(); // restore the quality settings
        }

        private class PlanarReflectionSettingData
        {
            private readonly bool _fog;
            private readonly int _maxLod;
            private readonly float _lodBias;

            public PlanarReflectionSettingData()
            {
                _fog = RenderSettings.fog;
                _maxLod = QualitySettings.maximumLODLevel;
                _lodBias = QualitySettings.lodBias;
            }

            public void Set()
            {
                GL.invertCulling = true;
                RenderSettings.fog = false; // disable fog for now as it's incorrect with projection
                QualitySettings.maximumLODLevel = 1;
                QualitySettings.lodBias = _lodBias * 0.5f;
            }

            public void Restore()
            {
                GL.invertCulling = false;
                RenderSettings.fog = _fog;
                QualitySettings.maximumLODLevel = _maxLod;
                QualitySettings.lodBias = _lodBias;
            }
        }
    }
}
