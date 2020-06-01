using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Assertions;
using UnityEngine.U2D;
using UnityEditor;

public class SpriteData : MonoBehaviour
{

    public SpriteAtlas atlas;
    private string path = "Sprites/fonts/white_undertale_font2";

    char[] chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789!?#,.:-".ToCharArray();

    private Dictionary<char, CharData> spriteDictionary;

    private Sprite[] subsprites;
    private SpriteMetaData[] spritesheet;

    public struct CharData
    {
        public float width;
        public float offsetX; //in world units
        public float offsetY; //in world units
        public Sprite sprite;

        public CharData(float width, float offsetX, float offsetY, Sprite sprite)
        {
            this.width = width;
            this.offsetX = offsetX;
            this.offsetY = offsetY;
            this.sprite = sprite;
        }
    }

    public void Awake()
    {
        subsprites = new Sprite[chars.Length];
        
        GetSubsprites();
        GetSpriteData();
    }

    public CharData? GetSprite(char c)
    {
        if (!spriteDictionary.ContainsKey(c)) return null;
        return spriteDictionary[c];
    }

    private void GetSubsprites()
    {
        atlas.GetSprites(subsprites);
    }

    private void GetSpriteData()
    {
        int charIndex = 0;

        spriteDictionary = new Dictionary<char, CharData>();

        Assert.IsNotNull(subsprites);

        Texture2D fontTexture = Resources.Load(path) as Texture2D;
        string texturePath = AssetDatabase.GetAssetPath(fontTexture);
       TextureImporter importer = AssetImporter.GetAtPath(texturePath) as TextureImporter;
        importer.isReadable = true;
        spritesheet = importer.spritesheet;

        Assert.AreEqual<int>(importer.spritesheet.Length, chars.Length, "EXCEPTION: sprite meta data length != chars length at SpriteData.cs");
        Assert.AreEqual<int>(subsprites.Length, chars.Length, "EXCEPTION: subsprite array length != chars length at SpriteData.cs");

        for (int i = 0; i < importer.spritesheet.Length; i++)
        {
            SpriteMetaData meta = importer.spritesheet[i];

            string letter = chars[i].ToString();
            if (letter.Equals("?")) letter = "questionmark";
            if (letter.Equals(":")) letter = "colon";

            Sprite subsprite = atlas.GetSprite(letter);

            int x = 0;
            int y = 0;

            int height = (int) meta.rect.height;
            int width = (int) meta.rect.width;

            int minX = 0;
            int maxX = width;

            bool found = false;

            while (x <= maxX && !found)
            {
                for (y = 0; y < height; y++)
                {
                    Color color = subsprite.texture.GetPixel(x, y);
                    if (color.a != 0)
                    {
                        minX = x;
                        found = true;
                        break;
                    }
                }
                ++x;
            }

            x = maxX;
            found = false;
            while (x >= minX && !found)
            {
                for (y = 0; y < height; y++)
                {
                    Color color = subsprite.texture.GetPixel(x, y);
                    if (color.a != 0)
                    {
                        maxX = x;
                        found = true;
                        break;
                    }
                }
                --x;
            }

            int minY = 0;
            int maxY = height;

            y = 0;
            found = false;
            while (y <= maxY && !found)
            {
                for (x = 0; x < width; x++)
                {
                    Color color = subsprite.texture.GetPixel(x, y);
                    if (color.a != 0)
                    {
                        minY = y;
                        found = true;
                        break; 
                    }
                }
                ++y;
            }

            float pivot_x = meta.pivot.x * subsprite.pixelsPerUnit;
            float pivot_y = meta.pivot.y * subsprite.pixelsPerUnit;

            float offsetX = pivot_x - minX;
            float offsetY = pivot_y - Math.Max(pivot_y, minY);

            offsetX /= subsprite.pixelsPerUnit;
            offsetY /= subsprite.pixelsPerUnit;

            float charWidth = (maxX - minX + 1) / subsprite.pixelsPerUnit;

            if (charWidth <= width)
            {
                CharData cd = new CharData(charWidth, offsetX, offsetY, subsprite);
                spriteDictionary.Add(chars[i], cd);
            }

            Vector2 offsets = new Vector2(offsetX, offsetY);
           /* Debug.Log("data of " + chars[i] + ": pivot_x = " + meta.pivot.x * subsprite.pixelsPerUnit + ", pivot_y = " + meta.pivot.y * subsprite.pixelsPerUnit);
            Debug.Log("pivot_x = " + pivot_x + ", pivot_y = " + pivot_y);

            Debug.Log("offsets : " + offsets);
            Debug.Log("minX : " + minX + ", maxX : " + maxX);
            Debug.Log("minY : " + minY + ", maxY : " + maxY);

            Debug.Log("height : " + meta.rect.height + ", width : " + meta.rect.width);*/


        }

    }


    
}
