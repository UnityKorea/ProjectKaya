
# ProjectKaya  
![image](https://user-images.githubusercontent.com/33303599/132323335-2ffb8e2c-600a-4672-8a60-14c312b16aeb.png)  
  
프로젝트 카야는 유니티 코리아에서 진행하는 URP를 활용한 모바일 예제 프로젝트 입니다.  
공개된 repository는 지속적으로 업데이트 되며, 사용자가 프로젝트에 기여하는 것도 가능합니다.   
Mobile Platform을 위한 추가적인 최적화는 [모바일 게임 성능 최적화 01] [모바일 게임 성능 최적화 02] 문서를 참고하시기 바랍니다.   


프로젝트에 대한 보다 많은 정보는 [Unity Webinar]와 [UniteSeoul 2020 Session] 영상을 참고 바랍니다.  

<p align="center">
<a href="https://youtu.be/QqTeElxbTA0" target="_blank">
<img src="https://img.youtube.com/vi/QqTeElxbTA0/0.jpg" style="margin: 1em; width:45%">
</a>

<a href="https://youtu.be/SJBRPsziteQ" target="_blank">
<img src="https://img.youtube.com/vi/SJBRPsziteQ/0.jpg" style="margin: 1em; width:45%">
</a>
</p>

###### Project Kaya는 Unity Technologies Korea의 URP 모바일 플랫폼을 사용하는 Unity 사용자를 위한 예시 프로젝트입니다. 유니티 프로젝트 내에서 본 프로젝트의 자원을 상업적/비 상업적으로 사용하는 것은 허용되지만 재배포는 허용되지 않습니다. 이 프로젝트의 저작권은 유니티코리아에 있습니다. 라이센스와 관련한 자세한 내용은 프로젝트내의 [Asset End User License Agreement] 문서를 참고해주세요.   
###### Project Kaya is an example project for unity users using  URP mobile platform from Unity Technologies Korea. Commercial/non-commercial use of this project's resources is permitted in the unity project, but redistribution is not permitted. All right reserved by copyrights of this project belong to Unity Korea.For further details, please refer to the [Asset End User License Agreement] document in the project.   

---
## Requirement  
- at least Unity 2021.1.13f1(Android module required) or above  
- [URP 11.0]   
- Android Platform module Required  
- Vulkan API supported(Visual Effect Graph를 사용하지 않는다면 ES 3.0이상)  
- LFS Required (GitHub 리포지토리에서 다운로드시 Texture 미포함) 
- 윈도우 터미널, cmd, git bash 등에서 명령어 실행 ```git clone https://github.com/UnityKorea/ProjectKaya```  

### editor setting  
![image](https://user-images.githubusercontent.com/33303599/133048003-a38cb6cf-04b7-4670-833d-cf9ecd193a22.png)  
Kaya project는 android 플랫폼을 기준으로 작업되고 있습니다. 이를 위해서 editor를 이와 같은 환경으로 셋팅해주어야 합니다. 커맨드라인 인자 추가에 -force-vulkan을 입력해 vulkan api로 동작하도록 설정합니다.   
![image](https://user-images.githubusercontent.com/33303599/135035994-c7b63346-3e73-4d49-8b49-49caabd6894d.png)  
플랫폼을 android로 선택합니다(에디터 설치시에 android 모듈이 설치되어야 합니다  
![image](https://user-images.githubusercontent.com/33303599/135036095-4d0c0222-89cc-43b0-83fc-aed965a11a69.png)  
## Project Setting  
### Renderer setting
#### Anti Aliasing  
모바일 프로젝트에서 사용하는 MSAA 옵션은 프로젝트 상황에 따라 적절하게 조절해주는것이 좋습니다. 아래는 MSAA 샘플링 수에 따른 메시 외곽의 퀄리티 비교입니다. quality setting이 Low, medium, high에 따라 아래와 같이 변경됩니다.  
![image](https://user-images.githubusercontent.com/33303599/136010444-07b126f3-a70b-4381-a52b-fcd8630167af.png)  

### Resource Compression  
대부분의 Texture compression은 [ASTC] 를 사용하고 있습니다.  
일부 리소스의 경우 압축하지 않은 RGB24 혹은 RGBA32입니다.  
![image](https://user-images.githubusercontent.com/33303599/132826800-5ec62cdb-d038-4847-9660-ad9879a9b69d.png)  
_- 2048 normal texture를 ASTC12x12(0.6MB)으로 압축한것(좌)과 ASTC6x6(2.4MB)로 압축한 결과(우) 비교  
Albedo texture의 경우보다 normal texture에서 이런 증상이 두드러지며 이럴경우는 리소스 압축 포맷과 옵션을 직접 선택하는 것을 권장합니다._  
## Scene List  
Lobby scene은 로비 구현에 필요한 연출과 shader 예제를 제공하고 있습니다.  
Animation type은 Generic으로 mechanim으로 구성되어 있습니다.  
  
### Lobby Scene
----------------------------  
#### FPS Counter & information display  
![image](https://user-images.githubusercontent.com/33303599/135036207-4f35c27e-f50f-43a0-8613-7cae79fe5447.png)  
- Frame Rate Counter : 화면 왼측 상단에는 현재 Frame Rate에 대한 정보를 밀리세컨(millisecond, ms)과 프레임으로 표시되고 있습니다.   
- Display pixel Resoultion : Rendering 되고 있는 현재 해상도를 보여줍니다.  
- Graphics API : 현재 렌더링 되고 있는 Graphics API 정보를 보여줍니다.   
#### Cinemachine  
Lobby Scene에서 카메라 전환은 Cinemachine을 활용해서 이루어지고 있습니다. 카메라의 Priority값을 UI에서 바꿔줌으로써 카메라간 전환이 이루어지게 됩니다.   
![image](https://user-images.githubusercontent.com/33303599/133052473-d765f541-a6ed-469f-b6d4-0036be3f4c18.png)  
Scene에 CinemachineVirtualCamera를 배치하면 카메라는 해당 버추얼 카메라의 포지션으로 이동하게 됩니다.   
![kayaCinemachine](https://user-images.githubusercontent.com/33303599/133038603-8427de53-bc4b-4c95-b415-5661d6afb4ce.gif)  
  - 1번 카메라 : 얼굴 근접 CM Face Shot. 1번 카메라에만 Depth of Field가 적용되어 있습니다.  
![image](https://user-images.githubusercontent.com/33303599/135036376-6bcccff5-583a-4534-9f9f-50b6d7178210.png)  
  - 2번 카메라 : 전신 CM FullBody shot  
![image](https://user-images.githubusercontent.com/33303599/135036388-4832901c-1a81-4774-b409-7cf06a9fcddb.png)  
  - 3번 카메라 : 스킬 CM Skill Shot  
![image](https://user-images.githubusercontent.com/33303599/135036409-f38029eb-bc78-4f36-adfe-bf6bea23f073.png)  
  
### Realtime Reflection   
Render texture와 shader custom을 통한 실시간 reflection을 구현한 예제입니다.  
![image](https://user-images.githubusercontent.com/33303599/135037058-8404b58e-22da-416d-8b07-d5ac64934cdd.png)  
![image](https://user-images.githubusercontent.com/33303599/135055680-c36ab1b8-e85d-40e3-b19e-12592c462e57.png)  
- Resoultion Mutiplier : 렌더링되는 반사이미지의 해상도를 설정  
- Clip Plane Offset : 반사되는 이미지의 시작점을 설정  
- Reflect Layer : 반사를 적용할 오브젝트 레이어를 선택  
- Draw Dithering : 캐릭터 디더링을 반사에 그릴지 여부  
  
![image](https://user-images.githubusercontent.com/33303599/135055375-924ad498-233b-49e2-8f61-a0bd33d605bd.png)  
### PBR Custom Shader(Shader Graph)  
----------------------------  
![image](https://user-images.githubusercontent.com/33303599/135037941-8754a264-e9e6-4da9-8fea-2e8f9122cf58.png)  
kaya에 쓰이는 기본 shader는 URP Lit shader를 기반으로 shader graph에서 제작한 셰이더가 쓰이고 있습니다.   
하나의 캐릭터 금속, 천, 가죽등 다양한 재질을 표현하기 위해 캐릭터 컨셉에서부터 이런 점을 고려하여 진행하였습니다.   
![image](https://user-images.githubusercontent.com/33303599/135037221-a71e5a9c-e64d-4b04-8676-1d02244c96b5.png)  
Shader Graph를 사용해서 Lit shader의 metallic과 smoothness, AO를 하나의 mask texture로 사용하게 되었으며, smoothness 값은 remap으로 처리하고 있습니다. 
### Character Object Overdraw(Shader Graph)  
![image](https://user-images.githubusercontent.com/33303599/135214608-f9313255-e593-4655-ad54-d332d7f6131d.png)  
캐릭터가 오브젝트에 겹쳐질때의 표시는 Renderer의 Render Features를 사용해 구현했습니다. 자세한 내용은 [Universal Rendering Examples]에서 확인할 수 있습니다.  
![image](https://user-images.githubusercontent.com/33303599/135214183-fefdd1a7-21a5-4495-8a4a-7bc37aa61c8a.png)  
Character Layer만 그리지 않고 Dither를 Depth Test후 opaque를 그린뒤(AfterRenderingOpaques) 나머지를 그리게 설정되어 있습니다.    
### Hair shader(Shader Graph)  
----------------------------  
#### UTKTemplate/URPHairKajiyaKay  
![kaya01](https://user-images.githubusercontent.com/33303599/135043964-720a90af-bb83-41bd-9098-7a3aa19708a4.gif)  
헤어셰이더에서 많이 사용되는 UV를 세로로 펴지 사용하는 방식이 아닌 flowmap을 사용해 라이팅을 구현한 예제입니다. flowmap으로 헤어의 방향을 기록하고 shiftmap으로 하이라이트의 위치를 조절할 수 있습니다.  
flowmap은 후디니에서 LabsFlowmap 노드를 사용해서 머리카락의 방향대로 vector을 정하고 그걸 렌더링해서 사용했습니다.  
![image](https://user-images.githubusercontent.com/33303599/136017237-c981c3e8-aeb5-4ce5-b97c-c34bcd50a6ca.png)  
사용시에 텍스쳐의 sRGB옵션(Gamma correction)을 끄고 사용해야 합니다.  
![image](https://user-images.githubusercontent.com/33303599/136018431-3f5c4b87-33bf-4c7a-9498-bbceabe878c9.png)  

이 셰이더는 addlight, addlightshadow 까지 모두 지원합니다.  
![image](https://user-images.githubusercontent.com/33303599/135622097-206fa0f1-13c3-466b-a318-89e962469bad.png)  
#### Shader Graphs/KajiyaKay  
![image](https://user-images.githubusercontent.com/33303599/135035913-072f97b9-72f3-400d-a64a-bfa81719d604.png)  
위방식과 다르게 Shader Graph로 작성되어있으며, 두개의 하이라이트를 조절해 헤어의 하이라이트를 표현하는 방식입니다.   

### Skin shader(Shader Graph)  
----------------------------  
![image](https://user-images.githubusercontent.com/33303599/135212110-1222b0f1-c557-459e-9482-8476a8f20156.png)  
SSS(SubSurface Scattering)의 구현은 Shader Graph를 사용해 구현되었습니다. Skin Texture의 Alpha Channel이 Thickness map으로 사용됩니다.  

## Animation Setting  
### Rig Setting  
Animation Setting은 Generic을 사용하고 있습니다. 유니티의 Humanoid는 애니메이션 리타겟팅을 목적으로 하지 않는 경우를 제외하고는 권장하지 않습니다. 제너릭과 휴머노이드의 최적화 관련 문서는 [unity forum]의 문서를 참조하세요.캐릭터의 Rig setting은 Model의 optimize Game Objects를 클릭해 성능을 높이며, 사용하는 무기 슬롯만 하이라키에 노출하게 됩니다([Extra Transforms to Expose])  
![image](https://user-images.githubusercontent.com/33303599/135617460-a70c9de6-0e69-4b8a-8f11-cb494fe125c8.png)
본의 갯수는 88개 입니다.  

## VFX Setting  
### Scene VFX  
![image](https://user-images.githubusercontent.com/33303599/135985372-e6a9ae89-7907-4b0b-bd9b-ffcd4034c27b.png)  
배경에 쓰인 VFX Graph는 낮에는 벛꽃, 밤에는 불씨가 흩날리며 추가로 밤엔 불길을 VFX Graph로 구현했습니다.  
![image](https://user-images.githubusercontent.com/33303599/135985065-82e2c403-974b-447e-bf15-e31b071c8016.png)  
 
### Character VFX  
캐릭터 1번과 2번 슬롯의 스킬에는 캐릭터 이펙트가 붙어있습니다. 이 이펙트는 shuriken이 아닌 Visual effect graph로 제작되어 있습니다.  
![UTKTemplate01_skillv1_3](https://user-images.githubusercontent.com/33303599/136010021-6196df85-12ec-4765-9d62-9077d0dedc6f.gif)  
실제 모바일 빌드시 OpenGL ES 3.2 환경에서도 제대로 출력되지 않는 부분들이 있습니다.(vulkan 권장)  
![image](https://user-images.githubusercontent.com/33303599/136010929-f6f3b48a-4321-427a-87ba-a6e988217300.png)

---------------------------------------
![UTK_LobyEffectcherry_re](https://user-images.githubusercontent.com/33303599/136016992-ab0e663d-e509-4ea1-9cde-6b0d82bd3ce5.gif)  
** Unity Technologies Korea Evangelism dev 2021

[모바일 게임 성능 최적화 01]: https://blog.unity.com/kr/technology/optimize-your-mobile-game-performance-tips-on-profiling-memory-and-code-architecture  
[모바일 게임 성능 최적화 02]: https://blog.unity.com/kr/technology/optimize-your-mobile-game-performance-get-expert-tips-on-physics-ui-and-audio-settings  
[Unity Webinar]: https://youtu.be/QqTeElxbTA0  
[UniteSeoul 2020 Session]: https://youtu.be/SJBRPsziteQ  
[Asset End User License Agreement]: https://github.com/UnityKorea/ProjectKaya/blob/main/Asset%20End%20User%20License%20Agreement.pdf  
[URP 11.0]: https://docs.unity3d.com/Packages/com.unity.render-pipelines.universal@11.0/changelog/CHANGELOG.html  
[ASTC]: https://en.wikipedia.org/wiki/Adaptive_scalable_texture_compression  
[Universal Rendering Examples]: https://github.com/Unity-Technologies/UniversalRenderingExamples  
[unity forum]: https://forum.unity.com/threads/using-humanoid-rigs-in-2020.923771/  
[Extra Transforms to Expose]: https://docs.unity3d.com/kr/2019.4/Manual/FBXImporter-Rig.html
