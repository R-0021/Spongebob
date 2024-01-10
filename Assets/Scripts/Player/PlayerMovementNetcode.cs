using Unity.Netcode;
using UnityEngine;

public class PlayerMovementNetCode : NetworkBehaviour
{
	public float moveSpeed = 5f;
	public float runSpeed = 10f;
	public float jumpForce = 8f;
	public bool IsGrounded => Physics.Raycast(transform.position, Vector3.down, 0.2f);

	private float speed = 5f;

	private Rigidbody rb;
	private Transform camera;

	void Start()
	{
		rb = GetComponent<Rigidbody>();
		camera = Camera.main.transform;
	}

	void Update()
	{
		if (!IsOwner) return;
		HandleRun();
		Move();

		if (IsGrounded && Input.GetKeyDown(KeyCode.Space))
			Jump();
	}

	void HandleRun()
	{
		bool run = Input.GetKey(KeyCode.LeftShift);
		speed = (run) ? runSpeed : moveSpeed;
	}

	void Move()
	{
		float horizontalInput = Input.GetAxisRaw("Horizontal");
		float verticalInput = Input.GetAxisRaw("Vertical");
		Vector3 movement = camera.forward * verticalInput + camera.right * horizontalInput;
		movement.y = 0;
		movement.Normalize();

		Vector3 newPosition = transform.position + movement * speed * Time.deltaTime;
		rb.MovePosition(newPosition);
	}

	void Jump()
	{
		rb.AddForce(Vector3.up * jumpForce, ForceMode.VelocityChange);
	}

    public override void OnNetworkSpawn()
    {
        if (!IsLocalPlayer) enabled = false;
    }
}