using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening; // Add this import for DOTween
using System.IO;


[System.Serializable]
public class CustomerSaveData
{
    public int savedIndex;
    public int completedCustomers; // ������� ������ �մ� �� ����
}

public class CustomerManager : MonoBehaviour
{
    public static CustomerManager Instance;

    public List<GameObject> StageCustomerList;

    public List<GameObject> StageAllCustomerSpecialPrefabList;
   
    
    public List<GameObject> Stage2CustomerSpecialPrefabList;
    public List<GameObject> Stage3CustomerSpecialPrefabList;
    public List<GameObject> CustomerOnlyTextPrefabList;
    public GameObject CustomerNormalPrefab;
    public GameObject CustomerSpecialBeggarPrefab;
    public Transform CustomerParentTransform;

    [SerializeField]
    private int _customerNormalCountPerStage = 5;
    [SerializeField]
    private int _customerOnlyTextCountPerStage = 2;
    [SerializeField]
    private int _customerSpecialCountPerStage = 2;

    // [SerializeField]
    // private int _completedCustomers;
    // [SerializeField]
    // private int _maxCustomers;
    public int MaxCustomers = 6;
    public int CompletedCustomers;

    [SerializeField]
    public Vector3 CustomerSpawnPosition = new Vector3(0, -0.42f, 0);

    //private GameObject _instancedCustomer;

    public GameObject InstancedCustomer;

    private string savePath;//0420


    /*private void Update()//0417//���� ȭ�鿡 ���̴°� �� ��° �մ�����
    {
        if (InstancedCustomer != null)
        {
            // �ڽ� �����ؼ� SpriteRenderer ã��
            SpriteRenderer spriteRenderer = InstancedCustomer.GetComponentInChildren<SpriteRenderer>();

            if (spriteRenderer != null && spriteRenderer.sprite != null)
            {
                string spriteName = spriteRenderer.sprite.name;
                Debug.Log("���� �մ� ��������Ʈ �̸�: " + spriteName);
            }
        }
     }*/


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
    }

    private void Start()
    {
        savePath = Path.Combine(Application.persistentDataPath, "savecustomer.json");//0420

        System.Random random = new System.Random();
        switch (GameManager.Instance.CurrentLevel)
        {
            case 0:
                {
                    GameManager.Instance.RandomSpeicalPrefabsPerStage.AddRange(StageAllCustomerSpecialPrefabList);
                    break;
                }
            case 1:
                {
                    if(GameManager.Instance.RandomSpeicalPrefabsPerStage.Count <= 0)
                    {
                        GameManager.Instance.RandomSpeicalPrefabsPerStage.AddRange(StageAllCustomerSpecialPrefabList);
                    }
                    GameManager.Instance.RandomSpeicalPrefabsPerStage.AddRange(Stage2CustomerSpecialPrefabList);
                    break;
                }
            case 2:
                {
                    if (GameManager.Instance.RandomSpeicalPrefabsPerStage.Count <= 0)
                    {
                        GameManager.Instance.RandomSpeicalPrefabsPerStage.AddRange(Stage2CustomerSpecialPrefabList);
                    }
                    GameManager.Instance.RandomSpeicalPrefabsPerStage.AddRange(Stage3CustomerSpecialPrefabList);
                    break;
                }
        }
        MakeStage();

        UI_CustomerText.Instance.OnNextCustomer += DestroyCustomer;
        UI_CustomerText.Instance.OnNextCustomer += GetCustomerInStage;

        Debug.Log("�մ� �Ŵ������� �ٽ� �ε� ����");


    }

    private void MakeStage()
    {
        // Customer_Normal ���������� �մԼ���ŭ �߰��ϱ�
        for (int i = 0; i < _customerNormalCountPerStage; i++)
        {
            StageCustomerList.Add(CustomerNormalPrefab);
        }
        // Customer_OnlyText ���������� �մԼ���ŭ �߰��ϱ�
        for (int i = 0; i < _customerOnlyTextCountPerStage; i++)
        {
            StageCustomerList.Add(RandomCustomerInList(CustomerOnlyTextPrefabList));
        }
        // Customer_Special ���������� �մԼ���ŭ �߰��ϱ�
        for (int i = 0; i < _customerOnlyTextCountPerStage; i++)
        {
            StageCustomerList.Add(RandomCustomerInList(GameManager.Instance.RandomSpeicalPrefabsPerStage));
        }
    }
    // �������� Prefab ������.
    private GameObject RandomCustomerInList(List<GameObject> customerList)
    {
        System.Random random = new System.Random();
        int randomIndex = random.Next(customerList.Count);
        GameObject randomCustomer = customerList[randomIndex];
        customerList.RemoveAt(randomIndex);
        return randomCustomer;
    }
    // ȣ���ϸ� ���� �մ� ����.
    public void GetCustomerInStage()
    {
        if (StageCustomerList.Count <= 0)
        {
            StageCustomerList.Add(CustomerNormalPrefab);
        }
        GameObject randomCustomer = RandomCustomerInList(StageCustomerList);

        System.Random random = new System.Random();
        if (random.Next(2) == 0)
        {
            InstancedCustomer = Instantiate(randomCustomer, new Vector3(10, CustomerSpawnPosition.y, CustomerSpawnPosition.z), Quaternion.identity, CustomerParentTransform);
            InstancedCustomer.transform.DOMove(CustomerSpawnPosition, 1.0f).SetEase(Ease.OutQuad).OnComplete(() =>
            {
                UI_CustomerText.Instance.SetCustomerText();
            });
        }
        else
        {
            Vector3 startPosition = new Vector3(CustomerSpawnPosition.x, CustomerSpawnPosition.y - 1.0f, CustomerSpawnPosition.z);
            InstancedCustomer = Instantiate(randomCustomer, startPosition, Quaternion.identity, CustomerParentTransform);
            InstancedCustomer.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
            InstancedCustomer.transform.DOMove(CustomerSpawnPosition, 1.0f).SetEase(Ease.OutQuad);
            InstancedCustomer.transform.DOScale(Vector3.one, 1.0f).SetEase(Ease.OutQuad).OnComplete(() =>
            {
                UI_CustomerText.Instance.SetCustomerText();
            });
        }
        SoundManager.Instance.PlaySoundEffect(SoundManager.Instance.customerArriveSound);
        SoundManager.Instance.PlaySoundEffect(SoundManager.Instance.DoorOpenSound);

       // Debug.Log($"���� �մ� ��ȣ: {_completedCustomers + 1} / {_maxCustomers}");
    }


    public void DestroyCustomer()
    {
        CompletedCustomers++;
        Debug.Log("�� ��° �մ��� ���� ��������?" + CompletedCustomers + "��° �մ�");

       IngredientManager.Instance.SaveAllIngredientCounts();
       CurrencyManager.Instance.SaveCurrencyData();//��ȭ �� ������ ����
 

        //�մ� ������� �� ����
        SaveCustomerIndex(CompletedCustomers);
;

        if (CompletedCustomers >= MaxCustomers) 
        {
            //GameManager.Instance.EndStage();
            ShopOpenManager.Instance.EndStage();//���â
            //GameManager.Instance.SetCurrentLevel(GameManager.Instance.CurrentLevel+1);
            GameManager.Instance.EnabledIngredients = null;
            //_enabledIngredients = null;
            // GameManager.Instance.LoadLevelData();
            SaveManager.SaveGame((GameManager.Instance.CurrentLevel+1), GameManager.Instance.PlayerCurrencyData, GameManager.Instance.GetIngredientData(), Scenes.Match3Game);
   //+1������

            Debug.Log("�մ� ������ ���� ���â����");
            Debug.Log("�� ��° �մ��̱淡 ��������?" + CompletedCustomers + "��° �մ�");

            //�մ� ����
            DeleteCustomerSave();
           
        }

        else
        {
            //GameManager.Instance.EndStage();//�߰�0424
            //GameManager.Instance.SetCurrentLevel(GameManager.Instance.CurrentLevel);
            //GameManager.Instance.EnabledIngredients = null;
           // SaveManager.SaveGame(GameManager.Instance.GetCurrentLevel(), GameManager.Instance.PlayerCurrencyData, GameManager.Instance.GetIngredientData(), Scenes.ShopOpen);
            SaveManager.SaveGame(GameManager.Instance.CurrentLevel, GameManager.Instance.PlayerCurrencyData, GameManager.Instance.GetIngredientData(), Scenes.ShopOpen);
            Debug.Log("�մ� �̾ ������");

            Debug.Log("�� ��° �մ��� ���� ��������?" + CompletedCustomers + "��° �մ�");

            CurrencyManager.Instance.SaveCurrencyInOutData();//���� �Ϸ絿�� ���� ������ ����
        }

        Destroy(InstancedCustomer.gameObject);

        //IngredientManager.Instance.SaveAllIngredientCounts();
         //CurrencyManager.Instance.SaveCurrencyData();//��ȭ �� ������ ����

    }

    //�� ��° �մ����� ����/�ҷ�����
    public int LoadCustomerIndex()
    {
        if (File.Exists(savePath))
        {
            string json = File.ReadAllText(savePath);
            CustomerSaveData data = JsonUtility.FromJson<CustomerSaveData>(json);

            Debug.Log("�ҷ��� �մ� �ε���: " + data.savedIndex);
            Debug.Log("���� �� ��° �մ�����: " + data.completedCustomers); // Ȯ�� �α�

            // �մ� �� �ҷ��ͼ� ����
            CompletedCustomers = data.completedCustomers;

            // ���� _completedCustomers�� 6���� ũ�� ����
            if (CompletedCustomers > 6)
            {
                DeleteCustomerSave();
                //_completedCustomers = 0;
                Debug.Log("�մ� ���� 6���� ũ�Ƿ� ���µǾ����ϴ�.");

                //���Ͱ� ������ ����
                CurrencyManager.Instance.DayChangeCurrencyInOutData();
                Debug.Log("���� �ູ�̶� ���� �����ϰ� �ٲ�Ŵ� ����");
            }

            return data.savedIndex;
        }

        Debug.Log("����� �ε��� ����, �⺻�� 0 ��ȯ");
        return 0;
    }

    public void SaveCustomerIndex(int index)
    {
        CustomerSaveData data = new CustomerSaveData();
        data.savedIndex = index;
        data.completedCustomers = CompletedCustomers; //  �մ� �� ����
        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(savePath, json);
    }

    //�մ� �ʱ�ȭ
    public void DeleteCustomerSave()
    {
        if (File.Exists(savePath))
        {
            File.Delete(savePath);  // ���� ���� ����
            Debug.Log("�մ� �� ���� ���� ���� �Ϸ�");
        }
        else
        {
            Debug.Log("������ ���� ������ �����ϴ�");
        }

        CompletedCustomers = 0;
        Debug.Log("������ �մ� �� ���� �Ϸ�: 0");

        SaveCustomerIndex(0);
        int resetIndex = LoadCustomerIndex();  // �⺻�� 0 ��ȯ
        Debug.Log("���µ� �ε���: " + resetIndex);  // 0
    }



}
