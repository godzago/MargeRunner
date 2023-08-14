using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace HyperCasualRunner
{

public class EventManager : MonoBehaviour
{
    public class Event0 : UnityEvent { }

    public static readonly UnityEvent levelFailEvent = new();
    public static readonly UnityEvent levelSuccessEvent = new();
    public static readonly UnityEvent FirstFlag = new();
    public static readonly UnityEvent AfterFlag = new();
    public static readonly UnityEvent Flag2 = new();
    public static readonly UnityEvent Flag3 = new();
    }
}