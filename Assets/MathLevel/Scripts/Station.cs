using TMPro;
using UnityEngine;

public class Station : MonoBehaviour
{
    public int slot1 = -1, op = -1, slot2 = -1, solution = -1;
    public GameObject Dig1, Dig2, Op, Solution, SolutionInvalid;
    public TextMeshProUGUI Dig1Value, Dig2Value, OpValue, SolutionValue, SolutionInvalidValue;
    public WaveSpawner wveSpawner;
    public Transform Arrow;
    bool Shot;
    public AudioSource Tap, Clear;
    private void Start()
    {
        SetVisuals();
    }
    public void SetSlot(int value)
    {
        if (!MathLvl.Instance.Started || MathLvl.Instance.Finished)
        {
            return;
        }
        if (slot1 == -1)
        {
            slot1 = value;
        }
        else
        {
            if (op != -1)
            {
                slot2 = value;
                SetSolution();
            }
        }
        SetVisuals();
        Tap.Play();
    }

    public void SetOperator(int op)
    {
        if (!MathLvl.Instance.Started || MathLvl.Instance.Finished)
        {
            return;
        }
        this.op = op;
        if (slot2 != -1)
        {
            SetSolution();
        }
        SetVisuals();
        Tap.Play();
    }


    void SetSolution()
    {
        switch (op)
        {
            case 0:
                solution = slot1 + slot2;
                break;
            case 1:
                solution = slot1 - slot2;
                break;
            case 2:
                solution = slot1 * slot2;
                break;
            case 3:
                solution = slot1 / slot2;
                int remainder = slot1 % slot2;
                if (remainder != 0)
                {
                    solution = -4;
                }
                break;
        }
        if (solution != -4)
        {
            if (solution > 100)
            {
                solution = -2;
            }
            else if (solution < 0)
            {
                solution = -3;
            }
        }

        SetVisuals();
    }

    public void ClearEquation()
    {
        if (!MathLvl.Instance.Started || MathLvl.Instance.Finished)
        {
            return;
        }
        slot1 = -1;
        slot2 = -1;
        op = -1;
        solution = -1;
        SetVisuals();
        Clear.Play();
    }

    public void SendEquation()
    {
        if (solution < 0 || !MathLvl.Instance.Started || MathLvl.Instance.Finished)
        {
            return;
        }
        int answer = solution;
        wveSpawner.ShootEnemy(answer);
        ClearEquation();
    }

    public void ReUseSolution()
    {
        if (!MathLvl.Instance.Started || MathLvl.Instance.Finished)
        {
            return;
        }
        int answer = solution;
        ClearEquation();
        SetSlot(answer);
    }

    void SetVisuals()
    {
        Dig1.SetActive(slot1 != -1);
        Dig2.SetActive(slot2 != -1);
        Op.SetActive(op != -1);
        Solution.SetActive(false);
        SolutionInvalid.SetActive(false);

        switch (solution)
        {
            case -2:
                SolutionInvalid.SetActive(true);
                SolutionInvalidValue.text = "Too Big";
                break;
            case -3:
                SolutionInvalid.SetActive(true);
                SolutionInvalidValue.text = "Too Small";
                break;
            case -4:
                SolutionInvalid.SetActive(true);
                SolutionInvalidValue.text = "Not a Whole Number";
                break;
            default:
                if (solution != -1)
                {
                    Solution.SetActive(true);
                    SolutionValue.text = solution.ToString();
                }
                break;
        }

        Dig1Value.text = slot1.ToString();
        Dig2Value.text = slot2.ToString();
        OpValue.text = op switch
        {
            0 => "+",
            1 => "-",
            2 => "x",
            3 => "/",
            _ => ""
        };

    }
}
