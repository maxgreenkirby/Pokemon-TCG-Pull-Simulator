using System;
using System.IO;
using UnityEditor;
using UnityEngine;

public class CVSConverter
{
    private static string _cvsPath = "/Data/Cards.csv";

    // 0 = ID, 1 = Name, 2 = HP, 3 = Rarity, 4 = EffectType, 5 = Sprite Name
    [MenuItem("Generate/Generate Cards")]
    public static void GenerateCards()
    {
        string[] allLines = File.ReadAllLines(Application.dataPath + _cvsPath);

        // Skip the first line (headers)
        allLines = allLines[1..];
        
        string directoryPath = "Assets/Scriptable Objects/Cards/";

        if (Directory.Exists(directoryPath) == false)
        {
            Directory.CreateDirectory(directoryPath);
        }
        
        foreach (string line in allLines)
        {
            string[] values = line.Split(',');

            Card card = ScriptableObject.CreateInstance<Card>();

            // Check if the card already exists
            Card existingCard = AssetDatabase.LoadAssetAtPath<Card>($"{directoryPath}{values[0]}.asset");
            if (existingCard != null)
            {
                existingCard.ID = int.Parse(values[0]);
                existingCard.Name = values[1];
                existingCard.HP = int.Parse(values[2]);
                existingCard.Rarity = (ERarity)Enum.Parse(typeof(ERarity), values[3]);
                existingCard.EffectType = (EEffectType)Enum.Parse(typeof(EEffectType), values[4]);
                existingCard.Sprite = AssetDatabase.LoadAssetAtPath<Sprite>($"Assets/Sprites/Card Sprites/VFS/{values[5]}.jpg");

                EditorUtility.SetDirty(existingCard);

                continue;
            }

            // Create new card
            card.ID = int.Parse(values[0]);
            card.Name = values[1];
            card.HP = int.Parse(values[2]);
            card.Rarity = (ERarity)Enum.Parse(typeof(ERarity), values[3]);
            card.EffectType = (EEffectType)Enum.Parse(typeof(EEffectType), values[4]);
            card.Sprite = AssetDatabase.LoadAssetAtPath<Sprite>($"Assets/Sprites/Card Sprites/VFS/{values[5]}.jpg");

            AssetDatabase.CreateAsset(card, $"{directoryPath}{card.ID}.asset");
        }
    }
}
