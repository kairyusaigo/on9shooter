using UnityEngine;
using System.Collections;

public class PlayerSetup : MonoBehaviour {

	void Start () {
		if (!GetComponent <PlayerSync>().isLocalPlayer) {
			return;
		}
		this.name = "Player";
		GameObject.Find ("Main Camera").GetComponent<CameraFollow> ().target = this.transform;
		GameObject.Find ("Main Camera").GetComponent<CameraFollow> ().enabled = true;
	}
}
