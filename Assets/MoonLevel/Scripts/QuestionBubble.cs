using UnityEngine;

public class QuestionBubble : MonoBehaviour
{
    public int boxNumb;
    public bool Selected, Enabled;
    public Renderer Edge;
    public Material SelOn, SelOff;
    private void Update()
    {
        Edge.material = Selected ? SelOn : SelOff;
    }
}
