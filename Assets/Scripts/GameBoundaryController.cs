using UnityEngine;
using System.Collections;

public class GameBoundaryController : MonoBehaviour {

    // Cuando un objeto deja de tocar
    void OnTriggerExit(Collider other) {
        Destroy(other.gameObject);
	}
}
