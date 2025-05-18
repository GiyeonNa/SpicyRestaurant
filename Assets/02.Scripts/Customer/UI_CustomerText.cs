using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System;
using DG.Tweening;

public struct CustomerTextStruct
{
    public CustomerTypes CustomerType;
    public List<string> OrderText;
    public string PositiveText;
    public string NegativeText;
}

public class UI_CustomerText : MonoBehaviour
{
    public static UI_CustomerText Instance;


    public TextMeshProUGUI CustomerTextUI;
    public Button YesButton;
    public Button GetOutButton;

    public Action OnNextCustomer;

    public CustomerTextStruct _customerTextStruct;//0416
    public int _currentIndex;
    public bool _isTextEnd = false;

    public float _duration;

    //[SerializeField]
    public float textSpeed;

    public UI_IngredientsHolder IngredientsHolder;


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
        gameObject.SetActive(false);
    }

    public void SetCustomerText(CustomerTextStruct inputTextStruct)
    {
        _customerTextStruct = inputTextStruct;
    }

    public void SetCustomerText()
    {
        gameObject.SetActive(true);
        PrintText();
    }

    public void PrintText()
    {


        //�Ʒ����� ���� �־��� ��
        CustomerTextUI.DOKill();

        CustomerTextUI.text = _customerTextStruct.OrderText[_currentIndex];



        CustomerTextUI.maxVisibleCharacters = 0;

        _duration = _customerTextStruct.OrderText[_currentIndex].Length * textSpeed;

        int previousCharCount = 0;

        DOTween.To(x =>
        {
            int currentCharCount = (int)x;
            CustomerTextUI.maxVisibleCharacters = currentCharCount;

            if (currentCharCount > previousCharCount)
            {
                SoundManager.Instance.PlayRandomSoundInList(SoundManager.Instance.typeSounds);
                previousCharCount = currentCharCount;
            }
        }
        , 0f, CustomerTextUI.text.Length, _duration).SetEase(Ease.Linear).SetId("TextTween");

        // Mark as text end if this is the last text in the list
        if (_currentIndex == _customerTextStruct.OrderText.Count - 1)
        {
            _isTextEnd = true;
        }

        // Hide the YesButton if the customer type is OnlyText and the text has ended
        if (_isTextEnd && _customerTextStruct.CustomerType == CustomerTypes.OnlyText)
        {
            YesButton.gameObject.SetActive(false);
        }
    }

    public void PrintResutlText(bool result)
    {
        gameObject.SetActive(true);
        YesButton.gameObject.SetActive(false);

        _duration = _customerTextStruct.PositiveText.Length * textSpeed;

        //UI_Shop.Instance.AllowAnimation = true;

        if (result)
        {
            CustomerTextUI.text = _customerTextStruct.PositiveText;
            SoundManager.Instance.PlayRandomSoundInList(SoundManager.Instance.customerPositiveSounds);
        }
        else
        {
            CustomerTextUI.text = _customerTextStruct.NegativeText;
            //SoundManager.Instance.PlayRandomSoundInList(SoundManager.Instance.customerNegativeSounds);
            if (CustomerManager.Instance.InstancedCustomer != null)
            {
                // �ڽ� �����ؼ� SpriteRenderer ã��
                SpriteRenderer spriteRenderer = CustomerManager.Instance.InstancedCustomer.GetComponentInChildren<SpriteRenderer>();

                if (spriteRenderer != null && spriteRenderer.sprite != null)
                {
                    string spriteName = spriteRenderer.sprite.name;
                    Debug.Log("���� �մ� ��������Ʈ �̸�: " + spriteName);

                    // ���� �մ�
                    if (new List<string> { "customer_2", "customer_4", "customer_6"
    , "customer_8", "customer_11", "customer_12", "customer_13", "customer_15"
    , "customer_16", "customer_23", "customer_17"}.Contains(spriteName))
                    {
                        Debug.Log("�ֹ� ���� ���� �մ� �Ѽ�");

                        // int randomIndex = UnityEngine.Random.Range(0, 2); // 0 �Ǵ� 1
                        AudioClip clip = SoundManager.Instance.customerNegativeSounds[0];
                        SoundManager.Instance.PlaySoundEffect(clip);

                    }
                    // ���� �մ�
                    else if (new List<string> { "customer_1", "customer_3", "customer_5"
    , "customer_9", "customer_10", "customer_14", "customer_24"
    , "customer_7", "customer_18", "customer_19", "customer_20"
    , "customer_21", "customer_22"}.Contains(spriteName))
                    {
                        Debug.Log("�ֹ� ���� ���� �մ� �Ѽ�");

                        //int randomIndex = UnityEngine.Random.Range(2, 4); // 2 �Ǵ� 3
                        AudioClip clip = SoundManager.Instance.customerNegativeSounds[2];
                        SoundManager.Instance.PlaySoundEffect(clip);

                    }
                }
            }

            else
            {
                Debug.Log("�մ� ����");
            }
        }

        int previousCharCount = 0;
        CustomerTextUI.DOKill();
        DOTween.To(x =>
        {
            int currentCharCount = (int)x;
            CustomerTextUI.maxVisibleCharacters = currentCharCount;

            if (currentCharCount > previousCharCount)
            {
                SoundManager.Instance.PlayRandomSoundInList(SoundManager.Instance.typeSounds);
                previousCharCount = currentCharCount;
            }
        }
        , 0f, CustomerTextUI.text.Length, _duration).SetEase(Ease.Linear).SetId("TextTween");

        //UI_Shop.Instance.AllowAnimation = false;

    }

    public void OnYesButtonClick()
    {
        SoundManager.Instance.PlaySoundEffect(SoundManager.Instance.popSound);
        if (!_isTextEnd)
        {
            _currentIndex++;
            PrintText();
            return;
        }
        DOTween.Kill("TextTween");
        UI_Shop.Instance.ToggleKitchen();
        //�����̴� �ʱ�ȭ
        IngredientsHolder.SetHolderPosition();

        //SoundManager.Instance.PlaySoundEffect(SoundManager.Instance.popSound);
        gameObject.SetActive(false);

        //UI_Shop.Instance.AllowAnimation = true;
        //UI_Shop.Instance.AskButton.interactable = true;
    }

    public void OnGetOutButtonClick()
    {
        Debug.Log("�� ��° �մ� ���´���?" + CustomerManager.Instance.CompletedCustomers + "��° �մ�");

        SoundManager.Instance.PlaySoundEffect(SoundManager.Instance.popSound);
        if (CustomerManager.Instance.CompletedCustomers + 1 == CustomerManager.Instance.MaxCustomers)
        {
            CustomerManager.Instance.DestroyCustomer();
        }
        else
        {
            OnNextCustomer();
        }


        _currentIndex = 0;
        _isTextEnd = false;

        DOTween.Kill("TextTween");
        // SoundManager.Instance.PlaySoundEffect(SoundManager.Instance.matchSound);
        YesButton.gameObject.SetActive(true);
        gameObject.SetActive(false);
    }
}
