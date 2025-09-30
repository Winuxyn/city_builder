using UnityEngine;
using UnityEditor;
using System.IO;

public class CreatePrefabsFromSprites : EditorWindow
{
    private GameObject basePrefab;
    private string spriteFolder = "Assets/Sprites";
    private string prefabOutputFolder = "Assets/Prefabs";

    [MenuItem("Tools/Create Prefabs from Sprites")]
    public static void ShowWindow()
    {
        GetWindow<CreatePrefabsFromSprites>("Create Prefabs");
    }

    void OnGUI()
    {
        GUILayout.Label("Prefab Creation Settings", EditorStyles.boldLabel);
        basePrefab = (GameObject)EditorGUILayout.ObjectField("Base Prefab", basePrefab, typeof(GameObject), false);
        spriteFolder = EditorGUILayout.TextField("Sprite Folder", spriteFolder);
        prefabOutputFolder = EditorGUILayout.TextField("Prefab Output Folder", prefabOutputFolder);

        if (GUILayout.Button("Create Prefabs"))
        {
            CreatePrefabs();
        }
    }

    void CreatePrefabs()
    {
        if (basePrefab == null)
        {
            EditorUtility.DisplayDialog("Error", "Please assign a base prefab.", "OK");
            return;
        }

        if (!Directory.Exists(spriteFolder))
        {
            EditorUtility.DisplayDialog("Error", "Sprite folder does not exist.", "OK");
            return;
        }

        Directory.CreateDirectory(prefabOutputFolder);

        string[] spriteGuids = AssetDatabase.FindAssets("t:Sprite", new[] { spriteFolder });
        foreach (string guid in spriteGuids)
        {
            string spritePath = AssetDatabase.GUIDToAssetPath(guid);
            Sprite sprite = AssetDatabase.LoadAssetAtPath<Sprite>(spritePath);
            if (sprite != null)
            {
                GameObject newPrefab = PrefabUtility.InstantiatePrefab(basePrefab) as GameObject;
                newPrefab.name = sprite.name;

                SpriteRenderer sr = newPrefab.GetComponentInChildren<SpriteRenderer>();
                if (sr != null)
                {
                    sr.sprite = sprite;
                    sr.size = new Vector2(sprite.rect.width / sprite.pixelsPerUnit, sprite.rect.height / sprite.pixelsPerUnit);
                }

                Building buildingScript = newPrefab.GetComponentInChildren<Building>();
                if (buildingScript != null)
                {
                    int width = Mathf.RoundToInt(sprite.rect.width / sprite.pixelsPerUnit);
                    int height = Mathf.RoundToInt((sprite.rect.height / sprite.pixelsPerUnit) * 2);
                    buildingScript.area.size = new Vector3Int(width, height, 1);
                    EditorUtility.SetDirty(buildingScript);
                }

                string prefabPath = Path.Combine(prefabOutputFolder, sprite.name + ".prefab");
                PrefabUtility.SaveAsPrefabAsset(newPrefab, prefabPath);
                DestroyImmediate(newPrefab);
            }
        }

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        EditorUtility.DisplayDialog("Success", "Prefabs created successfully!", "OK");
    }
}