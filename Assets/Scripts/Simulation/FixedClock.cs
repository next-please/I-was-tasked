using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * This clock is the engine for simulation
 * Currently uses a very naiive implementation that will be updated after testing
 * It uses unity fixed update to generate ticks
 */
public sealed class FixedClock : MonoBehaviour
{
    // Singleton Pattern
    public static FixedClock Instance
    {
        get
        {
            if (_instance == null)
            {
                Debug.LogError("Missing FixedClock, make sure to create object in scene with FixedClock Component and only reference after Awake");
            }
            return _instance;
        }
    }

    public float deltaTime
    {
        get
        {
            return Time.fixedDeltaTime;
        }
    }

    private static readonly object Instancelock = new object();
    private static FixedClock _instance;
    private long _tick = 0;
    private List<Tickable> _tickables = new List<Tickable>();

    void Awake()
    {
        if (_instance == null)
        {
            lock(Instancelock)
            {
                if (_instance == null)
                {
                    _instance = this;
                    DontDestroyOnLoad(_instance);
                }
            }
        }

        if (_instance != this)
        {
            Destroy(gameObject);
        }
    }

    public void AddTickable(Tickable tickable)
    {
        _tickables.Add(tickable);
    }

    public void RemoveTickable(Tickable tickable)
    {
        _tickables.Remove(tickable);
    }

    void FixedUpdate()
    {
        _tick++;
        foreach (Tickable tickable in _tickables)
        {
            tickable.Tick(_tick);
        }
    }
}
