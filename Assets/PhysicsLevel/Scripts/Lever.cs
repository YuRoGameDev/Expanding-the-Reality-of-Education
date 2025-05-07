using UnityEngine;

public class Lever : MonoBehaviour
{
    [SerializeField] private float MinX, MaxX;
    [SerializeField] Vector3 LowerOffsetR, LowerOffsetL;
    [SerializeField] Vector3 offset;
    public bool isUp;

    [SerializeField] int type;

    private void Start()
    {
        UpdateVals();
    }
    private void Update()
    {
        UpdateVals();
    }

    public void GrabLever(QuestInteractable button)
    {
        Transform Target = button.AttachedToHand.transform;
        Vector3 offset = button.AttachedToHand.LeftHand ? LowerOffsetL : LowerOffsetR;
        this.offset = offset;
        Turn(Target, transform, offset);
        UpdateVals();
    }

    void Turn(Transform Target, Transform Slider, Vector3 offset)
    {
        if (isUp)
        {
            Slider.position = new Vector3(Slider.position.x, Target.position.y + offset.y, Slider.position.z);
        }
        else
        {
            Slider.position = new Vector3(Slider.position.x, Slider.position.y, Target.position.z + offset.z);
        }

        Slider.localPosition = new Vector3(Mathf.Clamp(Slider.localPosition.x, MinX, MaxX), 0, 0);
    }

    void UpdateVals()
    {
        float normalizedValue;
        float remappedValue;
        switch (type)
        {
            case 0:
                normalizedValue = Mathf.InverseLerp(MaxX, MinX, transform.localPosition.x); // Returns a value between 0 and 1
                remappedValue = Mathf.Lerp(-45f, 0f, normalizedValue);
                CannonCtrl.Instance.XTurn = remappedValue;
                break;

            case 1:
                normalizedValue = Mathf.InverseLerp(MaxX, MinX, transform.localPosition.x); // Returns a value between 0 and 1
                remappedValue = Mathf.Lerp(-45f, 45f, normalizedValue);
                CannonCtrl.Instance.YTurn = remappedValue;
                break;

            case 2:
                normalizedValue = Mathf.InverseLerp(MaxX, MinX, transform.localPosition.x); // Returns a value between 0 and 1
                remappedValue = Mathf.Lerp(0f, 100f, normalizedValue);
                CannonCtrl.Instance.Power = remappedValue;
                break;

            case 3:
                normalizedValue = Mathf.InverseLerp(MaxX, MinX, transform.localPosition.x); // Returns a value between 0 and 1
                remappedValue = Mathf.Lerp(-9.81f, 9.81f, normalizedValue);
                CannonCtrl.Instance.Gravity = remappedValue;
                break;

            case 4:
                normalizedValue = Mathf.InverseLerp(MaxX, MinX, transform.localPosition.x); // Returns a value between 0 and 1
                remappedValue = Mathf.Lerp(0f, 1f, normalizedValue);
                CannonCtrl.Instance.WindResistance = remappedValue;
                break;
        }
    }
}
