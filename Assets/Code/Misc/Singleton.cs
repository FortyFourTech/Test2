using UnityEngine;

namespace Dimar
{
    /// <summary>
    /// The base abstract Singleton class.
    /// </summary>
    public abstract class Singleton<T>: MonoBehaviour where T: Singleton<T>
    {
        [SerializeField] protected ESingletonMode _singletonMode = ESingletonMode.main;
        [SerializeField] protected bool _dontDestroyOnLoad = true;
        
        protected static T _instance = null;
        public static T Instance => _instance;
        
        protected virtual void Awake()
        {
            if (!Application.isPlaying)
                return;

            if (_instance != null)
            {
                Debug.LogWarning($"Singleton warning: {name} already initialized", this);

                var singletonMode = ChooseMode(_instance);

                switch (singletonMode)
                {
                    case ESingletonMode.main:
                        Destroy(_instance.gameObject);
                        _instance = (T)this;
                        break;
                    case ESingletonMode.secondary:
                        Destroy(this.gameObject);
                        break;
                }
            }
            else
            {
                _instance = (T)this;
            }

            if (_dontDestroyOnLoad)
            {
                DontDestroyOnLoad(gameObject);
            }
        }

        protected virtual ESingletonMode ChooseMode(T concurrent)
        {
            return _singletonMode;
        }

        protected virtual void OnDestroy()
        {
            if (_instance == this)
            {
                _instance = null;
            }
        }
    }

    public enum ESingletonMode
    {
        main,
        secondary,
    }
}
