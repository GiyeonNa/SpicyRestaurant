using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;

public class ResultManager : MonoBehaviour
{
    public static ResultManager Instance;

    [SerializeField]
    private Image fadeImage;
    private float fadeDuration = 1f;

    private Sequence _resultSequence;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        _resultSequence = DOTween.Sequence();

        _resultSequence.AppendCallback(()=> 
                        {
                            StartCoroutine(FadeIn());
                            fadeImage.gameObject.SetActive(false);
                        })
                        .AppendCallback(()=> StartStage());

        //SaveManager.SaveGame((GameManager.Instance._currentLevel + 1), GameManager.Instance.PlayerCurrencyData, GameManager.Instance.GetIngredientData(), Scenes.Match3Game);
        //GameManager.Instance.SetCurrentLevel(GameManager.Instance._currentLevel + 1);
        //  SaveManager.SaveGame(GameManager.Instance.CurrentLevel + 1, GameManager.Instance.PlayerCurrencyData, GameManager.Instance.GetIngredientData(), Scenes.Match3Game);

    }

    private void StartStage()
    {
        UI_Result.Instance.AnimateResultPanel();
        SoundManager.Instance.PlaySoundEffect(SoundManager.Instance.resultAppearSound);
    }

    public IEnumerator FadeIn()
    {
        if (fadeImage == null)
        {
            yield break;
        }

        fadeImage.gameObject.SetActive(true);
        Color color = fadeImage.color;
        float elapsedTime = 0f;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            color.a = Mathf.Lerp(1f, 0f, elapsedTime/fadeDuration);
            fadeImage.color = color;
            yield return null;
        }
    }


    public void OnNextStageButtonClick()
    {
        UI_Result.Instance.DOKill();
        SoundManager.Instance.PlaySoundEffect(SoundManager.Instance.popSound);

        if(GameManager.Instance.CurrentLevel > 2)//(GameManager.Instance.CheckEnding())
        {
            Debug.Log("ResultManager �������� ���� �� Ȯ��2");
            //LoadingSceneManager.LoadScene("Match3Game", LoadingSceneTypes.ResultToMatch3);
            LoadingSceneManager.LoadScene("EndingScene", LoadingSceneTypes.ResultToEnding);
            //�������� ���� ��


            //���Ͱ� ������ ����
            CurrencyManager.Instance.ResetCurrencyInOutData();

            //SaveManager.SaveGame((GameManager.Instance.CurrentLevel+1), GameManager.Instance.PlayerCurrencyData, GameManager.Instance.GetIngredientData(), Scenes.Match3Game);
            SaveManager.SaveGame(GameManager.Instance.CurrentLevel, GameManager.Instance.PlayerCurrencyData, GameManager.Instance.GetIngredientData(), Scenes.Match3Game);
            //GameManager.Instance.CurrentLevel + 1 �̾���
            Debug.Log("���� �ʱ�ȭ�Ǿ��⿡ �Ϸ� ���Ͱ� ������ �ʱ�ȭ");
        }
        else
        {
            //GameManager.Instance.EndStage();
            LoadingSceneManager.LoadScene("Match3Game", LoadingSceneTypes.ResultToMatch3);

            //���Ͱ� ������ ����
            CurrencyManager.Instance.DayChangeCurrencyInOutData();

            //SaveManager.SaveGame((GameManager.Instance.CurrentLevel+1), GameManager.Instance.PlayerCurrencyData, GameManager.Instance.GetIngredientData(), Scenes.Match3Game);
            SaveManager.SaveGame(GameManager.Instance.CurrentLevel, GameManager.Instance.PlayerCurrencyData, GameManager.Instance.GetIngredientData(), Scenes.Match3Game);
            Debug.Log("���� �ູ�� �� ����. �Ϸ� ���Ͱ� ������ �ʱ�ȭ");
            //GameManager.Instance.CurrentLevel + 1 �̾���
        }

       
    }

    public void OnMainMenuButtonClick()//���� ����� ����
    {
/*#if UNITY_EDITOR
        EditorApplication.isPlaying = false; // �����Ϳ����� �÷��� ��� ����
#else
    Application.Quit(); // ���� ���忡���� ����
#endif*/
         UI_Result.Instance.DOKill();
         SoundManager.Instance.PlaySoundEffect(SoundManager.Instance.popSound);
         //GameManager.Instance.EndStage();
         LoadingSceneManager.LoadScene("StartScene", LoadingSceneTypes.ResultToMatch3);

         //���Ͱ� ������ ����
         CurrencyManager.Instance.DayChangeCurrencyInOutData();
         Debug.Log("���� �ູ�� �� ����. �Ϸ� ���Ͱ� ������ �ʱ�ȭ");
         SaveManager.SaveGame(GameManager.Instance.CurrentLevel, GameManager.Instance.PlayerCurrencyData, GameManager.Instance.GetIngredientData(), Scenes.Match3Game);
        
    }
}
