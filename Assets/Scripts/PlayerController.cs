using UnityEngine;
using System.Collections;
using UnityStandardAssets.CrossPlatformInput;

public class PlayerController : MonoBehaviour {
    [Header("Movement")]
    public int speed = 9;
    public float tiltAngle = 45f;
    [Header("Shooting")]
    public GameObject shot;
    public GameObject shotN2;    
    public Transform shotSpawnTransform;
    public Transform shotSpawnN2;
    public Transform shotSpawnDL;
    public Transform shotSpawnDR;
    public float fireRate;
    public int points = 0;

    private Rigidbody rigidBody;
    private PlayerBoundary boundary;
    private PlayerUIInpunController uiInpunt;
    private float nextFire;
    private GameObject currentShot;
    private GameObject currentAlternativetShot;
    private int shotLevel = 1;
    private int MAX_DROPS = 6;
    private int dropsInstantiated = 0;

    private void Start()
    {
        NotificationCenter.DefaultCenter().AddObserver(this, "PowerUp");
    }

    void Awake() {
        this.rigidBody = GetComponent<Rigidbody>();
        this.boundary = new PlayerBoundary();
        this.uiInpunt = new PlayerUIInpunController();
        this.currentShot = this.shot;
	}

    // Actualizar en cada frame
    void Update() {
        this.uiInpunt.UpdateShooting();
        bool canShoot = uiInpunt.isAShoot && Time.time > nextFire;
        if (canShoot) {
            nextFire = Time.time + fireRate;
           // NotificationCenter.DefaultCenter().AddObserver(this, "AsteroidDeleted");

            if(shotLevel == 1) Instantiate(this.currentShot, this.shotSpawnTransform.position, shotSpawnTransform.rotation);
            else Instantiate(this.currentShot, this.shotSpawnN2.position, shotSpawnN2.rotation);

            if (shotLevel >= 4) {
                Instantiate(this.currentAlternativetShot, this.shotSpawnDL.position, shotSpawnDL.rotation);
                Instantiate(this.currentAlternativetShot, this.shotSpawnDR.position, shotSpawnDR.rotation);
            }
        }
    }

    void AsteroidDeleted() {
        this.points += 10;
    }

    void changeWeapon() {
        if (this.points >= 150) {
            this.fireRate = 0.1f;
        }
        if (this.points >= 100) {
            this.currentShot = this.shotN2;            
        } else {
            this.currentShot = this.shot;
        }
    }

    // Actualizar fisicas
    void FixedUpdate() {        
        this.uiInpunt.UpdateMovement();
        MoveShip();
        TiltShip();
        BoundsClamp();        
    }

    //Mover la nave
    void MoveShip() {
        Vector3 movement = new Vector3(speed * this.uiInpunt.hMove, 0f, speed * this.uiInpunt.vMove);
        this.rigidBody.velocity = movement;
    }

    //Girar la nave segun velocidad que lleve en x
    void TiltShip() {
        this.rigidBody.rotation = Quaternion.Euler(0f, 0f, this.rigidBody.velocity.x * tiltAngle * (-0.1f));
    }

    //Evitar que la nave salga de pantalla
    void BoundsClamp() {
        float xClamp = Mathf.Clamp(this.rigidBody.position.x, boundary.xMin, boundary.xMax);
        float zClamp = Mathf.Clamp(this.rigidBody.position.z, boundary.zMin, boundary.zMax);
        this.rigidBody.position = new Vector3(xClamp, 0f, zClamp);
    }

    void PowerUp() {
        shotLevel++;
        switch (shotLevel) {
            case 2: currentShot = shotN2; break; // double
            case 3: fireRate = 0.22f; break; // incrase fireRate
            case 4: currentAlternativetShot = shot; break;// increase shots
            case 5: fireRate = 0.11f; break; // incrase fire Rate
            case 6: currentAlternativetShot = shotN2; break; // alternative shot double
            case 7: fireRate = 0.06f;break; // incrase fire Rate
        }
        Debug.Log("PowerUp - shotLevel: " + shotLevel);
    }

    void NewDrop() {
        dropsInstantiated++;
        if (dropsInstantiated == MAX_DROPS) NotificationCenter.DefaultCenter().PostNotification(this, "StopDrop");

    }
}
//==============================================================================================================================================================
[System.Serializable]
public class PlayerBoundary {
    public float xMin = -6f;
    public float xMax = 6f;
    public float zMin = -4;  
    public float zMax = 14;
}
//==============================================================================================================================================================
public class PlayerUIInpunController {
    //PC - 
    public float hMove;
    public float vMove;
    public bool isAShoot;

    //Actualizar la respuesta del usuario
    public void UpdateMovement() {
        this.hMove = CrossPlatformInputManager.GetAxis("Horizontal");
        this.vMove = CrossPlatformInputManager.GetAxis("Vertical");
    }

    public void UpdateShooting() {
        this.isAShoot = CrossPlatformInputManager.GetButton("Fire1");
    }
}
//==============================================================================================================================================================