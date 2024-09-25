using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

public class UIController : MonoBehaviour
{
    // 루트
    VisualElement root;

// 메인 버튼
    // 퀴즈 버튼 
    private Button Quiz_Button;
    // 시뮬레이션 버튼 
    private Button Simulation_Button;
    // 종료 버튼 
    private Button Exit_Button;

    // 옵션창 
    private VisualElement Option_Container;
    // 옵션 열기 버튼 
    private Button OpenOption_Button;
    // 옵션 닫기 버튼 
    private Button CloseOption_Button;

    // 해상도 드롭다운
    private DropdownField resolutionDropdown;
    // 해상도 목록 
    private List<string> resolutionOptions;
    private Resolution[] availableResolutions;

    // 기본 해상도 인덱스 
    private int defaultResolutionIndex = 1; 

    // 사운드 슬라이더
    private Slider SoundSlider;
    private Label SoundRatio;

    // 기본 소리 크기 
    private float DefaultSound = 50f;

    // 소리 에셋 
    public AudioSource audioSource1;
    public AudioSource audioSource2;

    private VisualElement blackPanel;
    public float fadeDuration = 4.0f; // 어두워지는 시간
    public bool UseFadeAnimation = true; // 페이트 에니메이션 사용 여부 

    // Start is called before the first frame update
    void Start()
    {
        // 루트 가져오기 
        root = GetComponent<UIDocument>().rootVisualElement;

        // 퀴즈 버튼 
        Quiz_Button = root.Q<Button>("Quiz_Button");
        // 시뮬레이션 버튼 
        Simulation_Button = root.Q<Button>("Simulation_Button");
        // 종료 버튼 
        Exit_Button = root.Q<Button>("Exit_Button");

        // 클릭시 이벤트 함수 바인딩 
        Quiz_Button.RegisterCallback<ClickEvent>(QuizButtonClicked);
        Simulation_Button.RegisterCallback<ClickEvent>(SimulationButtonClicked);
        Exit_Button.RegisterCallback<ClickEvent>(ExitButtonClicked);

        // 옵션 
        Option_Container = root.Q<VisualElement>("Option_Container");
        OpenOption_Button = root.Q<Button>("OpenOption_Button");
        CloseOption_Button = root.Q<Button>("CloseOption_Button");
        Option_Container.style.display = DisplayStyle.None;

        // 옵션 버튼을 클릭했을때 이벤트 함수 바인딩 
        OpenOption_Button.RegisterCallback<ClickEvent>(OnOpenButtonClicked);
        CloseOption_Button.RegisterCallback<ClickEvent>(OnCloseButtonClicked);

        // 해상도
        // UI Builder에서 만든 DropdownField를 이름으로 가져오기
        resolutionDropdown = root.Q<DropdownField>("ResolutionDropdown");

        // 사용 가능한 해상도 목록 가져오기
        availableResolutions = Screen.resolutions;
        resolutionOptions = new List<string>();

        // 해상도 옵션 문자열 리스트 만들기
        foreach (Resolution res in availableResolutions)
        {
            resolutionOptions.Add($"{res.width} x {res.height} ({(int)res.refreshRateRatio.value}Hz)");
        }

        // DropdownField에 해상도 옵션 추가
        resolutionDropdown.choices = resolutionOptions;

        // 선택 시 해상도 설정 콜백 등록
        resolutionDropdown.RegisterValueChangedCallback(evt =>
        {
            int selectedIndex = resolutionOptions.IndexOf(evt.newValue);
            SetResolution(selectedIndex);
        });

        // 초기 해상도 설정
        Screen.fullScreenMode = FullScreenMode.Windowed;

        if (defaultResolutionIndex >= 0 && defaultResolutionIndex < resolutionDropdown.choices.Count)
        {
            resolutionDropdown.value = resolutionDropdown.choices[defaultResolutionIndex];
        }

        // 사운드
        SoundSlider = root.Q<Slider>("SoundSlider");

        // 최솟값 0, 최대값 100, 기본값 50 으로 설정 
        SoundSlider.lowValue = 0;
        SoundSlider.highValue = 100;
        SoundSlider.value = DefaultSound;

        // 사이드 라벨 
        SoundRatio = root.Q<Label>("SoundRatio");
        SoundRatio.text = DefaultSound.ToString("F1") + "%";

        SoundSlider.RegisterValueChangedCallback(evt =>
        {
            SoundRatio.text = SoundSlider.value.ToString("F1") + "%";

            // 0.0(무음) ~ 1.0(최대)
            audioSource1.volume = evt.newValue / 100.0f;
            audioSource2.volume = evt.newValue / 100.0f;
        });

        // 페이드 패널
        blackPanel = root.Q<VisualElement>("BlackPanel");

        // 초기 Opacity 설정
        blackPanel.style.opacity = 0;

        // 숨김
        blackPanel.style.display = DisplayStyle.None;
    }

    private void QuizButtonClicked(ClickEvent evt)
    {
        // Move Quiz Scene
        audioSource2.Play();
        Debug.Log("Quiz");
        FadeOut("Quiz");
    }

    private void SimulationButtonClicked(ClickEvent evt)
    {
        // Move Simulation Scene
        audioSource2.Play();
        Debug.Log("Simulation");
        FadeOut("Simulation");
    }

    private void ExitButtonClicked(ClickEvent evt)
    {
        // Exit Game
        Debug.Log("Exit");
        Quit();
    }

    // 옵션 버튼 이벤트 
    private void OnOpenButtonClicked(ClickEvent evt)
    {
        Option_Container.style.display = DisplayStyle.Flex;
        audioSource1.Play();
    }
    private void OnCloseButtonClicked(ClickEvent evt)
    {
        Option_Container.style.display = DisplayStyle.None;
        audioSource1.Play();
    }

    // 해상도 설정 메서드
    private void SetResolution(int index)
    {
        Resolution selectedResolution = availableResolutions[index];
        Screen.SetResolution(selectedResolution.width, selectedResolution.height, Screen.fullScreenMode, selectedResolution.refreshRateRatio);
        Debug.Log($"Resolution set to: {selectedResolution.width} x {selectedResolution.height} @ {selectedResolution.refreshRateRatio}Hz");
    }

    // 페이드 에님 
    private async void FadeOut(string SceneName)
    {
        if (!UseFadeAnimation) {
            SceneManager.LoadScene(SceneName);
            return;
        };

        // 노출 
        blackPanel.style.display = DisplayStyle.Flex;

        float elapsedTime = 0f;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Clamp01(elapsedTime / fadeDuration); // Alpha를 0에서 1로 증가
            blackPanel.style.opacity = alpha;
            await System.Threading.Tasks.Task.Yield(); // 다음 프레임까지 대기
        }

        // 최종 Opacity 설정
        blackPanel.style.opacity = 1;
        SceneManager.LoadScene(SceneName);
    }

    // 게임 종료 
    public void Quit()
    {
        // 에디터에서 실행 중일 때 종료하지 않도록 설정
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false; // 에디터에서 게임 종료
        #else
            Application.Quit(); // 빌드된 애플리케이션 종료
        #endif
    }
}
