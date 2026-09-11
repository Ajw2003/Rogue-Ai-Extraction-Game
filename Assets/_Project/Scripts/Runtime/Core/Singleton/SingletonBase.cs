using UnityEngine;

namespace Code.Scripts.Singleton
{
    // Generic Singleton base class to ensure a single instance of a MonoBehaviour-derived class
    public class SingletonBase<T> : MonoBehaviour where T : MonoBehaviour
    {
        private static T _instance;

        protected virtual bool PersistBetweenScenes => true;

        // Never auto-creates a GameObject: an absent instance means no scene owner has been
        // set up yet, and callers (EventManager.Instance?.Publish(...)) rely on that being null
        // rather than silently spawning a stray object after teardown.
        public static T Instance => _instance;

        protected virtual void Awake()
        {
            if (!_instance)
            {
                _instance = this as T;
                if (PersistBetweenScenes) DontDestroyOnLoad(gameObject);
            }
            else if (_instance != this)
            {
                Destroy(gameObject);
            }
        }
    }
}
