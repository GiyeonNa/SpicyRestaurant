using System.Collections.Generic;
using UnityEngine;
using static TMPro.SpriteAssetUtilities.TexturePacker_JsonArray;
using static UnityEngine.Rendering.DebugUI;

public class Customer_Normal : Customer_Base
{
    [SerializeField]
    private ScriptManager.CustomerNormalTextData _customerNomalTextData;



    private void Start()
    {
        _customerSprite = ScriptManager.Instance.GetRandomCustomerNormalImages();
        _customerSpriterenderer = GetComponent<SpriteRenderer>();
        _customerSpriterenderer.sprite = _customerSprite;
        _selectableIngredients = IngredientManager.Instance.GetIngredientsTypeString();
        // �������� �䱸�� ��� ���ϱ�
        SetRequiredIngredients();

        // �ֹ��ϱ�
        OrderManager.Instance.SetOrder(_requiredIngredients);
        MakeCustomerTextStruct();
    }

    private void SetRequiredIngredients()
    {
        _requiredIngredients = new Dictionary<IngredientType, uint>();
        for (int i = 0; i < _requiredIngredientsCount; i++)
        {
            string randomIngredientKorean = _selectableIngredients[Random.Range(0, _selectableIngredients.Count)];
            var randomIngredient = ConvertKoreanStringToIngredientType(randomIngredientKorean);
            if (_requiredIngredients.ContainsKey(randomIngredient))
            {
                _requiredIngredients[randomIngredient]++;
            }
            else
            {
                _requiredIngredients[randomIngredient] = 1;
            }
        }


        SetText();
        var a = _orderText;
        foreach (var i in a)
        {
            Debug.Log(i);
        }
        Debug.Log(GetPositiveText());
        Debug.Log(GetNegativeText());
    }

    

    private void SetText()///******���⼭ �� �κ� �ٹٲ� �Ұ���.
    {
        _orderText = new List<string>();

        // ��簡 2�� ���´ٸ� ���� ���� ���
        string _foreText = "";
        _customerNomalTextData = ScriptManager.Instance.GetRandomCustomerText();



        _foreText += _customerNomalTextData.foreword + " ";

        //0415
        //�ٹٲ��� �ߴ��� ����
        bool addedLineBreak = false;


        //0415
        foreach (var ingredient in _requiredIngredients)
        {
            // ù ��° ��ῡ���� �ٹٲ��� �߰�
            if (!addedLineBreak)
            {
                _foreText += "\n"; // ù ��° ��� �տ� �ٹٲ� �߰�
                addedLineBreak = true; // �ٹٲ��� �߰��� �� �÷��׸� true�� ����
            }

            _foreText += $"{ConvertIngredientTypeToKoreanString(ingredient.Key)} {ingredient.Value}��, ";
        }

        /*foreach (var ingredient in _requiredIngredients)
        {
            _foreText += $"{ConvertIngredientTypeToKoreanString(ingredient.Key)} {ingredient.Value}��, ";
        }*/
        _foreText = _foreText[..^2];
        _foreText += _customerNomalTextData.afterword;

        _orderText.Add(_foreText);

        // ��ȭ�� 2�� ���´ٸ� ���߿� ���� ���.
        if (_customerNomalTextData.nexttext != "")
        {
            _orderText.Add(_customerNomalTextData.nexttext);
        }

        // ���� ���� �޾ƿͼ� �־��ش�.
        var positiveTextData = ScriptManager.Instance.GetRandomCustomerPositive();
        var negativeTextData = ScriptManager.Instance.GetRandomCustomerNegative();
        _positiveText = positiveTextData.positivetext;
        _negativeText = negativeTextData.negativetext;
    }
}
