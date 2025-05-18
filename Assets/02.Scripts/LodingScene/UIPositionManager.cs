using UnityEngine;
using TMPro;

public class UIPositionManager : MonoBehaviour
{
    public RectTransform titleText;
    public RectTransform loadingText;
    public TextMeshProUGUI loadingTMP;
    public RectTransform touchToStartText;

    private RectTransform canvasRect;
    private float baseLoadingTextHeight = 0f;

    void Start()
    {
        //0413
        SoundManager.Instance.StopBackgroundMusic();

        Canvas canvas = GetComponentInParent<Canvas>();
        if (canvas != null)
        {
            canvasRect = canvas.GetComponent<RectTransform>();
        }

        if (loadingTMP != null)
        {
            loadingTMP.ForceMeshUpdate();
            baseLoadingTextHeight = loadingTMP.preferredHeight;
        }
    }

    void Update()
    {
        if (canvasRect == null) return;
        UpdateUIPositions();
    }

    void UpdateUIPositions()
    {
        // ������: ȭ�� �߽� (anchoredPosition = Vector2.zero)
        /*float centerY = 0f;

        // ������ ���� ���
        float titleY = centerY + 650f;

        // loadingText�� �߽ɺ��� �Ʒ��� ��ġ
        float loadingTextY = centerY + 180f;//190

        // �ؽ�Ʈ �� ���� ���� touchToStartText�� �� �Ʒ��� �б�
        float extraHeight = 0f;
        if (loadingTMP != null)
        {
            loadingTMP.ForceMeshUpdate();
            float currentHeight = loadingTMP.preferredHeight;
            extraHeight = Mathf.Max(0, currentHeight - baseLoadingTextHeight);
        }

        float touchToStartY = loadingTextY - 100f - extraHeight;//100
        */
        float centerY = 0f;
        float titleY = centerY + 650f;
        float loadingTextY = centerY + 180f;

        int lineCount = loadingTMP.textInfo.lineCount;
        float lineSpacingOffset = (lineCount - 1) * 30f; // ���ϴ� ��ŭ�� ���

        float touchToStartY = loadingTextY - 100f - lineSpacingOffset;

        // ��ġ ����
        if (titleText)
            titleText.anchoredPosition = new Vector2(titleText.anchoredPosition.x, titleY);

        if (loadingText)
            loadingText.anchoredPosition = new Vector2(loadingText.anchoredPosition.x, loadingTextY);

        if (touchToStartText)
            touchToStartText.anchoredPosition = new Vector2(touchToStartText.anchoredPosition.x, touchToStartY);
    }
}
