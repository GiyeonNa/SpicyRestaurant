using System.Collections.Generic;
using UnityEngine;

public class Customer_Special_overbearingMom : Customer_Special
{
    protected override void Initialize()
    {
        base.Initialize();

        _requiredIngredients = new Dictionary<IngredientType, uint>()
        {
            {IngredientType.Meat, 9999}
        };

        // �� �ϵ� ���� ����� �Ѵ�.
        _positiveText = _negativeText;

        OrderManager.Instance.SetOrder(_requiredIngredients);
    }
    protected override void SpriteInitialize()
    {
        return;
    }
}
