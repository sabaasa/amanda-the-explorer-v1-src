using TMPro;
using UnityEngine;

[RequireComponent(typeof(TextMeshProUGUI))]
public class TMPWiggle : MonoBehaviour
{
	public float XWobble = 3f;

	public float YWobble = 1.5f;

	private TMP_Text textMesh;

	private Mesh mesh;

	private Vector3[] verts;

	private void Start()
	{
		textMesh = GetComponent<TMP_Text>();
	}

	private void Update()
	{
		textMesh.ForceMeshUpdate();
		mesh = textMesh.mesh;
		verts = mesh.vertices;
		for (int i = 0; i < textMesh.textInfo.characterCount; i++)
		{
			int vertexIndex = textMesh.textInfo.characterInfo[i].vertexIndex;
			Vector3 vector = Wobble(Time.time + (float)i);
			verts[vertexIndex] += vector;
			verts[vertexIndex + 1] += vector;
			verts[vertexIndex + 2] += vector;
			verts[vertexIndex + 3] += vector;
		}
		mesh.vertices = verts;
		textMesh.canvasRenderer.SetMesh(mesh);
	}

	private Vector2 Wobble(float time)
	{
		return new Vector2(Mathf.Sin(time * XWobble), Mathf.Cos(time * YWobble));
	}
}
