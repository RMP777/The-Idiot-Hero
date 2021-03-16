using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreeCam : MonoBehaviour
{

	Rigidbody rb;

	public float lookSensitivity = 50;
	public float speed = 300f;

	float rotationX = 0.0f;
	float rotationY = 0.0f;


	// Use this for initialization
	void Start()
	{
		Cursor.lockState = CursorLockMode.Locked;

		rb = GetComponent<Rigidbody>();
	}

	// Update is called once per frame
	void Update()
	{
		rotationX += Input.GetAxis("Mouse X") * lookSensitivity * Time.deltaTime;
		rotationY += Input.GetAxis("Mouse Y") * lookSensitivity * Time.deltaTime;
		rotationY = Mathf.Clamp(rotationY, -90, 90);

		transform.localRotation = Quaternion.AngleAxis(rotationX, Vector3.up);
		transform.localRotation *= Quaternion.AngleAxis(rotationY, Vector3.left);

		rb.velocity = Vector3.zero;

		Vector3 fw = transform.forward;
		fw.y = 0;
		fw.Normalize();

		rb.velocity += fw * Input.GetAxis("Vertical") * Time.deltaTime * speed;

		Vector3 r = transform.right;
		r.y = 0;
		r.Normalize();

		rb.velocity += r * Input.GetAxis("Horizontal") * Time.deltaTime * speed;

		rb.velocity += Vector3.up * Input.GetAxis("UpAndDown") * Time.deltaTime * speed;
	}
}