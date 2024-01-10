using Photon.Pun;
using Unity.Netcode;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
	public float moveSpeed = 5f;
	public float runSpeed = 10f;
	public float jumpForce = 8f;
	public bool IsGrounded => Physics.Raycast(transform.position + Vector3.up * 0.1f, Vector3.down, 0.2f);

	private float speed = 5f;

	private Rigidbody rb;
	private Animator animator;
	private Transform camera;

	private Vector2 input;

	void Start()
	{
		rb = GetComponent<Rigidbody>();
		animator = GetComponentInChildren<Animator>();
		camera = Camera.main.transform;
	}

	private void Update()
	{
		if (IsGrounded && Input.GetKeyDown(KeyCode.Space))
			Jump();

		if (Input.GetKeyDown(KeyCode.F))
			GetComponent<PhotonView>().RPC("TakeDamage", RpcTarget.All, 25);

    }

	private void FixedUpdate()
	{
		HandleRun();
		Move();
	}

	void HandleRun()
	{
		bool run = Input.GetKey(KeyCode.LeftShift);
		speed = (run) ? runSpeed : moveSpeed;
	}

	void Move()
	{
		input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
		input.Normalize();

		if (input.sqrMagnitude > 0.1f)
			animator.SetBool("Chase", true);
		else
			animator.SetBool("Chase", false);

		Vector3 movement = camera.forward * input.y + camera.right * input.x;
		movement.y = 0;
		movement.Normalize();

		Vector3 newPosition = transform.position + movement * speed * Time.fixedDeltaTime;
		rb.MovePosition(newPosition);
	}

	void Jump()
	{
		rb.AddForce(Vector3.up * jumpForce, ForceMode.VelocityChange);

	}

}