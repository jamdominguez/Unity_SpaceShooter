using UnityEngine;
using System.Collections;

public class RandomRotator : MonoBehaviour {

    public float tumble = 3f;
    public float speed = 5f;

    private Rigidbody rigidBody;

    void Awake() {
        rigidBody = GetComponent<Rigidbody>();
    }

	// Use this for initialization
	void Start () {      
        rigidBody.angularVelocity = tumble * Random.insideUnitSphere;        
    }    
}
