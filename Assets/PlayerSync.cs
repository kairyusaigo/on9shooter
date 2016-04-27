using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class PlayerSync : NetworkBehaviour {

	[SyncVar]
	private Vector3 syncPos; 
	[SyncVar]
	private Quaternion syncRot;
	[SyncVar]
	public bool syncFire;

	[SerializeField]Transform playerTransform;
	[SerializeField]float lerpRate = 15;

	private bool lastFire;
	private Vector3 lastPos;
	private float PosThreshold = 0.5f;

	private Quaternion lastRot, lastCamRot;
	private float RotThreshold = 5;

	// Use this for initialization
	void FixedUpdate () {
		TransmitTransform ();
		LerpTransform ();
	}

	void Update ()
	{
		if (isLocalPlayer) {
			#if !MOBILE_INPUT
			if (Input.GetButton ("Fire1")) {
				if (!lastFire) {
					CmdTransmitFire (true);
					lastFire = true;
				}
			} else {
				if (lastFire) {
					CmdTransmitFire (false);
					lastFire = false;
				}
			}
			#else
			if (CrossPlatformInputManager.GetAxisRaw("Mouse X") != 0 || CrossPlatformInputManager.GetAxisRaw("Mouse Y") != 0) {
				if (!lastFire) {
					CmdTransmitFire (true);
					lastFire = true;
				}
			} else {
				if (lastFire) {
					CmdTransmitFire (false);
					lastFire = false;
				}
			}
			#endif
		}
	}

	void LerpTransform () {
		if (!isLocalPlayer) {
			playerTransform.position = Vector3.Lerp (playerTransform.position, syncPos, Time.deltaTime * lerpRate);
			playerTransform.rotation = Quaternion.Lerp (playerTransform.rotation, syncRot, Time.deltaTime * lerpRate);
		}
	}

	[Command]
	void CmdProvideTransformToServer (Vector3 pos, Quaternion rot) {
		syncPos = pos;
		syncRot = rot;
	}

	[Command]
	public void CmdTransmitFire (bool isFire) {
		syncFire = isFire;
	}

	[ClientCallback]
	void TransmitTransform () {
		if (isLocalPlayer) {
			if (Vector3.Distance(playerTransform.position, lastPos) > PosThreshold || Quaternion.Angle(playerTransform.rotation, lastRot) > RotThreshold) {
				CmdProvideTransformToServer (playerTransform.position, playerTransform.rotation);
				lastPos = playerTransform.position;
			}
		}	
	}
}
