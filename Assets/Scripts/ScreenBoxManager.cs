using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenBoxManager : MonoBehaviour
{
    public static ScreenBoxManager instance;
    public List<ScreenBox> screenBoxes = new List<ScreenBox>();
    [SerializeField] ScreenBox currentBox;
    bool canChange = true;
    private void Awake()
    {
        ScreenBoxManager.instance = this;

    }

    void Start()
    {
        EnableBox(0);
    }

    public void EnableBox(int val)
    {
        if (canChange)
        {
            if (currentBox != screenBoxes[val] && (currentBox == null || !currentBox.isOpen || currentBox.isClosing))
            {
                currentBox = screenBoxes[val];
                currentBox.SetScreen(true);
            }
            StartCoroutine(canChangeDelay());
        }

        IEnumerator canChangeDelay()
        {
            canChange = false;
            yield return new WaitForSeconds(0.5f);
            canChange = true;
        }
    }

    public void DisableBox()
    {
        if (currentBox != null)
        {
            Debug.Log("Close");
            currentBox.SetScreen(false);
            currentBox = null;
        }
    }
}
