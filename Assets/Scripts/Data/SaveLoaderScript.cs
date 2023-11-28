using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using Utilities;

namespace Data
{
    public class SaveLoaderScript:MonoBehaviour
    {
        private static string _filepath;
        
        public void Save()
        {
            if(PlayerSave.Instance==null)
                return;
            using (FileStream file = File.Create(_filepath))
                new BinaryFormatter().Serialize(file, PlayerSave.Instance);
        }

        public void Load()
        {
            using (FileStream file = File.Open(_filepath, FileMode.OpenOrCreate))
            {
                if (file.Length == 0)
                    PlayerSave.Instance = new PlayerSave();
                else
                    PlayerSave.Instance = (PlayerSave) new BinaryFormatter().Deserialize(file);
            }
        }

        public void CopySave()
        {
            
        }

        public void DeleteSave()
        {
            File.Delete(_filepath);
            PlayerSave.Instance = new PlayerSave();
        }

        private void OnEnable()
        {
            EventBus.Subscribe(EventBus.EventType.GAME_SAVE, Save);
        }

        private void OnDisable()
        {
            EventBus.Unsubscribe(EventBus.EventType.GAME_SAVE, Save);
        }

        private void OnApplicationQuit()
        {
            EventBus.Publish(EventBus.EventType.GAME_SAVE);
        }

        private void Awake()
        {
            _filepath = Application.persistentDataPath + "/Save.dat";
        }

        private void Start()
        {
            Load();
            Variables.UpdateVariables();
        }
    }
}