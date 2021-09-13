
# ProjectKaya
![image](https://user-images.githubusercontent.com/33303599/132323335-2ffb8e2c-600a-4672-8a60-14c312b16aeb.png)


프로젝트 카야는 유니티 코리아에서 진행하는 URP를 활용한 모바일 예제 프로젝트 입니다. 공개된 repository는 지속적으로 업데이트 되며, 사용자가 프로젝트에 기여하는 것도 가능합니다. 

Project Kaya is a mobile example project for using Unity URP by Unity Technologies Korea.

## Requirement
- Unity 2021.1.13f1(Android module required)
- [URP 11.0] version needed
- Android Platform
- Vulkan API supported

- editor setting

![image](https://user-images.githubusercontent.com/33303599/133048003-a38cb6cf-04b7-4670-833d-cf9ecd193a22.png)

Kaya project는 android 플랫폼을 기준으로 작업되고 있습니다. 이를 위해서 editor를 이와 같은 환경으로 셋팅해주어야 합니다. 커맨드라인 인자 추가에 -force-vulkan을 입력해 vulkan api로 동작하도록 설정합니다. 

![image](https://user-images.githubusercontent.com/33303599/133015950-73405cb4-9ffd-4001-bf77-15a320f4172e.png)

플랫폼을 android로 선택합니다(에디터 설치시에 android 모듈이 설치되어야 합니다

![image](https://user-images.githubusercontent.com/33303599/133015976-bac16f85-cfd3-415c-8812-edd9923efc1f.png)


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
![image](https://user-images.githubusercontent.com/33303599/132303319-4ed3b427-a6fb-485f-abe9-9378622a5b42.png)

- Frame Rate Counter : 화면 왼측 상단에는 현재 Frame Rate에 대한 정보를 밀리세컨(millisecond, ms)과 프레임으로 표시되고 있습니다. 
- Display pixel Resoultion : Rendering 되고 있는 현재 해상도를 보여줍니다.
- Graphics API : 현재 렌더링 되고 있는 Graphics API 정보를 보여줍니다. 

#### Cinemachine

Roby Scene에서 카메라 전환은 Cinemachine을 활용해서 이루어지고 있습니다. 카메라의 Priority값을 UI에서 바꿔줌으로써 카메라간 전환이 이루어지게 됩니다. 

![image](https://user-images.githubusercontent.com/33303599/133052473-d765f541-a6ed-469f-b6d4-0036be3f4c18.png)

Scene에 CinemachineVirtualCamera를 배치하면 카메라는 해당 버추얼 카메라의 포지션으로 이동하게 됩니다. 

![kayaCinemachine](https://user-images.githubusercontent.com/33303599/133038603-8427de53-bc4b-4c95-b415-5661d6afb4ce.gif)



  - 1번 카메라 : 얼굴 근접 CM Face Shot. 1번 카메라에만 Depth of Field가 적용되어 있습니다.
![image](https://user-images.githubusercontent.com/33303599/132324090-1fa2b310-a6e5-4496-a2cc-a63432fb663a.png)

  - 2번 카메라 : 전신 CM FullBody shot
![image](https://user-images.githubusercontent.com/33303599/132324179-a99e83dd-9ccf-4f04-8598-84573a4584ae.png)

  - 3번 카메라 : 스킬 CM Skill Shot
![image](https://user-images.githubusercontent.com/33303599/132324217-9febd61d-715f-4fd4-b97b-47d3301a5fe3.png)

### Realtime Reflection 
Render texture와 shader custom을 통한 실시간 reflection을 구현한 예제입니다.
![image](https://user-images.githubusercontent.com/33303599/132324579-3c4eae96-c885-4447-9133-6b7e1b2245f6.png)





### PBR Custom Shader(Shader Graph)



### hair shader(Shader Graph)
-------------------------------
#### UTKTemplate/URPHairKajiyaKay

![kaya01](https://user-images.githubusercontent.com/33303599/133038253-0d4a0aa0-c172-4540-a75c-4569732005a7.gif)

- HLSL code based shader
- supported flowmap UV(does not need to vertical wrapped UVs)
- supported additional light & additional light shadow
- supported speucular shiftmap

![image](https://user-images.githubusercontent.com/33303599/133017036-204d8e9f-37df-4ab0-a27d-8dcfbeb42e26.png)


[URP 11.0]: https://docs.unity3d.com/Packages/com.unity.render-pipelines.universal@11.0/changelog/CHANGELOG.html
[ASTC]: https://en.wikipedia.org/wiki/Adaptive_scalable_texture_compression
