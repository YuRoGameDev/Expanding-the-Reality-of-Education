using UnityEngine;

public class Ball : MonoBehaviour
{
    public GameObject BallObj;
    public AudioSource TargetHit, OtherHit;
    private void Start()
    {
        SetBall(false);
    }
    public void SetBall(bool Status)
    {
        BallObj.SetActive(Status);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Target")
        {
            other.gameObject.SetActive(false);
            PhysicsLvl.Instance.TargetHit();
            TargetHit.Play();
        }
        else
        {
            OtherHit.Play();
        }
        CannonCtrl.Instance.isShot = false;
        SetBall(false);
    }
}
