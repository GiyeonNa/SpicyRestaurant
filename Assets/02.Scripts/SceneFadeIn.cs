using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class SceneFadeIn : MonoBehaviour
{
    [SerializeField] private Image _fadeImage;
    [SerializeField] private float _fadeDuration = 1.5f;
    [SerializeField] private Ease _fadeEaseType = Ease.InOutQuad;

    private void Start()
    {
        // ���� �� ������ �̹����� ȭ���� ������ �����ϴ�
        if (_fadeImage != null)
        {
            // �̹����� �������̰� ������ ���������� Ȯ��
            _fadeImage.color = new Color(0, 0, 0, 1);

            // ���̵� �� �ִϸ��̼� ���� (���İ��� 0���� ����)
            _fadeImage.DOFade(0f, _fadeDuration)
                .SetEase(_fadeEaseType)
                .OnComplete(() =>
                {
                    _fadeImage.raycastTarget = false;
                    _fadeImage.maskable = false;
                });
        }
    }
}