using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

	//Movement Variables
	public float movementspeed = 10;
	//Combat Variables
	public GameObject bullet;
	public GameObject bulletspawnleft;
	public GameObject bulletspawnright;
	public GameObject muzzleflash;
	public GameObject muzzlespawnleft;
	public GameObject muzzlespawnright;
	public float cooldowntime = 1;
	public float bulletspeed = 1;
	public float unaccuracydegrees = 10;
	private float rateoffiretimerleft = 0;
	private float rateoffiretimerright = 0;
	//Animation
	public GameObject revolverarmleft;
	public GameObject revolverarmright;
	//Camera
	public GameObject Camera;
	public GameObject localcompass;
	RaycastHit cameraRayHit;
	private float shakeintensity = 0;
	private GameObject cameraoriginpos;

	void Start () {
		//Getting Origin position for Camera Shake
		cameraoriginpos = new GameObject ();
		cameraoriginpos.transform.position = Camera.transform.position;
		cameraoriginpos.transform.parent = Camera.transform.parent;
		cameraoriginpos.name = "CameraOrigin";
	}

	void Update () { //Graphical (Not Physics)
		//Mouse Look
		if (Physics.Raycast(Camera.GetComponent<Camera>().ScreenPointToRay(Input.mousePosition), out cameraRayHit)) 
		{
			Vector3 targetPosition = new Vector3(cameraRayHit.point.x, transform.position.y, cameraRayHit.point.z);
			transform.LookAt(targetPosition);
		}
		//Camera Shake
		//Shaking the camera
		if (shakeintensity > 0) {
			Camera.transform.position = cameraoriginpos.transform.position + Random.insideUnitSphere * (shakeintensity);
			shakeintensity -= Time.deltaTime; //Shake intensity subtracts by 1 every second
		} else {
			shakeintensity = 0;
		}
		//Camera Control
		GameObject campivot = Camera.transform.parent.gameObject;
		if (Input.GetKey (KeyCode.Q)) {
			campivot.transform.RotateAround (localcompass.transform.position, -localcompass.transform.up, Time.deltaTime * 50);
		} else if (Input.GetKey (KeyCode.E)) {
			campivot.transform.RotateAround (localcompass.transform.position, localcompass.transform.up, Time.deltaTime * 50);
		}
		//Shooting
		//Left Revolver
		if (Input.GetKey(KeyCode.Mouse0) && rateoffiretimerleft < 0) { //Fire
			//Bullet Spawn
			GameObject bulletobject = Instantiate(bullet, bulletspawnleft.transform.position, Quaternion.Euler(bulletspawnleft.transform.eulerAngles + new Vector3(0, Random.Range(unaccuracydegrees, -unaccuracydegrees), 0))) as GameObject;
			bulletobject.GetComponent<Rigidbody> ().AddForce (bulletobject.transform.forward * bulletspeed * 1000);
			//Muzzle Flash
			GameObject muzzleflashobject = Instantiate(muzzleflash, muzzlespawnleft.transform.position, muzzlespawnleft.transform.rotation) as GameObject;
			muzzleflashobject.transform.parent = muzzlespawnleft.transform;
			//Animation
			revolverarmleft.GetComponent<Animator>().Play("LeftArmShoot");
			//ScreenShake
			if (shakeintensity <= 0.25f) {
				shakeintensity = 0.25f;
			}
			rateoffiretimerleft = cooldowntime;
		} else {
			rateoffiretimerleft -= Time.deltaTime;
		}
		//Right Revolver
		if (Input.GetKey(KeyCode.Mouse1) && rateoffiretimerright < 0) { //Fire
			//Bullet Spawn
			GameObject bulletobject = Instantiate(bullet, bulletspawnright.transform.position, Quaternion.Euler(bulletspawnright.transform.eulerAngles + new Vector3(0, Random.Range(unaccuracydegrees, -unaccuracydegrees), 0))) as GameObject;
			bulletobject.GetComponent<Rigidbody> ().AddForce (bulletobject.transform.forward * bulletspeed * 1000);
			//Muzzle Flash
			GameObject muzzleflashobject = Instantiate(muzzleflash, muzzlespawnright.transform.position, muzzlespawnright.transform.rotation) as GameObject;
			muzzleflashobject.transform.parent = muzzlespawnright.transform;
			//Animation
			revolverarmright.GetComponent<Animator>().Play("RightArmShoot");
			//ScreenShake
			if (shakeintensity <= 0.25f) {
				shakeintensity = 0.25f;
			}
			rateoffiretimerright = cooldowntime;
		} else {
			rateoffiretimerright -= Time.deltaTime;
		}
	}

	void FixedUpdate(){ //Physics Stuff
		//Player Control (Uses Camera Pivot because it is static at a 45 degree angle)
		if (GetComponent<Rigidbody> ().velocity.magnitude <= movementspeed) {
			if (Input.GetKey (KeyCode.W)) { //Up
				GetComponent<Rigidbody> ().AddForce (localcompass.transform.forward * 20);
			}
			if (Input.GetKey (KeyCode.S)) { //Down
				GetComponent<Rigidbody> ().AddForce (-localcompass.transform.forward * 20);
			}
			if (Input.GetKey (KeyCode.D)) { //Right
				GetComponent<Rigidbody> ().AddForce (localcompass.transform.right * 20);
			}
			if (Input.GetKey (KeyCode.A)) { //Left
				GetComponent<Rigidbody> ().AddForce (-localcompass.transform.right * 20);
			}
		} else {
			GetComponent<Rigidbody> ().velocity = GetComponent<Rigidbody> ().velocity.normalized * movementspeed;
		}
		//Is Moving (On x and z axis) and not falling
		if(Mathf.Abs(GetComponent<Rigidbody> ().velocity.y) <= 0.5f && Mathf.Sqrt(GetComponent<Rigidbody> ().velocity.x * GetComponent<Rigidbody> ().velocity.x + GetComponent<Rigidbody> ().velocity.z * GetComponent<Rigidbody> ().velocity.z) >= 1.0f){
			transform.GetChild (0).GetComponent<Animator> ().SetBool ("IsWalking", true);
		} else {
			transform.GetChild (0).GetComponent<Animator> ().SetBool ("IsWalking", false);
		}
	}
}
