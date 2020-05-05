using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public abstract class MonoSingleton<T> : MonoBehaviour where T: MonoSingleton<T>
{
    private static T _instance;
    public static T Instance
    {
        get
        {
            if (_instance == null)
            {
                Debug.LogError(typeof(T).ToString() + " is null");
            }
            return _instance;
        }
    }

    // Awake is used to initialize any variables or game state before the game starts.
    // Awake is called only once during the lifetime of the script _instance. 
    // Awake is called after all objects are initialized so you can safely speak to other objects or query them using for example GameObject.FindWithTag.
    // Each GameObject's Awake is called in a random order between objects. Because of this, you should use Awake to set up references between scripts, 
    // and use Start to pass any information back and forth. Awake is always called before any Start functions. 
    // This allows you to order initialization of scripts. Awake can not act as a coroutine.
    // Note: Use Awake instead of the constructor for initialization, as the serialized state of the component is undefined at construction time. 
    // Awake is called once, just like the constructor.
    void Awake()
    {
        _instance = this as T; // (T)this is also ok
        Init();
    }

    public virtual void Init() { }
}

