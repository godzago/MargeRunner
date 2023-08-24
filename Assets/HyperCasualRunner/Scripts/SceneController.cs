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

    public static int sceneNumber = 1;
     
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

        if (sceneNumber <= 3)
        {
            Debug.Log("" + sceneNumber);
        }
        else
        {
            sceneNumber = Random.Range(1, 3);
        }
    }

    public void SceneManagerClass()
    {      

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + sceneNumber );

    }
}
