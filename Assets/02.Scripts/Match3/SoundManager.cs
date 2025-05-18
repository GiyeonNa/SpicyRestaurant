using UnityEngine;
using System.Collections.Generic;
using System.IO;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine.SceneManagement;

public class SoundManager : MonoBehaviour
{
    #region 공용
    [Header("공용")]
    public AudioClip popSound;
    #endregion

    #region Match3 사용 오디오
    [Header("Match3 사용 오디오")]
    public AudioClip matchBGMusic;
    public AudioClip swapSound;
    public AudioClip matchSound;
    public AudioClip gameOverSound;
    public AudioClip countdownSound;
    public AudioClip reBoardSound;
    public List<AudioClip> itemUseSounds; // Replace single sound with a list
    #endregion

    #region 손님접객 사운드
    [Header("손님접객 사운드")]
    public AudioClip customerArriveSound;
    public List<AudioClip> customerCashSound;
    public List<AudioClip> customerPositiveSounds;
    public List<AudioClip> customerNegativeSounds;
    public List<AudioClip> typeSounds;
    public List<AudioClip> BoillingSounds;
    public List<AudioClip> WaterPouringSounds;
    public AudioClip FireOnSound;
    public AudioClip DoorOpenSound;
    public AudioClip DoorCloseSound;
    public AudioClip SquishSound;
    public AudioClip MetalPlaceSound;
    public AudioClip BeepSound;
    public AudioClip BeepBeepSound;
    public AudioClip NotifiySound;
    public AudioClip ReceiptSound;
    public AudioClip WooshSound;
    public AudioClip completeSound;
    private AudioSource BoillingAudioSource;
    #endregion

    #region 결과화면 
    [Header("결과화면")]
    public AudioClip resultAppearSound;
    public List<AudioClip> resultElementSound;
    public AudioClip ReusltPositiveBGMSound;
    public AudioClip ReusltPositiveSound;
    public AudioClip ReusltNegativeBGMSound;
    public AudioClip ReusltNegativeSound;
    #endregion

    #region 스토리
    [Header("스토리화면")]
    public AudioClip OpeningBGM;
    public AudioClip HappyEndingBGM;
    public AudioClip BadEndingBGM;
    public AudioClip FireworkSound1;
    //public AudioClip FireworkSound2;
    //public AudioClip FireworkSound3;
    #endregion

    // 볼륨값
    public float bgmVolume { get; private set; } = 1.0f; // Default BGM volume
    public float sfxVolume { get; private set; } = 1.0f; // Default SFX volume

    // BGM 재생기
    public AudioSource BGMmusicSource;
    // 효과음 재생기
    private List<AudioSource> effectsSources = new List<AudioSource>(); // Pool of AudioSources
    private const int initialPoolSize = 5; // Initial size of the pool

    //이 위에는 원래 있었떤 거

    public static SoundManager Instance;

   
    private void Start()
    {
        Instance = this;
    }

    void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(gameObject); // 씬 넘어가도 파괴되지 않게 설정

        BGMmusicSource = gameObject.AddComponent<AudioSource>();
        BGMmusicSource.loop = true;

        for (int i = 0; i < initialPoolSize; i++)
        {
            AudioSource source = gameObject.AddComponent<AudioSource>();
            effectsSources.Add(source);
        }

        LoadVolume(); // 앱 시작할 때 볼륨 불러오기
    }



    // 기존의 방법대로 사용 가능하도록 AudioSource를 반환하는 메서드
    public AudioSource GetAvailableAudioSource()
    {
        foreach (var source in effectsSources)
        {
            if (!source.isPlaying)
            {
                return source;
            }
        }

        AudioSource newSource = gameObject.AddComponent<AudioSource>();
        effectsSources.Add(newSource);
        return newSource;
    }

    // 오프닝 및 타이틀 음악 재생
    public void PlayOpenTitleMusic()
    {
        if (OpeningBGM != null)
        {
            BGMmusicSource.clip = OpeningBGM;
            BGMmusicSource.loop = true; // Ensure looping is enabled
            BGMmusicSource.Play();
        }
    }

    // BGM 음악 재생
    public void PlayBackgroundMusic()
    {
        if (matchBGMusic != null)
        {
            BGMmusicSource.clip = matchBGMusic;
            BGMmusicSource.loop = true; // Ensure looping is enabled
            BGMmusicSource.Play();
        }
    }

    // BGM 음악 중지
    public void StopBackgroundMusic()
    {
        BGMmusicSource.Stop();
    }

    // 엔딩 음악 재생
    public void PlayEndingBGM(bool isGoodEnding)
    {
        if (isGoodEnding)
        {
            BGMmusicSource.clip = HappyEndingBGM;
        }
        else
        {
            BGMmusicSource.clip = BadEndingBGM;
        }
        BGMmusicSource.loop = true;
        BGMmusicSource.Play();
    }

    // 효과음 재생
    public void PlaySoundEffect(AudioClip clip)
    {
        if (clip != null)
        {
            AudioSource availableSource = GetAvailableAudioSource();
            availableSource.volume = sfxVolume; // 반드시 현재 SFX 값 반영
            availableSource.PlayOneShot(clip);
        }
    }

   /* public void PlayRandomSoundInList(List<AudioClip> clipList)
    {
        if (clipList.Count != 0)
        {
            int randomIndex = Random.Range(0, clipList.Count);
            AudioSource availableSource = GetAvailableAudioSource();
            availableSource.PlayOneShot(clipList[randomIndex]);
        }
    }*/

    public void PlayRandomSoundInList(List<AudioClip> clipList)
    {
        if (clipList.Count != 0)
        {
            int randomIndex = Random.Range(0, clipList.Count);
            AudioSource availableSource = GetAvailableAudioSource();
            availableSource.volume = sfxVolume;
            availableSource.PlayOneShot(clipList[randomIndex]);
        }
    }

    // 끓는 소리 재생
    public void PlayBoillingSound()
    {
        int randomIndex = Random.Range(0, BoillingSounds.Count - 1);
        BoillingAudioSource = GetAvailableAudioSource();
        BoillingAudioSource.clip = BoillingSounds[randomIndex];
        BoillingAudioSource.Play();
    }

    // 끓는 소리 멈추기
    public void StopBoillingSound()
    {
        if (BoillingAudioSource.clip != null)
        {
            BoillingAudioSource.Stop();
        }
    }

    //모든 효과음 멈추기
    public void StopAllSoundEffects()
    {
        foreach (var source in effectsSources)
        {
            if (source.isPlaying)
            {
                source.Stop();
            }
        }
    }

    #region 볼륨 조절
    // BGM 볼륨 설정
    public void SetMusicVolume(float volume)
    {
        bgmVolume = Mathf.Clamp01(volume); // Ensure volume is between 0 and 1
        BGMmusicSource.volume = bgmVolume;
        Debug.Log("**볼륨 조절하는 중" + BGMmusicSource.volume);

        UI_Option.Instance.initialBgmVolume = UI_Option.Instance.soundManager.bgmVolume;
        UI_Option.Instance.BgmBar.value = UI_Option.Instance.initialBgmVolume;

        // SaveVolume();
    }

    // 효과음 볼륨 설정
    public void SetEffectsVolume(float volume)
    {
        sfxVolume = Mathf.Clamp01(volume); // Ensure volume is between 0 and 1
        foreach (var source in effectsSources)
        {
            source.volume = sfxVolume;
        }

        UI_Option.Instance.initialSfxVolume = UI_Option.Instance.soundManager.sfxVolume;
        UI_Option.Instance.SfxBar.value = UI_Option.Instance.initialSfxVolume;
        //SaveVolume();
    }
    #endregion

    #region JSON 저장 및 불러오기
    [System.Serializable]
    public class VolumeData
    {
        public float bgmVolume;
        public float sfxVolume;
    }

    // 볼륨을 JSON 형식으로 저장
    public void SaveVolume()
    {
        VolumeData volumeData = new VolumeData
        {
            bgmVolume = bgmVolume,
            sfxVolume = sfxVolume
        };

        // 디버그 메시지 추가
        Debug.Log("Saving Volume: BGM Volume = " + volumeData.bgmVolume + ", SFX Volume = " + volumeData.sfxVolume);

        string json = JsonUtility.ToJson(volumeData, true);  // JSON 형식으로 변환
        File.WriteAllText(Application.persistentDataPath + "/volumeSettings.json", json);  // 파일로 저장

        UI_Option.Instance.BgmBar.value = volumeData.bgmVolume;
        UI_Option.Instance.SfxBar.value = volumeData.sfxVolume;

        UI_Option.Instance.initialBgmVolume = UI_Option.Instance.BgmBar.value;
        UI_Option.Instance.initialSfxVolume = UI_Option.Instance.SfxBar.value;
    }

    // 저장된 JSON 파일에서 볼륨 값을 불러오기
    public void LoadVolume()
    {
        string path = Application.persistentDataPath + "/volumeSettings.json";
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);  // 파일 내용 읽기
            VolumeData volumeData = JsonUtility.FromJson<VolumeData>(json);  // JSON 파싱
            bgmVolume = volumeData.bgmVolume;
            sfxVolume = volumeData.sfxVolume;
            SetMusicVolume(bgmVolume);  // 불러온 볼륨으로 설정
            SetEffectsVolume(sfxVolume);

            // 불러온 볼륨 값 확인용 디버그 메시지
            Debug.Log("Loaded BGM Volume: " + bgmVolume);
            Debug.Log("Loaded SFX Volume: " + sfxVolume);

            UI_Option.Instance.BgmBar.value = bgmVolume;
            UI_Option.Instance.SfxBar.value = sfxVolume;

            UI_Option.Instance.initialBgmVolume = UI_Option.Instance.BgmBar.value;
            UI_Option.Instance.initialSfxVolume = UI_Option.Instance.SfxBar.value;
        }
        else
        {
            // 파일이 없으면 기본값으로 설정
            Debug.LogWarning("No volume settings file found. Using default values.");
            SaveVolume();  // 기본값 저장

            UI_Option.Instance.BgmBar.value = bgmVolume;
            UI_Option.Instance.BgmBar.value = sfxVolume;

            UI_Option.Instance.initialBgmVolume = UI_Option.Instance.BgmBar.value;
            UI_Option.Instance.initialSfxVolume = UI_Option.Instance.SfxBar.value;
        }
    }

    public void DeleteVolumeData()
    {
        string path = Application.persistentDataPath + "/volumeSettings.json";
        if (File.Exists(path))
        {
            File.Delete(path);
            Debug.Log("Volume settings deleted.");
        }
        else
        {
            Debug.LogWarning("No volume settings file to delete.");
        }

        // 기본값으로 설정하고 저장
        SetMusicVolume(1.0f); // 기본값
        SetEffectsVolume(1.0f); // 기본값

       
    }
    #endregion

    // 아이템 사용 사운드 재생
    public void PlayItemUseSound(BonusType bonusType) // Updated method to accept BonusType parameter
    {
        if (itemUseSounds == null || itemUseSounds.Count == 0)
            return;

        AudioClip clipToPlay = null;

        if (bonusType == BonusType.DestroySurrounding)
        {
            // Play the last element for DestroySurrounding
            clipToPlay = itemUseSounds[itemUseSounds.Count - 1];
        }
        else if (bonusType == BonusType.DestroyWholeRow || bonusType == BonusType.DestroyWholeColumn)
        {
            // Randomly play the first or second element for DestroyWholeRow or DestroyWholeColumn
            int randomIndex = Random.Range(0, 2);
            clipToPlay = itemUseSounds[randomIndex];
        }

        if (clipToPlay != null)
        {
            PlaySoundEffect(clipToPlay);
        }
    }
}

