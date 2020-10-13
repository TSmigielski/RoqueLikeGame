using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CharacterSelector : MonoBehaviour
{
	public Character[] characters;
	[Space]
	public Image menuImage;
	public TextMeshProUGUI fullName;
	public TextMeshProUGUI description;
	public TextMeshProUGUI speed;
	public TextMeshProUGUI inteligence;
	public TextMeshProUGUI strength;
	public TextMeshProUGUI endurance;
	public TextMeshProUGUI immunity;

	int charIndex = 0;

	private void Start()
	{
		SelectCharacter(characters[charIndex]);
	}

	public void SelectNextCharacter()
	{
		if (characters.Length - 1 > charIndex)
		{
			charIndex++;
			SelectCharacter(characters[charIndex]);
		}
		else
		{
			charIndex = 0;
			SelectCharacter(characters[charIndex]);
		}
	}

	public void SelectPreviousCharacter()
	{
		if (0 < charIndex)
		{
			charIndex--;
			SelectCharacter(characters[charIndex]);
		}
		else
		{
			charIndex = characters.Length - 1;
			SelectCharacter(characters[charIndex]);
		}
	}

	public void SelectCharacter(Character _character)
	{
		MyData.Instance.MyCharacter = _character;
		UpdateInfoPanel();
	}

	public void UpdateInfoPanel()
	{
		fullName.text = $"{MyData.Instance.MyCharacter.firstName} {MyData.Instance.MyCharacter.lastName}";
		description.text = MyData.Instance.MyCharacter.description;
		menuImage.sprite = MyData.Instance.MyCharacter.menuImage;
		speed.text = "Speed: " + MyData.Instance.MyCharacter.speed.ToString();
		inteligence.text = "Inteligence: " + MyData.Instance.MyCharacter.inteligence.ToString();
		strength.text = "Strength: " + MyData.Instance.MyCharacter.strength.ToString();
		endurance.text = "Endurance: " + MyData.Instance.MyCharacter.endurance.ToString();
		immunity.text = "Immunity: " + MyData.Instance.MyCharacter.immunity.ToString();
	}
}
