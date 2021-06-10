using UnityEngine;

public abstract class SingletonScriptableObject<T> : ScriptableObject where T : ScriptableObject
{
    private static T _instance = null;

    public static T Instance
    {
        get
        {
            if (!_instance)
            {
                //_instance = Resources.FindObjectsOfTypeAll<T>().FirstOrDefault();
                string name = typeof(T).Name;

                _instance = Resources.Load<T>(name);
            }

            return _instance;
        }
    }
}