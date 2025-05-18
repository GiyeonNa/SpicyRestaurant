using System;
using System.Collections.Generic;
using UnityEngine;

public class IngredientManager : MonoBehaviour
{
    public static IngredientManager Instance;

    public List<Ingredient> IngredientsList = new List<Ingredient>();

    [SerializeField]
    private List<IngredientDataSO> _ingredientDataSOList;

    // ingredientCounts�� private�� �����ϰ�, �ܺο��� ������ �� �ֵ��� ������Ƽ ����
    private Dictionary<IngredientType, int> _ingredientCounts = new Dictionary<IngredientType, int>();

    public Dictionary<IngredientType, int> IngredientCounts
    {
        get { return _ingredientCounts; }
    }

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

        InitIngredientList();

       //SaveAllIngredientCounts();
       LoadAllIngredientCounts();

    }

    private void Start()
    {
        
        SetIngridentsAmount(GameManager.Instance.GetIngredientData());
    }

    
    private void InitIngredientList()
    {
        List<IngredientType> enabledIngredients = GameManager.Instance.GetEnableIngredients();

        foreach (IngredientDataSO ingredientData in _ingredientDataSOList)
        {
            for (int i = 0; i < enabledIngredients.Count; i++)
            {
                if (enabledIngredients[i] == ingredientData.Type)
                {
                    Ingredient ingredient = new Ingredient(ingredientData, 0);
                    IngredientsList.Add(ingredient);
                    // Initialize ingredient count to 0
                    _ingredientCounts[ingredientData.Type] = 0;
                }
            }
        }
    }

    public Ingredient GetIngredient(IngredientType type)
    {
        foreach (Ingredient ingredient in IngredientsList)
        {
            if (type == ingredient.Type)
            {
                return ingredient;
            }
        }

        Debug.Log($"{type} ��ᰡ �����ϴ�!");
        return null;
    }

    public List<string> GetIngredientsTypeString()
    {
        if (IngredientsList.Count == 0)
        {
            Debug.Log("IngredeintsList�� ����ֽ��ϴ�. (�����: ������)");
            return null;
        }

        List<string> selectableIngredients = new List<string>();
        foreach (Ingredient ingredient in IngredientsList)
        {
            if (ingredient.IsBasic)
            {
                continue;
            }
            string ingredientName = ConvertIngredientTypeToKoreanString(ingredient.Type);

            selectableIngredients.Add(ingredientName);
        }

        return selectableIngredients;
    }

    public void SetIngridentsAmount(Dictionary<IngredientType, uint> ingredientsDict)
    {
        foreach (Ingredient ingredient in IngredientsList)
        {
            if (ingredient.IsBasic)
            {
                ingredient.Amount = 0;
                continue;
            }

            if (!ingredientsDict.TryGetValue(ingredient.Type, out ingredient.Amount))
            {
                Debug.Log($"{ingredient.Type} ���� �Ҵ� �����Ͽ����ϴ�. (�����: ������)");
            }
        }
    }

    public void AddAllIngredient(IngredientType type, int amount)
    {
        if (_ingredientCounts.ContainsKey(type))
        {
            _ingredientCounts[type] += amount;
        }
        else
        {
            _ingredientCounts[type] = amount;
        }
    }

    public int GetAllIngredientCount(IngredientType type)
    {
        if (_ingredientCounts.ContainsKey(type))
        {
            return _ingredientCounts[type];
        }
        return 0;
    }

    public void SaveAllIngredientCounts()
    {
        Dictionary<string, int> dict = new();

        foreach (var ingredient in IngredientsList)
        {
            dict[ingredient.Type.ToString()] = (int)ingredient.Amount;
            Debug.Log($"��� ����� {ingredient.Type}: {ingredient.Amount}");
        }

        GameManager.Instance.SetIngredientManage(dict);
        LoadAllIngredientCounts();
    }


    //���� �� �ƾ���
    /*public void LoadAllIngredientCounts()//�̰� �ȴ�
    {

        Dictionary<IngredientType, uint> savedCounts = GameManager.Instance.GetIngredientData();

        foreach (var ingredient in IngredientsList)
        {
            if (savedCounts.TryGetValue(ingredient.Type, out uint savedAmount))
            {
                ingredient.Amount = savedAmount;
                Debug.Log($"��� �ε�� {ingredient.Type} �� {ingredient.Amount}");
            }
        }
    }*/

    public void LoadAllIngredientCounts()
    {
        Dictionary<IngredientType, uint> savedCounts = GameManager.Instance.GetIngredientData();

        foreach (var ingredient in IngredientsList)
        {
            // ����� ���� ������ �װ� ����, ������ 0���� �ʱ�ȭ
            if (!savedCounts.TryGetValue(ingredient.Type, out uint savedAmount))
            {
                savedAmount = 0;
                Debug.Log($"����� ������ ����: {ingredient.Type}, 0���� �ʱ�ȭ��");
            }

            ingredient.Amount = savedAmount;

            Debug.Log($"��� �ε�� {ingredient.Type} �� {ingredient.Amount}");
        }
    }



    //����
    public void ClearAllPlayerPrefsData()
    {
        // ��� ��� ������ ����
        foreach (var ingredient in IngredientsList)
        {
            string key = ingredient.Type.ToString();
            if (PlayerPrefs.HasKey(key))
            {
                PlayerPrefs.DeleteKey(key);
                Debug.Log($"{key} �����Ͱ� PlayerPrefs���� �����Ǿ����ϴ�.");
            }
        }

        // ���� ������ ����
        SaveAllIngredientCounts();
       // LoadAllIngredientCounts();
    }

    private string ConvertIngredientTypeToKoreanString(IngredientType type)
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


}



/*using System.Collections.Generic;
using UnityEngine;

public class IngredientManager : MonoBehaviour
{
    public static IngredientManager Instance;

    public List<Ingredient> IngredientsList = new List<Ingredient>();

    [SerializeField]
    private List<IngredientDataSO> _ingredientDataSOList;



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

        InitIngredientList();
    }

    private void Start()
    {
        SetIngridentsAmount(GameManager.Instance.GetIngredientData());
    }


    private void InitIngredientList()
    {
        List<IngredientType> enabledIngredients = GameManager.Instance.GetEnableIngredients();

        foreach (IngredientDataSO ingredientData in _ingredientDataSOList)
        {
            for (int i = 0; i < enabledIngredients.Count; i++)
            {
                if (enabledIngredients[i] == ingredientData.Type)
                {
                    Ingredient ingredient = new Ingredient(ingredientData, 0);
                    IngredientsList.Add(ingredient);
                }
            }
        }
    }

    public Ingredient GetIngredient(IngredientType type)
    {
        foreach (Ingredient ingredient in IngredientsList)
        {
            if (type == ingredient.Type)
            {
                return ingredient;
            }
        }

        Debug.LogWarning($"{type} ��ᰡ �����ϴ�!");
        return null;
    }

    public List<string> GetIngredientsTypeString()
    {
        if(IngredientsList.Count == 0)
        {
            Debug.Log("IngredeintsList�� ����ֽ��ϴ�. (�����: ������)");
            return null;
        }

        List<string> selectableIngredients = new List<string>();
        foreach(Ingredient ingredient in IngredientsList)
        {
            if(ingredient.IsBasic)
            {
                continue;
            }
            string ingredientName = ConvertIngredientTypeToKoreanString(ingredient.Type);

            selectableIngredients.Add(ingredientName);
        }

        return selectableIngredients;
    }

    public void SetIngridentsAmount(Dictionary<IngredientType, uint> ingredientsDict)
    {
        foreach(Ingredient ingredient in IngredientsList)
        {
            if(ingredient.IsBasic)
            {
                ingredient.Amount = 0;
                continue;
            }

            if(!ingredientsDict.TryGetValue(ingredient.Type, out ingredient.Amount))
            {
                Debug.Log($"{ingredient.Type} ���� �Ҵ� �����Ͽ����ϴ�. (�����: ������)");
            }
        }
    }

    private string ConvertIngredientTypeToKoreanString(IngredientType type)
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

}*/
