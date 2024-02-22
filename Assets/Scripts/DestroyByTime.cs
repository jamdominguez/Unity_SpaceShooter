using UnityEngine;
using System.Collections;

public class DestroyByTime : MonoBehaviour {

    public float liveTime;

	// Use this for initialization
	void Start () {
        Destroy(this.gameObject, liveTime);
	}
	

}
