using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine.SceneManagement;

public class UI_Option : MonoBehaviour
{
    public static UI_Option Instance;

    public Slider BgmBar;
    [SerializeField] public Slider SfxBar;

    [SerializeField] private Button howPlayButton;
    [SerializeField] private Button okButton;
    [SerializeField] private Button cancelButton;

    public Transform BlackBackground;
    public Transform BackgroundCanvasGroup;
    public Image BackgroundBlackImage;

    [Header("How to Play UI")]
    [SerializeField] private GameObject howPlayUI;
    [SerializeField] private GameObject howPlayUIBG;
    [SerializeField] private Button howPlayCloseButton;
    [SerializeField] private Button howPlayLeftButton;
    [SerializeField] private Button howPlayRightButton;
    [SerializeField] private Image explanationImage;
    [SerializeField] private List<Sprite> explanationImages;
    [SerializeField] private TextMeshProUGUI explanationText;

    [Header("Credit UI")]
    [SerializeField] private GameObject creditUI;
    [SerializeField] private GameObject creditUIBG;
    [SerializeField] private Button creditcancelButton;
    [SerializeField] private Button creditButton;
    [SerializeField] private RectTransform creditContent;

    [Header("Reset UI")]
    [SerializeField] private GameObject resetUI;
    [SerializeField] private Button resetButton;
    [SerializeField] private Button yesResetButton;
    [SerializeField] private Button noResetButton;


    [Header("Fade Image")]
    public GameObject FadeImage;
    public Image FadeImageRaycast;
    public Animator FadeImageAnimation;

    public SoundManager soundManager;

    private List<string> explanationTexts = new List<string>
    {
        "���� ��Ḧ 3�� �̻� ���缭\n��Ḧ ��������.",//1
        "4�� �̻� ���� �� �����Ǵ�\n�������� �������� ��ġ�ϸ�,\n��Ḧ �� ���� ȹ���� �� �־��.",//2
        "���� �ܰ迡���� �մ��� �ֹ���\n�����ϰų� ������ �� �־��.",//3
        "��� ��ư�� ���� �����ϰ�,\n�巡���ؼ� ���� ��������.",//4
        "��� ��Ḧ �־��ٸ�\n������������ ���� ��ư�� ��������.",//5
        "������ ������ ���� ����\n�巡���ؼ� �մԿ��� �����ϼ���.",//6
        "��Ḧ �߸� �־��� �� ����\n�¿�� �巡���� ���� �� �־��.",//7
        "���� ��Ḧ ������ �ϸ�,\n��ᰪ��ŭ ���� �پ����.",//8
        "�ֹ��� ���� �������� �����,\n�ְ��� ������ �����ϼ���!",//9
    };

    public float initialBgmVolume;
    public float initialSfxVolume;
    private int currentIndex = 0;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);

        if (howPlayButton != null)
            howPlayButton.onClick.AddListener(OnClickHowPlay);

        if (okButton != null)
            okButton.onClick.AddListener(OnClickOK);

        if (cancelButton != null)
            cancelButton.onClick.AddListener(OnClickCancel);

        if (howPlayCloseButton != null)
            howPlayCloseButton.onClick.AddListener(OnClickHowPlayClose);

        if (howPlayLeftButton != null)
            howPlayLeftButton.onClick.AddListener(OnClickLeft);

        if (howPlayRightButton != null)
            howPlayRightButton.onClick.AddListener(OnClickRight);


        if (creditButton != null)
            creditButton.onClick.AddListener(GoCredit);

        if (creditcancelButton != null)
            creditcancelButton.onClick.AddListener(ExitCredit);

        //����
        if (resetButton != null)
            resetButton.onClick.AddListener(GameReset);

        if (yesResetButton != null)
            yesResetButton.onClick.AddListener(GameResetYes);

        if (noResetButton != null)
            noResetButton.onClick.AddListener(GameResetNo);
    }

    private void Start()
    {
        FadeImage.SetActive(false);//���̵� �̹��� ��Ȱ��

        BlackBackground.localScale = Vector3.zero;

        Instance = this;

        if (soundManager != null)
        {
            if (BgmBar != null)
            {
                BgmBar.value = soundManager.bgmVolume;
                BgmBar.onValueChanged.AddListener(OnBGMVolumeChange);
            }

            if (SfxBar != null)
            {
                SfxBar.value = soundManager.sfxVolume;
                SfxBar.onValueChanged.AddListener(OnSFXVolumeChange);
            }
        }


        //SaveManager.SaveGame(GameManager.Instance.CurrentLevel, GameManager.Instance.PlayerCurrencyData, GameManager.Instance.GetIngredientData(), Scenes.ShopOpen);
        UpdateExplanationImage();

        
    }

    private void OnEnable()
    {
        if (soundManager != null)
        {
            if (BgmBar != null)
            {
                initialBgmVolume = soundManager.bgmVolume;
                BgmBar.value = initialBgmVolume;
            }

            if (SfxBar != null)
            {
                initialSfxVolume = soundManager.sfxVolume;
                SfxBar.value = initialSfxVolume;
            }
        }

        BackgroundCanvasGroup.localScale = Vector3.zero;
        BackgroundCanvasGroup.DOScale(Vector3.one, 0.5f).SetEase(Ease.OutBack);

        

    }

    private void OnDisable()
    {
        BackgroundCanvasGroup.DOScale(Vector3.zero, 0.5f).SetEase(Ease.InBack).OnComplete(() =>
        {
            gameObject.SetActive(false);
        });
    }

    #region �����̴� ���
    /*private void OnBGMVolumeChange(float value)
    {
        soundManager.SetMusicVolume(value);
        soundManager.SaveVolume();
        Debug.Log("����� ����");
    }

    private void OnSFXVolumeChange(float value)
    {
        soundManager.SetEffectsVolume(value);
        soundManager.SaveVolume();
        Debug.Log("ȿ���� ����");
    }*/
    private void OnBGMVolumeChange(float value)
    {
        SoundManager.Instance.SetMusicVolume(value);
        Debug.Log("����� �̸���� (���� �� ��)");
    }

    private void OnSFXVolumeChange(float value)
    {
        SoundManager.Instance.SetEffectsVolume(value);
        Debug.Log("ȿ���� �̸���� (���� �� ��)");

    }
    #endregion

    #region ��ư ��ɵ�
    private void OnClickHowPlay()
    {
        SoundManager.Instance.PlaySoundEffect(SoundManager.Instance.popSound);
        howPlayUI.SetActive(true);
        currentIndex = 0;
        UpdateExplanationImage();

        if (howPlayUIBG != null)
        {
            howPlayUIBG.transform.localScale = Vector3.zero;
            howPlayUIBG.transform.DOScale(Vector3.one, 0.5f).SetEase(Ease.OutBack);
        }
    }

    //ũ����
    public void GoCredit()
    {
        SoundManager.Instance.PlaySoundEffect(SoundManager.Instance.popSound);
        creditUI.SetActive(true);

        if (howPlayUIBG != null)
        {
            creditUIBG.transform.localScale = Vector3.zero;
            creditUIBG.transform.DOScale(Vector3.one, 0.5f).SetEase(Ease.OutBack);
        }

        creditContent.offsetMin = new Vector2(creditContent.offsetMin.x, -1103.565f);
        creditContent.offsetMax = new Vector2(creditContent.offsetMax.x, 0.0004882813f);
    }

    public void ExitCredit()
    {
        if (creditUIBG != null)
        {
            creditUIBG.transform.DOScale(Vector3.zero, 0.5f).SetEase(Ease.InBack).OnComplete(() =>
            {
                creditUI.SetActive(false);
            });
        }

        SoundManager.Instance.PlaySoundEffect(SoundManager.Instance.popSound);
    }

    //���� ����
    public void GameReset()
    {
        SoundManager.Instance.PlaySoundEffect(SoundManager.Instance.popSound);
        if (MatchUIManager.Instance != null)
        {
            MatchUIManager.Instance.PauseUIBackground.SetActive(true);
        }
        SoundManager.Instance.PlaySoundEffect(SoundManager.Instance.popSound);
        resetUI.SetActive(true);

    }

    public void GameResetYes()
    {
        Debug.Log("���� ������ ����");

        BlackBackground.localScale = Vector3.zero;
        if (MatchUIManager.Instance != null)
        {
            MatchUIManager.Instance.PauseUIBackground.SetActive(true);
        }
        resetUI.SetActive(false);
        SoundManager.Instance.PlaySoundEffect(SoundManager.Instance.popSound);

        SoundManager.Instance.StopBackgroundMusic();
        
        if(MatchUIManager.Instance != null)
        {
            MatchUIManager.Instance.PauseUI.SetActive(false);
            ShapesManager.Instance.PauseTimer(); // Pause the timer in ShapesManager
            ShapesManager.Instance.PauseBlockMovement(); // Pause block movement in ShapesManager
        }
        
        StorySaveManager.Instance.DeleteSave();

        FadeImageRaycast.raycastTarget = true;
        FadeImage.SetActive(true);
        FadeImageAnimation.SetTrigger("Black");

        StartCoroutine(ResetAndGoStart());

        IEnumerator ResetAndGoStart()
        {
            yield return new WaitForSeconds(1.0f);
            //  StorySaveManager.Instance.DeleteSave();
            CurrencyManager.Instance.ResetCurrencyData();
            GameManager.Instance.ResetLevel();
            //���Ͱ� ������ ����
            CurrencyManager.Instance.ResetCurrencyInOutData();
            // ���� ���� ����
            EndingStateManager.Instance.ResetEnding();

            if (CustomerManager.Instance != null)
            {
                CustomerManager.Instance.DeleteCustomerSave();//�մ� ���嵵 ����
            }

            if (IngredientManager.Instance != null)
            {
                IngredientManager.Instance.ClearAllPlayerPrefsData();
            }

            //�� �̵� ����
            Scene currentScene = SceneManager.GetActiveScene();
            string sceneName = currentScene.name;

            SceneManager.LoadScene("StoryStartScene");

            SoundManager.Instance.DeleteVolumeData(); // ����: ���� ���� ���̺� ����
            SoundManager.Instance.SaveVolume();
            SoundManager.Instance.LoadVolume();
            
        }
 
    }

    public void GameResetNo()
    {

        SoundManager.Instance.PlaySoundEffect(SoundManager.Instance.popSound);
        resetUI.SetActive(false);
    }


    private void OnClickOK()
    {
        SoundManager.Instance.SetMusicVolume(BgmBar.value);
        SoundManager.Instance.SetEffectsVolume(SfxBar.value);

        // ������ ���⼭��
        soundManager.SaveVolume();

        // ���� ���� �� �ʱⰪ���� ����
        initialBgmVolume = BgmBar.value;
        initialSfxVolume = SfxBar.value;

        SoundManager.Instance.PlaySoundEffect(SoundManager.Instance.popSound);
        BlackBackground.localScale = Vector3.zero;

        if (MatchUIManager.Instance != null)
        {
            MatchUIManager.Instance.PauseUIBackground.SetActive(true);
        }
    }


    private void OnClickCancel()
    {
        // ����� �� �ҷ�����
        soundManager.LoadVolume();

        SoundManager.Instance.PlaySoundEffect(SoundManager.Instance.popSound);
        BlackBackground.localScale = Vector3.zero;

        if (MatchUIManager.Instance != null)
        {
            MatchUIManager.Instance.PauseUIBackground.SetActive(true);
        }

        initialBgmVolume = soundManager.bgmVolume;
        BgmBar.value = initialBgmVolume;
        initialSfxVolume = soundManager.sfxVolume;
        SfxBar.value = initialSfxVolume;
        soundManager.SaveVolume();
        Debug.Log("������" + BgmBar.value);
        Debug.Log("ȿ����" + SfxBar.value);
    }


    private void OnClickHowPlayClose()
    {
        if (howPlayUIBG != null)
        {
            howPlayUIBG.transform.DOScale(Vector3.zero, 0.5f).SetEase(Ease.InBack).OnComplete(() =>
            {
                howPlayUI.SetActive(false);
            });
        }

        soundManager.PlaySoundEffect(soundManager.popSound);
    }
    #endregion

    #region ���� �̹��� ��ɵ�

    private void OnClickLeft()
    {
        if (explanationImages == null || explanationImages.Count == 0) return;

        currentIndex = (currentIndex - 1 + explanationImages.Count) % explanationImages.Count;
        UpdateExplanationImage();
        soundManager.PlaySoundEffect(soundManager.popSound);
    }

    private void OnClickRight()
    {
        if (explanationImages == null || explanationImages.Count == 0) return;

        currentIndex = (currentIndex + 1) % explanationImages.Count;
        UpdateExplanationImage();
        soundManager.PlaySoundEffect(soundManager.popSound);
    }

    private void UpdateExplanationImage()
    {
        if (explanationImage != null && explanationImages.Count > 0)
        {
            explanationImage.sprite = explanationImages[currentIndex];
        }

        if (explanationText != null && explanationTexts.Count > 0)
        {
            explanationText.text = explanationTexts[currentIndex];
        }
    }
    #endregion
}
