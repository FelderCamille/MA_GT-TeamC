using System.IO;
using UnityEngine;

namespace Utils
{
    public static class JsonUtils<T>
    {

        public static T Read(string jsonPath)
        {
            var jsonFile = Resources.Load<TextAsset>(jsonPath);
            if (jsonFile == null)
            {
                throw new FileNotFoundException(jsonPath + " not found");
            }
            return JsonUtility.FromJson<T>(jsonFile.text);
        }
    }
}