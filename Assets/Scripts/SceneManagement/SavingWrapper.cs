using System;
using UnityEngine;
using RPG.Saving;
namespace RPG.SceneManagement
{
    public class SavingWrapper : MonoBehaviour
    {
        const string DEFAULT_SAVE_FILE = "save";

        // cache
        SavingSystem _savingSystem;

        private void Start() 
        {
            _savingSystem = GetComponent<SavingSystem>();
        }

        // Update is called once per frame
        void Update()
        {
            if(Input.GetKeyDown(KeyCode.L))
            {
                Load();
            }

            if(Input.GetKeyDown(KeyCode.S))
            {
                Save();
            }
        }

        public void Save()
        {
            _savingSystem.Save(DEFAULT_SAVE_FILE);
        }

        public void Load()
        {
            _savingSystem.Load(DEFAULT_SAVE_FILE);
        }
    }
}
