using System.IO;
using UnityEngine;

[System.Serializable]
public class EndingData
{
    public bool IsGoodEnding = false;
}

public class EndingStateManager : MonoBehaviour
{
    public static EndingStateManager Instance { get; private set; }

    private string savePath;
    private EndingData currentData = new EndingData();

    public int GoodEndingMoney = 8100;

    // �ܺο��� ���� ������ ������Ƽ
    public bool IsGoodEnding
    {
        get => currentData.IsGoodEnding;
        set
        {
            currentData.IsGoodEnding = value;
            SaveEnding(); // ���� �ٲ�� �ڵ� ����
        }
    }

    private void Awake()
    {
        // �̱��� ����
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        savePath = Path.Combine(Application.persistentDataPath, "endingData.json");
        LoadEnding();
    }

    // ����
    public void SaveEnding()
    {
        string json = JsonUtility.ToJson(currentData, true);
        File.WriteAllText(savePath, json);
        Debug.Log("[����]���� �� �������� �� �� �ִ���?: " + json);
    }

    // �ҷ�����
    public void LoadEnding()
    {
        if (File.Exists(savePath))
        {
            string json = File.ReadAllText(savePath);
            currentData = JsonUtility.FromJson<EndingData>(json);
            Debug.Log("[�ҷ�����]���� �� �������� �� �� �ִ���?: " + json);
        }
        else
        {
            Debug.Log("���� ������ ����. ���� ������.");
            currentData = new EndingData();
        }
    }

    // ����
    public void ResetEnding()
    {
        IsGoodEnding = false;
        Debug.Log("���� ���� ���µ�.");
    }
}
