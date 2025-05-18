using System.Collections.Generic;
using UnityEngine;
using System.Text.RegularExpressions;

public class ScriptManager : MonoBehaviour
{
    public static ScriptManager Instance;
    // CSV ������ �ν����Ϳ��� �Ҵ��� �� �ֵ��� ��
    [SerializeField] private TextAsset customerNormalcsvFile;
    [SerializeField] private TextAsset customerNormalPositivecsvFile;
    [SerializeField] private TextAsset customerNormalNegativecsvFile;
    [SerializeField] private TextAsset customerOnlyTextcsvFile;
    [SerializeField] private TextAsset customerSpecialcsvFile;
    [SerializeField] private TextAsset loadingTextcsvFile;
    //-----------------------------------------Customer_Normal ------------------------------------
    // CSV �����͸� ������ Ŭ����
    [System.Serializable]
    public class CustomerNormalTextData
    {
        public string foreword = "";
        public string afterword = "";
        public string nexttext = "";

        public override string ToString()
        {
            return $"foreword='{foreword}', afterword='{afterword}', nexttext='{nexttext}'";
        }
    }

    // CSV���Ͽ��� �о�� Customer_Normal Data
    public List<CustomerNormalTextData> AllCustomerNormalData = new List<CustomerNormalTextData>();
    // Customer_Norml�� ���� �ؽ�Ʈ�� ���� �ε��� ����Ʈ
    private List<int> _customerNormalIndexList;
    [SerializeField]
    private Queue<int> _customerNormalIndexQueue;
    //--------------------------------------------------------------------------------------------


    //-----------------------------------------Customer_Normal_Positive ------------------------------------
    // CSV �����͸� ������ Ŭ����
    [System.Serializable]
    public class CustomerPositiveTextData
    {
        public string positivetext = "";

        public override string ToString()
        {
            return $"positivetext='{positivetext}'";
        }
    }

    // CSV���Ͽ��� �о�� Customer_Positive Data
    public List<CustomerPositiveTextData> AllCustomerPositiveData = new List<CustomerPositiveTextData>();
    // ���� �ؽ�Ʈ�� ���� �ε��� ����Ʈ
    private List<int> _customerPositiveIndexList;
    [SerializeField]
    private Queue<int> _customerPositiveIndexQueue;
    //--------------------------------------------------------------------------------------------------

    //-----------------------------------------Customer_Normal_Negative ------------------------------------
    // CSV �����͸� ������ Ŭ����
    [System.Serializable]
    public class CustomerNegativeTextData
    {
        public string negativetext = "";

        public override string ToString()
        {
            return $"positivetext='{negativetext}'";
        }
    }

    // CSV���Ͽ��� �о�� Customer_Positive Data
    public List<CustomerNegativeTextData> AllCustomerNegativeData = new List<CustomerNegativeTextData>();
    // ���� �ؽ�Ʈ�� ���� �ε��� ����Ʈ
    private List<int> _customerNegativeIndexList;
    [SerializeField]
    private Queue<int> _customerNegativeIndexQueue;
    //--------------------------------------------------------------------------------------------------

    //-----------------------------------------Customer_OnlyText ------------------------------------
    // CSV �����͸� ������ Ŭ����
    [System.Serializable]
    public class CustomerOnlyTextData
    {
        public string name = "";
        public string ordertext = "";
        public string nexttext = "";

        public override string ToString()
        {
            return $"name='{name}', ordertext='{ordertext}', nexttext='{nexttext}'";
        }
    }

    // CSV���Ͽ��� �о�� Customer_OnlyText Data
    public List<CustomerOnlyTextData> AllCustomerOnlyTextData = new List<CustomerOnlyTextData>();
    // ���� �ؽ�Ʈ�� ���� �ε��� ����Ʈ
    private List<int> _customerOnlyTextIndexList;
    [SerializeField]
    private Queue<int> _customerOnlyTextIndexQueue;
    //--------------------------------------------------------------------------------------------------

    //-----------------------------------------Customer_Special ------------------------------------
    // CSV �����͸� ������ Ŭ����
    [System.Serializable]
    public class CustomerSpecialTextData
    {
        public string name = "";
        public string ordertext = "";
        public string nexttext = "";
        public string positivetext = "";
        public string negativetext = "";

        public override string ToString()
        {
            return $"name='{name}', ordertext='{ordertext}', nexttext='{nexttext}', positivetext='{positivetext}', negativetext='{negativetext}'";
        }
    }

    // CSV���Ͽ��� �о�� Customer_Special Data
    public List<CustomerSpecialTextData> AllCustomerSpecialData = new List<CustomerSpecialTextData>();
    private Dictionary<string, CustomerSpecialTextData> _customerSpecialDataByName = new Dictionary<string, CustomerSpecialTextData>();
    //--------------------------------------------------------------------------------------------------

    //-----------------------------------------Text_LoadingText ------------------------------------
    // CSV �����͸� ������ Ŭ����
    [System.Serializable]
    public class LoadingTextData
    {
        public string loadingtext = "";

        public override string ToString()
        {
            return $"loadingtext='{loadingtext}'";
        }
    }

    // CSV���Ͽ��� �о�� Customer_Positive Data
    public List<LoadingTextData> AllLoadingTextData = new List<LoadingTextData>();
    // ���� �ؽ�Ʈ�� ���� �ε��� ����Ʈ
    private List<int> _loadingIndexList;
    [SerializeField]
    private Queue<int> _loadingIndexQueue;
    //--------------------------------------------------------------------------------------------------

    public List<Sprite> CustomerNormalImages;
    [SerializeField]
    private List<int> _customerNormalImageList;
    [SerializeField]
    public Queue<int> _customerNormalImageQueue;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // ���� �ٲ� ����
        }
        else
        {
            Destroy(gameObject); // �̹� ������ ���� ���� �� ����
        }
    }

    private void Start()
    {

        if(!GameManager.Instance.IsReadCSV)
        {
            ReadCSV();
            ReadPositiveCSV();
            ReadNegativeCSV();
            ReadOnlyTextCSV();
            ReadSpecialCSV();
            ReadLoadingTextCSV();
            ShuffleCustomerImage();
            SaveDataToGamemanager();
            GameManager.Instance.IsReadCSV = true;
        }
        else
        {
            InitializeLists();
            LoadDataFromGamemanager();

        }
    }
    /*
    private void OnDestroy()
    {
        // ���� ScriptManager�� ť ���¸� GameManager�� ����
        SaveQueueDataToGameManager();
    }
    */
    

    private void InitializeLists()
    {
        _loadingIndexList = new List<int>();
        _customerNormalIndexList = new List<int>();
        _customerPositiveIndexList = new List<int>();
        _customerNegativeIndexList = new List<int>();
        _customerOnlyTextIndexList = new List<int>();
    }

    private void LoadDataFromGamemanager()
    {
        this.CustomerNormalImages = GameManager.Instance.CustomerNormalImages;
        AllCustomerSpecialData = GameManager.Instance.CustomerSpecialData;
        AllCustomerOnlyTextData = GameManager.Instance.CustomerOnlyTextData;
        AllCustomerNegativeData = GameManager.Instance.CustomerNegativeData;
        AllCustomerPositiveData = GameManager.Instance.CustomerPositiveData;
        AllCustomerNormalData = GameManager.Instance.CustomerNormalData;
        _customerSpecialDataByName = GameManager.Instance.CustomerSpecialDataByName;
        AllLoadingTextData = GameManager.Instance.LoadingTextData;

        _customerNormalIndexQueue = GameManager.Instance.CustomerNormalIndexQueue;
        _customerPositiveIndexQueue = GameManager.Instance.CustomerPositiveIndexQueue;
        _customerNegativeIndexQueue = GameManager.Instance.CustomerNegativeIndexQueue;
        _customerOnlyTextIndexQueue = GameManager.Instance.CustomerOnlyTextIndexQueue;
        _customerNormalImageQueue = GameManager.Instance.CustomerNormalImageQueue;
        _loadingIndexQueue = GameManager.Instance.LoadingIndexQueue;

        
    }

    private void SaveDataToGamemanager()
    {
        GameManager.Instance.CustomerNormalData = AllCustomerNormalData;
        GameManager.Instance.CustomerPositiveData = AllCustomerPositiveData;
        GameManager.Instance.CustomerNegativeData = AllCustomerNegativeData;
        GameManager.Instance.CustomerOnlyTextData = AllCustomerOnlyTextData;
        GameManager.Instance.CustomerSpecialData = AllCustomerSpecialData;
        GameManager.Instance.CustomerNormalImages = this.CustomerNormalImages;
        GameManager.Instance.CustomerSpecialDataByName = _customerSpecialDataByName;
        GameManager.Instance.LoadingTextData = AllLoadingTextData;
        SaveQueueDataToGameManager();

    }
    
    private void SaveQueueDataToGameManager()
    {
        GameManager.Instance.CustomerNormalIndexQueue = _customerNormalIndexQueue;
        GameManager.Instance.CustomerPositiveIndexQueue = _customerPositiveIndexQueue;
        GameManager.Instance.CustomerNegativeIndexQueue = _customerNegativeIndexQueue;
        GameManager.Instance.CustomerOnlyTextIndexQueue = _customerOnlyTextIndexQueue;
        GameManager.Instance.CustomerNormalImageQueue = _customerNormalImageQueue;
        GameManager.Instance.LoadingIndexQueue = _loadingIndexQueue;
    }
    



    // --------------------------- ó�� ����(ù ��) �������� ȣ�� --------------------------------
    void ReadCSV()
    {
        if (customerNormalcsvFile == null)
        {
            Debug.LogError("CSV ������ �Ҵ���� �ʾҽ��ϴ�!");
            return;
        }

        string[] lines = customerNormalcsvFile.text.Split(new[] { '\n', '\r' }, System.StringSplitOptions.RemoveEmptyEntries);

        // ù ��° ���� ����̹Ƿ� �ǳʶݴϴ�
        for (int i = 1; i < lines.Length; i++)
        {
            string line = lines[i].Trim();
            if (string.IsNullOrEmpty(line))
                continue;
         

            CustomerNormalTextData data = new CustomerNormalTextData();



            // ���Խ��� ����� �� ������ ���
            if (line.Contains("\""))
            {
                List<string> fields = new List<string>();
                string pattern = @"(?:^|,)(?=[^""]|(""|^))(""(?:[^""]|"""")*""|[^,]*)";

                foreach (Match match in Regex.Matches(line, pattern))
                {
                    string value = match.Value;
                    if (value.StartsWith(","))
                        value = value.Substring(1);
                    if (value.StartsWith("\"") && value.EndsWith("\""))
                        value = value.Substring(1, value.Length - 2);

                    // ū����ǥ ���� �̽��������� ū����ǥ ó��
                    value = value.Replace("\"\"", "\"");

                    // ���� ��������� �� ���ڿ��� ó��
                    if (string.IsNullOrWhiteSpace(value) || value.ToLower() == "nan" || value.ToLower() == "null")
                        value = "";

                    fields.Add(value);
                }

                if (fields.Count > 0) data.foreword = fields[0];
                else data.foreword = "";

                if (fields.Count > 1) data.afterword = fields[1];
                else data.afterword = "";

                if (fields.Count > 2) data.nexttext = fields[2];
                else data.nexttext = "";
            }
            else
            {
                // ������ ���: ��ǥ�� �и��Ͽ� ó��
                string[] splitByComma = line.Split(',');

                // �ε��� Ȯ�� �� �Ҵ�
                data.foreword = splitByComma.Length > 0 ? splitByComma[0] : "";
                data.afterword = splitByComma.Length > 1 ? splitByComma[1] : "";
                data.nexttext = splitByComma.Length > 2 ? splitByComma[2] : "";
            }


            AllCustomerNormalData.Add(data);
        }

        Debug.Log($"CSV �б� �Ϸ�. �� {AllCustomerNormalData.Count}���� �����͸� �ε��߽��ϴ�.");

        _customerNormalIndexList = new List<int>();

        // ���� �ؽ�Ʈ�� ���� �ε��� ����Ʈ ����
        CustomerNormalIndexShuffle(_customerNormalIndexList, ref _customerNormalIndexQueue, AllCustomerNormalData.Count);

    }

    private void ReadPositiveCSV()
    {
        if (customerNormalPositivecsvFile == null)
        {
            Debug.LogError("Positive CSV ������ �Ҵ���� �ʾҽ��ϴ�!");
            return;
        }

        string[] lines = customerNormalPositivecsvFile.text.Split(new[] { '\n', '\r' }, System.StringSplitOptions.RemoveEmptyEntries);

        // ù ��° ���� ����̹Ƿ� �ǳʶݴϴ�
        for (int i = 1; i < lines.Length; i++)
        {
            string line = lines[i].Trim();
            if (string.IsNullOrEmpty(line))
                continue;

            CustomerPositiveTextData data = new CustomerPositiveTextData();

            // ����ǥ�� �����ϰ� ������ ���, ����ǥ ����
            if (line.StartsWith("\"") && line.EndsWith("\""))
            {
                // ����ǥ ���� (ù ��°�� ������)
                line = line.Substring(1, line.Length - 2);
                // �̽��������� ����ǥ("") ó�� -> ���� ����ǥ(")�� ��ȯ
                line = line.Replace("\"\"", "\"");
            }

            // ������ ���: �� ���� �� �׸�
            data.positivetext = line;


            AllCustomerPositiveData.Add(data);
        }
        _customerPositiveIndexList = new List<int>();
        CustomerNormalIndexShuffle(_customerPositiveIndexList, ref _customerPositiveIndexQueue, AllCustomerPositiveData.Count);
    }

    private void ReadNegativeCSV()
    {
        if (customerNormalNegativecsvFile == null)
        {
            Debug.LogError("Negative CSV ������ �Ҵ���� �ʾҽ��ϴ�!");
            return;
        }

        string[] lines = customerNormalNegativecsvFile.text.Split(new[] { '\n', '\r' }, System.StringSplitOptions.RemoveEmptyEntries);

        // ù ��° ���� ����̹Ƿ� �ǳʶݴϴ�
        for (int i = 1; i < lines.Length; i++)
        {
            string line = lines[i].Trim();
            if (string.IsNullOrEmpty(line))
                continue;

            CustomerNegativeTextData data = new CustomerNegativeTextData();

            // ����ǥ�� �����ϰ� ������ ���, ����ǥ ����
            if (line.StartsWith("\"") && line.EndsWith("\""))
            {
                // ����ǥ ���� (ù ��°�� ������)
                line = line.Substring(1, line.Length - 2);
                // �̽��������� ����ǥ("") ó�� -> ���� ����ǥ(")�� ��ȯ
                line = line.Replace("\"\"", "\"");
            }

            // ������ ���: �� ���� �� �׸�
            data.negativetext = line;


            AllCustomerNegativeData.Add(data);
        }
        _customerNegativeIndexList = new List<int>();
        CustomerNormalIndexShuffle(_customerNegativeIndexList, ref _customerNegativeIndexQueue, AllCustomerNegativeData.Count);
    }

    private void ReadOnlyTextCSV()
    {
        if (customerOnlyTextcsvFile == null)
        {
            Debug.LogError("OnlyText CSV ������ �Ҵ���� �ʾҽ��ϴ�!");
            return;
        }

        string[] lines = customerOnlyTextcsvFile.text.Split(new[] { '\n', '\r' }, System.StringSplitOptions.RemoveEmptyEntries);

        // ù ��° ���� ����̹Ƿ� �ǳʶݴϴ�
        for (int i = 1; i < lines.Length; i++)
        {
            string line = lines[i].Trim();
            if (string.IsNullOrEmpty(line))
                continue;

            CustomerOnlyTextData data = new CustomerOnlyTextData();

            // ���Խ��� ����� �� ������ ���
            if (line.Contains("\""))
            {
                List<string> fields = new List<string>();
                string pattern = @"(?:^|,)(?=[^""]|(""|^))(""(?:[^""]|"""")*""|[^,]*)";

                foreach (Match match in Regex.Matches(line, pattern))
                {
                    string value = match.Value;
                    if (value.StartsWith(","))
                        value = value.Substring(1);
                    if (value.StartsWith("\"") && value.EndsWith("\""))
                        value = value.Substring(1, value.Length - 2);

                    // ū����ǥ ���� �̽��������� ū����ǥ ó��
                    value = value.Replace("\"\"", "\"");

                    // ���� ��������� �� ���ڿ��� ó��
                    if (string.IsNullOrWhiteSpace(value) || value.ToLower() == "nan" || value.ToLower() == "null")
                        value = "";

                    fields.Add(value);
                }

                if (fields.Count > 0) data.name = fields[0];
                else data.name = "";

                if (fields.Count > 1) data.ordertext = fields[1];
                else data.ordertext = "";

                if (fields.Count > 2) data.nexttext = fields[2];
                else data.nexttext = "";
            }
            else
            {
                // ������ ���: ��ǥ�� �и��Ͽ� ó��
                string[] splitByComma = line.Split(',');

                // �ε��� Ȯ�� �� �Ҵ�
                data.name = splitByComma.Length > 0 ? splitByComma[0] : "";
                data.ordertext = splitByComma.Length > 1 ? splitByComma[1] : "";
                data.nexttext = splitByComma.Length > 2 ? splitByComma[2] : "";
            }

            AllCustomerOnlyTextData.Add(data);
        }

        Debug.Log($"OnlyText CSV �б� �Ϸ�. �� {AllCustomerOnlyTextData.Count}���� �����͸� �ε��߽��ϴ�.");

        _customerOnlyTextIndexList = new List<int>();
        CustomerNormalIndexShuffle(_customerOnlyTextIndexList, ref _customerOnlyTextIndexQueue, AllCustomerOnlyTextData.Count);
    }

    private void ReadSpecialCSV()
    {
        if (customerSpecialcsvFile == null)
        {
            Debug.LogError("Special CSV ������ �Ҵ���� �ʾҽ��ϴ�!");
            return;
        }

        string[] lines = customerSpecialcsvFile.text.Split(new[] { '\n', '\r' }, System.StringSplitOptions.RemoveEmptyEntries);

        // ù ��° ���� ����̹Ƿ� �ǳʶݴϴ�
        for (int i = 1; i < lines.Length; i++)
        {
            string line = lines[i].Trim();
            if (string.IsNullOrEmpty(line))
                continue;

            CustomerSpecialTextData data = new CustomerSpecialTextData();

            // ���Խ��� ����� �� ������ ���
            if (line.Contains("\""))
            {
                List<string> fields = new List<string>();
                string pattern = @"(?:^|,)(?=[^""]|(""|^))(""(?:[^""]|"""")*""|[^,]*)";

                foreach (Match match in Regex.Matches(line, pattern))
                {
                    string value = match.Value;
                    if (value.StartsWith(","))
                        value = value.Substring(1);
                    if (value.StartsWith("\"") && value.EndsWith("\""))
                        value = value.Substring(1, value.Length - 2);

                    // ū����ǥ ���� �̽��������� ū����ǥ ó��
                    value = value.Replace("\"\"", "\"");

                    // ���� ��������� �� ���ڿ��� ó��
                    if (string.IsNullOrWhiteSpace(value) || value.ToLower() == "nan" || value.ToLower() == "null")
                        value = "";

                    fields.Add(value);
                }

                if (fields.Count > 0) data.name = fields[0];
                else data.name = "";

                if (fields.Count > 1) data.ordertext = fields[1];
                else data.ordertext = "";

                if (fields.Count > 2) data.nexttext = fields[2];
                else data.nexttext = "";

                if (fields.Count > 3) data.positivetext = fields[3];
                else data.positivetext = "";

                if (fields.Count > 4) data.negativetext = fields[4];
                else data.negativetext = "";
            }
            else
            {
                // ������ ���: ��ǥ�� �и��Ͽ� ó��
                string[] splitByComma = line.Split(',');

                // �ε��� Ȯ�� �� �Ҵ�
                data.name = splitByComma.Length > 0 ? splitByComma[0] : "";
                data.ordertext = splitByComma.Length > 1 ? splitByComma[1] : "";
                data.nexttext = splitByComma.Length > 2 ? splitByComma[2] : "";
                data.positivetext = splitByComma.Length > 3 ? splitByComma[3] : "";
                data.negativetext = splitByComma.Length > 4 ? splitByComma[4] : "";
            }

            AllCustomerSpecialData.Add(data);

            // Add to dictionary for name-based lookup
            if (!string.IsNullOrEmpty(data.name) && !_customerSpecialDataByName.ContainsKey(data.name))
            {
                _customerSpecialDataByName.Add(data.name, data);
            }
        }

        Debug.Log($"Special CSV �б� �Ϸ�. �� {AllCustomerSpecialData.Count}���� �����͸� �ε��߽��ϴ�.");
    }

    private void ReadLoadingTextCSV()
    {
        if (loadingTextcsvFile == null)
        {
            Debug.LogError("loading CSV ������ �Ҵ���� �ʾҽ��ϴ�!");
            return;
        }

        string[] lines = loadingTextcsvFile.text.Split(new[] { '\n', '\r' }, System.StringSplitOptions.RemoveEmptyEntries);

        // ù ��° ���� ����̹Ƿ� �ǳʶݴϴ�
        for (int i = 1; i < lines.Length; i++)
        {
            string line = lines[i].Trim();
            if (string.IsNullOrEmpty(line))
                continue;

            LoadingTextData data = new LoadingTextData();

            // ����ǥ�� �����ϰ� ������ ���, ����ǥ ����
            if (line.StartsWith("\"") && line.EndsWith("\""))
            {
                // ����ǥ ���� (ù ��°�� ������)
                line = line.Substring(1, line.Length - 2);
                // �̽��������� ����ǥ("") ó�� -> ���� ����ǥ(")�� ��ȯ
                line = line.Replace("\"\"", "\"");
            }

            // ������ ���: �� ���� �� �׸�
            data.loadingtext = line;


            AllLoadingTextData.Add(data);
        }
        _loadingIndexList = new List<int>();
        CustomerNormalIndexShuffle(_loadingIndexList, ref _loadingIndexQueue, AllLoadingTextData.Count);
    }

    private void ShuffleCustomerImage()
    {
        CustomerNormalIndexShuffle(_customerNormalImageList, ref _customerNormalImageQueue, CustomerNormalImages.Count);
    }

    private void CustomerNormalIndexShuffle(List<int> list, ref Queue<int> queue, int count)
    {
        int listCount = count;
        for (int i = 0; i < listCount; i++)
        {
            list.Add(i);
        }
        Shuffle(list);
        queue = new Queue<int>(list);
    }

    private void Shuffle(List<int> list)
    {
        int n = list.Count;
        for (int i = 0; i < n - 1; i++)
        {
            // i���� n-1������ �������� ������ �ε��� ����
            int j = Random.Range(i, n);

            // i��°�� j��° ��� ��ȯ
            int temp = list[i];
            list[i] = list[j];
            list[j] = temp;
        }
    }


    //0417


    // ----------------------------�� ���δ� ù ��(ù ����)���� ȣ��--------------------------------------------------------

    // ���� �ؽ�Ʈ �����͸� �������� �޼���
    public CustomerNormalTextData GetRandomCustomerText()
    {
        return GetRandomOrderText(AllCustomerNormalData, _customerNormalIndexQueue);
    }
    public CustomerPositiveTextData GetRandomCustomerPositive()
    {
        return GetRandomOrderText(AllCustomerPositiveData, _customerPositiveIndexQueue);
    }
    public CustomerNegativeTextData GetRandomCustomerNegative()
    {
        return GetRandomOrderText(AllCustomerNegativeData, _customerNegativeIndexQueue);
    }
    public CustomerOnlyTextData GetRandomCustomerOnlyText()
    {
        return GetRandomOrderText(AllCustomerOnlyTextData, _customerOnlyTextIndexQueue);
    }
    public Sprite GetRandomCustomerNormalImages()
    {
        return GetRandomOrderText<Sprite>(CustomerNormalImages, _customerNormalImageQueue);
    }
    public LoadingTextData GetRandomLoadingText()
    {
        return GetRandomOrderText(AllLoadingTextData, _loadingIndexQueue);
    }

    private T GetRandomOrderText<T>(List<T> datas, Queue<int> queue)
    {
        if (datas.Count == 0)
            return default(T);
        if (queue.Count <= 0)
        {
            // �ش� ������ Ÿ�Կ� �´� �ε��� ����Ʈ�� ť�� �ĺ��Ͽ� ����
            if (typeof(T) == typeof(CustomerNormalTextData))
            {
                queue =  RefillQueue(_customerNormalIndexList, ref _customerNormalIndexQueue, AllCustomerNormalData.Count);
            }
            else if (typeof(T) == typeof(CustomerPositiveTextData))
            {
                queue =  RefillQueue(_customerPositiveIndexList,ref _customerPositiveIndexQueue, AllCustomerPositiveData.Count);
            }
            else if (typeof(T) == typeof(CustomerNegativeTextData))
            {
                queue =  RefillQueue(_customerNegativeIndexList, ref _customerNegativeIndexQueue, AllCustomerNegativeData.Count);
            }
            else if (typeof(T) == typeof(CustomerOnlyTextData))
            {
                queue =  RefillQueue(_customerOnlyTextIndexList, ref _customerOnlyTextIndexQueue, AllCustomerOnlyTextData.Count);
            }
            else if (typeof(T) == typeof(Sprite))
            {
                queue = RefillQueue(_customerNormalImageList, ref _customerNormalImageQueue, CustomerNormalImages.Count);
            }
            else if (typeof(T) == typeof(LoadingTextData))
            {
                queue = RefillQueue(_loadingIndexList, ref _loadingIndexQueue, AllLoadingTextData.Count);
            }
        }

        int randomIndex = queue.Dequeue();
        return datas[randomIndex];
    }

    private  Queue<int> RefillQueue(List<int> list, ref Queue<int> queue, int count)
    {
        CustomerNormalIndexShuffle(list, ref queue, count);
        return queue;
        /*
        // ť�� ���� ���ο� �׸����� ä��
        queue.Clear();
        foreach (int index in list)
        {
            queue.Enqueue(index);
        }
        */
    }

    // �ε����� �ؽ�Ʈ �Ϲ� �մ� �����͸� �������� �޼���
    public CustomerNormalTextData GetCustomerTextByIndex(int index)
    {
        if (index < 0 || index >= AllCustomerNormalData.Count)
            return null;

        return AllCustomerNormalData[index];
    }
    
    // �ε����� �ֹ� X �ؽ�Ʈ �´� �� �����͸� �������� �޼���
    public CustomerOnlyTextData GetCustomerOnlyTextByIndex(int index)
    {
        if (index < 0 || index >= AllCustomerOnlyTextData.Count)
            return null;

        return AllCustomerOnlyTextData[index];
    }

    // �̸����� Ư�� �� �����͸� �������� �޼���
    public CustomerSpecialTextData GetSpecialCustomerByName(string name)
    {
        if (_customerSpecialDataByName.TryGetValue(name, out CustomerSpecialTextData data))
        {
            return data;
        }
        return null;
    }

    // �ε����� Ư�� �� �����͸� �������� �޼���
    public CustomerSpecialTextData GetSpecialCustomerByIndex(int index)
    {
        if (index < 0 || index >= AllCustomerSpecialData.Count)
            return null;

        return AllCustomerSpecialData[index];
    }
}