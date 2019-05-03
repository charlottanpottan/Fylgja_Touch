using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(Rigidbody))]

public class PolygonBuoyancy : MonoBehaviour
{
	public float density = .6f;
	public Vector3 cg;
	private Vector3 c; //centroid for the submerged volume
	private Vector3[] vertices;
	private int[] tris;
	private int triCount;
	private int vertCount;
	private float meshVolume;

	private int layerMask = 1 << 4;

	private float drag;
	private float angularDrag;

	void Start()
	{
		GetComponent<Rigidbody>().SetDensity(density);
		GetComponent<Rigidbody>().centerOfMass = cg;

		Mesh mesh = ((MeshFilter) GetComponent(typeof(MeshFilter))).mesh;
		vertices = mesh.vertices;
		tris = mesh.triangles;
		triCount = tris.Length / 3;
		vertCount = vertices.Length;
		meshVolume = ComputeVolume();

		drag = GetComponent<Rigidbody>().drag;
		angularDrag = GetComponent<Rigidbody>().angularDrag;
		GetComponent<Rigidbody>().centerOfMass = cg;
	}

	void Update()
	{
	}

	void FixedUpdate()
	{
		ComputeBuoyancy();
	}

	private float TetrahedronVolume(Vector3 p, Vector3 v1, Vector3 v2, Vector3 v3)
	{
		Vector3 a = v2 - v1;
		Vector3 b = v3 - v1;
		Vector3 r = p - v1;

		float volume = (1f / 6f) * Vector3.Dot(Vector3.Cross(b, a), r);

		c += .25f * volume * (v1 + v2 + v3 + p);
		return volume;
	}

	private float ClipTriangle(Vector3 p, Vector3 v1, Vector3 v2, Vector3 v3, float d1, float d2, float d3)
	{
		Vector3 vc1 = v1 + (d1 / (d1 - d2)) * (v2 - v1);
		float volume = 0;

		if (d1 < 0)
		{
			if (d3 < 0)
			{
				// Case B - a quadrilateral or two triangles.
				Vector3 vc2 = v2 + (d2 / (d2 - d3)) * (v3 - v2);
				volume += TetrahedronVolume(p, vc1, vc2, v1);
				volume += TetrahedronVolume(p, vc2, v3, v1);
			}
			else
			{
				// Case A - a single triangle.
				Vector3 vc2 = v1 + (d1 / (d1 - d3)) * (v3 - v1);
				volume += TetrahedronVolume(p, vc1, vc2, v1);
			}
		}
		else
		{
			if (d3 < 0)
			{
				// Case B
				Vector3 vc2 = v1 + (d1 / (d1 - d3)) * (v3 - v1);
				volume += TetrahedronVolume(p, vc1, v2, v3);
				volume += TetrahedronVolume(p, vc1, v3, vc2);
			}
			else
			{
				// Case A
				Vector3 vc2 = v2 + (d2 / (d2 - d3)) * (v3 - v2);
				volume += TetrahedronVolume(p, vc1, v2, vc2);
			}
		}

		return volume;
	}

	private float SubmergedVolume()
	{
		const float TINY_DEPTH = -1e-6f;

		float[] vertexWaterDistances = new float[vertCount];

		// Compute the depth of each vertex.
		int numberOfVerticesSubmerged = 0;
		int sampleVert = 0;
		for (int i = 0; i < vertCount; ++i)
		{
			vertexWaterDistances[i] = transform.TransformPoint(vertices[i]).y - FloatableWaterPlane.instance.transform.position.y;
			if (vertexWaterDistances[i] < TINY_DEPTH)
			{
				++numberOfVerticesSubmerged;
				sampleVert = i;
				Debug.DrawLine(Vector3.zero, transform.TransformPoint(vertices[i]), Color.white);
			}
		}

		if (numberOfVerticesSubmerged == 0)
		{
			c = Vector3.zero;
			return 0;
		}

		Vector3 p = vertices[sampleVert];
		p.y = FloatableWaterPlane.instance.transform.position.y;

		float volume = 0;
		c = Vector3.zero;

		for (int i = 0; i < triCount; ++i)
		{
			int i1 = tris[i * 3];
			int i2 = tris[i * 3 + 1];
			int i3 = tris[i * 3 + 2];

			Vector3 v1 = vertices[i1];
			float d1 = vertexWaterDistances[i1];

			Vector3 v2 = vertices[i2];
			float d2 = vertexWaterDistances[i2];

			Vector3 v3 = vertices[i3];
			float d3 = vertexWaterDistances[i3];

			if (d1 * d2 < 0)
			{
				// v1-v2 crosses the plane
				volume += ClipTriangle(p, v1, v2, v3, d1, d2, d3);
			}
			else if (d1 * d3 < 0)
			{
				// v1-v3 crosses the plane
				volume += ClipTriangle(p, v3, v1, v2, d3, d1, d2);
			}
			else if (d2 * d3 < 0)
			{
				// v2-v3 crosses the plane
				volume += ClipTriangle(p, v2, v3, v1, d2, d3, d1);
			}
			else if (d1 < 0 || d2 < 0 || d3 < 0)
			{
				// fully submerged
				volume += TetrahedronVolume(p, v1, v2, v3);
			}
		}
		return volume;
	}

	private float ComputeVolume()
	{
		float volume = 0;
		Vector3 zero = Vector3.zero;

		c = Vector3.zero;

		for (int i = 0; i < triCount; ++i)
		{
			volume += TetrahedronVolume(zero, transform.TransformPoint(vertices[tris[i * 3]]), transform.TransformPoint(vertices[tris[i * 3 + 1]]), transform.TransformPoint(vertices[tris[i * 3 + 2]]));
		}
		return volume;
	}

	void ComputeBuoyancy()
	{
		c = Vector3.zero;
		float gravity = Physics.gravity.magnitude;
		float absoluteVolume = SubmergedVolume() * meshVolume;

		if (absoluteVolume > 0)
		{
			Vector3 buoyancyForce = (FloatableWaterPlane.instance.waterDensity * absoluteVolume * gravity) * Vector3.up;
			float amountInWater = Mathf.Clamp01(absoluteVolume / meshVolume); //use this to change drag & angularDrag
			float submergedMass = GetComponent<Rigidbody>().mass * amountInWater;
//			Vector3 rc = c - rigidbody.centerOfMass;
			Vector3 velocityAtCenterOfBuoyancy = GetComponent<Rigidbody>().GetPointVelocity(transform.TransformPoint(c));
			Vector3 dragForce = (submergedMass * FloatableWaterPlane.instance.waterDrag) * (GetWaterCurrent() - velocityAtCenterOfBuoyancy);

			Vector3 totalForce = buoyancyForce + dragForce;
			GetComponent<Rigidbody>().AddForceAtPosition(totalForce, transform.TransformPoint(c));
			GetComponent<Rigidbody>().drag = Mathf.Lerp(drag, FloatableWaterPlane.instance.waterDrag, amountInWater);
			GetComponent<Rigidbody>().angularDrag = Mathf.Lerp(angularDrag, FloatableWaterPlane.instance.waterAngularDrag, amountInWater);
		}
	}

	Vector3 GetWaterCurrent()
	{
		if (!FloatableWaterPlane.instance.currents)
		{
			return Vector3.zero;
		}
		Vector3 origin = transform.TransformPoint(c);
		origin.y = FloatableWaterPlane.instance.transform.position.y + 1;
		Ray ray = new Ray(origin, -Vector3.up);

		RaycastHit hit;

		if (!Physics.Raycast(ray, out hit, 2, layerMask))
		{
			return Vector3.zero;
		}
		Vector2 uv = hit.textureCoord;
		Vector3 currentDir = Vector3.zero;
		Color dir = FloatableWaterPlane.instance.currents.GetPixelBilinear(uv.x, uv.y);
		float angle = dir.grayscale * 360;

		currentDir.x = Mathf.Cos(angle * Mathf.Deg2Rad);
		currentDir.z = Mathf.Sin(angle * Mathf.Deg2Rad);

		currentDir = currentDir.normalized;
		//Debug.DrawLine(transform.position, transform.position + currentDir*dir.a);//Draw current
		return currentDir * dir.a * FloatableWaterPlane.instance.currentStrength;
	}
}