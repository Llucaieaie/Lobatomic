using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum PowerUps
{
    UNKNOWN = -1,
    FRENESI,
    LAB_GOOGLES,
    DOUBLE_POINTS,
    STOP_TIME
}

public class PowerUpManager : MonoBehaviour
{
    public PlayerMovement PlayerMovement;
    public PlayerWeapon PlayerWeapon;
    public ScoreController ScoreController;
    public TimerController TimerController;
    public Camera cam;

    [Range(0, 100)] public int appearPercentage;

    [Range(1, 20)] public float FrenesiTime;
    [Range(1, 20)] public float LabGlassesTime;
    [Range(1, 20)] public float DoublePointsTime;
    [Range(1, 20)] public float StopTimeTime;

    [SerializeField] public bool activeFrenesi;
    [SerializeField] public bool activeLabG;
    [SerializeField] public bool activeDoubleP;
    [SerializeField] public bool activeStopT;

    public Image[] powerUpIcons;

    public AudioSource stopTAudio;
    public AudioSource powerUpAudio;


    public void NewPowerUp()
    {
        Debug.Log("Achus");

        if (Random.Range(0, 100) <= appearPercentage)
        {
            PowerUps powerUp = (PowerUps)Random.Range(0, 4);
            switch (powerUp)
            {
                case PowerUps.UNKNOWN:
                    break;
                case PowerUps.FRENESI: // -----------------------------------------------
                    if (!activeFrenesi)
                    {
                        powerUpAudio.Play();
                        StartCoroutine(ApplyFrenesi(FrenesiTime));
                    }
                    break;
                case PowerUps.LAB_GOOGLES: // -------------------------------------------
                    if (!activeLabG)
                    {
                        powerUpAudio.Play();
                        StartCoroutine(ApplyLabG(LabGlassesTime));
                    }
                    break;
                case PowerUps.DOUBLE_POINTS: // -----------------------------------------
                    if (!activeDoubleP)
                    {
                        powerUpAudio.Play();
                        StartCoroutine(ApplyDoubleP(DoublePointsTime));
                    }
                    break;
                case PowerUps.STOP_TIME: // ---------------------------------------------
                    if (!activeStopT)
                    {
                        powerUpAudio.Play();
                        StartCoroutine(ApplyStopT(StopTimeTime));
                    }
                    break;
                default:
                    break;
            }
        }
    }

    private IEnumerator ApplyFrenesi(float time)
    {
        Debug.Log("Pop Frenesi");

        activeFrenesi = true;
        powerUpIcons[0].enabled = true;

        StartCoroutine(cam.GetComponent<CameraManager>().ChangeZoom(cam.GetComponent<CameraManager>().CurrentSize - 1, 0.5f));

        float auxSpeed = PlayerMovement.maxSpeed;
        PlayerMovement.maxSpeed += PlayerMovement.maxSpeed;

        float auxAttTime = PlayerWeapon.attackingTime;
        PlayerWeapon.attackingTime = 0;
    
        float auxCd = PlayerWeapon.attackCoolDown;
        PlayerWeapon.attackCoolDown = 0;

        yield return new WaitForSecondsRealtime(time);

        PlayerMovement.maxSpeed = auxSpeed;
        PlayerWeapon.attackCoolDown = auxCd;
        PlayerWeapon.attackingTime = auxAttTime;

        if (activeLabG)
        {
            StartCoroutine(cam.GetComponent<CameraManager>().ChangeZoom(cam.GetComponent<CameraManager>().DefaultSize + 2.5f, 0.5f));
        }
        else
        {
            StartCoroutine(cam.GetComponent<CameraManager>().ChangeZoom(cam.GetComponent<CameraManager>().DefaultSize, 0.5f));
        }

        powerUpIcons[0].enabled = false;
        activeFrenesi = false;
    }
    private IEnumerator ApplyLabG(float time)
    {
        Debug.Log("Pop LabGoogles");
        
        activeLabG = true;
        powerUpIcons[1].enabled = true;

        StartCoroutine(cam.GetComponent<CameraManager>().ChangeZoom(cam.GetComponent<CameraManager>().CurrentSize + 2.5f, 0.5f));

        yield return new WaitForSecondsRealtime(time);

        if (activeFrenesi)
        {
            StartCoroutine(cam.GetComponent<CameraManager>().ChangeZoom(cam.GetComponent<CameraManager>().DefaultSize - 1, 0.5f));
        }
        else
        {
            StartCoroutine(cam.GetComponent<CameraManager>().ChangeZoom(cam.GetComponent<CameraManager>().DefaultSize, 0.5f));
        }

        powerUpIcons[1].enabled = false;
        activeLabG = false;
    }
    private IEnumerator ApplyDoubleP(float time)
    {
        Debug.Log("Pop DoublePoints");

        activeDoubleP = true;
        powerUpIcons[2].enabled = true;

        int auxScore = ScoreController.scoreMultiplier;
        ScoreController.scoreMultiplier += ScoreController.scoreMultiplier;

        yield return new WaitForSecondsRealtime(time);

        ScoreController.scoreMultiplier = auxScore;

        powerUpIcons[2].enabled = false;
        activeDoubleP = false;
    }
    private IEnumerator ApplyStopT(float time)
    {
        Debug.Log("Pop StopTime");

        stopTAudio.Play();

        activeStopT = true;
        powerUpIcons[3].enabled = true;

        TimerController.stopTime = true;
        
        yield return new WaitForSecondsRealtime(time);

        TimerController.stopTime = false;

        powerUpIcons[3].enabled = false;
        activeStopT = false;
    }
}