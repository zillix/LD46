using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PolygonMesh2D))]
public class MeshWobbler : MonoBehaviour
{
	public float wobbleScale = 1f;
	public float wobbleSpeedScale = 1f;

	public static float wobblerMultiplier = .7f;

	private Vector3[] baseVertices;
	private Vector3[] currentVertices;
	private MeshFilter mf;
	private Dictionary<int, WobbleData> data = new Dictionary<int, WobbleData>();

	public bool Glitching = false;

	public bool Wobbling = true;

	private struct WobbleData
	{
		public float radius;
		public float angle;
		public float angleVel;
	}

    // Start is called before the first frame update
    void Start()
    {
		if (wobblerMultiplier == 0)
		{
			return;
		}

		mf = GetComponent<MeshFilter>();
		baseVertices = mf.mesh.vertices;
		currentVertices = (Vector3[])baseVertices.Clone(); // copy

		for (int i = 0; i < baseVertices.Length; ++i)
		{
			WobbleData wobble = new WobbleData();
			wobble.radius = Random.Range(.03f, .05f) * wobbleScale;
			wobble.angle = Random.Range(0, 360);
			wobble.angleVel = Random.Range(200, 255) * wobbleSpeedScale;
			if (Random.value < 0.5f)
			{
				wobble.angleVel *= -1;
			}
			data[i] = wobble;
		}
    }

	public void ToggleGlitch(float amt)
	{
		Glitching = !Glitching;
		List<WobbleData> updated = new List<WobbleData>();
		foreach (int i in data.Keys)
		{
			WobbleData wd = data[i];
			if (Glitching)
			{
				wd.angleVel *= amt;
				wd.radius *= amt;
			}
			else
			{
				wd.angleVel /= amt;
				wd.radius /= amt;
			}
			updated.Add(wd);
		}
		for (int i = 0; i < updated.Count; ++i)
		{
			data[i] = updated[i];
		}
	}

    // Update is called once per frame
    void Update()
    {
		if (wobblerMultiplier == 0)
		{
			return;
		}

		if (Wobbling)
		{
			for (int i = 0; i < baseVertices.Length; ++i)
			{
				WobbleData wobble = data[i];
				wobble.angle += Time.deltaTime * wobble.angleVel;

				float globalMultiplier = wobblerMultiplier;
				currentVertices[i].x = baseVertices[i].x + Mathf.Cos(Mathf.Deg2Rad * wobble.angle) * wobble.radius * globalMultiplier;
				currentVertices[i].y = baseVertices[i].y + Mathf.Sin(Mathf.Deg2Rad * wobble.angle) * wobble.radius * globalMultiplier;
				data[i] = wobble;
			}

			ColliderToMesh.SetMeshVertices(mf.mesh, currentVertices);
		}
    }
}
