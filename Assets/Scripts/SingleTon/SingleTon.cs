using UnityEngine;

public class SingleTon<T> : MonoBehaviour where T : Component
{
    public static T instance;
    public static bool HasInstance => instance != null;

    public static T GetInstance => HasInstance ? instance : Instance;

    public static T Instance
    {
        get
        {
            if (HasInstance)
                return instance;
            else
            {
                instance = FindAnyObjectByType<T>();

                if (!instance)
                {
                    GameObject o = new GameObject(nameof(T));
                    instance = o.AddComponent<T>();

                    return instance;
                }

                return instance;
            }

        }
    }

    private void Awake()
    {
        if (!instance)
            instance = this as T;
        else
            Destroy(gameObject);
    }

}
