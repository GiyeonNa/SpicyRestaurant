using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System;
using DG.Tweening;


public class UI_Shop : MonoBehaviour
{
    public DialogWindowSetting DialogWindow;//�߰�

    public static UI_Shop Instance;


    [SerializeField]
    private RectTransform KitchenPanel;
    [SerializeField]
    private RectTransform FrontDeskpanel;

    //[SerializeField]
    //private Image OptionPanel;//0414

    //0414
    /*[SerializeField]
    private GameObject OptionPanel;
    [SerializeField]
    private Transform pauseUIBG;
    */


    [SerializeField]
    private Image _bowl;

    // [SerializeField]
    //private Button _askButton;
    public Button AskButton;

    /*[SerializeField]
    private TextMeshProUGUI DayText;
    [SerializeField]
    private TextMeshProUGUI BudestText;
    [SerializeField]
    private TextMeshProUGUI SatisfactionText;
    */

    public TextMeshProUGUI DayText;
    public TextMeshProUGUI BudestText;
    public TextMeshProUGUI SatisfactionText;

    public bool IsKitchenOpened = false;
    public float KitchenMoveAmount = 10f;

    public int AskPenalty;

    public Action OnKitchenToggled;

    private float _frontDeskPanelYPosition;


    // �ִϸ��̼� ���� ����
    public bool AllowAnimation = false;

    private Sequence _askSequence; // �ٽ� ��� �� �������� ��춧���� ����
    private Coroutine _askCoroutine; // �ٽ� ��� �� �������� ��춧���� ����
    private Tween _typingTween;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

    }

    private void Start()
    {
        AllowAnimation = false;

        //RefreshDay();

        KitchenPanel.anchoredPosition = new Vector2(0, -KitchenPanel.rect.height);
        _frontDeskPanelYPosition = FrontDeskpanel.transform.position.y;

        //�ε� //0420
        //  SaveManager.Instance.LoadCurrentGame();
        //Debug.Log("��� ���� �ҷ�����");





        CurrencyManager.Instance.LoadCurrencyData();//��ȭ �� ������ �ҷ�����
        CurrencyManager.Instance.LoadCurrencyInOutData();//��ȭ �� ������ ���شٿ� �ҷ�����

        // IngredientManager.Instance.SaveAllIngredientCounts();//��� �ҷ�����
        // IngredientManager.Instance.LoadAllIngredientCounts();//��� �ҷ�����

        // SaveManager.LoadGame();
        CustomerManager.Instance.LoadCustomerIndex();//�ҷ�����


        //GameManager.Instance.LoadLevelData();//�̰� �̻��� �� ����


        // ���� �� �̸� Ȯ�� (����׿�)
        //  string currentScene = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
        // Debug.Log("���� �� �����ϸ� �����͸� �ҷ���: " + currentScene);

        CurrencyManager.Instance.LoadCurrencyData();
    }



    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            ToggleKitchen();
        }


    }

    public void ToggleKitchen()
    {



        if (IsKitchenOpened)
        {
            KitchenPanel.DOMoveY(KitchenPanel.position.y - KitchenMoveAmount, 1f);
            FrontDeskpanel.DOMoveY(_frontDeskPanelYPosition, 1f);

            OnKitchenToggled();

            IsKitchenOpened = false;
        }
        else
        {
            KitchenPanel.DOMoveY(KitchenPanel.position.y + KitchenMoveAmount, 1f);
            FrontDeskpanel.DOMoveY(_frontDeskPanelYPosition + KitchenMoveAmount, 1f);

            OnKitchenToggled();

            IsKitchenOpened = true;
        }
    }

    /* public void RefreshDay()
     {
         //DayText.text = $"Day {GameManager.Instance.CurrentLevel + 1}";
         DayText.text = $"Day {GameManager.Instance.CurrentLevel + 1}";
         Debug.Log("��¥ ����մϴ�" + (GameManager.Instance.CurrentLevel + 1) + "�� °");
     }*/

    public void RefreshCurrency(CurrencyType currencyType, int amount)
    {
        // Apply animations for subsequent updates
        if (currencyType == CurrencyType.Money)
        {
            int currentAmount = int.Parse(BudestText.text);
            int changeAmount = amount - currentAmount;
            BudestText.text = $"{amount}";

            // Display change value
            /*ShowChangeValue(BudestText, changeAmount);

            // Use helper method for animations
            AnimateTextChange(BudestText, changeAmount);
            */
            if (AllowAnimation)
            {
                ShowChangeValue(BudestText, changeAmount);
                AnimateTextChange(BudestText, changeAmount);
            }
            return;
        }

        if (currencyType == CurrencyType.Satisfaction)
        {
            int currentSatisfaction = int.Parse(SatisfactionText.text.Replace("%", ""));
            int changeAmount = amount - currentSatisfaction;
            SatisfactionText.text = $"{amount}%";

            // Display change value
            //ShowChangeValue(SatisfactionText, changeAmount);

            // Use helper method for animations
            //AnimateTextChange(SatisfactionText, changeAmount);

            if (AllowAnimation)
            {
                ShowChangeValue(SatisfactionText, changeAmount);
                AnimateTextChange(SatisfactionText, changeAmount);
            }
            return;
        }
        //  SaveManager.SaveGame(GameManager.Instance.CurrentLevel, GameManager.Instance.PlayerCurrencyData, GameManager.Instance.GetIngredientData(), Scenes.ShopOpen);
        // Debug.Log("��� �����ϱ�");


    }

    private void AnimateTextChange(TextMeshProUGUI targetText, int changeAmount)
    {
        Color targetColor = new Color(119f / 255f, 87f / 255f, 82f / 255f);//0414
        Color RedColor = new Color(203f / 255f, 8f / 255f, 8f / 255f); // ����
        Color GreenColor = new Color(63f / 255f, 200f / 255f, 45f / 255f); // �ʷ�



        if (changeAmount > 0)
        {
            // Animation for increasing value
            targetText.transform.DOScale(1.2f, 0.2f).OnComplete(() =>
                targetText.transform.DOScale(1f, 0.2f));
            targetText.DOColor(GreenColor, 0.2f).OnComplete(() =>
                //targetText.DOColor(Color.black, 0.2f));
                targetText.DOColor(targetColor, 0.2f));

        }
        else if (changeAmount < 0)
        {
            // Animation for decreasing value
            targetText.transform.DOScale(0.8f, 0.2f).OnComplete(() =>
                targetText.transform.DOScale(1f, 0.2f));
            targetText.DOColor(RedColor, 0.2f).OnComplete(() =>
                //targetText.DOColor(Color.black, 0.2f));
                targetText.DOColor(targetColor, 0.2f));
        }

        Debug.Log("�ٲ�� �� ȣ���" + "�ٲ� ��" + changeAmount);
        //SaveManager.SaveGame(GameManager.Instance.CurrentLevel, GameManager.Instance.PlayerCurrencyData, GameManager.Instance.GetIngredientData(), Scenes.ShopOpen);
        // SaveManager.Instance.SaveCurrentGame();
        // Debug.Log("��� �����ϱ�");
        //AllowAnimation = false;
    }


    private void ShowChangeValue(TextMeshProUGUI targetText, int changeAmount)
    {

        Color RedColor = new Color(203f / 255f, 8f / 255f, 8f / 255f); // ����
        Color GreenColor = new Color(63f / 255f, 200f / 255f, 45f / 255f); // �ʷ�

        // Create a temporary text object
        GameObject changeTextObj = new GameObject("ChangeText");
        changeTextObj.transform.SetParent(targetText.transform.parent);
        TextMeshProUGUI changeText = changeTextObj.AddComponent<TextMeshProUGUI>();

        // Set text properties
        changeText.text = changeAmount > 0 ? $"+{changeAmount}" : $"{changeAmount}";
        changeText.fontSize = targetText.fontSize * 1.2f; // �� ũ��
        changeText.color = changeAmount > 0 ? GreenColor : RedColor;
        changeText.alignment = TextAlignmentOptions.Center;

        // **Bold ����**
        changeText.fontStyle = FontStyles.Bold;

        // ���������� ���� �ؽ�Ʈ�� ��Ʈ/��Ƽ���� ����
        changeText.font = targetText.font;
        changeText.fontSharedMaterial = targetText.fontSharedMaterial;

        // Position the text above the target text
        RectTransform rectTransform = changeText.GetComponent<RectTransform>();
        rectTransform.anchorMin = targetText.rectTransform.anchorMin;
        rectTransform.anchorMax = targetText.rectTransform.anchorMax;
        rectTransform.pivot = targetText.rectTransform.pivot;
        rectTransform.anchoredPosition = targetText.rectTransform.anchoredPosition + new Vector2(0, 30);
        rectTransform.localScale = Vector3.one;
        rectTransform.sizeDelta = targetText.rectTransform.sizeDelta;

        // Animate the text (fade out and move upward)
        changeText.DOFade(0, 1f).SetEase(Ease.OutQuad);
        rectTransform.DOAnchorPosY(rectTransform.anchoredPosition.y + 50, 1f).SetEase(Ease.OutQuad).OnComplete(() =>
        {
            Destroy(changeTextObj);
        });

        Debug.Log("�ٲ�� �� ȣ���2" + "�ٲ� ��" + changeAmount);

        //AllowAnimation = false;
        // SaveManager.Instance.SaveCurrentGame();
        //Debug.Log("��� �����ϱ�");
    }


    public void OnOptionClick()
    {
        // OptionPanel.transform.parent.gameObject.SetActive(true);//0414

        //�ɼ� ��ư ������ ��

        // Play pop sound
        //0414~
        SoundManager.Instance.PlaySoundEffect(SoundManager.Instance.popSound);

        UI_Option.Instance.BlackBackground.localScale = Vector3.one;//
        UI_Option.Instance.BackgroundBlackImage.enabled = true;
        UI_Option.Instance.BackgroundCanvasGroup.localScale = Vector3.zero; // Start at zero scale
        UI_Option.Instance.BackgroundCanvasGroup.DOScale(Vector3.one, 0.5f).SetEase(Ease.OutBack); // Scale to full size with an easing effect
        //pauseUIBG.transform.localScale = Vector3.zero; // Start at zero scale
        //OptionPanel.SetActive(true); // Activate the UI
        //pauseUIBG.transform.DOScale(Vector3.one, 0.5f).SetEase(Ease.OutBack); // Scale to full size with an easing effect
    }
    public void OnAskButtonClick()
    {


        SoundManager.Instance.PlaySoundEffect(SoundManager.Instance.popSound);

        // ������ ���� ���� �������� �ڷ�ƾ ����
        _askSequence?.Kill();
        _askSequence = null;

        if (_askCoroutine != null)
        {
            StopCoroutine(_askCoroutine);
            _askCoroutine = null;
        }

        AskButton.interactable = false;
        FrontDeskpanel.gameObject.SetActive(true);
        FrontDeskpanel.transform.GetChild(1).gameObject.SetActive(false);

        UI_CustomerText.Instance._currentIndex = 0;
        UI_CustomerText.Instance._isTextEnd = false;
        UI_CustomerText.Instance.CustomerTextUI.text = "";

        _bowl.raycastTarget = true;
        AllowAnimation = true;

        _askSequence = DOTween.Sequence();

        _askSequence.Append(FrontDeskpanel.DOMoveY(_frontDeskPanelYPosition, 1f))
                   .AppendCallback(() =>
                   {
                       CurrencyManager.Instance.RemoveCurrency(CurrencyType.Satisfaction, AskPenalty);

                       if (CustomerManager.Instance.InstancedCustomer != null)
                       {
                           SpriteRenderer spriteRenderer = CustomerManager.Instance.InstancedCustomer.GetComponentInChildren<SpriteRenderer>();

                           if (spriteRenderer != null && spriteRenderer.sprite != null)
                           {
                               string spriteName = spriteRenderer.sprite.name;
                               Debug.Log("���� �մ� ��������Ʈ �̸�: " + spriteName);

                               if (new List<string> {
                               "customer_2", "customer_4", "customer_6", "customer_8",
                               "customer_11", "customer_12", "customer_13", "customer_15",
                               "customer_16", "customer_23", "customer_17"
                               }.Contains(spriteName))
                               {
                                   Debug.Log("���� �մ� �Ѽ�");
                                   AudioClip clip = SoundManager.Instance.customerNegativeSounds[0];
                                   SoundManager.Instance.PlaySoundEffect(clip);
                               }
                               else if (new List<string> {
                               "customer_1", "customer_3", "customer_5", "customer_9",
                               "customer_10", "customer_14", "customer_24", "customer_7",
                               "customer_18", "customer_19", "customer_20", "customer_21", "customer_22"
                               }.Contains(spriteName))
                               {
                                   Debug.Log("���� �մ� �Ѽ�");
                                   AudioClip clip = SoundManager.Instance.customerNegativeSounds[2];
                                   SoundManager.Instance.PlaySoundEffect(clip);
                               }
                           }
                       }
                       else
                       {
                           Debug.Log("�մ� ����");
                       }

                       // ù ��° ��� ��� + �ݹ�
                       PrintText(() =>
                       {
                           Debug.Log("0��° ��� ������ ProceedToNextText ����");
                           ProceedToNextText();
                       });
                   });
    }

    private void ProceedToNextText()
    {
        DialogWindow.Asking = false;
        Debug.Log("�����ߴ�");
        UI_CustomerText.Instance._currentIndex++;

        if (!UI_CustomerText.Instance._isTextEnd)
        {
            Debug.Log("���� �� ������");
            //CurrencyManager.Instance.RemoveCurrency(CurrencyType.Satisfaction, AskPenalty);

            _askCoroutine = StartCoroutine(WaitThenPrintText());
        }
        else
        {
            Debug.Log("������");
            DialogWindow.Asking = false;
            _askCoroutine = StartCoroutine(FinalWaits());
        }
    }

    private IEnumerator WaitThenPrintText()
    {
        yield return new WaitForSeconds(1.0f);
        PrintText(() =>
        {
            Debug.Log("2����");
            ProceedToNextText();
        });
    }

    private IEnumerator FinalWaits()
    {
        yield return new WaitForSeconds(1.0f);

        _askSequence?.Append(FrontDeskpanel.DOMoveY(_frontDeskPanelYPosition + KitchenMoveAmount, 1f))
                    .OnKill(() =>
                    {
                        Debug.Log("�ִϸ��̼��� ����ǰ� OnKill �ݹ� �����");
                    });

        yield return new WaitForSeconds(1.0f);

        FrontDeskpanel.gameObject.SetActive(false);
        FrontDeskpanel.transform.GetChild(1).gameObject.SetActive(true);
        AskButton.interactable = true;
        _bowl.raycastTarget = true;

        Debug.Log("�ٽ� ��ġ�ϰ� ����");
    }


    public void PrintText(Action onComplete = null)
    {
        DialogWindow.Asking = true;

        AskButton.interactable = false;
        UI_CustomerText.Instance.CustomerTextUI.DOKill();

        var customerText = UI_CustomerText.Instance._customerTextStruct.OrderText[UI_CustomerText.Instance._currentIndex];

        UI_CustomerText.Instance.CustomerTextUI.text = customerText;
        UI_CustomerText.Instance.CustomerTextUI.maxVisibleCharacters = 0;

        float duration = customerText.Length * UI_CustomerText.Instance.textSpeed;

        int previousCharCount = 0;

        _typingTween = DOTween.To(() => 0, x =>
        {
            int charCount = Mathf.FloorToInt(x);
            UI_CustomerText.Instance.CustomerTextUI.maxVisibleCharacters = charCount;

            if (charCount > previousCharCount)
            {
                previousCharCount = charCount;
            }
        },
        customerText.Length,
        duration)
        .SetEase(Ease.Linear)
        .OnComplete(() =>
        {
            UI_CustomerText.Instance._isTextEnd = (UI_CustomerText.Instance._currentIndex >= UI_CustomerText.Instance._customerTextStruct.OrderText.Count - 1);
            _typingTween = null;
            onComplete?.Invoke();
        });
    }



    public void StopCurrentProcess()
    {
        // ��� �ڷ�ƾ �ߴ�
        DialogWindow.Asking = false;
        StopAllCoroutines(); // �ڷ�ƾ�� �̴�� �ε�, �Ʒ����� �ʿ��� �͸� ���� ������ ���� ����

        // DOTween �ִϸ��̼� ����
        DOTween.Kill(_askSequence); // FrontDesk �ִϸ��̼Ǹ� ���� (OnAskButtonClick���� ���� seq)
        _askSequence = null;

        if (_typingTween != null && _typingTween.IsActive())
        {
            _typingTween.Kill();
            _typingTween = null;
        }

        // ��� �ؽ�Ʈ �ʱ�ȭ
        UI_CustomerText.Instance.CustomerTextUI.DOKill();
        UI_CustomerText.Instance.CustomerTextUI.text = "";
        UI_CustomerText.Instance._currentIndex = 0;
        UI_CustomerText.Instance._isTextEnd = false;

        // UI ��ġ �ʱ�ȭ
        FrontDeskpanel.transform.DOMoveY(_frontDeskPanelYPosition + KitchenMoveAmount, 0.0f);

        // UI ���� �ʱ�ȭ
        FrontDeskpanel.gameObject.SetActive(false);
        FrontDeskpanel.transform.GetChild(1).gameObject.SetActive(true);
        AskButton.interactable = true;
        _bowl.raycastTarget = true;

        Debug.Log("��� ���� ���� �۾��� ��ҵǾ����ϴ�.");
    }

}
