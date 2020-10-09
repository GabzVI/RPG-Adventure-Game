using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

namespace RPG.Core
{
	public class CameraMovement : MonoBehaviour
	{
		[SerializeField] float camMoveSpeed = 20f;
		[SerializeField] float screenOffset = 10f;
		[SerializeField] GameObject player;
		[SerializeField] Vector2 screenLimit;
		[SerializeField] float scrollSpeed = 20f;
		[SerializeField] float maxYScroll = 120f;
		[SerializeField] float minYScroll = 20f;
		Vector3 vCameraPos;

		// Update is called once per frame
		private void Start()
		{
			vCameraPos.x = player.transform.position.x;
			vCameraPos.z = player.transform.position.z - 8.5f;
			transform.position = vCameraPos;
		}

		public void RecenterPlayer()
		{
			vCameraPos.x = player.transform.position.x;
			vCameraPos.z = player.transform.position.z - 8.5f;
			transform.position = vCameraPos;
		}

		private void Update()
		{
			vCameraPos = gameObject.transform.position;
			Cursor.lockState = CursorLockMode.Confined;
			if(Input.mousePosition.y >= Screen.height - screenOffset)
			{
				vCameraPos.z += camMoveSpeed * Time.deltaTime;
			}

			if(Input.mousePosition.y <= screenOffset)
			{
				vCameraPos.z -= camMoveSpeed * Time.deltaTime;
			}

			if (Input.mousePosition.x >= Screen.width - screenOffset)
			{
				vCameraPos.x += camMoveSpeed * Time.deltaTime;
			}

			if (Input.mousePosition.x <= screenOffset)
			{
				vCameraPos.x -= camMoveSpeed * Time.deltaTime;
			}

			if (Input.GetKey(KeyCode.Space))
			{
				vCameraPos.x = player.transform.position.x;
				vCameraPos.z = player.transform.position.z - 8.5f;
			}

			float scroll = Input.GetAxis("Mouse ScrollWheel");
			vCameraPos.y -= scroll * scrollSpeed * 100f * Time.deltaTime;

			vCameraPos.x = Mathf.Clamp(vCameraPos.x, -screenLimit.x, screenLimit.x);
			vCameraPos.y = Mathf.Clamp(vCameraPos.y, minYScroll, maxYScroll);
			vCameraPos.z = Mathf.Clamp(vCameraPos.z, -screenLimit.y, screenLimit.y);

			transform.position = vCameraPos;
		}
	}
}

