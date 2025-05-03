    using System.Collections;
    using UnityEngine;

    public class CoroutineExecutor : MonoBehaviour
    {
        public static CoroutineExecutor Instance;
        private void Awake()
        {
            if (Instance != null)
            {
                Destroy(gameObject);
            }
            Instance = this;
        }

        public Coroutine ExecuteCoroutine(IEnumerator coroutine)
        {
           return StartCoroutine(coroutine);
        }
        
        public void StopCoroutineExecution(Coroutine coroutine)
        {
            StopCoroutine(coroutine);
        }
    }
