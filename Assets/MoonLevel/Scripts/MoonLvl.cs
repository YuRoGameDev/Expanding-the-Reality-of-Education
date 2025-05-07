using UnityEngine;
using UnityEngine.InputSystem;
public class MoonLvl : MonoBehaviour
{
    public Rigidbody XROrigin;
    public Transform[] QuestionMarks, ScreenBoxes;
    public InputActionReference LStick, RStick, LTrigger, RTrigger;
    public float JumpForce;
    public Transform LHand, RHand;
    public LayerMask layerMask;
    public QuestionBubble currentBubble, selectedBubble;
    [SerializeField] Transform targetedBubbleL, targetedBubbleR;
    public float pointingThreshold = 15f; // degrees
    public LineRenderer lineL, lineR;
    public AudioSource openSelect;
    void Awake()
    {
        Physics.gravity = new Vector3(0, -1.62f, 0);

        foreach (Transform trans in QuestionMarks)
        {
            trans.LookAt(XROrigin.transform.position);
            trans.rotation = Quaternion.Euler(0, trans.eulerAngles.y, 0);
        }

        foreach (Transform trans in ScreenBoxes)
        {
            trans.LookAt(XROrigin.transform.position);
            trans.rotation = Quaternion.Euler(0, trans.eulerAngles.y + -90, 0);
        }
        lineL.enabled = false;
        lineR.enabled = false;
    }

    private void Update()
    {
        XROrigin.position = new Vector3(XROrigin.position.x, Mathf.Clamp(XROrigin.position.y, 10.048f, Mathf.Infinity), XROrigin.position.z);
        if (XROrigin.position.y <= 10.1f)
        {
            if (LStick.action.WasPressedThisFrame() || RStick.action.WasPressedThisFrame())
            {
                XROrigin.AddForce(Vector3.up * JumpForce, ForceMode.Impulse);
            }
        }
        Debug.DrawRay(LHand.position, LHand.forward * 100, Color.red);

        Debug.DrawRay(RHand.position, RHand.forward * 100, Color.blue);

        lineL.SetPosition(0, LHand.position);
        lineL.SetPosition(1, LHand.position + LHand.forward * 7);
        lineR.SetPosition(0, RHand.position);
        lineR.SetPosition(1, RHand.position + RHand.forward * 7);

        RaycastHit hitL, hitR;
        bool isHitL = Physics.Raycast(LHand.position, LHand.forward, out hitL, Mathf.Infinity, layerMask, QueryTriggerInteraction.Collide);
        bool isHitR = Physics.Raycast(RHand.position, RHand.forward, out hitR, Mathf.Infinity, layerMask, QueryTriggerInteraction.Collide);


        if (!targetedBubbleL && !targetedBubbleR)
        {
            foreach (Transform qm in QuestionMarks)
            {
                Vector3 toTargetL = (qm.position - LHand.position).normalized;
                float angleL = Vector3.Angle(LHand.forward, toTargetL);

                Vector3 toTargetR = (qm.position - RHand.position).normalized;
                float angleR = Vector3.Angle(RHand.forward, toTargetR);

                if (angleL <= pointingThreshold && !isHitR)
                {
                    Debug.Log("Pointing at: " + qm.name);
                    targetedBubbleL = qm;
                    break;
                }

                if (angleR <= pointingThreshold && !isHitL)
                {
                    Debug.Log("Pointing at: " + qm.name);
                    targetedBubbleR = qm;
                    break;
                }
            }
        }
        else
        {
            if (targetedBubbleL != null)
            {
                Vector3 toTarget = (targetedBubbleL.position - LHand.position).normalized;
                float angle = Vector3.Angle(LHand.forward, toTarget);
                lineL.enabled = true;
                if (angle >= pointingThreshold)
                {
                    targetedBubbleL = null;
                    lineL.enabled = false;
                }
            }
            if (targetedBubbleR != null)
            {
                Vector3 toTarget = (targetedBubbleR.position - RHand.position).normalized;
                float angle = Vector3.Angle(RHand.forward, toTarget);
                lineR.enabled = true;
                if (angle >= pointingThreshold)
                {
                    targetedBubbleR = null;
                    lineR.enabled = false;
                }
            }
        }


        if (isHitL && !isHitR)
        {
            Debug.Log("L");

            currentBubble = hitL.transform.gameObject.GetComponent<QuestionBubble>();

            if (currentBubble != selectedBubble)
            {
                currentBubble.Selected = true;
                if (LTrigger.action.WasPressedThisFrame())
                {
                    if (selectedBubble != null)
                    {
                        selectedBubble.Selected = false;
                    }
                    if (selectedBubble == currentBubble)
                    {
                        Debug.Log("Same");
                    }
                    selectedBubble = currentBubble;
                    selectedBubble.Selected = true;
                    ScreenBoxManager.instance.DisableBox();
                    ScreenBoxManager.instance.EnableBox(currentBubble.boxNumb);
                    openSelect.Play();
                }
            }
            else
            {
                if (LTrigger.action.WasPressedThisFrame())
                {
                    if (selectedBubble != null)
                    {
                        selectedBubble.Selected = false;
                    }
                    if (selectedBubble == currentBubble)
                    {
                        Debug.Log("Same");
                    }
                    selectedBubble = null;
                    ScreenBoxManager.instance.DisableBox();
                    openSelect.Play();
                }
            }

        }
        else
        {
            if (currentBubble != null && !isHitR)
            {
                if (currentBubble != selectedBubble)
                {
                    currentBubble.Selected = false;
                }
                currentBubble = null;
            }
        }

        if (isHitR && !isHitL)
        {
            Debug.Log("R");
            currentBubble = hitR.transform.gameObject.GetComponent<QuestionBubble>();
            if (currentBubble != selectedBubble)
            {
                currentBubble.Selected = true;
                if (RTrigger.action.WasPressedThisFrame())
                {
                    if (selectedBubble != null)
                    {
                        selectedBubble.Selected = false;
                    }
                    if (selectedBubble == currentBubble)
                    {
                        Debug.Log("Same");
                    }
                    selectedBubble = currentBubble;
                    selectedBubble.Selected = true;
                    ScreenBoxManager.instance.DisableBox();
                    ScreenBoxManager.instance.EnableBox(currentBubble.boxNumb);
                    openSelect.Play();
                }

            }
            else
            {
                if (RTrigger.action.WasPressedThisFrame())
                {
                    if (selectedBubble != null)
                    {
                        selectedBubble.Selected = false;
                    }
                    if (selectedBubble == currentBubble)
                    {
                        Debug.Log("Same");
                    }
                    selectedBubble = null;
                    ScreenBoxManager.instance.DisableBox();
                    openSelect.Play();
                }
            }
        }
        else
        {
            if (currentBubble != null && !isHitL)
            {
                if (currentBubble != selectedBubble)
                {
                    currentBubble.Selected = false;
                }

                currentBubble = null;
            }
        }
    }
}
