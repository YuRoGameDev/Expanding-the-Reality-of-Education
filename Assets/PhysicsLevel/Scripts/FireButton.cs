using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class FireButton : MonoBehaviour
{
    public UnityEvent OnPress;

    public AnimationCurve AnimCurve;
    public SkinnedMeshRenderer button;
    public float ButtonDelaySpeed;
    public bool CanPress = true;
    float initialZ;
    public float ButtnInfluece;
    public string _tag = "HandPalm";

    private void Start()
    {
        initialZ = transform.localPosition.z;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == _tag && CanPress)
        {
            StartCoroutine(PressButton());
        }
    }
    IEnumerator PressButton()
    {
        CanPress = false;
        float x = 0;
        OnPress.Invoke();
        while (x < 1)
        {
            x += Time.deltaTime * ButtonDelaySpeed;
            float Val = AnimCurve.Evaluate(x) * ButtnInfluece;
            button.SetBlendShapeWeight(0, Val);
            yield return null;
        }
        CanPress = true;
    }
}
