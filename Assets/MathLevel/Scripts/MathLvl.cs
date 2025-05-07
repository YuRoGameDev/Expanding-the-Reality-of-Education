using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
public class MathLvl : MonoBehaviour
{
    public static MathLvl Instance;
    public bool Started = false, Finished = false, transition = false;
    public InputActionReference TriggerR, TriggerL;
    float x = 0;
    public WaveSpawner waveSpawner;
    public ScreenBox SBox, WinBox, LoseBox;
    public int Health = 5;
    public GameObject canvas;
    public TextMeshProUGUI health;
    private void Awake()
    {
        MathLvl.Instance = this;
    }

    private void Start()
    {
        SBox.SetScreen(true);
    }

    private void Update()
    {
        if (transition) { return; }
        if (Finished)
        {
            canvas.SetActive(false);
            if (TriggerR.action.IsPressed() || TriggerL.action.IsPressed() || Input.GetKey(KeyCode.D))
            {
                x += Time.deltaTime;
                if (x > 1)
                {
                    StartCoroutine(Delay(1));
                    transition = true;
                    x = 0;
                }
            }
            else
            {
                x = 0;
            }
            return;
        }

        if (Health == 0 && Started)
        {
            waveSpawner.DisableEnemies();
            Finished = true;
            LoseBox.SetScreen(true);
            Started = false;
        }

        health.text = "Health: ";
        for (int i = 0; i < Health; i++)
        {
            health.text += "I";
        }
        if (!Started)
        {
            if (TriggerR.action.IsPressed() || TriggerL.action.IsPressed() || Input.GetKey(KeyCode.D))
            {
                x += Time.deltaTime;
                if (x > 1)
                {
                    canvas.SetActive(true);
                    SBox.SetScreen(false);
                    Started = true;
                    x = 0;
                    waveSpawner.SpawnWave();
                }
            }
            else
            {
                x = 0;
            }
            return;
        }
    }

    IEnumerator Delay(int scene)
    {
        PlayerManager.Instance.SetFade(false);
        yield return new WaitForSeconds(1.1f);
        SceneManager.LoadScene(scene);
    }

    public void Win()
    {
        if (!Finished)
        {
            Finished = true;
            Started = false;
            WinBox.SetScreen(true);
        }
    }
}
