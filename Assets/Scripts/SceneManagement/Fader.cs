using System.Collections;
using UnityEngine;

namespace RPG.SceneManagement
{
    public class Fader : MonoBehaviour
    {
        // cache
        CanvasGroup _canvasGroup;
        Coroutine currentActiveFade = null;

        private void Awake() 
        {
            _canvasGroup = GetComponent<CanvasGroup>();
        }

        public void FadeOutInmediate() 
        {
            _canvasGroup.alpha = 1;
        }

        public IEnumerator FadeOut(float time)
        {
            return Fade(1, time);
        }

        public IEnumerator FadeIn(float time)
        {
            return Fade(0, time);
        }

        private IEnumerator Fade(float target, float time)
        {
            if (currentActiveFade != null)
            {
                StopCoroutine(currentActiveFade);
            }

            currentActiveFade = StartCoroutine(FadeRoutine(target, time));

            yield return currentActiveFade;
        }

        private IEnumerator FadeRoutine(float target, float time)
        {
            while (!Mathf.Approximately(_canvasGroup.alpha, target))
            {
                _canvasGroup.alpha = Mathf.MoveTowards(_canvasGroup.alpha, target, Time.deltaTime / time);
                yield return null;
            }
        }
    }
}
