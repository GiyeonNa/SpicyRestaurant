using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EndingManager : MonoBehaviour
{
    public static EndingManager Instance;

    [Header("Good Ending")]
    //[SerializeField, TextArea(3, 10)]
    public string _goodEndingText;// =
        /*"ȸ�翡�� ���� ���� �־�����,\n�������� ���� �� �̻��ϰ� ������ ���ߴ�.\n" +
        "�� ������, �̷��� �� ������� ������ �� ���� ������.\n" +
        "����, �� ���Դ� ��õ ���� ���ƿ�� �Բ�...\n�� �� ��° �λ��� �Ǿ���.";
        */

    [SerializeField] private Sprite _goodEndingSprite;

    [Header("Bad Ending")]
    //[SerializeField, TextArea(3, 10)]
    public string _badEndingText;// =
        /*"�ʹݿ� �Լҹ��� Ÿ�� �մ��� ��������,\n���� ���߳����ϴٴ� ���� �ϳ��� �þ���\n" +
        "���� �� �ϳ��� ���� ������ ������� ������...\n�׷��� Ȯ���� ��������.";*/
    [SerializeField] private Sprite _badEndingSprite;

    [Header("UI")]
    [SerializeField] private Image _backGroundImage;
    [SerializeField] private TextMeshProUGUI _endingText;

    //private float _timer = 0f;
    //[SerializeField] private float _minEndingTimer = 3f;

    private float textSpeed = 0.05f;
    private bool _isTyping = false;
    private Tween _typingTween;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        if (GameManager.Instance.IsGoodEnding || EndingStateManager.Instance.IsGoodEnding == true)
        {
            LoadEnding(_goodEndingText, _goodEndingSprite, true);
        }
        else if (!GameManager.Instance.IsGoodEnding || EndingStateManager.Instance.IsGoodEnding == false)
        {
            LoadEnding(_badEndingText, _badEndingSprite, false);
        }

        //�ʱ�ȭ�� ��
        SaveManager.DeleteSave(); // ����: SaveManager���� ���丮 ���̺� ����
       // SoundManager.Instance.DeleteVolumeData(); // ����: ���� ���� ���̺� ����

        CustomerManager.Instance.DeleteCustomerSave();//�մ� ���嵵 ����
        CurrencyManager.Instance.ResetCurrencyData();
        IngredientManager.Instance.ClearAllPlayerPrefsData();

        GameManager.Instance.ResetLevel();

        //���Ͱ� ������ ����
        CurrencyManager.Instance.ResetCurrencyInOutData();

        // ���� ���� ����
        EndingStateManager.Instance.ResetEnding();
        Debug.Log("���� �ʱ�ȭ�Ǿ��⿡ �Ϸ� ���Ͱ� ������ �ʱ�ȭ");
    }



    private void LoadEnding(string text, Sprite bgSprite, bool isGood)
    {
        _backGroundImage.sprite = bgSprite;
        _endingText.text = text;
        _endingText.maxVisibleCharacters = 0;

        TypeText(_endingText);

        SoundManager.Instance.PlayEndingBGM(isGood);
    }

    private void TypeText(TextMeshProUGUI textUI)
    {
        float duration = textUI.text.Length * textSpeed;
        int previousCharCount = 0;

        _isTyping = true;

        _typingTween = DOTween.To(x =>
        {
            int currentCharCount = (int)x;
            textUI.maxVisibleCharacters = currentCharCount;

            if (currentCharCount > previousCharCount)
            {
                SoundManager.Instance.PlayRandomSoundInList(SoundManager.Instance.typeSounds);
                previousCharCount = currentCharCount;
            }
        },
        0f, textUI.text.Length, duration)
        .SetEase(Ease.Linear)
        .SetId("TextTween")
        .OnComplete(() => _isTyping = false);
    }

    public void OnNextButtonClicked()
    {
        if (_isTyping)
        {
            // Ÿ���� ���̸� �ߴ��ϰ� �ؽ�Ʈ ��ü ���
            _typingTween.Kill();
            _endingText.maxVisibleCharacters = _endingText.text.Length;
            _isTyping = false;
        }
        else
        {
            // Ÿ������ �������� �� ��ȯ �� ������ ����
            SoundManager.Instance.StopBackgroundMusic();
            SceneManager.LoadScene("StoryStartScene");
            StorySaveManager.Instance.DeleteSave();

            SaveManager.DeleteSave(); // ����: SaveManager���� ���丮 ���̺� ����
            SoundManager.Instance.DeleteVolumeData(); // ����: ���� ���� ���̺� ����

            SoundManager.Instance.SetMusicVolume(UI_Option.Instance.BgmBar.value);//
            SoundManager.Instance.SetEffectsVolume(UI_Option.Instance.SfxBar.value);


            CustomerManager.Instance.DeleteCustomerSave();//�մ� ���嵵 ����
            CurrencyManager.Instance.ResetCurrencyData();
            IngredientManager.Instance.ClearAllPlayerPrefsData();

            GameManager.Instance.ResetLevel();
      
            //���Ͱ� ������ ����
            CurrencyManager.Instance.ResetCurrencyInOutData();

            // ���� ���� ����
            EndingStateManager.Instance.ResetEnding();
            Debug.Log("****���� �ʱ�ȭ�Ǿ��⿡ �Ϸ� ���Ͱ� ������ �ʱ�ȭ****");
        }
    }

}


