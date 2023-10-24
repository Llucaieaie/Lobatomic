using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpManager : MonoBehaviour
{
    [HideInInspector]
    public bool active;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void NewPowerUp()
    {
        PowerUps powerUp = (PowerUps)Random.Range(0, 4);

        switch (powerUp)
        {
            case PowerUps.UNKNOWN:
                break;
            case PowerUps.FRENESI:
                break;
            case PowerUps.LAB_GOGGLES:
                break;
            case PowerUps.DOUBLE_POINTS:
                break;
            case PowerUps.STOP_TIME:
                break;
            default:
                break;
        }
    }
}
