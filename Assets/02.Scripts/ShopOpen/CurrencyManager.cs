using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum CurrencyType
{
    Money,
    Satisfaction,
    Count
}

public struct CurrencyInOut
{
    public int TodayGainBudget;
    public int TodayLossBudget;
    public int TodayStartSatisfaction;
    public int TodayEndSatisfaction;
}

public class CurrencyManager : MonoBehaviour
{
    public static CurrencyManager Instance;

    public int Satisfaction => _playerCurrency[CurrencyType.Satisfaction];
    public int Budget => _playerCurrency[CurrencyType.Satisfaction];

    public CurrencyInOut CurrencyInOut;
    //[SerializeField]
    //private CurrencyInOut _currencyInOut;
    private Dictionary<CurrencyType, int> _playerCurrency;


    [SerializeField]
    private const int _initialBudget = 500;
    [SerializeField]
    private const int _initialSatisfaction = 50;
    private const int _maxSatisfaction = 100;

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

        // TODO
        //_playerCurrency = GameManager.Instance.PlayerCurrencyData;

        /* if (_playerCurrency == null)
         {
             _playerCurrency = new Dictionary<CurrencyType, int>((int)CurrencyType.Count);
             _playerCurrency.Add(CurrencyType.Money, _initialBudget);
             _playerCurrency.Add(CurrencyType.Satisfaction, _initialSatisfaction);
         }*/

        if (_playerCurrency == null)
        {
            _playerCurrency = new Dictionary<CurrencyType, int>((int)CurrencyType.Count);
        }

        if (!_playerCurrency.ContainsKey(CurrencyType.Money))
        {
            _playerCurrency[CurrencyType.Money] = _initialBudget;
        }

        if (!_playerCurrency.ContainsKey(CurrencyType.Satisfaction))
        {
            _playerCurrency[CurrencyType.Satisfaction] = _initialSatisfaction;
        }

        CurrencyInOut.TodayStartSatisfaction = _playerCurrency[CurrencyType.Satisfaction];

        ///LoadCurrencyData();
    }

    //����-���� �ٲ� ������ ȣ��
    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // �� �ε��� �Ϸ�Ǹ� �� ���� �����
        Debug.Log($"�� �ε��: {scene.name}");

        LoadCurrencyData();
        LoadCurrencyInOutData();

        GameManager.Instance.LoadLevelData();

    }


    private void Start()
    {
        if(UI_Shop.Instance != null)
        {
            UI_Shop.Instance.RefreshCurrency(CurrencyType.Money, _playerCurrency[CurrencyType.Money]);
            UI_Shop.Instance.RefreshCurrency(CurrencyType.Satisfaction, _playerCurrency[CurrencyType.Satisfaction]);
        }
        
    }

    public bool AddCurrency(CurrencyType currencyType, int amount)
    {
        if (amount < 0)
        {
           // Debug.LogWarning($"��ȿ���� �ʴ� ��ȭ ����: {amount}");
            return false;
        }

        _playerCurrency[currencyType] += amount;

        if (currencyType == CurrencyType.Money)
        {
            CurrencyInOut.TodayGainBudget += amount;
        }

        if (_playerCurrency[CurrencyType.Satisfaction] > _maxSatisfaction)
        {
            _playerCurrency[CurrencyType.Satisfaction] = _maxSatisfaction;
        }

        UI_Shop.Instance.RefreshCurrency(currencyType, _playerCurrency[currencyType]);

        return true;
    }


    public bool RemoveCurrency(CurrencyType currencyType, int amount)
    {
        if (amount < 0)
        {
          //  Debug.LogWarning($"��ȿ���� �ʴ� ��ȭ ����: {amount}");
            return false;
        }

        _playerCurrency[currencyType] -= amount;

        if (currencyType == CurrencyType.Money)
        {
            CurrencyInOut.TodayLossBudget += amount;
        }

        if (_playerCurrency[CurrencyType.Satisfaction] < 0)
        {
            _playerCurrency[CurrencyType.Satisfaction] = 0;
        }

        UI_Shop.Instance.RefreshCurrency(currencyType, _playerCurrency[currencyType]);

        return true;
    }

    private void OnDestroy()
    {
        CurrencyInOut.TodayEndSatisfaction = _playerCurrency[CurrencyType.Satisfaction];

        GameManager.Instance.SetPlayerCurrecyData(_playerCurrency);
        GameManager.Instance.SetCurrencyInOut(CurrencyInOut);
    }


    public void SaveCurrencyData()
    {
        CurrencyInOut.TodayEndSatisfaction = _playerCurrency[CurrencyType.Satisfaction];

        GameManager.Instance.SetPlayerCurrecyData(_playerCurrency);
        GameManager.Instance.SetCurrencyInOut(CurrencyInOut);

        Debug.Log($"[���� �Ϸ�Money = {_playerCurrency[CurrencyType.Money]}, Satisfaction = {_playerCurrency[CurrencyType.Satisfaction]}");

        GameManager.Instance.SaveCurrencyInOut();
        //UI_Shop.Instance.RefreshCurrency(CurrencyType.Money, _playerCurrency[CurrencyType.Money]);
        //UI_Shop.Instance.RefreshCurrency(CurrencyType.Satisfaction, _playerCurrency[CurrencyType.Satisfaction]);

        if (_playerCurrency[CurrencyType.Money] >= EndingStateManager.Instance.GoodEndingMoney)//���� Ȯ��
        {
            //�� �������� ����
            EndingStateManager.Instance.IsGoodEnding = true;

            EndingStateManager.Instance.SaveEnding();
            Debug.Log("8100���� Ŀ�� �� ���� ����");
        }

        else
        {
            //��� �������� ����
            EndingStateManager.Instance.IsGoodEnding = false;
            Debug.Log("8100���� �۾Ƽ� �� ���� �Ұ�");
            EndingStateManager.Instance.SaveEnding();
        }
    }

    public void LoadCurrencyData()
    {
        // GameManager���� ����� �����͸� �ҷ���
        Dictionary<CurrencyType, int> loadedCurrency = GameManager.Instance.GetPlayerCurrencyData();

        // ���� �ʼ� Ű(Money �Ǵ� Satisfaction)�� ������ �ʱ�ȭ
        if (!loadedCurrency.ContainsKey(CurrencyType.Money) || !loadedCurrency.ContainsKey(CurrencyType.Satisfaction))
        {
            Debug.Log("[�ε� ����] Ű �������� ���� ResetCurrencyData ����");
            ResetCurrencyData();
            return;
        }

        // ��ųʸ��� �����̶�� ���� Ŭ������ ������ ����
        _playerCurrency = loadedCurrency;

        Debug.Log($"[�ҷ����� �Ϸ�] Money = {_playerCurrency[CurrencyType.Money]}, Satisfaction = {_playerCurrency[CurrencyType.Satisfaction]}");

        if(UI_Shop.Instance != null)
        {
            UI_Shop.Instance.RefreshCurrency(CurrencyType.Money, _playerCurrency[CurrencyType.Money]);
            UI_Shop.Instance.RefreshCurrency(CurrencyType.Satisfaction, _playerCurrency[CurrencyType.Satisfaction]);
        }


        // ���� ���� Ȯ��
        if (_playerCurrency[CurrencyType.Money] >= EndingStateManager.Instance.GoodEndingMoney)
        {
            EndingStateManager.Instance.IsGoodEnding = true;
            Debug.Log("8100���� Ŀ�� �� ���� ����");
        }
        else if (_playerCurrency[CurrencyType.Money] < EndingStateManager.Instance.GoodEndingMoney)
        {
            EndingStateManager.Instance.IsGoodEnding = false;
            Debug.Log("8100���� �۾Ƽ� �� ���� �Ұ�");
        }

        EndingStateManager.Instance.SaveEnding();
    }



    public void ResetCurrencyData()
    {
        // �ʱ�ȭ: ���� ������ 0���� ����
        _playerCurrency[CurrencyType.Money] = 500;
        _playerCurrency[CurrencyType.Satisfaction] = 50;


        // GameManager�� �ݿ�
        GameManager.Instance.SetPlayerCurrecyData(_playerCurrency);
        GameManager.Instance.SetCurrencyInOut(CurrencyInOut);

        // UI ����

        if(UI_Shop.Instance != null)
        {
            UI_Shop.Instance.RefreshCurrency(CurrencyType.Money, 0);
            UI_Shop.Instance.RefreshCurrency(CurrencyType.Satisfaction, 0);
        }

        Debug.Log("[���� �Ϸ�] ��� ��ȭ�� �ʱ�ȭ�߽��ϴ�.");

        GameManager.Instance.SaveCurrencyInOut();
        ResetCurrencyInOutData();
        SaveCurrencyInOutData();
    }

    //�Ϸ絿�� ���� ���� ����/�ε�/����
    public void SaveCurrencyInOutData()
    {
        CurrencyInOut.TodayEndSatisfaction = _playerCurrency[CurrencyType.Satisfaction];

        GameManager.Instance.SetCurrencyInOut(CurrencyInOut);

        Debug.Log($"[InOut ���� �Ϸ�] ���� ���� ����: {CurrencyInOut.TodayGainBudget}, ����: {CurrencyInOut.TodayLossBudget}, ���� ������: {CurrencyInOut.TodayStartSatisfaction}, ���� ������: {CurrencyInOut.TodayEndSatisfaction}");
    }

    public void LoadCurrencyInOutData()
    {
        CurrencyInOut loadedCurrencyInOut = GameManager.Instance.GetCurrencyInOut();
        CurrencyInOut = loadedCurrencyInOut;


        Debug.Log($"[InOut �ҷ����� �Ϸ�] ���� ���� ����: {CurrencyInOut.TodayGainBudget}, ����: {CurrencyInOut.TodayLossBudget}, ���� ������: {CurrencyInOut.TodayStartSatisfaction}, ���� ������: {CurrencyInOut.TodayEndSatisfaction}");
    }

    //���� ����
    public void ResetCurrencyInOutData()
    {
        CurrencyInOut = new CurrencyInOut
        {
            TodayGainBudget = 0,
            TodayLossBudget = 0,
            TodayStartSatisfaction = _initialSatisfaction,
            TodayEndSatisfaction = _initialSatisfaction
        };

        GameManager.Instance.SetCurrencyInOut(CurrencyInOut);

        Debug.Log("[InOut ���� �Ϸ�] ���� ��ȭ �帧 �ʱ�ȭ��.");
    }
   
    //���� �ٲ� ����
    public void DayChangeCurrencyInOutData()
    {
        LoadCurrencyInOutData();

        CurrencyInOut = new CurrencyInOut
        {
            TodayGainBudget = 0,
            TodayLossBudget = 0,
            TodayStartSatisfaction = CurrencyInOut.TodayEndSatisfaction,//CurrencyInOut.TodayStartSatisfaction,
            TodayEndSatisfaction = CurrencyInOut.TodayEndSatisfaction
        };

        GameManager.Instance.SetCurrencyInOut(CurrencyInOut);

        Debug.Log("[InOut ���� �Ϸ�] ���� ��ȭ �帧 �ʱ�ȭ��.");
    }


}


