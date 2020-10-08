using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using TMPro;

namespace RPG.Menu
{
	public class SettingMenu : MonoBehaviour
	{

		[SerializeField] AudioMixer audioMixer;
		[SerializeField] GameObject ToggleOn;
		[SerializeField] GameObject ToggleOff;
		[SerializeField] GameObject musicOn;
		[SerializeField] GameObject musicOff;
		[SerializeField] TMP_Dropdown resolutionDropdown;
		[SerializeField] AudioSource audioClip;

		Resolution[] resolutions;

		public void Start()
		{
			resolutions = Screen.resolutions;

			resolutionDropdown.ClearOptions();

			List<string> options = new List<string>();

			int currentResolutionIndex = 0;
			for(int i = 0; i < resolutions.Length; i++)
			{
				string option = resolutions[i].width + " x " + resolutions[i].height;
				options.Add(option);

				if(resolutions[i].width == Screen.currentResolution.width && resolutions[i].height == Screen.currentResolution.height)
				{
					currentResolutionIndex = i;
				}
			}

			resolutionDropdown.AddOptions(options);
			resolutionDropdown.value = currentResolutionIndex;
			resolutionDropdown.RefreshShownValue();
			SetFullscreen(true);
			MuteMusic(false);
		}

		public void SetResolution(int resolutionIndex)
		{
			Resolution resolution = resolutions[resolutionIndex];
			Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
		}

		public void SetVolume(float volume)
		{
			audioMixer.SetFloat("volume", volume);
		}

		public void SetQuality(int qualityIndex)
		{
			QualitySettings.SetQualityLevel(qualityIndex);
		}

		public void SetFullscreen(bool isFullscreen)
		{
			Screen.fullScreen = isFullscreen;
			if (isFullscreen)
			{
				ToggleOn.SetActive(true);
				ToggleOff.SetActive(false);
			}
			
			if(!isFullscreen)
			{
				ToggleOn.SetActive(false);
				ToggleOff.SetActive(true); 
			}
		}

		public void MuteMusic(bool isMuted)
		{
			if (!isMuted)
			{
				musicOff.SetActive(false);
				musicOn.SetActive(true);
				audioClip.volume = 1;
			}

			
			if (isMuted)
			{
				musicOn.SetActive(false);
				musicOff.SetActive(true);
				audioClip.volume = 0;
			}
		}

	}
}

