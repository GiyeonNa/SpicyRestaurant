using UnityEngine;
using UnityEditor;
using UnityEngine.UI;

public class EndGameManager : MonoSingleton<EndGameManager>
{
    public GameObject EndPanel;

    //�����Ͻðڽ��ϱ� �г��� �����ִ����� ���� �ٲ� ��
    public GameObject ResetPanel;
    public Image EndGameBlack;

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if(EndPanel.activeSelf == true)
            {
                //Ȱ��ȭ �Ǿ��ִ� ����
                EndPanel.SetActive(false);
            }

            else
            {
                //���࿡ ���� �г��� �����ִ� ���¶��
                if(ResetPanel.activeSelf == true)
                {
                    EndGameBlack.enabled = false;
                }

                //�׷��� �ʴٸ�
                else
                {
                    EndGameBlack.enabled = true;
                }

                EndPanel.SetActive(true);

                if (ShapesManager.Instance != null)
                {
                    ShapesManager.Instance.PauseTimer();
                    ShapesManager.Instance.PauseBlockMovement();
                }
                else
                {
                    Debug.Log("ShapesManager�� ���� ���� �������� �ʽ��ϴ�.");
                }
            }

        }
    }

    public void EndGameYes()
    {
#if UNITY_EDITOR
        EditorApplication.isPlaying = false; // �����Ϳ����� �÷��� ��� ����
#else
    Application.Quit(); // ���� ���忡���� ����
#endif
    }

    public void Continue()
    {
        SoundManager.Instance.PlaySoundEffect(SoundManager.Instance.popSound);
        EndPanel.SetActive(false);

        if (ShapesManager.Instance != null)
        {
            ShapesManager.Instance.ResumeTimer();
            ShapesManager.Instance.ResumeBlockMovement();
        }
        else
        {
            Debug.Log("ShapesManager�� ���� ���� �������� �ʽ��ϴ�.");
        }
    }
}
