using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;

internal class GraphicsAPIValidate
{
	[InitializeOnLoadMethod]
	private static void Check()
	{
		const string kCheckGraphicsAPIValidateState = "ProjectKaya.GraphicsAPIValidate";
		
		// 에디터 세션 당 한 번만 확인
		if (!SessionState.GetBool(kCheckGraphicsAPIValidateState, false ))
		{
			SessionState.SetBool(kCheckGraphicsAPIValidateState, true);

			if (SystemInfo.graphicsDeviceType != GraphicsDeviceType.Vulkan)
			{
				if (EditorUtility.DisplayDialog("주의",
					"본 프로젝트는 Vulkan 그래픽스 백엔드로 구현되었습니다. UnityHub에서 커맨드라인을 통해 Vulkan 모드로 실행해주세요.",
					"세팅 방법 보기", "닫기"))
				{
					Application.OpenURL("https://github.com/UnityKorea/ProjectKaya#editor-setting"); 
				}
			}
		} 
	}

	[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSplashScreen)]
	private static void CheckInPlaymode()
	{
		if (SystemInfo.graphicsDeviceType != GraphicsDeviceType.Vulkan)
		{
			Debug.LogWarning($"[현재 그래픽스 : {SystemInfo.graphicsDeviceType}] 본 프로젝트는 Vulkan 그래픽스 백엔드로 구현되었습니다. UnityHub에서 커맨드라인을 통해 Vulkan 모드로 실행해주세요.");
		}
	}
}