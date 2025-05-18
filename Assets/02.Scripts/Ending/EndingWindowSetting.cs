using DG.Tweening;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EndingWindowSetting : MonoBehaviour
{
    [Header("DialogWindowParts")]
    public RectTransform DialogWindow;
    public TextMeshProUGUI DialogText;

    [Header("DialogWindowSettings")]
    public float Padding = 30f;
    public float AnimationDuration = 0.3f;
    public bool AnimateInPlayMode = true;

    private LayoutElement _dialogLayout;
    private string _lastText = "";

    void Awake()
    {
        _dialogLayout = DialogWindow.GetComponent<LayoutElement>();

        //  pivot�� anchor ������ �ڵ忡���� Ȯ���� ���� (���û���)
        if (DialogWindow != null)
        {
            DialogWindow.pivot = new Vector2(0.5f, 1f);        // �� ����
            DialogWindow.anchorMin = new Vector2(0f, 1f);       // ���� ����
            DialogWindow.anchorMax = new Vector2(1f, 1f);
        }
    }

    void Update()
    {
        if (DialogText == null || _dialogLayout == null) return;

        if (DialogText.text != _lastText)
        {
            _lastText = DialogText.text;
            UpdateBubble();
        }
    }

    void UpdateBubble()
    {
        // �ؽ�Ʈ �¿� ���� (���ϸ� Top, Bottom ���鵵 �� �� ����)
        DialogText.margin = new Vector4(40, 0, 40, 0);

        // �ؽ�Ʈ ���̾ƿ� ������ ����
        LayoutRebuilder.ForceRebuildLayoutImmediate(DialogText.rectTransform);

        float targetHeight = DialogText.preferredHeight + Padding;

        if (Application.isPlaying && AnimateInPlayMode)
        {
            DOTween.Kill(_dialogLayout);
            DOTween.To(
                () => _dialogLayout.preferredHeight,
                x => _dialogLayout.preferredHeight = x,
                targetHeight,
                AnimationDuration
            ).SetEase(Ease.OutCubic).SetId(_dialogLayout);
        }
        else
        {
            _dialogLayout.preferredHeight = targetHeight;
        }
    }
}
