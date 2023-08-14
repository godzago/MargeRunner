using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneController : MonoBehaviour
{
    private static SceneController instance;
    public static SceneController Instance { get { return instance; } }


    [SerializeField] private Button applyButton;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    void Start()
    {
        if (applyButton != null)
        {
            applyButton.onClick.AddListener(SceneManagerClass);
        }
    }


    public void SceneManagerClass()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex +1 );
    }
}
