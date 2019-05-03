using UnityEngine;

public class VolumeBuoyancy : MonoBehaviour
{
	public float volume = 1.0f;
	public Vector3 size = new Vector3(1, 1, 1);
	public Vector3 drag = new Vector3(0.9f, 0.9f, 0.9f);

	FloatableWater waterSurface;

	void FixedUpdate()
	{
		if (waterSurface != null)
		{
			UpdateBuoyancy();
			UpdateDrag();
		}
	}

	void UpdateBuoyancy()
	{
		var waterLevel = waterSurface.collider.bounds.max.y;
		var distanceFromWaterLevel = transform.position.y - waterLevel;

		var percentUnderWater = Mathf.Clamp01((-distanceFromWaterLevel + 0.5f * size.y) / size.y);

		var buoyancyPlane = new Vector3(-transform.right.y * size.x * 0.5f, 0.0f, -transform.forward.y * size.z * 0.5f);
		var buoyancyPos = transform.TransformPoint(buoyancyPlane);

		buoyancyPos = UpdateWaveMotion(buoyancyPos, waterSurface);

		rigidbody.AddForceAtPosition(-volume * percentUnderWater * Physics.gravity, buoyancyPos);
	}

	void UpdateDrag()
	{
		var waterLevel = waterSurface.collider.bounds.max.y;
		var distanceFromWaterLevel = transform.position.y - waterLevel;

		var percentUnderWater = Mathf.Clamp01((-distanceFromWaterLevel + 0.5f * size.y) / size.y);

		var dragDirection = transform.InverseTransformDirection(rigidbody.velocity);
		var dragForces = Vector3.Scale(-dragDirection, drag);

		var depthUnderWater = Mathf.Abs(transform.forward.y) * size.z * 0.5f + Mathf.Abs(transform.up.y) * size.y * 0.5f;

		var dragAttackPosition = new Vector3(transform.position.x, waterLevel - depthUnderWater, transform.position.z);

		rigidbody.AddForceAtPosition(transform.TransformDirection(dragForces) * rigidbody.velocity.magnitude * (1.0f + percentUnderWater * (waterSurface.waterDragFactor - 1.0f)), dragAttackPosition);
		rigidbody.AddForce(transform.TransformDirection(dragForces) * 500.0f);
	}

	Vector3 UpdateWaveMotion(Vector3 buoyancyPos, FloatableWater waterSurface)
	{
		buoyancyPos.x += waterSurface.waveXMotion1 * Mathf.Sin(waterSurface.waveFreq1 * Time.time)
				 + waterSurface.waveXMotion2 * Mathf.Sin(waterSurface.waveFreq2 * Time.time)
				 + waterSurface.waveXMotion3 * Mathf.Sin(waterSurface.waveFreq3 * Time.time);

		buoyancyPos.z += waterSurface.waveYMotion1 * Mathf.Sin(waterSurface.waveFreq1 * Time.time)
				 + waterSurface.waveYMotion2 * Mathf.Sin(waterSurface.waveFreq2 * Time.time)
				 + waterSurface.waveYMotion3 * Mathf.Sin(waterSurface.waveFreq3 * Time.time);

		return buoyancyPos;
	}

	void OnTriggerStay(Collider collider)
	{
		var newWaterSurface = collider.GetComponent(typeof(FloatableWater)) as FloatableWater;

		if (newWaterSurface != null)
		{
			waterSurface = newWaterSurface;
		}
	}
}
