using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
	public Player Player;
	public StoryManager storyManager;

	public GameObject renderTexturePlane;
	public IslandCameraController islandCamera;

	public MainCameraController mainCamera;
	public Image titleRenderTexture;
	public Image titleBlocker;
	public float titleFadeTime = 2f;
	public Transform cameraTitle;

	private Coroutine titleCoroutine;

	private bool gameStarted = false;

	public static bool DEBUG = true;

	public void Start()
	{
		// this wasn't necessary, we just needed to turn off hdr on the camera to get pixel-perfect goodness
		// also doesn't play nicely with distortion :(
		/*RenderTexture rt = new RenderTexture(192, 128, 32, RenderTextureFormat.ARGB32, RenderTextureReadWrite.Linear);
		rt.filterMode = FilterMode.Point;
		rt.antiAliasing = 1;
		rt.Create();
		islandCamera.camera.targetTexture = rt;
		renderTexturePlane.GetComponent<MeshRenderer>().material.mainTexture = rt;
		storyManager.fireController.particleSystemRenderer.material.mainTexture = rt;*/
		islandCamera.ForceTransform(cameraTitle);

		// clean up the title render material so we don't get it dirty each time
		Material newMaterial = Instantiate(titleRenderTexture.material);
		titleRenderTexture.material = newMaterial;

		titleRenderTexture.enabled = true;
		titleBlocker.enabled = true;

	}

	public void Update()
	{
		if (Input.anyKeyDown && !gameStarted)
		{
			if (titleCoroutine == null)
			{
				titleCoroutine = StartCoroutine(titleFade(titleFadeTime));
			}
		}
	}
	
	private IEnumerator titleFade(float time)
	{
		float time1 = time;
		while (time1 > 0)
		{
			time1 -= Time.deltaTime;
			float alpha = Mathf.Lerp(1, 0, 1 - (time1 / time));
			titleRenderTexture.material.SetColor("_Color", new Color(1, 1, 1, alpha));
			yield return null;
		}

		float time2 = time;
		while (time2 > 0)
		{
			time2 -= Time.deltaTime;
			float alpha = Mathf.Lerp(1, 0, 1 - (time2 / time));
			titleBlocker.color = new Color(0, 0, 0, alpha);
			yield return null;
		}

		startGame();
	}

	private void startGame()
	{
		if (gameStarted)
		{
			return;
		}
		gameStarted = true;
		storyManager.StartGame();
		//storyManager.PlayStory(storyManager.startStory);
	}

}
