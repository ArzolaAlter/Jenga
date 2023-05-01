using System.Collections;
using System.Collections.Generic;
using System.IO;
using Jenga.Main;
using Newtonsoft.Json;
using UnityEngine;

namespace Jenga.Persistence
{
    public class SaveManager : Singleton<SaveManager>
    {
        //------------------------------------------------------------------------------------//
        /*----------------------------------- FIELDS -----------------------------------------*/
        //------------------------------------------------------------------------------------//

        private const string EXTENSION = ".jenga";
        private const string STUDENT_CACHE_FILE_NAME = "STUDENT_CACHE";

        //------------------------------------------------------------------------------------//
        /*--------------------------------- PROPERTIES ---------------------------------------*/
        //------------------------------------------------------------------------------------//

        public bool HasCachedStudentData => HasFile(STUDENT_CACHE_FILE_NAME);

        public JengaData CachedStudentData
        {
            get
            {
                string filePath = GetFilePath(STUDENT_CACHE_FILE_NAME);
                string json = File.ReadAllText(filePath);
                return JsonConvert.DeserializeObject<JengaData>(json);
            }
        }

        protected override SaveManager InstanceTarget
        {
            get => Instances.SaveManager;
            set => Instances.SaveManager = value;
        }

        //------------------------------------------------------------------------------------//
        /*---------------------------------- METHODS -----------------------------------------*/
        //------------------------------------------------------------------------------------//

        private string GetFilePath(string fileId, params string[] folders) => $"{Application.persistentDataPath}{Path.DirectorySeparatorChar}{fileId}{EXTENSION}";

        private bool HasFile(string fileId) => File.Exists(GetFilePath(fileId));

        private void Save<T>(string fileId, T toSave)
        {
            string json = JsonConvert.SerializeObject(toSave,
#if SEEKERS_DEBUG
                Formatting.Indented
#else
                Formatting.None
#endif
            );
            File.WriteAllText(GetFilePath(fileId), json);
        }

        public void SaveStudentData(JengaData data) => Save(STUDENT_CACHE_FILE_NAME, data);
    }
}
