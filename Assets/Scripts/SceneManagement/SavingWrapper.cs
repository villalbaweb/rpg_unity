using System;
using UnityEngine;
using RPG.Saving;
using System.Collections;

namespace RPG.SceneManagement
{
    public class SavingWrapper : MonoBehaviour
    {
        const string DEFAULT_SAVE_FILE = "save";

        // config params
        [SerializeField] float timeToFadeIn = 0.2f;

        // cache
        SavingSystem _savingSystem;
        Fader _fader;

        private void Awake()
        {
            StartCoroutine(LoadLastScene());
        }

        IEnumerator LoadLastScene() 
        {
            _savingSystem = GetComponent<SavingSystem>();
            _fader = FindObjectOfType<Fader>();
            _fader.FadeOutInmediate();
            yield return _savingSystem.LoadLastScene(DEFAULT_SAVE_FILE);
            yield return _fader.FadeIn(timeToFadeIn);
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

            if(Input.GetKeyDown(KeyCode.Delete))
            {
                Delete();
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

        public void Delete()
        {
            _savingSystem.Delete(DEFAULT_SAVE_FILE);
        }
    }
}
