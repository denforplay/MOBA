using UnityEngine;

namespace Common
{
    public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        private static T _instance;
        public static bool IsDestroy = false;
        public static T GetInstance
        {
            get
            {
                if (_instance is null)
                {
                    _instance = new GameObject().AddComponent<T>();
                    if (!IsDestroy)
                        DontDestroyOnLoad(_instance);
                }

                return _instance;
            }
        }


        public void Awake()
        {
            if (_instance == null)
            {
                _instance = this as T;
            }
            else if (_instance != this as T)
            {
                Destroy(gameObject);
            }
        }

    }
}