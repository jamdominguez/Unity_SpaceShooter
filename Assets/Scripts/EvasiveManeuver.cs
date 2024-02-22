using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EvasiveManeuver : MonoBehaviour
{
    public Vector2 startWait;
    public Vector2 maneuverTime;
    public Vector2 maneuverWait;
    private PlayerBoundary boundary;
    public float dodge;
    public float smoothing;
    public float tiltAngle;

    private float targetManeuver;
    private Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Evade());
    }

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        boundary = new PlayerBoundary();

    }

    IEnumerator Evade() {
        yield return new WaitForSeconds(Random.Range(startWait.x, startWait.y)); // wait for start

        while (true) {
            targetManeuver = Random.Range(1, dodge) * -Mathf.Sign(transform.position.x); // calculate maneuver velocity
            yield return new WaitForSeconds(Random.Range(maneuverTime.x, maneuverTime.y)); // maneuver time because targetManeuver is != 0
            targetManeuver = 0; // stop maneuver
            yield return new WaitForSeconds(Random.Range(maneuverWait.x, maneuverWait.y)); // wait for next maneuver loop
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float newManuver = Mathf.MoveTowards(rb.velocity.x, targetManeuver, Time.deltaTime * smoothing); // progressive aceleration
        rb.velocity = new Vector3(newManuver, 0.0f, rb.velocity.z);
        Tilt();
        BoundsClamp();
    }

    void Tilt()
    {
        this.rb.rotation = Quaternion.Euler(0f, 0f, this.rb.velocity.x * tiltAngle * (-0.1f));
    }

    //Evitar que la nave salga de pantalla
    void BoundsClamp()
    {
        float xClamp = Mathf.Clamp(this.rb.position.x, boundary.xMin, boundary.xMax);
        rb.position = new Vector3(xClamp, 0f, rb.position.z);
    }

}
