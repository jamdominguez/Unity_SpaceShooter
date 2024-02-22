using UnityEngine;
using System.Collections;

public class DestroyByContact : MonoBehaviour {

    
    public GameObject playerExplosion;
    public GameObject explosion;
    public GameObject drop;

    private bool canDrop = true;

    private void Start()
    {
        NotificationCenter.DefaultCenter().AddObserver(this, "StopDrop");
    }

    void OnTriggerEnter(Collider other) {
        
        if (other.CompareTag("GameBoundary") || other.CompareTag("Enemy")) return;
        if ((CompareTag("Item") && other.CompareTag("Enemy")) || (CompareTag("Enemy") && other.CompareTag("Item"))) return;
        
        //Pintar las explosiones
        if (explosion != null) Instantiate(explosion, transform.position, transform.rotation);


        if (other.CompareTag("Player")) {

            if (CompareTag("Item")) {
                Destroy(gameObject);
                NotificationCenter.DefaultCenter().PostNotification(this, "PowerUp");
                return;
            } else {
                Instantiate(playerExplosion, other.transform.position, other.transform.rotation);
                NotificationCenter.DefaultCenter().PostNotification(this, "PlayerDeleted");
            }

        } else if (other.CompareTag("PlayerBolt")) {            
            if (CompareTag("Item") || CompareTag("PlayerBolt")) {
                return;
            } else {
                NotificationCenter.DefaultCenter().PostNotification(this, "AsteroidDeleted");
                if (drop != null && canDrop)
                {
                    Instantiate(drop, transform.position, transform.rotation);
                    NotificationCenter.DefaultCenter().PostNotification(this, "NewDrop");
                }
            }
            

        }            
            
        //Eliminar los objetos de la escena
        
        Destroy(other.gameObject);
        Destroy(gameObject);
        
    }

    void StopDrop() {
        canDrop = false;
    }
}
