using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class CsvWriter : MonoBehaviour
{
    public string dirPath;

    private string fileName;
    private StreamWriter stream;

    public void Open(string filename)
    {
        fileName = filename;
    }

    public void WriteCsv(string[] datas)
    {
        stream = new StreamWriter(Application.dataPath + dirPath + fileName, true);
        string result = "";
        for (int i = 0; i < datas.Length; i++)
        {
            result += datas[i];
            if (i < datas.Length - 1) result += ",";
            else result += "\n";
        }

        stream.Write(result);
        stream.Close();
        stream.Dispose();
    }
}
