using Assets;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using UnityEngine;

public class LearningDataManager : MonoBehaviour
{
    #region fields

    private List<LearningData> dataList;

    #endregion

    #region start // update

    private void Start()
    {
        dataList = new List<LearningData>();
    }

    #endregion

    #region methods

    /// <summary>
    /// Add gathered data to the list.
    /// </summary>
    /// <param name="gatheredData">The gathered data.</param>
    void AddData(LearningData gatheredData)
    {
        dataList.Add(gatheredData);
    }

    /// <summary>
    /// Save the gathered data to the file system after a learning session.
    /// </summary>
    void SaveGatheredData()
    {
        SaveData(dataList, "PongLearningData");

        string csv = ToCsv(", ", dataList);
        File.WriteAllText(@"E:\data.csv", csv);
    }

    public static string ToCsv<T>(string separator, IEnumerable<T> objectlist)
    {
        Type t = typeof(T);
        PropertyInfo[] fields = t.GetProperties();

        string header = String.Join(separator, fields.Select(f => f.Name).ToArray());

        StringBuilder csvdata = new StringBuilder();
        csvdata.AppendLine(header);

        foreach (var o in objectlist)
            csvdata.AppendLine(ToCsvFields(separator, fields, o));

        return csvdata.ToString();
    }

    public static string ToCsvFields(string separator, PropertyInfo[] fields, object o)
    {
        StringBuilder linie = new StringBuilder();

        foreach (var f in fields)
        {
            if (linie.Length > 0)
                linie.Append(separator);

            var x = f.GetValue(o, null);

            if (x != null)
                linie.Append(x.ToString());
        }

        return linie.ToString();
    }

    /// <summary>
    /// Load the gathered data from the file system.
    /// </summary>
    void LoadGatheredData()
    {
        object loadedData = LoadData("PongLearningData");

        if (loadedData != null)
        {
            dataList = loadedData as List<LearningData>;
        }
    }

    /// <summary>
    /// Serialize an object to the devices File System.
    /// </summary>
    /// <param name="objectToSave">The Object that will be Serialized.</param>
    /// <param name="fileName">Name of the file to be Serialized.</param>
    public void SaveData(object objectToSave, string fileName)
    {
        // Add the File Path together with the files name and extension.
        // We will use .bin to represent that this is a Binary file.
        string FullFilePath = Application.persistentDataPath + "/" + fileName + ".bin";
        // We must create a new Formattwr to Serialize with.
        BinaryFormatter Formatter = new BinaryFormatter();
        // Create a streaming path to our new file location.
        FileStream fileStream = new FileStream(FullFilePath, FileMode.Create, FileAccess.Write, FileShare.None);
        // Serialize the objedt to the File Stream
        Formatter.Serialize(fileStream, objectToSave);
        // FInally Close the FileStream and let the rest wrap itself up.
        fileStream.Close();
    }

    /// <summary>
    /// Deserialize an object from the FileSystem.
    /// </summary>
    /// <param name="fileName">Name of the file to deserialize.</param>
    /// <returns>Deserialized Object</returns>
    public object LoadData(string fileName)
    {
        string FullFilePath = Application.persistentDataPath + "/" + fileName + ".bin";
        Debug.Log(FullFilePath);
        // Check if our file exists, if it does not, just return a null object.
        if (File.Exists(FullFilePath))
        {
            BinaryFormatter Formatter = new BinaryFormatter();
            FileStream fileStream = new FileStream(FullFilePath, FileMode.Open);
            object obj = Formatter.Deserialize(fileStream);
            fileStream.Close();
            // Return the uncast untyped object.
            return obj;
        }
        else
        {
            return null;
        }
    }

    public static object LoadAICoefficients()
    {
        string fileName = "FunctionCoefficients";
        string FullFilePath = Application.persistentDataPath + "/" + fileName + ".bin";
        Debug.Log(FullFilePath);
        // Check if our file exists, if it does not, just return a null object.
        if (File.Exists(FullFilePath))
        {
            BinaryFormatter Formatter = new BinaryFormatter();
            FileStream fileStream = new FileStream(FullFilePath, FileMode.Open);
            object obj = Formatter.Deserialize(fileStream);
            fileStream.Close();
            // Return the uncast untyped object.
            return obj;
        }
        else
        {
            return null;
        }
    }

    #endregion
}
