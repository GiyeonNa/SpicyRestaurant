using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[System.Serializable]
public enum Scenes
{
    Match3Game,
    ShopOpen,
    EndingScene,

    Count,
}

public class SaveData
{

    public int CurrentStage;
    // Dictionary�� ����ȭ�� �Ұ����ؼ� List<CurrencyData>��ü ������ ����Ѵ�.
    public List<CurrencyData> CurrencyList;
    public List<IngredientData> IngredientsList;
    public Scenes NextScene;

    // �ߺ������� ���� ť ����Ÿ
    // ť�� ����ȭ ���� �ʱ� ������ LIst�� �����Ѵ�.
    public List<int> CustomerNormalIndexList;
    public List<int> CustomerPositiveIndexList;
    public List<int> CustomerNegativeIndexList;
    public List<int> CustomerOnlyTextIndexList;
    public List<int> CustomerNormalImageList;
    public List<int> LoadingIndexList;

    public SaveData(int currentStage, Dictionary<CurrencyType, int> playerCurrency, Dictionary<IngredientType, uint> ingredient, Scenes scene,
        Queue<int> customerNormalQueue, Queue<int> customerPositiveQueue, Queue<int> customerNegativeQueue, Queue<int> customerOnlyTextQueue,
        Queue<int> customerNormalImageQueue, Queue<int> loadingIndexQueue)
    {
        CurrentStage = currentStage;
        NextScene = scene;

        // Dictionary�� List�� ��ȯ
        CurrencyList = new List<CurrencyData>();
        if (playerCurrency != null)
        {
            foreach (var pair in playerCurrency)
            {
                CurrencyList.Add(new CurrencyData { Type = pair.Key, Amount = pair.Value });
            }
        }

        IngredientsList = new List<IngredientData>();
        if (ingredient != null)
        {
            foreach (var pair in ingredient)
            {
                IngredientsList.Add(new IngredientData { Type = pair.Key, Amount = (int)pair.Value });
            }
        }

        // Queue�� List�� ��ȯ
        CustomerNormalIndexList = new List<int>(customerNormalQueue ?? new Queue<int>());
        CustomerPositiveIndexList = new List<int>(customerPositiveQueue ?? new Queue<int>());
        CustomerNegativeIndexList = new List<int>(customerNegativeQueue ?? new Queue<int>());
        CustomerOnlyTextIndexList = new List<int>(customerOnlyTextQueue ?? new Queue<int>());
        CustomerNormalImageList = new List<int>(customerNormalImageQueue ?? new Queue<int>());
        LoadingIndexList = new List<int>(loadingIndexQueue ?? new Queue<int>());

    }
    public SaveData() { }

    public Dictionary<CurrencyType, int> GetCurrencyDictionary()
    {
        Dictionary<CurrencyType, int> result = new Dictionary<CurrencyType, int>();
        if (CurrencyList != null)
        {
            foreach (var currency in CurrencyList)
            {
                result[currency.Type] = currency.Amount;
            }
        }
        return result;
    }
    public Dictionary<string, int> GetIngredientDictionary()
    {
        Dictionary<string, int> result = new Dictionary<string, int>();
        if (IngredientsList != null)
        {
            foreach (var ingredient in IngredientsList)
            {
                result[ingredient.Type.ToString()] = ingredient.Amount;
            }
        }
        return result;
    }

    // ����Ʈ�� ��ȯ�� ť����
    // �ٽ� ť�� �޴� �޼���
    public Queue<int> GetCustomerNormalIndexQueue()
    {
        Queue<int> queue = new Queue<int>();
        if (CustomerNormalIndexList != null)
        {
            foreach (var item in CustomerNormalIndexList)
            {
                queue.Enqueue(item);
            }
        }
        return queue;
    }

    public Queue<int> GetCustomerPositiveIndexQueue()
    {
        Queue<int> queue = new Queue<int>();
        if (CustomerPositiveIndexList != null)
        {
            foreach (var item in CustomerPositiveIndexList)
            {
                queue.Enqueue(item);
            }
        }
        return queue;
    }

    public Queue<int> GetCustomerNegativeIndexQueue()
    {
        Queue<int> queue = new Queue<int>();
        if (CustomerNegativeIndexList != null)
        {
            foreach (var item in CustomerNegativeIndexList)
            {
                queue.Enqueue(item);
            }
        }
        return queue;
    }

    public Queue<int> GetCustomerOnlyTextIndexQueue()
    {
        Queue<int> queue = new Queue<int>();
        if (CustomerOnlyTextIndexList != null)
        {
            foreach (var item in CustomerOnlyTextIndexList)
            {
                queue.Enqueue(item);
            }
        }
        return queue;
    }

    public Queue<int> GetCustomerNormalImageQueue()
    {
        Queue<int> queue = new Queue<int>();
        if (CustomerNormalImageList != null)
        {
            foreach (var item in CustomerNormalImageList)
            {
                queue.Enqueue(item);
            }
        }
        return queue;
    }

    public Queue<int> GetLoadingIndexQueue()
    {
        Queue<int> queue = new Queue<int>();
        if (LoadingIndexList != null)
        {
            foreach (var item in LoadingIndexList)
            {
                queue.Enqueue(item);
            }
        }
        return queue;
    }


}

[Serializable]
public class CurrencyData
{
    public CurrencyType Type;
    public int Amount;
}
[Serializable]
public class IngredientData
{
    public IngredientType Type;
    public int Amount;
}



public class SaveManager : MonoBehaviour
{

    private const string SAVE_KEY = "SaveData";

    // �� �̸� ���� Dictionary �߰�
    private static Dictionary<Scenes, string> sceneNameMapping = new Dictionary<Scenes, string>()
    {
        { Scenes.Match3Game, "Match3Game" },
        { Scenes.ShopOpen, "ShopOpen" },
        { Scenes.EndingScene, "EndingScene" },
        // �ٸ� ���鵵 �ʿ信 ���� �߰�
    };

    // �� �̸��� �������� �޼��� �߰�
    public static string GetSceneName(Scenes scene)
    {
        if (sceneNameMapping.TryGetValue(scene, out string sceneName))
        {
            return sceneName;
        }

        Debug.LogWarning($"�� �̸� ������ �����ϴ�: {scene}. �⺻ �̸��� ����մϴ�.");
        return scene.ToString();
    }

    public static void SaveGame(int currentStage, Dictionary<CurrencyType, int> playerCurrency, Dictionary<IngredientType, uint> ingredient, Scenes scene)
    {
        // ����� ���: ��� ���� Ȯ��
        if (ingredient != null && ingredient.Count > 0)
        {
            Debug.Log("����� ��� ����:");
            foreach (var pair in ingredient)
            {
                Debug.Log($"IngredientType: {pair.Key}, Amount: {pair.Value}");
            }
        }
        else
        {
            Debug.Log("����� ��� ������ �����ϴ�.");
        }

        SaveData data = new SaveData(currentStage, playerCurrency, ingredient, scene,
            GameManager.Instance.CustomerNormalIndexQueue,
            GameManager.Instance.CustomerPositiveIndexQueue,
            GameManager.Instance.CustomerNegativeIndexQueue,
            GameManager.Instance.CustomerOnlyTextIndexQueue,
            GameManager.Instance.CustomerNormalImageQueue,
            GameManager.Instance.LoadingIndexQueue);

        // �� �̸� ����� ���
        Debug.Log("����� �� �̸�: " + scene.ToString());

        string jsonData = JsonUtility.ToJson(data);
        PlayerPrefs.SetString(SAVE_KEY, jsonData);
        PlayerPrefs.Save();

        Debug.Log("Game saved: " + jsonData);
        Scenes savedScene = LoadGame();
    }
    public static Scenes LoadGame()
    {
        Debug.Log("�ε� ���� �Լ� ����");

        if (PlayerPrefs.HasKey(SAVE_KEY))
        {
            string jsonData = PlayerPrefs.GetString(SAVE_KEY);
            SaveData data = JsonUtility.FromJson<SaveData>(jsonData);
            Debug.Log("Game loaded: " + jsonData);

            // ����, ��ȭ, ��� ���� ����
            GameManager.Instance.SetCurrentLevel(data.CurrentStage);
            Debug.Log("SaveManager�� LoadGame���� �ҷ��� ���� �������� ������?" + data.CurrentStage);

            GameManager.Instance.SetPlayerCurrecyData(data.GetCurrencyDictionary());

            var ingredientDict = data.GetIngredientDictionary();
            if (ingredientDict != null && ingredientDict.Count > 0)
            {
                Debug.Log("�ε�� ��� ����:");
                foreach (var pair in ingredientDict)
                {
                    Debug.Log($"IngredientType: {pair.Key}, Amount: {pair.Value}");
                }
            }
            else
            {
                Debug.Log("�ε�� ��� ������ �����ϴ�.");
            }

            GameManager.Instance.SetIngredientManage(ingredientDict);

            // ť ������
            GameManager.Instance.CustomerNormalIndexQueue = data.GetCustomerNormalIndexQueue();
            GameManager.Instance.CustomerPositiveIndexQueue = data.GetCustomerPositiveIndexQueue();
            GameManager.Instance.CustomerNegativeIndexQueue = data.GetCustomerNegativeIndexQueue();
            GameManager.Instance.CustomerOnlyTextIndexQueue = data.GetCustomerOnlyTextIndexQueue();
            GameManager.Instance.CustomerNormalImageQueue = data.GetCustomerNormalImageQueue();
            GameManager.Instance.LoadingIndexQueue = data.GetLoadingIndexQueue();

            

            // ���� ó��
            if (data.CurrentStage > 2)//>=�̾��� //+1�� GameManager.Instance.MaxLevel+1�� �߰���
            {
                Debug.Log("***SaveManager** ���� ������ ���̱淡 ��������?" + data.CurrentStage);

                data.NextScene = Scenes.EndingScene;
                Debug.Log("����ó��");
            }


            Debug.Log("�ҷ��� �� �̸�: " + data.NextScene.ToString());
            return data.NextScene;
        }
        else
        {
            // ������ ���� �� �ʱ�ȭ
            GameManager.Instance.SetCurrentLevel(0);
            GameManager.Instance.SetPlayerCurrecyData(null);

            Dictionary<string, int> emptyIngredients = new Dictionary<string, int>();
            GameManager.Instance.SetIngredientManage(emptyIngredients);

            GameManager.Instance.CustomerNormalIndexQueue = new Queue<int>();
            GameManager.Instance.CustomerPositiveIndexQueue = new Queue<int>();
            GameManager.Instance.CustomerNegativeIndexQueue = new Queue<int>();
            GameManager.Instance.CustomerOnlyTextIndexQueue = new Queue<int>();
            GameManager.Instance.CustomerNormalImageQueue = new Queue<int>();
            GameManager.Instance.LoadingIndexQueue = new Queue<int>();

            Debug.Log("����� ������ ���� - �⺻ Match3Game ������ �̵�");

            Debug.Log("�ҷ��� �� �̸� (�⺻��): " + Scenes.Match3Game.ToString());

            return Scenes.Match3Game;


        }
    }




    public static void DeleteSave()
    {
        Debug.LogWarning("DeleteSave ȣ���!!!");

        PlayerPrefs.DeleteKey(SAVE_KEY);
        Debug.Log("���� ������ ������");

        // �⺻�� ����
        //GameManager.Instance.SetCurrentLevel(0);

        Dictionary<CurrencyType, int> defaultCurrency = new Dictionary<CurrencyType, int>()
    {
        { CurrencyType.Money, 500 },          // �⺻ ����
        { CurrencyType.Satisfaction, 50 }     // �⺻ ������
    };
        GameManager.Instance.SetPlayerCurrecyData(defaultCurrency);

        Dictionary<string, int> emptyIngredients = new Dictionary<string, int>();
        GameManager.Instance.SetIngredientManage(emptyIngredients);

        GameManager.Instance.CustomerNormalIndexQueue = new Queue<int>();
        GameManager.Instance.CustomerPositiveIndexQueue = new Queue<int>();
        GameManager.Instance.CustomerNegativeIndexQueue = new Queue<int>();
        GameManager.Instance.CustomerOnlyTextIndexQueue = new Queue<int>();
        GameManager.Instance.CustomerNormalImageQueue = new Queue<int>();
        GameManager.Instance.LoadingIndexQueue = new Queue<int>();

        //���丮 ó������ ������
        StorySaveManager.Instance.DeleteSave();
        //GameManager.Instance.SetCurrentLevel(0);
        // GameManager.Instance.LoadLevelData();
        GameManager.Instance.ResetLevel();

        CurrencyManager.Instance.ResetCurrencyData();
        IngredientManager.Instance.ClearAllPlayerPrefsData();

        //������ ��ȭ �ʱ�ȭ
        CurrencyManager.Instance.SaveCurrencyData();
        CurrencyManager.Instance.LoadCurrencyData();

        //��ȭ ���� �ʱ�ȭ
        CurrencyManager.Instance.ResetCurrencyInOutData();
        CurrencyManager.Instance.SaveCurrencyInOutData();
        CurrencyManager.Instance.LoadCurrencyInOutData();

        //��� �ʱ�ȭ
        IngredientManager.Instance.SaveAllIngredientCounts();
       // IngredientManager.Instance.LoadAllIngredientCounts();

        Debug.Log("���� �������� �ʱ�ȭ��: " + GameManager.Instance.CurrentLevel);

        // �ʱ� ����
        SaveGame(
             GameManager.Instance.CurrentLevel,  // �̰� ���� 0���� �޵���
            defaultCurrency,
            new Dictionary<IngredientType, uint>(),  // �� ���
            Scenes.Match3Game
        );


        Debug.Log("�⺻������ ���� �Ϸ�");
    }

}