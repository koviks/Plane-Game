using UnityEngine;
using System.Collections;

public class BulletScript : MonoBehaviour {

	public bool friendlybullet = true; //Fired from player
	public GameObject bullethitfx;
	public GameObject bloodhitfx;

	void OnTriggerEnter (Collider bullethit){
		if (friendlybullet) { //If bullet was fired from player
			if (bullethit.gameObject.tag == "Enemy") { //Hit Enemy
				GameObject bloodexplosion = Instantiate (bloodhitfx, transform.position, transform.rotation) as GameObject;
			} else if (bullethit.gameObject.tag == "Player") { //Hit Player

			} else if (bullethit.transform.name != "Player" && bullethit.transform.name != "PlayerPointCollider" && !bullethit.transform.name.Contains ("Bullet")) { //Hit Enviroment
				GameObject bulletexplosion = Instantiate (bullethitfx, transform.position, transform.rotation) as GameObject;
				Destroy (this.gameObject);
			}
		}
	}
}
