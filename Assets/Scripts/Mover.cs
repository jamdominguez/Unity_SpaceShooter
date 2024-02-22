using UnityEngine;
using System.Collections;

public class Mover : MonoBehaviour {

    public float speed;

    private Rigidbody rigidBody;

    void Awake() {
        this.rigidBody = GetComponent<Rigidbody>();
    }

	// Use this for initialization
	void Start () {
        this.rigidBody.velocity = speed * transform.forward;   
    }
}
