using System.IO;
using UnityEngine;

public class ScoreFileManager : MonoBehaviour
{
    public static ScoreFileManager Instance { get; private set; }
    
    public int Score { get; private set; }

    private const string fileName = "snakeSaveFile";
    private string fullFileName;
    private void Awake()
    {
        Instance = this;
        fullFileName = Path.Combine(Application.persistentDataPath, fileName);

        if (File.Exists(fullFileName))
        {
            using (BinaryReader reader = new BinaryReader(File.Open(fullFileName, FileMode.Open)))
            {
                Score = reader.ReadInt32();
            }    
        }
        else
        {
            using (BinaryWriter writer = new BinaryWriter(File.Open(fullFileName, FileMode.Create)))
            {
                writer.Write(0);
            }

            Score = 0;
        }
    }
    
    public void SaveScore(int score)
    {
        Score = score;
        using (BinaryWriter writer = new BinaryWriter(File.Open(fullFileName, FileMode.Open)))
        {
            writer.Write(score);
        }
    }

    public void DeleteSave()
    {
        Score = 0;
        using (BinaryWriter writer = new BinaryWriter(File.Open(fullFileName, FileMode.Open)))
        {
            writer.Write(0);
        }
    }
    
}
