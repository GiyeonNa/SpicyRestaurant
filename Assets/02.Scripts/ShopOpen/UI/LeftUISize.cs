using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LeftUISize : MonoBehaviour
{
    //���� �� ������ ���� ���� �ؽ�Ʈ ���� ���� ����
    public static LeftUISize Instance;

    public RectTransform MoneyPanel;//��
    public TextMeshProUGUI MoneyText;

    public RectTransform SatisfactionPanel;//������
    public TextMeshProUGUI SatisfactionText;

    private string _lastMoneyText = "";
    private string _lastSatisfactionText = "";

    private void FixedUpdate()
    {
        _lastMoneyText = MoneyText.text;
        _lastSatisfactionText = SatisfactionText.text;
        UpdateMoneySize();

        UpdateSatisfactionSize();
    }


    //ó���� ������ Ȯ��
    //���Ŀ��� �ؽ�Ʈ�� ����Ǿ��� �� �ҷ����� �ɷ�
    /*private void Start()
    {
        Instance = this;

        _lastMoneyText = MoneyText.text;
        _lastSatisfactionText = SatisfactionText.text;
        UpdateMoneySize();
        UpdateSatisfactionSize();
    }*/

    //��
    public void UpdateMoneySize()
    {
        //�ؽ�Ʈ ���� ����
        // �ؽ�Ʈ ���� ���� (����, ��, ������, �Ʒ�)
        MoneyText.margin = new Vector4(35, 0, 10, 0);

        float contentWidth = MoneyText.preferredWidth;
        float totalWidth = contentWidth + 45;

        Vector2 size = MoneyPanel.sizeDelta;
        size.x = totalWidth;
        MoneyPanel.sizeDelta = size;
    }

    //�ູ��
    public void UpdateSatisfactionSize()
    {
        //�ؽ�Ʈ ���� ����
        // �ؽ�Ʈ ���� ���� (����, ��, ������, �Ʒ�)
        SatisfactionText.margin = new Vector4(25, 0, 17, 0);

        float contentWidth = SatisfactionText.preferredWidth;
        float totalWidth = contentWidth + 42;

        Vector2 size = SatisfactionPanel.sizeDelta;
        size.x = totalWidth;
        SatisfactionPanel.sizeDelta = size;
    }

}
