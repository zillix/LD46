using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
	public Player Player;
	public StoryManager storyManager;

	public GameObject renderTexturePlane;
	public IslandCameraController islandCamera;

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

	}
}
