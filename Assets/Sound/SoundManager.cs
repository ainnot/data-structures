using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class Sound  // 컴포넌트 추가 불가능.  MonoBehaviour 상속 안 받아서. 그냥 C# 클래스.
{
    public string name;  // 곡 이름
    public AudioClip clip;  // 곡
}

public class SoundManager : MonoBehaviour
{
    #region singleton
    static public SoundManager instance; // 자기 자신을 공유 자원으로. static은 씬이 바뀌어도 유지된다.

    private void Awake()  // 객체 생성시 최초 실행 (그래서 싱글톤을 여기서 생성)
    {
        if (instance == null)   // 최초 생성
        {
            instance = this;  // 현재의 자기 자신(인스턴스)를 할당
            DontDestroyOnLoad(gameObject);  // 씬 전환되도 자기 자신이 파괴되지 않고 유지되도록
        }
        else  // 단 하나만 존재하게끔 새로 생긴 Sound Manager 오브젝트 인스턴스일 경우엔 파괴
        {
            Destroy(this.gameObject);
        }

        for (int i = 0; i < audioSourceEffects.Length; i++)
        {
            audioSourceEffects[i].clip = effectSounds[i].clip;
            audioSourceEffects[i].loop = false;
            audioSourceEffects[i].playOnAwake = false;
        }
    }
    #endregion singleton

    public Sound[] effectSounds;  // 효과음 오디오 클립들
    public Sound[] bgmSounds;  // BGM 오디오 클립들

    public AudioSource audioSourceBFM;  // BGM 재생기. BGM 재생은 한 군데서만 이루어지면 되므로 배열 X 
    public AudioSource[] audioSourceEffects;  // 효과음들은 동시에 여러개가 재생될 수 있으므로 'mp3 재생기'를 배열로 선언

    public string[] playSoundName;  // 재생 중인 효과음 사운드 이름 배열

    void Start()
    {
        playSoundName = new string[audioSourceEffects.Length];
    }

    public void PlaySE(string _name)
    {
        for (int i = 0; i < effectSounds.Length; i++)
        {
            if (_name == effectSounds[i].name)
            {
                for (int j = 0; j < audioSourceEffects.Length; j++)
                {
                    if (!audioSourceEffects[j].isPlaying)
                    {
                        audioSourceEffects[i].Play();
                        playSoundName[i] = effectSounds[i].name;
                        return;
                    }
                }
                Debug.Log("모든 가용 AudioSource가 사용 중입니다.");
                return;
            }
        }
        Debug.Log(_name + "사운드가 SoundManager에 등록되지 않았습니다.");
    }

    public void PlayBGM(string _name)
    {
        for (int i = 0; i < bgmSounds.Length; i++)
        {
            if (_name == bgmSounds[i].name)
            {
                audioSourceBFM.clip = bgmSounds[i].clip;
                audioSourceBFM.Play();
                return;
            }
        }
        Debug.Log(_name + "사운드가 SoundManager에 등록되지 않았습니다.");
    }

    public void StopAllSE()
    {
        for (int i = 0; i < audioSourceEffects.Length; i++)
        {
            audioSourceEffects[i].Stop();
        }
    }

    public void StopSE(string _name)
    {
        for (int i = 0; i < audioSourceEffects.Length; i++)
        {
            if (playSoundName[i] == _name)
            {
                audioSourceEffects[i].Stop();
                break;
            }
        }
        Debug.Log("재생 중인" + _name + "사운드가 없습니다. ");
    }

    public void SetSoundAmount(float Ratio)
    {
        for (int i = 0; i < audioSourceEffects.Length; i++)
        {
            audioSourceEffects[i].volume = Ratio / 100.0f;
        }
    }
}
