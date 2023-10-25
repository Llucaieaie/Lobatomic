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

    public void NewPowerUp()
    {
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
                        PowerUpFeedback();
                        StartCoroutine(ApplyFrenesi(FrenesiTime));
                    }
                    break;
                case PowerUps.LAB_GOOGLES: // -------------------------------------------
                    if (!activeLabG)
                    {
                        PowerUpFeedback();
                        StartCoroutine(ApplyLabG(LabGlassesTime));
                    }
                    break;
                case PowerUps.DOUBLE_POINTS: // -----------------------------------------
                    if (!activeDoubleP)
                    {
                        PowerUpFeedback();
                        StartCoroutine(ApplyDoubleP(DoublePointsTime));
                    }
                    break;
                case PowerUps.STOP_TIME: // ---------------------------------------------
                    if (!activeStopT)
                    {
                        PowerUpFeedback();
                        StartCoroutine(ApplyStopT(StopTimeTime));
                    }
                    break;
                default:
                    break;
            }
        }
    }

    private void PowerUpFeedback()
    {
        //Should give some visual and sound feedback to the player
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

        activeStopT = true;
        powerUpIcons[3].enabled = true;

        float auxTimeScale = Time.timeScale;
        Time.timeScale = Time.timeScale / 3;

        yield return new WaitForSecondsRealtime(time);

        Time.timeScale = auxTimeScale;

        powerUpIcons[3].enabled = false;
        activeStopT = false;
    }
}