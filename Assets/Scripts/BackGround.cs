using UnityEngine;
using System.Collections;

public class BackGround : MonoBehaviour {

    public float initTime = 0f;
    public float speed = 0f;

    private Renderer meshRenderer;

	// Use this for initialization
	void Awake () {
        this.meshRenderer = this.GetComponent<Renderer>();

    }
	
	// Update is called once per frame
	void Update () {
        float yOffset = ((Time.time - initTime) * speed);
        this.meshRenderer.material.mainTextureOffset = new Vector2(0, yOffset);
    }
}
