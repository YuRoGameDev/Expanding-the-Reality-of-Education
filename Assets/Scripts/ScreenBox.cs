using UnityEngine;

public class ScreenBox : MonoBehaviour
{
    Animator Box;
    public bool isOpen { get; private set; }
    public bool isClosing { get; private set; }
    private void Awake()
    {
        isOpen = false;
        Box = GetComponent<Animator>();
        transform.GetChild(0).gameObject.SetActive(false);
    }

    public void SetScreen(bool val)
    {
        if (isOpen && !val)
        {
            isClosing = true;
        }
        if (!isOpen && val)
        {
            isClosing = false;
        }
        Box.Play(val ? "Open" : "Close", 0, 0.0f);
    }

    public void setStatus(int status)
    {
        if (isOpen && status == 1)
        {
            isClosing = false;
        }
        isOpen = status == 1 ? true : false;
    }
}
