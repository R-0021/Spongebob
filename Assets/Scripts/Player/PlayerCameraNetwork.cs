using Unity.Netcode;
using UnityEngine;

public class PlayerCameraNetwork : NetworkBehaviour
{
	[Range(0.1f, 9f)]public float sensitivity = 2f;
	[Range(0f, 90f)] public float yRotationLimit = 88f;

	Vector2 rotation = Vector2.zero;
	const string xAxis = "Mouse X";
	const string yAxis = "Mouse Y";

	private void Start()
	{
		if (!IsLocalPlayer)
		{
			gameObject.GetComponent<Camera>().enabled = false;
			gameObject.GetComponent<AudioListener>().enabled = false;
			this.enabled = false;
			return;
		}

		Cursor.visible = false;
		Cursor.lockState = CursorLockMode.Locked;
	}

	private void Update()
	{
		rotation.x += Input.GetAxis(xAxis) * sensitivity;
		rotation.y += Input.GetAxis(yAxis) * sensitivity;
		rotation.y = Mathf.Clamp(rotation.y, -yRotationLimit, yRotationLimit);
		var xQuat = Quaternion.AngleAxis(rotation.x, Vector3.up);
		var yQuat = Quaternion.AngleAxis(rotation.y, Vector3.left);

		transform.rotation = xQuat * yQuat;

		transform.parent.localRotation = xQuat;
	}
}