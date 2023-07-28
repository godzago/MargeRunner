using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace HyperCasualRunner
{
	public class MenuController : MonoBehaviour
	{
		void Start()
		{
			transform.SetParent(null);
			DontDestroyOnLoad(gameObject);
		}

		void Update()
		{
			if (Input.GetKeyDown(KeyCode.Alpha1))
			{
				LoadScene(1);
			}
			else if (Input.GetKeyDown(KeyCode.Alpha2))
			{
				LoadScene(2);
			}
			else if (Input.GetKeyDown(KeyCode.Alpha3))
			{
				LoadScene(3);
			}
			else if (Input.GetKeyDown(KeyCode.Alpha4))
			{
				LoadScene(4);
			}
			else if (Input.GetKeyDown(KeyCode.Alpha5))
			{
				LoadScene(5);
			}
			else if (Input.GetKeyDown(KeyCode.Alpha6))
			{
				LoadScene(6);
			}

			if (Input.GetKeyDown(KeyCode.Space))
			{
				LoadScene(SceneManager.GetActiveScene().buildIndex);
			}
		}

		void LoadScene(int index)
		{
			SceneManager.LoadScene(index);
		}
	}
}
