using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class BowlMeatChecker : MonoBehaviour
{
    public static BowlMeatChecker Instance;
    public GameObject Bowl; // Bowl ������Ʈ
    public Sprite BrownMeatSprite;

    private List<GameObject> _putMeatObjects = new List<GameObject>(); // GameObject ����Ʈ�� ����

    void Start()
    {
        Instance = this;
    }

    public void CheckBowlChildren()
    {
        _putMeatObjects.Clear(); // ����Ʈ �ʱ�ȭ

        Image[] childImages = Bowl.GetComponentsInChildren<Image>(true);

        foreach (Image img in childImages)
        {
            if (img.sprite != null && img.sprite.name == "put_meat")
            {
                _putMeatObjects.Add(img.gameObject); // GameObject�� ����
                Debug.Log("**�� ���� ��� ã�Ҵ�! " + img.gameObject.name);
            }
        }

        Debug.Log($"put_meat ��������Ʈ�� ���� �ڽ� ������Ʈ ��: {_putMeatObjects.Count}");

    }

    public void ReplacePutMeatSprites()
    {
        if (BrownMeatSprite == null)
        {
            Debug.Log("���� ��Ⱑ �����Ǿ� ���� ����");
            return;
        }

        foreach (GameObject obj in _putMeatObjects)
        {
            if (obj != null)
            {
                Image img = obj.GetComponent<Image>();
                if (img != null)
                {
                    img.sprite = BrownMeatSprite;
                    Debug.Log("**���� �ɷ� ��ü �Ϸ�: " + obj.name);
                }
                else
                {
                    Debug.LogWarning($"������Ʈ {obj.name} �� Image ������Ʈ�� �����ϴ�!");
                }
            }
        }

        Debug.Log("**�� ���� �� ��� ���� ���� ��ü �Ϸ�");
    }
}
