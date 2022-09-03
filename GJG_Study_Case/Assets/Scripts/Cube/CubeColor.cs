using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeColor : MonoBehaviour
{
    public enum ColorType
    {
        BLUE,
        GREEN,
        PINK,
        PURPLE,
        RED,
        YELLOW
    };
    [SerializeField] private ControlValuesSO _controlValuesSo;
    [System.Serializable]
    public struct ColorSprite
    {
        public ColorType tileColor;
        public Sprite sprite;
    }
    public ColorSprite[] colorSprite;

    private ColorType cubeColors;

    private SpriteRenderer cubeImage;
    private Dictionary<ColorType, Sprite> colorSpriteDictionary;
    // Getters & Setters
    public ColorType CubeColors { get { return cubeColors; } set { SetColor(value); } }

    public int GetNumColors { get { return colorSprite.Length; } }

    private void Awake()
    {
        cubeImage = GetComponent<SpriteRenderer>();

        InitColorSpriteDictionary();
    }
    
    private void InitColorSpriteDictionary()
    {
        colorSpriteDictionary = new Dictionary<ColorType, Sprite>();

        // Loop through all the structs in our colorSprite array.
        for (int i = 0; i < colorSprite.Length; i++)
        {
            // Check that the dictionary does not already contain a key.
            if (!colorSpriteDictionary.ContainsKey(colorSprite[i].tileColor))
                // Add new key/value pair to our dictionary.
                colorSpriteDictionary.Add(colorSprite[i].tileColor, colorSprite[i].sprite);
        }
    }
    
    public void SetColor(ColorType newColor)
    {
        cubeColors = newColor;

        if (colorSpriteDictionary.ContainsKey(newColor))
        {
            ChangeCubeColor(newColor);
        }
    }
    
    public void ChangeCubeColor(ColorType cubeColor)
    {
        switch (cubeColor)
        {
            case ColorType.BLUE:
                cubeImage.sprite = _controlValuesSo.defaultImage[0];
                break;
            case ColorType.GREEN:
                cubeImage.sprite = _controlValuesSo.defaultImage[1];
                break;
            case ColorType.PINK:
                cubeImage.sprite = _controlValuesSo.defaultImage[2];
                break;
            case ColorType.PURPLE:
                cubeImage.sprite = _controlValuesSo.defaultImage[3];
                break;
            case ColorType.RED:
                cubeImage.sprite = _controlValuesSo.defaultImage[4];
                break;
            case ColorType.YELLOW:
                cubeImage.sprite = _controlValuesSo.defaultImage[5];
                break;
            default:
                break;
        }
    }
    public void ChangeCubeSpriteSecond(ColorType cubeColor)
    {
        switch (cubeColor)
        {
            case ColorType.BLUE:
                cubeImage.sprite = _controlValuesSo.secondImage[0];
                break;
            case ColorType.GREEN:
                cubeImage.sprite = _controlValuesSo.secondImage[1];
                break;
            case ColorType.PINK:
                cubeImage.sprite = _controlValuesSo.secondImage[2];
                break;
            case ColorType.PURPLE:
                cubeImage.sprite = _controlValuesSo.secondImage[3];
                break;
            case ColorType.RED:
                cubeImage.sprite = _controlValuesSo.secondImage[4];
                break;
            case ColorType.YELLOW:
                cubeImage.sprite = _controlValuesSo.secondImage[5];
                break;
            default:
                break;
        }
    }
    public void ChangeCubeSpriteThird(ColorType cubeColor)
    {
        switch (cubeColor)
        {
            case ColorType.BLUE:
                cubeImage.sprite = _controlValuesSo.thirdImage[0];
                break;
            case ColorType.GREEN:
                cubeImage.sprite = _controlValuesSo.thirdImage[1];
                break;
            case ColorType.PINK:
                cubeImage.sprite = _controlValuesSo.thirdImage[2];
                break;
            case ColorType.PURPLE:
                cubeImage.sprite = _controlValuesSo.thirdImage[3];
                break;
            case ColorType.RED:
                cubeImage.sprite = _controlValuesSo.thirdImage[4];
                break;
            case ColorType.YELLOW:
                cubeImage.sprite = _controlValuesSo.thirdImage[5];
                break;
            default:
                break;
        }
    }
    public void ChangeCubeSpriteFourth(ColorType cubeColor)
    {
        switch (cubeColor)
        {
            case ColorType.BLUE:
                cubeImage.sprite = _controlValuesSo.fourthImage[0];
                break;
            case ColorType.GREEN:
                cubeImage.sprite = _controlValuesSo.fourthImage[1];
                break;
            case ColorType.PINK:
                cubeImage.sprite = _controlValuesSo.fourthImage[2];
                break;
            case ColorType.PURPLE:
                cubeImage.sprite = _controlValuesSo.fourthImage[3];
                break;
            case ColorType.RED:
                cubeImage.sprite = _controlValuesSo.fourthImage[4];
                break;
            case ColorType.YELLOW:
                cubeImage.sprite = _controlValuesSo.fourthImage[5];
                break;
            default:
                break;
        }
    }
    
}
