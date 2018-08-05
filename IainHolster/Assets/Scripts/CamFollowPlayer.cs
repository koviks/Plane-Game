using UnityEngine;
using System.Collections;

public class CamFollowPlayer : MonoBehaviour {

	public GameObject player;
	public float followspeed;

	void FixedUpdate () {
		transform.position = Vector3.Lerp (transform.position, player.transform.position, Time.fixedDeltaTime * followspeed);
	}
}
