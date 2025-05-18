using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;
using static ScriptManager;
using Unity.VisualScripting;


public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    //Match3���� ���� �����͸� �����ϴ� ����
    public Dictionary<IngredientType, uint> IngredientDataDic = new Dictionary<IngredientType, uint>();


    // �մ� ��ũ��Ʈ, �̹��� ������
    public List<CustomerNormalTextData> CustomerNormalData = new List<CustomerNormalTextData>();
    public List<CustomerPositiveTextData> CustomerPositiveData = new List<CustomerPositiveTextData>();
    public List<CustomerNegativeTextData> CustomerNegativeData = new List<CustomerNegativeTextData>();
    public List<CustomerOnlyTextData> CustomerOnlyTextData = new List<CustomerOnlyTextData>();
    public List<CustomerSpecialTextData> CustomerSpecialData = new List<CustomerSpecialTextData>();
    public List<Sprite> CustomerNormalImages = new List<Sprite>();
    public Dictionary<string, CustomerSpecialTextData> CustomerSpecialDataByName = new Dictionary<string, CustomerSpecialTextData>();
    public List<LoadingTextData> LoadingTextData = new List<LoadingTextData>();

    // �մ� �������� �����ϱ� ���� ť
    public Queue<int> CustomerNormalIndexQueue;
    public Queue<int> CustomerPositiveIndexQueue;
    public Queue<int> CustomerNegativeIndexQueue;
    public Queue<int> CustomerOnlyTextIndexQueue;
    public Queue<int> CustomerNormalImageQueue;
    public Queue<int> LoadingIndexQueue;

    // ����� �մԸ���Ʈ
    // 1���������� 4��, 2���������� 2��, 3���������� 1�� �߰� �ȴ�.
    public List<GameObject> RandomSpeicalPrefabsPerStage;

    [SerializeField]
    private List<LevelDataSO> _levelDataSOs;
    [SerializeField]
    //public int _currentLevel = 0;
    private int _currentLevel = 0;
    
    private int _maxLevel;
    //�������� �߰� - ������ ������Ź�帳�ϴ�.
    public int MaxLevel => _maxLevel;
    public List<IngredientType> EnabledIngredients;

    //0421
    private Dictionary<CurrencyType, int> _playerCurrencyData = new();
    private CurrencyInOut _todayCurrencyInoutData;


    public Dictionary<CurrencyType, int> PlayerCurrencyData => _playerCurrencyData;
    public CurrencyInOut TodayCurrencyInoutData => _todayCurrencyInoutData;

    private bool _isGoodEnding;
    public bool IsGoodEnding => _isGoodEnding;

    // ������ ���� - �������� ����
    public int CurrentLevel => _currentLevel;

    public bool IsReadCSV = false;


    


    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);
        //�ҷ����� // ��� ������
        

        _maxLevel = _levelDataSOs.Count;
        //SaveManager.LoadGame();//0424

        

        LoadLevelData();//�ҷ���
    }


   




    //---------������ �׽�Ʈ�� Update ���� ����---
    private void Update()
    {
       // if (Input.GetKeyDown(KeyCode.D))
         //   SaveManager.DeleteSave();
    }
    //-------------------

    /// <summary>
    /// ������ ������ �� Match3���� ���� ������ IngredientManager�� �Ѱ��ش�.
    /// </summary>
    /// <param name="destroyedBlocksCount">string, int type dictionary</param>


    //0420
    public void SetIngredientManage(Dictionary<string, int> destroyedBlocksCount)
    {
        IngredientDataDic.Clear();//������ �ֶ� ��  ����


        Dictionary<IngredientType, uint> ingredientsDict = new Dictionary<IngredientType, uint>();

        foreach (var item in destroyedBlocksCount)
        {
            if (Enum.TryParse(item.Key, out IngredientType ingredientType))
            {
                ingredientsDict[ingredientType] = (uint)item.Value;
            }
            else
            {
                Debug.LogWarning($"Invalid ingredient type: {item.Key}");
            }
        }
        IngredientDataDic = ingredientsDict;
    }

    //IngredientDataDic ���� ��������
    public Dictionary<IngredientType, uint> GetIngredientData()
    {
        return IngredientDataDic;
    }


    public List<IngredientType> GetEnableIngredients()
    {
        if(EnabledIngredients == null )
        {
            LoadLevelData();
        }

        return EnabledIngredients;
      
    }


    //0421
    // ���� ��ȭ ���� ����
    public void SetPlayerCurrecyData(Dictionary<CurrencyType, int> playerCurrency)
    {
        if (playerCurrency == null || playerCurrency.Count == 0)
        {
            Debug.LogWarning($"{this.name}: ��ȿ���� �ʴ� ��ȭ ������ �Է�");
            return;
        }
        _playerCurrencyData = playerCurrency;
        Debug.Log($"{this.name}: ���� ��ȭ ���� ����");
    }

    // ���� ��ȭ ���� �ҷ�����
    public Dictionary<CurrencyType, int> GetPlayerCurrencyData()
    {
        if (_playerCurrencyData == null)
        {
            Debug.LogWarning($"{this.name}: ���� ��ȭ ������ �������");
            _playerCurrencyData = new Dictionary<CurrencyType, int>();
            
        }
        return _playerCurrencyData;
    }

    // ���� ���� ����
    public void SetCurrencyInOut(CurrencyInOut currencyInOut)
    {
        _todayCurrencyInoutData = currencyInOut;
        Debug.Log($"{this.name}: ���� ���� ����");
        SaveCurrencyInOut();
    }

    // ���� ���� ���� (PlayerPrefs)
    public void SaveCurrencyInOut()
    {
        string json = JsonUtility.ToJson(_todayCurrencyInoutData);
        PlayerPrefs.SetString("CurrencyInOutData", json);
        PlayerPrefs.Save();
    }

    // ���� ���� �ҷ����� (PlayerPrefs)
    public CurrencyInOut GetCurrencyInOut()
    {
        string json = PlayerPrefs.GetString("CurrencyInOutData", string.Empty);
        if (!string.IsNullOrEmpty(json))
        {
            _todayCurrencyInoutData = JsonUtility.FromJson<CurrencyInOut>(json);
            Debug.Log($"{this.name}: PlayerPrefs���� ���� ���� �ҷ����� �Ϸ�");
        }
        else
        {
            Debug.LogWarning($"{this.name}: ����� ���� ������ ���� �⺻�� ����");
            _todayCurrencyInoutData = new CurrencyInOut();

        }

        return _todayCurrencyInoutData;
    }

  
    public void SetCurrentLevel(int amount)
    {
        Debug.Log("[SetCurrentLevel ȣ���] amount: " + amount + "\n" + Environment.StackTrace);

        // ���� ��ȿ���� üũ
        if (amount > _levelDataSOs.Count)
        {
            Debug.Log("�������� ���� �ʰ�");
            _currentLevel = _levelDataSOs.Count;
        }
        else if (amount < 0)
        {
            _currentLevel = 0;
        }
        else
        {
            _currentLevel = amount;
        }

        Debug.Log("���� ��ĥ (���� ��): " + (CurrentLevel + 1));

        // PlayerPrefs�� ����
        PlayerPrefs.SetInt("CurrentLevel", _currentLevel);
        PlayerPrefs.Save();


    }

    public int GetCurrentLevel()
    {
        // PlayerPrefs���� CurrentLevel �ҷ����� (����� ���� ������ �⺻�� 1 ��ȯ)
        _currentLevel = PlayerPrefs.GetInt("CurrentLevel", 0);  // �⺻���� 1�� ����
        Debug.Log("[GetCurrentLevel ȣ���] amount: " + _currentLevel);
        return _currentLevel;
        //��
        //�� ģ�� ��ġ3���� ����� �� �ְ� �ؾ� ��
    }




    public void LoadLevelData()
    {
        int level = GetCurrentLevel(); // PlayerPrefs���� CurrentLevel �ҷ�����


        Debug.Log("�ҷ��°� ���� ��ĥ? " + (level + 1));
        EnabledIngredients = _levelDataSOs[level].EnabledIngredients;

        SetCurrentLevel(level); // �̰� ����ȭ �뵵�� ���ܵξ ������
        GetIngredientData();
        

        if (UI_Shop.Instance != null && UI_Shop.Instance.DayText != null)
        {
            UI_Shop.Instance.DayText.text = $"Day {level + 1}";
            Debug.Log("�̰� ����� �����̴�");
        }
    }


    //����
    public void ResetLevel()
    {
        // CurrentLevel�� 0���� �����Ͽ� ù ��° ������ ����
        SetCurrentLevel(0);

        // ù ��° ������ �����͸� �ҷ���
        SaveManager.SaveGame(0, PlayerCurrencyData, GetIngredientData(), Scenes.Match3Game);
        //LoadLevelData();
        Debug.Log("���� ����");
        Debug.Log("���� ������" + _currentLevel);
    }


}
