using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneratePatient : MonoBehaviour
{
    public Texture2D[] baseLayerM;
    public Texture2D[] hairLayerM;
    public Texture2D[] baseLayerF;
    public Texture2D[] hairLayerF;
    public Texture2D[] expressionLayer;
    public SpriteRenderer hairRenderer;
    public SpriteRenderer baseRenderer;
    public SpriteRenderer expressionRenderer;

    Rect size;

    public bool genero;

    void Start()
    {
        Rect size = new Rect(0, 0, baseLayerM[0].width, baseLayerM[0].height);
        
        genero = Random.Range(0, 2) == 0;
        if(!genero) //hombre
        {
            Sprite baseSprite = Sprite.Create(baseLayerM[Random.Range(0, baseLayerM.Length)], size, Vector2.zero);
            baseRenderer.sprite = baseSprite;

            Sprite hairSprite = Sprite.Create(hairLayerM[Random.Range(0, hairLayerM.Length)], size, Vector2.zero);
            hairRenderer.sprite = hairSprite;
            hairRenderer.sortingOrder = 2;
        }
        else //mujer
        {
            Sprite baseSprite = Sprite.Create(baseLayerF[Random.Range(0, baseLayerF.Length)], size, Vector2.zero);
            baseRenderer.sprite = baseSprite;

            Sprite hairSprite = Sprite.Create(hairLayerF[Random.Range(0, hairLayerF.Length)], size, Vector2.zero);
            hairRenderer.sprite = hairSprite;
            hairRenderer.sortingOrder = 0; 
        }
        //expressionRenderer.sprite = Sprite.Create(expressionLayer[1], size, Vector2.zero);
    }

    public void UpdateExpression(float current)
    {
        Rect size = new Rect(0, 0, expressionLayer[0].width, expressionLayer[0].height);
        if (current > 80)
        {
            expressionRenderer.sprite = Sprite.Create(expressionLayer[0], size, Vector2.zero);
            Debug.Log("contento");
        }
        else if (current >= 30)
        {
            expressionRenderer.sprite = Sprite.Create(expressionLayer[1], size, Vector2.zero);
            Debug.Log("normal");
        }
        else
        {
            expressionRenderer.sprite = Sprite.Create(expressionLayer[2], size, Vector2.zero);
            Debug.Log("loco");
        }
    }
}