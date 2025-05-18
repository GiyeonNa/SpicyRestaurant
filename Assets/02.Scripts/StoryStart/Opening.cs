using DG.Tweening;
using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class Opening : MonoBehaviour
{
    public GameObject FadeImage;
    public static Opening Instance;

    public TextMeshProUGUI DialogueText;
    [TextArea(2, 5)]
    public string[] Sentences;
    public float TypingSpeed = 0.05f;

    // public���� �����Ͽ� Unity �ν����Ϳ��� ���� Ȯ���ϰ� ������ �� �ְ� ��
    public int CurrentIndex = 0;
    public bool IsTyping = false;

    private Coroutine _typingCoroutine;
    public Action Action;

    [SerializeField] private List<GameObject> _fireworkList;
    private Coroutine _fireworkRoutine;
    public GameObject Banner;



    private void Start()
    {
        Instance = this;

        Banner.SetActive(false);
        int savedIndex = StorySaveManager.Instance.LoadIndex();

        if (savedIndex >= 2)//2
        {
            SceneManager.LoadScene("StartScene");
        }
        else
        {
            StorySaveManager.Instance.DeleteSave(); // ���� ������ ����
            foreach (GameObject firework in _fireworkList)
            {
                if (firework != null)
                {
                    firework.SetActive(false);
                }
            }
            CurrentIndex = 0;

            
            if(IsTyping == false)
            {
                StartTyping();
            }

            if (UI_Option.Instance.FadeImage.activeSelf == true)
            {
                UI_Option.Instance.FadeImageAnimation.SetTrigger("Empty");
                UI_Option.Instance.FadeImageRaycast.raycastTarget = false;
                FadeImage.SetActive(false);

                StartCoroutine(FadeImageFalse());

                IEnumerator FadeImageFalse()
                {
                    yield return new WaitForSeconds(1.5f);
                    UI_Option.Instance.FadeImage.SetActive(false);
                }
            }

            else if (UI_Option.Instance.FadeImage.activeSelf == false)
            {
                FadeImage.SetActive(true);
            }

        }
    }

    public void StartTyping()
    {
        if (CurrentIndex < Sentences.Length)
        {
            _typingCoroutine = StartCoroutine(TypeSentence(Sentences[CurrentIndex]));
        }
        /* else
         {
             SceneManager.LoadScene("StartScene");
         }*/
    }

    IEnumerator TypeSentence(string sentence)
    {
        IsTyping = true;
        DialogueText.text = "";

        // �ٹٲ� ó��
        sentence = sentence.Replace("\\\\", "\n");

        // �� ���� ����
        string[] lines = sentence.Split('\n');
        foreach (string line in lines)
        {
            string[] words = line.Split(' ');

            for (int i = 0; i < words.Length; i++)
            {
                foreach (char letter in words[i])
                {
                    DialogueText.text += letter;

                    // �� ���� ��� �ø��� ���带 ��� �ݺ� ���
                    SoundManager.Instance.PlayRandomSoundInList(SoundManager.Instance.typeSounds);

                    yield return new WaitForSeconds(TypingSpeed);
                }

                // ������ �ܾ �ƴ϶�� ���� �߰�
                if (i < words.Length - 1)
                {
                    DialogueText.text += ' ';
                }
            }

            // �ٹٲ� �� �� ����
            DialogueText.text += '\n';
        }

        // Ÿ������ �Ϸ�Ǹ� isTyping�� false�� ����
        IsTyping = false;

    }

    public void CompleteTyping()
    {
        if (!IsTyping) return;

        // �ڷ�ƾ ����
        StopCoroutine(_typingCoroutine);

        // ���� �ϼ��ؼ� ���
        string completedSentence = Sentences[CurrentIndex].Replace("\\\\", "\n");
        DialogueText.text = completedSentence;

        // Ÿ������ ���� �� isTyping�� false�� ����
        IsTyping = false;
    }


    public void OnNextButtonClick()
    {
        if (IsTyping)
        {
            // Ÿ���� ���� ���� Ÿ������ �Ϸ�
            CompleteTyping();
        }
        else
        {
            // Ÿ������ ���� �� ���� �������� �Ѿ
            if (CurrentIndex < Sentences.Length - 1)
            {
               

                CurrentIndex++;

                if (CurrentIndex == 2)
                {
                    if (_fireworkRoutine != null)
                        StopCoroutine(_fireworkRoutine);
                    _fireworkRoutine = StartCoroutine(RandomFireworkLoop());

                    Banner.SetActive(true);
                    FadeImage.SetActive(false);

                    // ���� ȣ��
                    Banner.GetComponent<BannerScale>().AnimateBanner();
                }


                else
                {
                    Banner.SetActive(false);
                    FadeImage.SetActive(true);
                    foreach (GameObject firework in _fireworkList)
                    {
                        if (firework != null)
                        {
                            firework.SetActive(false);
                        }
                    }
                }

                    StartTyping();
                StorySaveManager.Instance.SaveIndex(CurrentIndex);//�߰�

            }
            else
            {
               /* Banner.SetActive(true);

                FadeImage.SetActive(false);

                foreach (GameObject firework in _fireworkList)
                {
                    if (firework != null)
                    {
                        firework.SetActive(false);
                    }
                }*/

                StorySaveManager.Instance.SaveIndex(CurrentIndex);//�߰�
                //������� �߰�

                // ��� ������ ������ TitleScene���� �Ѿ
                SceneManager.LoadScene("StartScene");
                Debug.Log("��Ÿ�� ������!");
                StorySaveManager.Instance.SaveIndex(CurrentIndex);//�߰�
            }
        }
    }
    public IEnumerator RandomFireworkLoop()
    {
        while (CurrentIndex == 2)
        {
            if (_fireworkList == null || _fireworkList.Count == 0)
                yield break;

            List<GameObject> activeFireworks = new List<GameObject>();
            int randomCount = UnityEngine.Random.Range(4, 5);
            List<GameObject> shuffled = new List<GameObject>(_fireworkList);
            ShuffleList(shuffled);

            for (int i = 0; i < randomCount; i++)
            {
                GameObject firework = shuffled[i];
                firework.SetActive(true);
                activeFireworks.Add(firework);

                Animator animator = firework.GetComponent<Animator>();
                animator.SetTrigger("Firework");
            }

            SoundManager.Instance.PlaySoundEffect(SoundManager.Instance.FireworkSound1);
            yield return new WaitForSeconds(1.2f);

            foreach (var firework in activeFireworks)
            {
                firework.SetActive(false);
            }

            // ���� ���� ���� �� ������ ��� (���� �ٽ� Ȯ���ϰ�)
            yield return null;
        }
    }

    private void ShuffleList(List<GameObject> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            int rnd = UnityEngine.Random.Range(i, list.Count);
            GameObject temp = list[i];
            list[i] = list[rnd];
            list[rnd] = temp;
        }
    }
}
