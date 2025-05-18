using System.Collections.Generic;
using UnityEngine;

public class Customer_Special_blackwhiteAhn : Customer_Special
{
    protected override void Initialize()
    {
        base.Initialize();

        _requiredIngredients = new Dictionary<IngredientType, uint>()
        {
            {IngredientType.Meat, 1}, {IngredientType.EnokiMushroom, 1}, {IngredientType.BokChoy, 1}, {IngredientType.Cilantro, 2}
        };
        OrderManager.Instance.SetOrder(_requiredIngredients);
    }
}
