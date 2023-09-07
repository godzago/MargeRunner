using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fpsstabilizer : MonoBehaviour
{
    public static Fpsstabilizer instance;
    private int targerRate = 60;
    public void Awake()
    {
        Application.targetFrameRate = targerRate;

        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
