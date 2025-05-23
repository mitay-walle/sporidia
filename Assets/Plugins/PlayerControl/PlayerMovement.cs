using GameJam.Plugins.Timing;
using UltEvents;
using UnityEngine;

namespace GameJam.Plugins.PlayerControl
{
	public class PlayerMovement : MonoBehaviour
	{
		[SerializeField] private float moveSpeed;
		[SerializeField] private Timer _footstepTimer = new(.5f);
		public UltEvent OnFootstep;

		Rigidbody2D rb;

		protected void Awake()
		{
			rb = GetComponent<Rigidbody2D>();
		}

		private void FixedUpdate()
		{
			float inputX = Input.GetAxisRaw("Horizontal");
			float inputY = Input.GetAxisRaw("Vertical");

			Vector2 moveDirection = new Vector2(inputX, inputY).normalized;

			rb.linearVelocity = moveDirection * moveSpeed;
			if (inputX + inputY != 0 && _footstepTimer.CheckAndRestart())
			{
				OnFootstep?.Invoke();
			}
		}
	}
}