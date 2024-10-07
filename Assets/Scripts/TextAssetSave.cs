using System.IO;
using UnityEngine;

public class SaveFiles : MonoBehaviour
{
    void CreateText(string fileName)
    {
        //path
        string path = Application.dataPath + $"/{fileName}.txt";
        Debug.Log(path);
        //create file if file doesn't exist
        if (!File.Exists(path))
        {
            File.Create(path);
        }
        //content for file
        string content = $"Log Date/Time: {System.DateTime.Now}";
        //put content into file

    }
}
