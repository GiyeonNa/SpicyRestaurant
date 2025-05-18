using System.Collections.Generic;
using UnityEngine;

public enum CustomerTypes
{
    Normal,
    OnlyText,
    Special,


    Count,
}

public class Customer_Base : MonoBehaviour
{


    public CustomerTypes CustomerType;

    [SerializeField]
    protected Dictionary<IngredientType, uint> _requiredIngredients;
    [SerializeField]
    protected int _requiredIngredientsCount = 6;

    [SerializeField]
    protected List<string> _orderText;
    [SerializeField]
    protected string _positiveText = "";
    [SerializeField]
    protected string _negativeText = "";

    [SerializeField]
    protected Sprite _customerSprite;
    protected SpriteRenderer _customerSpriterenderer;

    protected CustomerTextStruct _customerTextStruct;

    protected List<string> _selectableIngredients = new List<string>();


    public virtual List<string> GetText()
    {
        return _orderText;
    }

    // ������ �������� ��� 
    public virtual string GetPositiveText()
    {
        return _positiveText;
    }

    public virtual string GetNegativeText()
    {
        return _negativeText;
    }

    public virtual Dictionary<IngredientType, uint> GetRequiredIngredients()
    {
        return _requiredIngredients;
    }

    protected IngredientType ConvertKoreanStringToIngredientType(string korean)
    {
        IngredientType ingredientType = IngredientType.GlassNoodle;
        switch (korean)
        {
            case "�߱����":
                ingredientType = IngredientType.GlassNoodle;
                break;
            case "����":
                ingredientType = IngredientType.Shrimp;
                break;
            case "���":
                ingredientType = IngredientType.Cilantro;
                break;
            case "���":
                ingredientType = IngredientType.Meat;
                break;
            case "�κ�":
                ingredientType = IngredientType.Tofu;
                break;
            case "û��ä":
                ingredientType = IngredientType.BokChoy;
                break;
            case "���̹���":
                ingredientType = IngredientType.EnokiMushroom;
                break;
            case "���̹���":
                ingredientType = IngredientType.WoodEarMushroom;
                break;
            default:
                Debug.Log("��ȿ���� ���� Ÿ��(������)");
                break;
        }
        return ingredientType;
    }


    protected string ConvertIngredientTypeToKoreanString(IngredientType type)
    {
        string ingredientName = "";
        switch (type)
        {
            case IngredientType.GlassNoodle:
                ingredientName = "�߱����";
                break;

            case IngredientType.Shrimp:
                ingredientName = "����";
                break;

            case IngredientType.Cilantro:
                ingredientName = "���";
                break;

            case IngredientType.Meat:
                ingredientName = "���";
                break;

            case IngredientType.Tofu:
                ingredientName = "�κ�";
                break;

            case IngredientType.BokChoy:
                ingredientName = "û��ä";
                break;

            case IngredientType.EnokiMushroom:
                ingredientName = "���̹���";
                break;

            case IngredientType.WoodEarMushroom:
                ingredientName = "���̹���";
                break;

            default:
                Debug.Log("��ȿ���� ���� Ÿ���Դϴ�. (�����: ������)");
                break;
        }

        return ingredientName;
    }

    protected void MakeCustomerTextStruct()
    {
        // �ؽ�Ʈ����ü �����
        _customerTextStruct.CustomerType = this.CustomerType;
        _customerTextStruct.OrderText = _orderText;
        _customerTextStruct.PositiveText = _positiveText;
        _customerTextStruct.NegativeText = _negativeText;

        // ��������
        UI_CustomerText.Instance.SetCustomerText(_customerTextStruct);
    }
}
