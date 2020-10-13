using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuPageSwapper : MonoBehaviour
{
	public List<GameObject> pages = new List<GameObject>();

	private void Start()
	{
		DisablePages();
		pages[0].SetActive(true);
	}

	public void SwapPage(GameObject _page)
	{
		DisablePages();
		_page.SetActive(true);
	}

	void DisablePages()
	{
		foreach (var page in pages)
		{
			page.SetActive(false);
		}
	}
}
