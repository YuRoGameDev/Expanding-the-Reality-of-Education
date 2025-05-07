using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestPalm : MonoBehaviour
{
    public List<Collider> currentcols = new List<Collider>();


    public void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Grabbable")
        {
            if (!currentcols.Contains(other))
            {
                currentcols.Add(other);
            }
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.tag == "Grabbable")
        {
            if (currentcols.Contains(other))
            {
                currentcols.Remove(other);
            }
        }
    }

    public void ClearList()
    {
        currentcols.Clear();
    }
}
