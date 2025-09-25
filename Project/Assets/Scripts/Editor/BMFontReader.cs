using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class BMFontEditor : EditorWindow
{
    [MenuItem("Tools/BMFont Maker")]
    static public void OpenBMFontMaker()
    {
        EditorWindow.GetWindow<BMFontEditor>(false, "BMFont Maker", true).Show();
    }

    [SerializeField]
    private Font targetFont;
    [SerializeField]
    private TextAsset fntData;
    [SerializeField]
    private Material fontMaterial;
    [SerializeField]
    private Texture2D fontTexture;

    public BMFontEditor()
    {
    }

    void OnGUI()
    {
        targetFont = EditorGUILayout.ObjectField("Target Font", targetFont, typeof(Font), false) as Font;
        fntData = EditorGUILayout.ObjectField("Fnt Data", fntData, typeof(TextAsset), false) as TextAsset;
        fontMaterial = EditorGUILayout.ObjectField("Font Material", fontMaterial, typeof(Material), false) as Material;
        fontTexture = EditorGUILayout.ObjectField("Font Texture", fontTexture, typeof(Texture2D), false) as Texture2D;

        if (GUILayout.Button("Create BMFont"))
        {
            targetFont.characterInfo = Load(fntData.bytes);
            if (fontMaterial)
            {
                fontMaterial.mainTexture = fontTexture;
            }
            targetFont.material = fontMaterial;
            fontMaterial.shader = Shader.Find("UI/Default");
            EditorUtility.SetDirty(targetFont);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            Debug.Log("create font <" + targetFont.name + "> success");
            //Close();
        }
    }

    static string GetString(string s)
    {
        int idx = s.IndexOf('=');
        return (idx == -1) ? "" : s.Substring(idx + 1);
    }

    /// <summary>
    /// Helper function that retrieves the integer value of the key=value pair.
    /// </summary>

    static int GetInt(string s)
    {
        int val = 0;
        string text = GetString(s);
#if UNITY_FLASH
		try { val = int.Parse(text); } catch (System.Exception) { }
#else
        int.TryParse(text, out val);
#endif
        return val;
    }

    /// <summary>
    /// Reload the font data.
    /// </summary>

    static public CharacterInfo [] Load(byte[] bytes)
    {
        List<CharacterInfo> cList = new List<CharacterInfo>();
        if (bytes != null)
        {
            ByteReader reader = new ByteReader(bytes);
            char[] separator = new char[] { ' ' };
            float texWidth = 1;
            float texHeight = 1;

            while (reader.canRead)
            {
                string line = reader.ReadLine();
                if (string.IsNullOrEmpty(line)) break;
                string[] split = line.Split(separator, System.StringSplitOptions.RemoveEmptyEntries);
                int len = split.Length;

                if (split[0] == "char")
                {
                    // Expected data style:
                    // char id=13 x=506 y=62 width=3 height=3 xoffset=-1 yoffset=50 xadvance=0 page=0 chnl=15

                    int channel = (len > 10) ? GetInt(split[10]) : 15;

                    if (len > 9 && GetInt(split[9]) > 0)
                    {
                        Debug.LogError("Your font was exported with more than one texture. Only one texture is supported by NGUI.\n" +
                            "You need to re-export your font, enlarging the texture's dimensions until everything fits into just one texture.");
                        break;
                    }

                        CharacterInfo info = new CharacterInfo();
                        int id = GetInt(split[1]);
                        float x = GetInt(split[2]);
                        float y = GetInt(split[3]);
                        float width = GetInt(split[4]);
                        float height = GetInt(split[5]);
                        int advance = GetInt(split[8]);
                        info.index = id;

                        float uvx = (float)x / (float)texWidth;
                        float uvy = 1 - (float)y / (float)texHeight;
                        float uvwidth = (float)width / (float)texWidth;
                        float uvheight = -1f * (float)height / (float)texHeight;

                        info.uvBottomLeft = new Vector2(uvx, uvy);
                        info.uvBottomRight = new Vector2(uvx + uvwidth, uvy);
                        info.uvTopRight = new Vector2(uvx + uvwidth, uvy + uvheight);
                        info.uvTopLeft = new Vector2(uvx, uvy + uvheight);
                        info.glyphWidth = (int)width;
                        info.bearing = 0;
                        info.glyphHeight = -(int)height;
                        info.advance = advance;

                        cList.Add(info);
                }
                else if (split[0] == "common")
                {
                    // Expected data style:
                    // common lineHeight=64 base=51 scaleW=512 scaleH=512 pages=1 packed=0 alphaChnl=1 redChnl=4 greenChnl=4 blueChnl=4

                    if (len > 5)
                    {
                        texWidth = GetInt(split[3]);
                        texHeight = GetInt(split[4]);
                    }
                    
                }
            }
        }
        return cList.ToArray();
    }
}