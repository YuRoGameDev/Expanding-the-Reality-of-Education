using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
public class PlayerManager : MonoBehaviour
{
    public static PlayerManager Instance;
    public Animator FadeAnim;
    public InputActionReference Menu;
    float x;
    bool Leaving = false;
    public bool canLeave = true;
    public AudioSource MenuChange;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        PlayerManager.Instance = this;
    }

    private void Start()
    {
        SetFade(true);
    }

    public void SetFade(bool val)
    {
        FadeAnim.Play(val ? "FadeIn" : "FadeOut", 0, 0.0f);
    }

    // Update is called once per frame
    void Update()
    {
        if (Menu.action.IsPressed() && !Leaving && canLeave)
        {
            x += Time.deltaTime;
            if (x > 1)
            {
                Leaving = true;
                MenuChange.Play();
                SetFade(false);
                StartCoroutine(QuitDelay());
                IEnumerator QuitDelay()
                {
                    yield return new WaitForSeconds(1.1f);
                    UnityEngine.SceneManagement.SceneManager.LoadScene(0);
                }
            }
        }
    }
}
