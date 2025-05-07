using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class QuestHand : MonoBehaviour
{
    /// <summary>
    /// - Fix Throwing
    /// - Fix Popcorn not clearing fields
    /// </summary>

    public bool LeftHand;
    Animator anim;
    public InputActionReference TriggerPress, GripPress, TouchpadTouch, Velocity, AngularVelocity;
    // Start is called before the first frame update

    public GameObject HandMesh;
    public Collider FingerCollider, PalmCollider;
    public QuestPalm handpalm;
    public bool hasGripped;
    void Start()
    {
        anim = GetComponent<Animator>();
    }
    public bool IgnoreExtra;
    public float x;
    // Update is called once per frame
    void Update()
    {
        anim.SetFloat("Thumb", TouchpadTouch.action.ReadValue<float>());
        anim.SetFloat("Index", TriggerPress.action.ReadValue<float>());
        anim.SetFloat("Middle", GripPress.action.ReadValue<float>());
        anim.SetFloat("Ring", GripPress.action.ReadValue<float>());
        anim.SetFloat("Pinky", GripPress.action.ReadValue<float>());
        if (IgnoreExtra) return;
        x = GripPress.action.ReadValue<float>();
        if (x >= 0.75f && !hasGripped)
        {
            hasGripped = true;
            if (!Grabbing && handpalm != null)
            {
                if (handpalm.currentcols.Count > 0)
                {
                    if (handpalm.currentcols[0] != null)
                    {
                        Debug.Log("TestA");
                        AttachObject(handpalm.currentcols[0].gameObject);
                    }
                }


            }
        }
        if (x < 0.75f && hasGripped)
        {
            hasGripped = false;
            if (Grabbing && CurrentAttachedObject != null)
            {
                DetachObject(CurrentAttachedObject);
            }
        }

        if (Grabbing && CurrentAttachedObject != null)
        {
            HandMesh.SetActive(false);
            FingerCollider.enabled = false;
            PalmCollider.enabled = false;
            CurrentAtachObjectInteractable.OnHeld.Invoke();

            if (LeftHand)
            {
                if (!CurrentAtachObjectInteractable.dontMove)
                {
                    initalRotOFfset = Quaternion.Inverse(CurrentAtachObjectInteractable.LeftPos.transform.rotation) * CurrentAttachedObject.transform.rotation;
                    CurrentAttachedObject.transform.rotation = AttachPoint.rotation * initalRotOFfset;

                    Vector3 posDiff = CurrentAttachedObject.transform.position - CurrentAtachObjectInteractable.LeftPos.transform.position;
                    CurrentAttachedObject.transform.position = AttachPoint.position + posDiff;
                }
            }
            else
            {
                if (!CurrentAtachObjectInteractable.dontMove)
                {
                    initalRotOFfset = Quaternion.Inverse(CurrentAtachObjectInteractable.RightPos.transform.rotation) * CurrentAttachedObject.transform.rotation;
                    CurrentAttachedObject.transform.rotation = AttachPoint.rotation * initalRotOFfset;

                    Vector3 posDiff = CurrentAttachedObject.transform.position - CurrentAtachObjectInteractable.RightPos.transform.position;
                    CurrentAttachedObject.transform.position = AttachPoint.position + posDiff;
                }

            }
        }
        else
        {
            HandMesh.SetActive(true);
            FingerCollider.enabled = true;
            PalmCollider.enabled = true;
        }

    }
    public Rigidbody currentRB;
    public Transform AttachPoint;

    public bool Grabbing;
    public GameObject CurrentAttachedObject;
    public QuestInteractable CurrentAtachObjectInteractable;

    public Vector3 initialPosOffset;
    public Quaternion initalRotOFfset;
    public void AttachObject(GameObject attachObj)
    {
        Debug.Log("TestB");
        CurrentAttachedObject = attachObj;
        Debug.Log("TestC");
        CurrentAtachObjectInteractable = attachObj.GetComponent<QuestInteractable>();
        Debug.Log("TestD");
        if (CurrentAtachObjectInteractable.AttachedToHand != null)
        {
            CurrentAtachObjectInteractable.AttachedToHand.DetachObject(CurrentAtachObjectInteractable.gameObject);
        }
        Debug.Log("TestE");
        CurrentAtachObjectInteractable.OnGrab.Invoke();
        Debug.Log("TestF");
        currentRB = CurrentAtachObjectInteractable.rb;
        Debug.Log("TestG");
        CurrentAtachObjectInteractable.AttachedToHand = this;
        Debug.Log("TestH");
        if (LeftHand)
        {
            if (!CurrentAtachObjectInteractable.dontMove)
            {
                initalRotOFfset = Quaternion.Inverse(CurrentAtachObjectInteractable.LeftPos.transform.rotation) * attachObj.transform.rotation;
                attachObj.transform.rotation = AttachPoint.rotation * initalRotOFfset;

                Vector3 posDiff = attachObj.transform.position - CurrentAtachObjectInteractable.LeftPos.transform.position;
                attachObj.transform.position = AttachPoint.position + posDiff;
            }
        }
        else
        {
            if (!CurrentAtachObjectInteractable.dontMove)
            {
                initalRotOFfset = Quaternion.Inverse(CurrentAtachObjectInteractable.RightPos.transform.rotation) * attachObj.transform.rotation;
                attachObj.transform.rotation = AttachPoint.rotation * initalRotOFfset;

                Vector3 posDiff = attachObj.transform.position - CurrentAtachObjectInteractable.RightPos.transform.position;
                attachObj.transform.position = AttachPoint.position + posDiff;
            }
        }

        Grabbing = true;
        handpalm.ClearList();
        HandMesh.SetActive(!true);
        FingerCollider.enabled = !true;
        PalmCollider.enabled = !true;

    }
    public void DetachObject(GameObject DetachObj)
    {
        if (CurrentAttachedObject != null)
        {
            CurrentAtachObjectInteractable.AttachedToHand = null;
            CurrentAtachObjectInteractable.OnLetGo.Invoke();
            if (CurrentAtachObjectInteractable.Throw)
            {
                currentRB.linearVelocity = transform.parent.parent.TransformVector(Velocity.action.ReadValue<Vector3>());
                currentRB.angularVelocity = transform.parent.parent.TransformVector(AngularVelocity.action.ReadValue<Vector3>());
                Debug.Log(currentRB.linearVelocity + "Throw");
            }
            CurrentAtachObjectInteractable = null;
            currentRB = null;
            Grabbing = false;
            CurrentAttachedObject = null;
            HandMesh.SetActive(true);
            FingerCollider.enabled = true;
            PalmCollider.enabled = true;
        }

    }
}
