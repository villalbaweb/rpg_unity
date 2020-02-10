using System.Collections;
using UnityEngine;

namespace RPG.SceneManagement
{
    public class Fader : MonoBehaviour
    {
        // cache
        CanvasGroup _canvasGroup;

        private void Start() 
        {
            _canvasGroup = GetComponent<CanvasGroup>();

            StartCoroutine(FadeoutIn());  
        }

        IEnumerator FadeoutIn()
        {
            yield return FadeOut(3.0f);
            print("Faded out conplete...");
            yield return FadeIn(3.0f);
            print("Faded in complete...");
        }

        public IEnumerator FadeOut(float time)
        {
            while (_canvasGroup.alpha < 1)
            {
                _canvasGroup.alpha += Time.deltaTime / time;
                yield return null;
            }
        }

        public IEnumerator FadeIn(float time)
        {
            while (_canvasGroup.alpha > 0)
            {
                _canvasGroup.alpha -= Time.deltaTime / time;
                yield return null;
            }
        }
    }
}
