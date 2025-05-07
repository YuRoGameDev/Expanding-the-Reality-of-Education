using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class WaveSpawner : MonoBehaviour
{
    public int Wave = 0;
    public int[] EnemyAmt;
    public GameObject Prefab;
    public Transform LeftPos, RightPos;
    public List<Enemy> enemies = new List<Enemy>();
    bool isSpawning = false, canSpawn = true;
    public Enemy currentTarget;
    public Transform CrossbowPitch, CrossbowYaw;
    public TextMeshProUGUI equatText, WaveTxt, EnemyCounterTxt;
    public AudioSource WaveStartSound, LossSound, GoodHit, BadHit;
    public void SpawnWave()
    {
        for (int i = 0; i < EnemyAmt[Wave]; i++)
        {
            GameObject obj = Instantiate(Prefab);

            obj.transform.position = pos();

            Enemy enemy = obj.GetComponent<Enemy>();
            enemy.Difficulty = Wave switch
            {
                0 => Random.Range(1, 2),
                1 => Random.Range(1, 3),
                2 => Random.Range(2, 4),
                3 => Random.Range(2, 5),
                4 => Random.Range(3, 5),

            };
            enemy.waveSpawner = this;
            enemy.SetMathProblem();
            enemies.Add(enemy);
        }
        StartCoroutine(Spawning());
        Wave++;
    }

    private void Update()
    {
        WaveTxt.text = "Wave " + Wave + "/5";
        EnemyCounterTxt.text = enemies.Count + "x";
        if (currentTarget != null)
        {
            currentTarget.txt.color = Color.yellow;

            Vector3 directionToTargetYaw = currentTarget.transform.position - CrossbowYaw.position;
            directionToTargetYaw.y = 0f;
            directionToTargetYaw.Normalize();

            Vector3 forward = Vector3.Cross(Vector3.up, directionToTargetYaw);

            if (forward.sqrMagnitude > 0.0001f)
            {
                Quaternion yawRotation = Quaternion.LookRotation(-forward, Vector3.up);
                CrossbowYaw.rotation = Quaternion.RotateTowards(
                    CrossbowYaw.rotation,
                    yawRotation,
                    Time.deltaTime * 360f
                );
            }

            Vector3 directionToTargetPitch = currentTarget.transform.position - CrossbowPitch.position;

            Quaternion pitchRotation = Quaternion.LookRotation(CrossbowPitch.forward, directionToTargetPitch.normalized);
            CrossbowPitch.rotation = Quaternion.RotateTowards(
                CrossbowPitch.rotation,
                pitchRotation,
                Time.deltaTime * 360f
            );
        }
        if (enemies.Count > 0)
        {
            currentTarget = enemies[0];
            if (enemies[0].gameObject.activeInHierarchy)
            {
                equatText.text = enemies[0].TxtDialogue;
            }

        }
        if (!isSpawning && enemies.Count == 0 && MathLvl.Instance.Started && canSpawn)
        {
            if (Wave == 5)
            {
                canSpawn = false;
                WaveStartSound.Play();
                MathLvl.Instance.Win();
                return;
            }
            SpawnWave();
        }
    }

    public void DisableEnemies()
    {
        LossSound.Play();
        canSpawn = false;
        foreach (Enemy enemy in enemies)
        {
            Destroy(enemy.gameObject);
        }
        enemies.Clear();
    }

    Vector3 pos()
    {
        Vector3 finalPos;
        int val = Random.Range(0, 2);
        if (val == 0)
        {
            finalPos = LeftPos.position + new Vector3(Random.Range(-2, 3), 0, Random.Range(-1, 2));
        }
        else
        {
            finalPos = RightPos.position + new Vector3(Random.Range(-2, 3), 0, Random.Range(-1, 2));
        }

        return finalPos;
    }

    IEnumerator Spawning()
    {
        equatText.text = "WAVE STARTING";
        WaveStartSound.Play();
        isSpawning = true;
        yield return new WaitForSeconds(3);
        while (enemies.Count > 0)
        {
            int x = 0;
            while (x < enemies.Count)
            {
                if (!enemies[x].gameObject.activeInHierarchy)
                {
                    enemies[x].gameObject.SetActive(true);
                    break;
                }
                x++;
                yield return null;
            }
            Debug.Log("B");
            yield return new WaitForSeconds(Random.Range(3f, 6f));
            yield return null;
        }
        isSpawning = false;
    }

    public void ShootEnemy(int val)
    {
        if (currentTarget != null)
        {
            if (val == currentTarget.Answer)
            {
                GoodHit.Play();
                enemies.Remove(currentTarget);
                Destroy(currentTarget.gameObject);
            }
            else
            {
                BadHit.Play();
            }

        }
    }
}
