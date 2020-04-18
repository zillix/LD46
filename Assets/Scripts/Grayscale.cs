using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grayscale : MonoBehaviour
{
	private MeshRenderer meshRenderer;

	public static bool ENABLE = false;

	private Color startColor;

	void Start()
    {
		meshRenderer = GetComponentInChildren<MeshRenderer>();
        if (ENABLE)
		{
			startColor = meshRenderer.material.color;
			float avg = (startColor.r + startColor.g + startColor.b) / 3;
			meshRenderer.material.color = new Color(avg, avg, avg);
		}
    }

    public void RestoreColor()
	{
		if (ENABLE)
		{
			meshRenderer.material.color = startColor;
		}
	}
}
