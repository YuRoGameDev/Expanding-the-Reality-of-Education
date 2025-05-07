using UnityEngine;
using UnityEngine.UI;
public class CannonCtrl : MonoBehaviour
{
    public static CannonCtrl Instance;
    public Ball ballComp;
    [SerializeField] Transform YawBody, PitchBody, ShootPoint, Ball;

    public float YTurn, XTurn, Gravity, WindResistance, Power, Scale = 0.2368016f;
    [SerializeField] Vector4 Clamps;
    public bool isShot;
    Vector3 velocity;
    [SerializeField] TrailRenderer ballLine;
    [SerializeField] Slider ForceSlider, GravitySlider, ResistanceSlider;
    public AudioSource Fire;
    private void Awake()
    {
        CannonCtrl.Instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        YTurn = Mathf.Clamp(YTurn, Clamps.x, Clamps.y);
        XTurn = Mathf.Clamp(XTurn, Clamps.z, Clamps.w);

        YawBody.localRotation = Quaternion.Euler(0f, YTurn, 0f);
        PitchBody.localRotation = Quaternion.Euler(XTurn, 0f, 0f);

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Shoot();
        }


        if (isShot)
        {
            velocity += Vector3.down * Gravity * Time.deltaTime;

            velocity *= (1f - WindResistance * Time.deltaTime);

            Ball.position += velocity * Time.deltaTime;
            Scale += Time.deltaTime * 2;
        }
        Scale = Mathf.Clamp(Scale, 0.2368016f, 2f);
        Ball.localScale = Vector3.one * Scale;

        var keys = ballLine.widthCurve.keys;
        Debug.Log(keys[0].value);
        keys[0].value = Scale;
        keys[1].value = 0;
        ballLine.widthCurve.keys = keys;

        ForceSlider.value = Power;
        GravitySlider.value = Gravity;
        ResistanceSlider.value = WindResistance;
    }

    public void Shoot()
    {
        Fire.Play();
        ballLine.enabled = false;
        ballLine.Clear();
        ballLine.enabled = true;
        ballComp.SetBall(true);
        Ball.position = ShootPoint.position;
        velocity = ShootPoint.forward * Power;
        Scale = 0.2368016f;
        isShot = true;
        Ball.gameObject.SetActive(true);

    }
}
