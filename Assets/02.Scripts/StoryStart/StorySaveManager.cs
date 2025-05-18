using UnityEngine;
using System.IO;

using UnityEngine.SceneManagement;//

[System.Serializable]
public class StorySaveData
{
    public int savedIndex;
}

public class StorySaveManager : MonoBehaviour
{
    public static StorySaveManager Instance;

    private string savePath;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            savePath = Path.Combine(Application.persistentDataPath, "saveOpeingIndex.json");
        }
        else
        {
            Destroy(gameObject);
        }

        SceneManager.sceneLoaded += OnSceneLoaded;//

    }


    //
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log("�� �ٲ� Ȯ��: " + scene.name);

        // 1�� �Ǵ� 2���� �� ���� ���
        if (scene.name == "StoryStartScene" || scene.name == "StartScene")//StoryStartScene//StartScene
        {
            if (SoundManager.Instance.OpeningBGM != null &&
                !SoundManager.Instance.BGMmusicSource.isPlaying)
            {
                SoundManager.Instance.PlayOpenTitleMusic();
            }
        }

    }


    public void SaveIndex(int index)
    {
        StorySaveData data = new StorySaveData();
        data.savedIndex = index;
        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(savePath, json);
    }

    public int LoadIndex()
    {
        if (File.Exists(savePath))
        {
            string json = File.ReadAllText(savePath);
            StorySaveData data = JsonUtility.FromJson<StorySaveData>(json);

            Debug.Log("�ҷ��� �ε���: " + data.savedIndex);

            return data.savedIndex;
        }

        Debug.Log("����� �ε��� ����, �⺻�� 0 ��ȯ");
        return 0;
    }


    public void DeleteSave()
    {
        if (File.Exists(savePath))
        {
            File.Delete(savePath);  // ���� ���� ����
            Debug.Log("���� ���� ���� �Ϸ�");

            if(Opening.Instance != null)
            {
                Opening.Instance.StartTyping();
            }
           
        }
        else
        {
            Debug.Log("������ ���� ������ �����ϴ�");
        }

        int resetIndex = LoadIndex();  // ���� ���� ��, �⺻�� 0�� ��ȯ����
        Debug.Log("���µ� �ε���: " + resetIndex);  // 0 ��ȯ
    }

    public bool HasSaveData()
    {
        return File.Exists(savePath);
    }
}
