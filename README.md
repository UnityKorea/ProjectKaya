
# ProjectKaya


프로젝트 카야는 유니티 코리아에서 진행하는 URP를 활용한 모바일 예제 프로젝트 입니다.

Project Kaya is a mobile example project for using Unity URP by Unity Technologies Korea.

## Requirement
- Unity 2021.1.13f1
- [URP 11.0] version needed
- Android Platform
- Vulkan API supported

## Resource Compression
대부분의 Texture compression은 [ASTC] 를 사용하고 있습니다.
일부 리소스의 경우 압축하지 않은 RGB24 혹은 RGBA32입니다.

Normal texture에서 용량을 줄이기 위해서는 ASTC와 none copmpressed를 적절히 사용하는 것을 권장합니다.


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

Roby Scene에서 카메라 전환은 Cinemachine을 활용해서 이루어지고 있습니다. 




### PBR Custom Shader(Shader Graph)


### Realtime Reflection 
Render texture와 shader custom을 통한 실시간 reflection을 구현한 예제입니다.

### hair shader(Shader Graph)


### hair shader(HLSL shader)

HLSL로 작성된 Shader는 아래와 같은 내용을 지원합니다.

- KajiyaKay Hair highlight
- Additional lighting
- Additional realtime shadow
- vertex light & pixel light


![image](https://user-images.githubusercontent.com/33303599/132298338-2db312c7-6c79-4b77-8190-74f73d875b8a.png)


![image](https://user-images.githubusercontent.com/33303599/132298274-afbfb960-daea-4c60-b359-a821382b4279.png)


[URP 11.0]: https://docs.unity3d.com/Packages/com.unity.render-pipelines.universal@11.0/changelog/CHANGELOG.html
[ASTC]: https://en.wikipedia.org/wiki/Adaptive_scalable_texture_compression
