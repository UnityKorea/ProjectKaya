
# ProjectKaya  
![image](https://user-images.githubusercontent.com/33303599/132323335-2ffb8e2c-600a-4672-8a60-14c312b16aeb.png)  
프로젝트 카야는 유니티 코리아에서 진행하는 URP를 활용한 모바일 예제 프로젝트 입니다. 공개된 repository는 지속적으로 업데이트 되며, 사용자가 프로젝트에 기여하는 것도 가능합니다.   
Project Kaya is a mobile example project for using Unity URP by Unity Technologies Korea.  
## Requirement  
- Unity 2021.1.13f1(Android module required)  
- [URP 11.0] version needed  
- Android Platform  
- Vulkan API supported  
### editor setting  
![image](https://user-images.githubusercontent.com/33303599/133048003-a38cb6cf-04b7-4670-833d-cf9ecd193a22.png)  
Kaya project는 android 플랫폼을 기준으로 작업되고 있습니다. 이를 위해서 editor를 이와 같은 환경으로 셋팅해주어야 합니다. 커맨드라인 인자 추가에 -force-vulkan을 입력해 vulkan api로 동작하도록 설정합니다.   
![image](https://user-images.githubusercontent.com/33303599/135035994-c7b63346-3e73-4d49-8b49-49caabd6894d.png)  
플랫폼을 android로 선택합니다(에디터 설치시에 android 모듈이 설치되어야 합니다  
![image](https://user-images.githubusercontent.com/33303599/135036095-4d0c0222-89cc-43b0-83fc-aed965a11a69.png)  
## Resource Compression  
대부분의 Texture compression은 [ASTC] 를 사용하고 있습니다.  
일부 리소스의 경우 압축하지 않은 RGB24 혹은 RGBA32입니다.  
![image](https://user-images.githubusercontent.com/33303599/132826800-5ec62cdb-d038-4847-9660-ad9879a9b69d.png)  
_- 2048 normal texture를 ASTC12x12(0.6MB)으로 압축한것(좌)과 ASTC6x6(2.4MB)로 압축한 결과(우) 비교  
Albedo texture의 경우보다 normal texture에서 이런 증상이 두드러지며 이럴경우는 리소스 압축 포맷과 옵션을 직접 선택하는 것을 권장합니다._  
## Scene List  
Roby scene은 로비 구현에 필요한 연출과 shader 예제를 제공하고 있습니다.  
Animation type은 Generic으로 mechanim으로 구성되어 있습니다.  
### Roby Scene  
#### FPS Counter & information display  
![image](https://user-images.githubusercontent.com/33303599/135036207-4f35c27e-f50f-43a0-8613-7cae79fe5447.png)  
- Frame Rate Counter : 화면 왼측 상단에는 현재 Frame Rate에 대한 정보를 밀리세컨(millisecond, ms)과 프레임으로 표시되고 있습니다.   
- Display pixel Resoultion : Rendering 되고 있는 현재 해상도를 보여줍니다.  
- Graphics API : 현재 렌더링 되고 있는 Graphics API 정보를 보여줍니다.   
#### Cinemachine  
Roby Scene에서 카메라 전환은 Cinemachine을 활용해서 이루어지고 있습니다. 카메라의 Priority값을 UI에서 바꿔줌으로써 카메라간 전환이 이루어지게 됩니다.   
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
### PBR Custom Shader(Shader Graph)  
![image](https://user-images.githubusercontent.com/33303599/135037941-8754a264-e9e6-4da9-8fea-2e8f9122cf58.png)  
kaya에 쓰이는 기본 shader는 URP Lit shader를 기반으로 shader graph에서 제작한 셰이더가 쓰이고 있습니다.   
하나의 캐릭터 금속, 천, 가죽등 다양한 재질을 표현하기 위해 캐릭터 컨셉에서부터 이런 점을 고려하여 진행하였습니다.   
![image](https://user-images.githubusercontent.com/33303599/135037221-a71e5a9c-e64d-4b04-8676-1d02244c96b5.png)  
- Shader Graph를 사용해서 Lit shader의 metallic과 smoothness, AO를 하나의 mask texture로 사용하게 되었으며, smoothness 값은 remap으로 처리하고 있습니다.  
### hair shader(Shader Graph)  
#### UTKTemplate/URPHairKajiyaKay  
![kaya01](https://user-images.githubusercontent.com/33303599/135043964-720a90af-bb83-41bd-9098-7a3aa19708a4.gif)  
- HLSL code based shader  
- supported flowmap UV(does not need to vertical wrapped UVs)  
- supported additional light & additional light shadow  
- supported speucular shiftmap  
![image](https://user-images.githubusercontent.com/33303599/133017036-204d8e9f-37df-4ab0-a27d-8dcfbeb42e26.png)  
#### Shader Graphs/KajiyaKay  
![image](https://user-images.githubusercontent.com/33303599/135035913-072f97b9-72f3-400d-a64a-bfa81719d604.png)  
### Skin shader(Shader Graph)   


[URP 11.0]: https://docs.unity3d.com/Packages/com.unity.render-pipelines.universal@11.0/changelog/CHANGELOG.html
[ASTC]: https://en.wikipedia.org/wiki/Adaptive_scalable_texture_compression
