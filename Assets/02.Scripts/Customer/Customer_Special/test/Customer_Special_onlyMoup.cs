using System.Collections.Generic;
using UnityEngine;

public class Customer_Special_onlyMoup : Customer_Special
{
    protected override void Initialize()
    {
        // º¸·ù
        base.Initialize();

        _requiredIngredients = new Dictionary<IngredientType, uint>();
        OrderManager.Instance.SetOrder(_requiredIngredients);
    }
}
