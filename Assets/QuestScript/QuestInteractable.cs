using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class QuestInteractable : MonoBehaviour
{
    public UnityEvent OnGrab, OnHeld, OnLetGo;
    public Transform LeftPos, RightPos;
    public Rigidbody rb;
    public QuestHand AttachedToHand;
    public bool Throw, dontMove, addListener = true;

    void Start()
    {
        if (LeftPos == null)
        {
            LeftPos = this.transform;
        }
        if (RightPos == null)
        {
            RightPos = this.transform;
        }
        rb = GetComponent<Rigidbody>();
        if (addListener)
        {
            OnGrab.AddListener(Grab);
            OnLetGo.AddListener(LetGo);
        }

    }

    void Grab()
    {
        rb.useGravity = false;
    }

    void LetGo()
    {
        rb.useGravity = true;
    }
}
