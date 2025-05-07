using UnityEngine;
using UnityEngine.AI;
using TMPro;
public class Enemy : MonoBehaviour
{
    public Transform Target, footPivot, lookTarget;
    NavMeshAgent agent;
    public int Difficulty, Answer;
    public string TxtDialogue;
    public TextMeshPro txt;
    System.Random rand = new System.Random();
    bool hasHit = false;
    public WaveSpawner waveSpawner;
    public AudioSource HitSound;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.destination = Target.position;
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, -Vector3.up, out hit, 1))
        {
            Quaternion targetRotation = Quaternion.FromToRotation(footPivot.up, hit.normal) * footPivot.rotation;
            footPivot.rotation = Quaternion.Slerp(footPivot.rotation, targetRotation, Time.deltaTime * 10f);
        }
        txt.transform.LookAt(lookTarget.position);

        if (agent.remainingDistance < 1f && !hasHit)
        {
            HitSound.Play();
            hasHit = true;
            MathLvl.Instance.Health--;
            waveSpawner.enemies.Remove(this);
            Destroy(gameObject);
        }

        if (TxtDialogue == "")
        {
            Difficulty = Random.Range(1, 3);
            SetMathProblem();
            Debug.Log("FixItself");
        }

        if (Input.GetKeyDown(KeyCode.A))
        {
            Difficulty = Random.Range(3, 5);
            SetMathProblem();
        }
    }


    public void SetMathProblem()
    {
        int a, b, c;
        string op = "";

        switch (Difficulty)
        {
            case 1: // Difficulty 1: Single number 1-9
                Answer = Random.Range(1, 10);
                TxtDialogue = Answer.ToString() + "= X";
                break;

            case 2: // Difficulty 2: Single number 10-100
                Answer = Random.Range(10, 101);
                TxtDialogue = Answer.ToString() + "= X";
                break;
            case 3:
                do
                {
                    a = Random.Range(1, 101);
                    b = Random.Range(1, 101);
                    int opIndex = Random.Range(0, 4); // 0:+, 1:-, 2:*, 3:/
                    switch (opIndex)
                    {
                        case 0: Answer = a + b; op = "+"; break;
                        case 1: Answer = a - b; op = "-"; break;
                        case 2: Answer = a * b; op = "*"; break;
                        case 3:
                            if (b != 0 && a % b == 0)
                            {
                                Answer = a / b;
                                op = "/";
                            }
                            else continue; // retry if not divisible
                            break;
                    }
                } while (Answer < 1 || Answer > 100 || op == "");

                TxtDialogue = $"{a} {op} {b} = X";
                break;
            case 4: // Difficulty 4: Inverse or 3-number equation
                if (Random.Range(0, 2) == 0)
                {
                    // Inverse equation: find missing number
                    do
                    {
                        a = Random.Range(1, 101);
                        b = Random.Range(1, 101);
                        int opIndex = Random.Range(0, 4);
                        int result = 0;
                        switch (opIndex)
                        {
                            case 0: Answer = b; TxtDialogue = $"{a} + X = {a + b}"; break;
                            case 1: Answer = b; TxtDialogue = $"{a + b} - X = {a}"; break;
                            case 2: // Multiplication (special limit: max result 500)
                                a = Random.Range(1, 21);
                                b = Random.Range(1, 21);
                                result = a * b;
                                if (result <= 500)
                                {
                                    Answer = b;
                                    TxtDialogue = $"{a} * X = {result}";
                                }
                                break;
                            case 3:
                                if (b != 0 && a * b <= 100)
                                {
                                    Answer = b;
                                    TxtDialogue = $"{a * b} / X = {a}";
                                }
                                else continue;
                                break;
                        }
                        break;
                    } while (false);
                }
                else
                {
                    // 3-number equation
                    do
                    {
                        a = Random.Range(1, 21);
                        b = Random.Range(1, 21);
                        c = Random.Range(1, 21);
                        int op1 = Random.Range(0, 4), op2 = Random.Range(0, 4);

                        System.Func<int, int, int, int> evaluate = (x, y, z) =>
                        {
                            int first = op1 == 0 ? x + y : op1 == 1 ? x - y : op1 == 2 ? x * y : (y != 0 && x % y == 0 ? x / y : int.MinValue);
                            if (first == int.MinValue) return int.MinValue;
                            return op2 == 0 ? first + z : op2 == 1 ? first - z : op2 == 2 ? first * z : (z != 0 && first % z == 0 ? first / z : int.MinValue);
                        };

                        Answer = evaluate(a, b, c);
                        if (Answer >= 1 && Answer <= 100)
                        {
                            string[] ops = { "+", "-", "*", "/" };
                            TxtDialogue = $"({a} {ops[op1]} {b}) {ops[op2]} {c} = X";
                            break;
                        }
                    } while (true);
                }
                break;
        }

        txt.text = TxtDialogue;
    }
}

