using UnityEngine;
using System.Collections;

public class PlaneController : MonoBehaviour {

	//Movement Variables
	public float movementspeed; //How fast the plane can accelerate
	public float minspeed;
	public float maxspeed;
	public float turningspeed = 1; //How fast the plane can turn
	public float airspeed = 0; //The current speed of plane
	public float maxrotationspeed; //Max speed airplane can rotate
	//Plane GameObjects
	public GameObject planebody; //Mesh and Collider
	public GameObject planepropeller; //Plane Propeller Mesh
	public GameObject frontleftflap;
	public GameObject frontrightflap;
	public GameObject backleftflap;
	public GameObject backrightflap;
	public GameObject leftwingrenderer;
	public GameObject rightwingrenderer;
	//Game Variables
	public bool ispaused = false; //Paused Game
	public bool iscrashed = false; //If plane is crashed

	void Start(){
		//Limiting turning speed
		GetComponent<Rigidbody>().maxAngularVelocity = maxrotationspeed;
	}

	void Update () {
		if(ispaused == false){ //If game isn't paused go through loop
			if (iscrashed == false) { //If plane hasn't crashed
				//Movement
				//Measuring Speed
				airspeed = GetComponent<Rigidbody> ().velocity.magnitude;
				//Air Movement
				GetComponent<Rigidbody> ().velocity = transform.forward * movementspeed;
				//Speed Control
				if (Input.GetKey (KeyCode.Mouse0)) { //Throttle Up
					movementspeed += Time.deltaTime * 20;
				} else if (Input.GetKey (KeyCode.Mouse1)) { //Throttle Down
					movementspeed -= Time.deltaTime * 20;
				}
				if (movementspeed < minspeed) { //Slowest Speed
					movementspeed = minspeed;
				}
				if (movementspeed > maxspeed) { //Max Speed
					movementspeed = maxspeed;
				}
				//Turning
				Vector2 mousescreenpos = Input.mousePosition;
				float xturn = ((mousescreenpos.y - Screen.height / 2) / (Screen.height / 2)); 
				float yturn = ((mousescreenpos.x - Screen.width / 2) / (Screen.width / 2));
				GetComponent<Rigidbody> ().AddTorque (-transform.forward * turningspeed * yturn * Time.deltaTime * 1500); //Right and Left
				GetComponent<Rigidbody> ().AddTorque (-transform.right * turningspeed * xturn * Time.deltaTime * 1500); //Up and Down
				//Model Turning
				//Model Flaps
				 //Roll Flaps
				float rollflapangle = yturn * 30;
				frontleftflap.transform.localEulerAngles = new Vector3(frontleftflap.transform.localEulerAngles.x, frontleftflap.transform.localEulerAngles.y, 90 - rollflapangle);
				frontrightflap.transform.localEulerAngles = new Vector3(frontrightflap.transform.localEulerAngles.x, frontrightflap.transform.localEulerAngles.y, 90 + rollflapangle);
				//Turning Speed Adjust
				float speedpercentage = (movementspeed - minspeed) / (maxspeed - minspeed);
				turningspeed = 2 - speedpercentage; //The slower the plane the faster it can turn
				//Propeller Control
				planepropeller.transform.eulerAngles += new Vector3 (0, 0, 25 * (speedpercentage + 0.2f));
				//Air Ripples (Lines in the air)
				float trailopacity = speedpercentage / 5;
				Debug.Log (trailopacity);
				leftwingrenderer.GetComponent<TrailRenderer> ().material.SetColor("_TintColor", new Color (1, 1, 1, trailopacity));
				rightwingrenderer.GetComponent<TrailRenderer> ().material.SetColor("_TintColor", new Color (1, 1, 1, trailopacity));
			} else { //If plane has crashed
				//Crashed Variables
				GetComponent<Rigidbody>().useGravity = true;
				GetComponent<Rigidbody> ().angularDrag = 0.25f;
			}
		}
	}
}
