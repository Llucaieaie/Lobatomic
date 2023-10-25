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

    public bool genero;

    public int newSortingOrder = 0;

    void Start()
    {
        genero = Random.Range(0, 2) == 0;
        
        Rect size = new Rect(0, 0, baseLayerM[0].width, baseLayerM[0].height);

        if(!genero) //hombre
        {
            Sprite baseSprite = Sprite.Create(baseLayerM[Random.Range(0, baseLayerM.Length)], new Rect(0, 0, size.width, size.height), Vector2.zero);
            baseRenderer.sprite = baseSprite;

            Sprite hairSprite = Sprite.Create(hairLayerM[Random.Range(0, hairLayerM.Length)], new Rect(0, 0, size.width, size.height), Vector2.zero);
            hairRenderer.sprite = hairSprite;
            hairRenderer.sortingOrder = 2;

            Sprite expressionSprite = Sprite.Create(expressionLayer[Random.Range(0, expressionLayer.Length)], new Rect(0, 0, size.width, size.height), Vector2.zero);
            expressionRenderer.sprite = expressionSprite;

        }
        else //mujer
        {
            Sprite baseSprite = Sprite.Create(baseLayerF[Random.Range(0, baseLayerF.Length)], new Rect(0, 0, size.width, size.height), Vector2.zero);
            baseRenderer.sprite = baseSprite;

            Sprite hairSprite = Sprite.Create(hairLayerF[Random.Range(0, hairLayerF.Length)], new Rect(0, 0, size.width, size.height), Vector2.zero);
            hairRenderer.sprite = hairSprite;
            hairRenderer.sortingOrder = 0; 

            Sprite expressionSprite = Sprite.Create(expressionLayer[Random.Range(0, expressionLayer.Length)], new Rect(0, 0, size.width, size.height), Vector2.zero);
            expressionRenderer.sprite = expressionSprite;
        }

    }
}