using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
public class LevelSelect : MonoBehaviour
{
    public bool canClick = true;
    public ScreenBox startScreen;
    public ScreenBox[] ScreenBoxes;
    public Renderer[] ScreenRenders;
    public Material SelOff, SelOn;
    public InputActionReference RTrigger;
    public Transform RHand;
    public LayerMask layerMask;

    [SerializeField] Transform targetedBubbleR;
    public float pointingThreshold = 15f; // degrees
    public LineRenderer lineR;
    int currentChosen;
    public AudioSource MenuChange;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Physics.gravity = new Vector3(0, -9.81f, 0);
        lineR.enabled = false;
        startScreen.SetScreen(true);
        foreach (ScreenBox qm in ScreenBoxes)
        {
            qm.SetScreen(true);
        }
    }

    // Update is called once per frame
    void Update()
    {
        lineR.SetPosition(0, RHand.position);
        lineR.SetPosition(1, RHand.position + RHand.forward * 7);

        if (!targetedBubbleR)
        {
            foreach (ScreenBox qm in ScreenBoxes)
            {
                Vector3 toTargetR = (qm.transform.position - RHand.position).normalized;
                float angleR = Vector3.Angle(RHand.forward, toTargetR);

                if (angleR <= pointingThreshold)
                {
                    Debug.Log("Pointing at: " + qm.name);
                    targetedBubbleR = qm.transform;
                    break;
                }
            }
        }
        else
        {
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
        if (!canClick) { return; }
        RaycastHit hitR;
        bool isHitR = Physics.Raycast(RHand.position, RHand.forward, out hitR, Mathf.Infinity, layerMask, QueryTriggerInteraction.Collide);

        if (isHitR)
        {
            currentChosen = hitR.collider.transform.tag switch
            {
                "Lvl1" => 1,
                "Lvl2" => 2,
                "Lvl3" => 3,
                _ => 0
            };

            Material[] mats = ScreenRenders[currentChosen - 1].materials;
            mats[0] = SelOn;
            ScreenRenders[currentChosen - 1].materials = mats;

            if (RTrigger.action.WasPressedThisFrame())
            {
                canClick = false;
                MenuChange.Play();
                StartCoroutine(LvlChange());
            }
        }
        else
        {
            if (currentChosen != 0)
            {
                Material[] mats = ScreenRenders[currentChosen - 1].materials;
                mats[0] = SelOff;
                ScreenRenders[currentChosen - 1].materials = mats;
            }

        }

    }

    IEnumerator LvlChange()
    {
        PlayerManager.Instance.SetFade(false);
        yield return new WaitForSeconds(1.1f);
        SceneManager.LoadScene(currentChosen);
    }
}
