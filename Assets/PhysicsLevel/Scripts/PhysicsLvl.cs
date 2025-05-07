using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PhysicsLvl : MonoBehaviour
{
    public static PhysicsLvl Instance;
    public bool Started = false, Finished = false;
    public ScreenBox screenBox;
    public int TotalTargets;
    public TextMeshProUGUI UITxt;
    public Animator anim;
    public AudioSource Win;

    private void Awake()
    {
        PhysicsLvl.Instance = this;
    }

    void Start()
    {
        screenBox.SetScreen(true);
    }
    private void Update()
    {
        UITxt.text = TotalTargets + "x";
    }

    public void StartGame()
    {
        if (!Started)
        {
            Started = true;
            screenBox.SetScreen(false);
        }
    }

    public void TargetHit()
    {
        TotalTargets--;
        if (TotalTargets <= 0)
        {
            if (!Finished)
            {
                Finished = true;
                anim.enabled = true;
                Win.Play();
                StartCoroutine(SceneDelay());

                IEnumerator SceneDelay()
                {
                    yield return new WaitForSeconds(3);
                    PlayerManager.Instance.SetFade(false);
                    yield return new WaitForSeconds(1.1f);
                    SceneManager.LoadScene(0);
                }
            }
        }
    }
}
