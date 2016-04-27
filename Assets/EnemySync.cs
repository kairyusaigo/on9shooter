using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class EnemySync : NetworkBehaviour {

	[SyncVar]
	private Vector3 syncPos; 
	[SyncVar]
	private Quaternion syncRot;

	[SerializeField]Transform playerTransform;
	[SerializeField]float lerpRate = 15;

	private Vector3 lastPos;
	private float PosThreshold = 0.5f;

	private Quaternion lastRot, lastCamRot;
	private float RotThreshold = 5;

	// Use this for initialization
	void FixedUpdate () {
		LerpTransform ();
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
}
