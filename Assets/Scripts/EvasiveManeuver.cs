using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EvasiveManeuver : MonoBehaviour {

  public Boundary boundary;
  public float dodge;
  public float smoothing;
  public float tilt;
  public Vector2 maneuverTime;
  public Vector2 maneuverWait;
  public Vector2 startWait;

  private float currentSpeed;
  private float targetManeuver;
  private Rigidbody rigidbody;
  private Transform playerTransform;

	// Use this for initialization
	void Start () {
    rigidbody = GetComponent<Rigidbody>();
    playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
    currentSpeed = rigidbody.velocity.z;
		StartCoroutine(Evade());
	}

  IEnumerator Evade() {
    yield return new WaitForSeconds(Random.Range(startWait.x, startWait.y));

    while (true) {
      if (playerTransform != null) {
        // targetManeuver = Random.Range(1, dodge) * -Mathf.Sign(transform.position.x);
        targetManeuver = playerTransform.position.x - rigidbody.transform.position.x;
        yield return new WaitForSeconds(Random.Range(maneuverTime.x, maneuverTime.y));
        targetManeuver = 0;
        yield return new WaitForSeconds(Random.Range(maneuverWait.x, maneuverWait.y));
      }
    }
  }

	void FixedUpdate () {
    float newManeuver = Mathf.MoveTowards(rigidbody.velocity.x, targetManeuver, Time.deltaTime * smoothing);
    rigidbody.velocity = new Vector3(newManeuver, 0.0f, currentSpeed);
    rigidbody.position = new Vector3(
      Mathf.Clamp(rigidbody.position.x, boundary.xMin, boundary.xMax),
      0.0f,
      Mathf.Clamp(rigidbody.position.z, boundary.zMin, boundary.zMax)
    );
    rigidbody.rotation = Quaternion.Euler(0.0f, 0.0f, rigidbody.velocity.x * -tilt);
	}

  void Update() {
    if (playerTransform != null) {
      Quaternion lookAtPlayer = Quaternion.LookRotation(transform.position - playerTransform.position);
      // transform.rotation = lookAtPlayer
      rigidbody.rotation = Quaternion.Euler(0.0f, lookAtPlayer.eulerAngles.y, rigidbody.velocity.x * -tilt);
    }
  }
}
