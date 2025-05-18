using UnityEngine;

public class Customer_Special : Customer_Base
{
    [SerializeField]
    private string _customerSpecialName;
    void Start()
    {
        Initialize();
        SpriteInit();
        MakeCustomerTextStruct();

        // �׽�Ʈ��
        Debug.Log($"{_customerSpecialName}");
        foreach (var i in _orderText)
        {
            Debug.Log(i);
        }
        Debug.Log($"{_positiveText}");
        Debug.Log($"{_negativeText}");
        Debug.Log("�ֹ���");

    }

    protected virtual void Initialize()
    {
        // Ÿ�� ����
        CustomerType = CustomerTypes.Special;
        _customerSpriterenderer = GetComponent<SpriteRenderer>();


        var data = ScriptManager.Instance.GetSpecialCustomerByName(_customerSpecialName);
        _orderText.Add(data.ordertext);
        if (data.nexttext != "")
        {
            _orderText.Add(data.nexttext);
        }
        _positiveText = data.positivetext;
        _negativeText = data.negativetext;

        
    }

    
    private void SpriteInit()
    {
        SpriteInitialize();
        _customerSpriterenderer.sprite = _customerSprite;
    }

    protected virtual void SpriteInitialize()
    {
        _customerSprite = ScriptManager.Instance.GetRandomCustomerNormalImages();
    }
    
}
