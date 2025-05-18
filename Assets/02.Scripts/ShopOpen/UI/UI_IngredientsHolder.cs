using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI_IngredientsHolder : MonoBehaviour
{

    public UI_Ingreident UI_IngreidentPrefab;
    public GameObject UI_IngredientDarkPrefab; // ��ư�� ���� ��ο� �����ܿ� ������

    [SerializeField]
    private RectTransform panelTransfrom;

    private List<UI_Ingreident> _ui_IngredientsList;
    private List<Ingredient> _ingredientsList;


    private void Start()
    {
        SetHolderPosition();

        _ui_IngredientsList = new List<UI_Ingreident>();
        _ingredientsList = IngredientManager.Instance.IngredientsList;

        SetContraintCount();

        int ingredientCount = _ingredientsList.Count;
        int darkCount = 14;  // ��ο� ������ ���� //19

        // ���� ������ ����Ʈ�� ���� ���� ������ ����
        List<GameObject> prefabs = new List<GameObject>();

        // 1. �ʿ��� �����յ��� ���� �ִ´�
        for (int i = 0; i < ingredientCount; i++)
        {
            UI_Ingreident uiIngredient = Instantiate(UI_IngreidentPrefab, this.transform);
            uiIngredient.Ingredient = _ingredientsList[i];
            uiIngredient.Refresh();
            _ui_IngredientsList.Add(uiIngredient);
            prefabs.Add(uiIngredient.gameObject);  // �ʿ��� ������ ����Ʈ�� �߰�
        }

        // 2. �� �Ŀ� ��ο� �����յ��� �ִ´�
        for (int i = 0; i < darkCount; i++)
        {
            GameObject darkPrefab = Instantiate(UI_IngredientDarkPrefab, this.transform);
            prefabs.Add(darkPrefab);  // ��ο� ������ ����Ʈ�� �߰�
        }

        // ���� GridLayoutGroup���� �� �������� ������� ��ġ�ϵ��� �Ѵ�.
        for (int i = 0; i < prefabs.Count; i++)
        {
            prefabs[i].transform.SetSiblingIndex(i);  // ������� ��ġ
        }
    }

    public void SetHolderPosition()
    {
        panelTransfrom = GetComponent<RectTransform>();
        Vector2 anchoredPos = panelTransfrom.anchoredPosition;
        anchoredPos.y = 0.000381582f;
        panelTransfrom.anchoredPosition = anchoredPos;
    }

    // GridLayoutGroup ����
    private void SetContraintCount()
    {
        GridLayoutGroup grid = GetComponent<GridLayoutGroup>();
        if (grid != null)
        {
            grid.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
            grid.constraintCount = 4;  // 4�� ����
        }
        else
        {
            Debug.LogWarning("GridLayoutGroup ������Ʈ�� ã�� �� �����ϴ�.");
        }
    }


}

