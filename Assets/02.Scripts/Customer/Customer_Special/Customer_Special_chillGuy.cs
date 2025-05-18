using System.Collections.Generic;
using UnityEngine;

public class Customer_Special_chillGuy : Customer_Special
{
    protected override void Initialize()
    {
        base.Initialize();

        _selectableIngredients = IngredientManager.Instance.GetIngredientsTypeString();
        _requiredIngredients = new Dictionary<IngredientType, uint>();
        foreach(string ingredients in _selectableIngredients)
        {
            IngredientType tmp = ConvertKoreanStringToIngredientType(ingredients);
            if(tmp == IngredientType.Cilantro)
            {
                continue;
            }
            _requiredIngredients[tmp] = 1;
        }

        // ĥ ���̴� ���� ����� ���� �ʴ´�.
        _negativeText = _positiveText;
        // �ֹ��ϱ�
        OrderManager.Instance.SetOrder(_requiredIngredients);
    }
    protected override void SpriteInitialize()
    {
        return;
    }

}
