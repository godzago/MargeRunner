using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace HyperCasualRunner
{
	public class GameOverPanel : MonoBehaviour
	{
		[SerializeField] GameObject _activatable;
		
		public void Show()
		{
			StartCoroutine(Co_DelayedShow());
		}

		IEnumerator Co_DelayedShow()
		{
			yield return new WaitForSeconds(2f);
			Debug.Log("Panel showed");
			_activatable.SetActive(true);
			Time.timeScale = 0f;
		}

		public void Restart()
		{
			SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
			Time.timeScale = 1f;
		}
	}
}
