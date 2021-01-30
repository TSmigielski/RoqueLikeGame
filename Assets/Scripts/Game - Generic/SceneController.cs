using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
	public void LoadLevel_0()
	{
		SceneManager.LoadScene("Level 0");
	}

	public void ExitGame()
	{
		Application.Quit();
	}
}
