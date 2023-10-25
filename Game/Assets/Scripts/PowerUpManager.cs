using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    public Camera cam;

    [Range(0, 100)] public int appearPercentage;

    [Range(1, 20)] public float FrenesiTime;
    [Range(1, 20)] public float LabGlassesTime;
    [Range(1, 20)] public float DoublePointsTime;
    [Range(1, 20)] public float StopTimeTime;

    [SerializeField] private bool activeFrenesi;
    [SerializeField] private bool activeLabG;
    [SerializeField] private bool activeDoubleP;
    [SerializeField] private bool activeStopT;


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

    private IEnumerator ShowInUI(PowerUps type)
    {

        yield return new WaitForEndOfFrame();
    }

    private IEnumerator ApplyFrenesi(float time)
    {
        Debug.Log("Pop Frenesi");

        activeFrenesi = true;

        StartCoroutine(cam.GetComponent<CameraManager>().ChangeZoom(4, 0.5f));

        float auxSpeed = PlayerMovement.maxSpeed;
        PlayerMovement.maxSpeed += PlayerMovement.maxSpeed;

        float auxCd = PlayerWeapon.attackCoolDown;
        PlayerWeapon.attackCoolDown = 0.1f;

        yield return new WaitForSeconds(time);

        PlayerMovement.maxSpeed = auxSpeed;
        PlayerWeapon.attackCoolDown = auxCd;

        StartCoroutine(cam.GetComponent<CameraManager>().ChangeZoom(cam.GetComponent<CameraManager>().DefaultSize, 0.5f));

        activeFrenesi = false;
    }
    private IEnumerator ApplyLabG(float time)
    {
        Debug.Log("Pop LabGoogles");
        
        activeLabG = true;
        yield return new WaitForSeconds(time);
        activeLabG = false;
    }
    private IEnumerator ApplyDoubleP(float time)
    {
        Debug.Log("Pop DoublePoints");

        activeDoubleP = true;
        yield return new WaitForSeconds(time);
        activeDoubleP = false;
    }
    private IEnumerator ApplyStopT(float time)
    {
        Debug.Log("Pop StopTime");

        activeStopT = true;
        yield return new WaitForSeconds(time);
        activeStopT = false;
    }
}