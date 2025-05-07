using System.Collections;
using UnityEngine;
using UnityEngine.Events;
public class MathButton : MonoBehaviour
{
    public UnityEvent OnPress;

    public float ButtonDelaySpeed;
    public bool CanPress = true;
    float initialZ;
    public float ButtnInfluece;
    public string _tag = "Finger";

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
    public void enableCanPress()
    {
        CanPress = true;
    }
    IEnumerator PressButton()
    {
        CanPress = false;
        OnPress.Invoke();
        yield return new WaitForSeconds(0.25f);
        CanPress = true;
    }
}
