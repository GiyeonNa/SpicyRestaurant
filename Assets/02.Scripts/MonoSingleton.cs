using UnityEngine;

public abstract class MonoSingleton<T> : MonoBehaviour where T : MonoBehaviour
{
    // Singleton instance
    private static T _instance;

    // Property to get the singleton instance
    public static T Instance
    {
        get
        {
            if (_instance == null)
            {
                // Try to find an existing instance in the scene
                _instance = FindAnyObjectByType<T>();

                if (_instance == null)
                {
                    // If no instance exists, create a new one
                    GameObject singletonObject = new GameObject(typeof(T).Name);
                    _instance = singletonObject.AddComponent<T>();
                }
            }

            return _instance;
        }
    }

    // Awake method to ensure singleton instance is not destroyed between scenes
    protected virtual void Awake()
    {
        if (_instance == null)
        {
            _instance = this as T;
            DontDestroyOnLoad(gameObject);  // Keep the instance between scenes
        }
        else if (_instance != this)
        {
            Destroy(gameObject);  // Destroy duplicate instances
        }
    }

    // Virtual method for initialization if needed
    public virtual void Init() { }

    // UpdateLogic: Called each frame, can be overridden in derived classes
    public virtual void UpdateLogic()
    {
        // Default behavior: does nothing. Can be overridden in derived classes to implement custom logic.
    }
}
