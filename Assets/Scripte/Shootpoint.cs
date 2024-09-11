using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Shootpoint : MonoBehaviour
{

    public Transform shootPoint;    // Referenz fuer die Zuordnung in Unity zu ShootPoint
    public GameObject bulletPrefab; // Referenz fuer die Zuordnung in Unity unter bulletPrefab

    public float shootForce = 50;       // Fuegt eine Feuerkraft von 50 hinzu
    public float cooldownTime = 3f;     // Fuegt ein Cooldown in hoehe von 3 sek. hinzu
    private float nextFireTime = 0f;    // Fuegt eine Zeit hinzu, wann der naechste Schuss fallen darf


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        //  Ueberprueft, ob die linke Maustasete & der Cooldown abgelaufen ist
        if (Input.GetMouseButtonDown(0) && Time.time >= nextFireTime)
        {
            //  Feuert den Schuss ab
            GameObject newBullet = Instantiate(bulletPrefab);
            newBullet.transform.position = shootPoint.transform.position;

            Rigidbody rb = newBullet.GetComponent<Rigidbody>();
            rb.AddForce(shootPoint.transform.forward * shootForce, ForceMode.Impulse);

            // Zerstoert das "newBullet" nach 2sek.
            Destroy(newBullet, 2);

            //  Setzt die naechste erlaubte Schusszeit auf die aktuelle, plus den Cooldown
            nextFireTime = Time.time + cooldownTime;
        }
    }
}





/*             ***** INFORMATION *****
 *  
 * Unterschied zwischen transform.position & rb.position
 *  
 * transform.position:  Aendert die Position eines Objekts direkt, unabhaengig von der Physik. 
 *  
 * (RigidBody)rb.position:  Aendert die Position eines Objekts ueber den "RigidBody", der die Physik beruecksichtigt.
 *                          
 * Warum wird VOID verwendet?   Wird verwendet, wenn eine Methode keinen Wert zurueckgibt. Sie fuehrt nur eine Aktion
 *                              aus oder reagiert auf Ereignisse, wie z.B. "OnCollisionEnter" in Unity.
 *                              
 *                              
 * transform.localRotation = Quaternion.Euler(0f, 270f, 0f);:   Diese Zeile setzt die lokale Rotation des Transform-Objektes.
 * 
 * transform:   In Unity ist transform eine Komponente eines GameObjects, die dessen Position, Rotation und Skalierung im Raum beschreibt.
 * 
 * localRotation:   localRotation bezieht sich auf die Rotation des GameObjects relativ zu seiner uebergeordneten Transform-Komponente.
 *                  Es ist ein Quaternion, das eine Rotation im 3D-Raum beschreibt.
 * 
 * Quaternion.Euler(0f, 270f, 0f):  Quaternion.Euler ist eine Methode, die eine Rotation in Form eines Quaternions aus Euler-Winkeln erzeugt.
 *                                  Hier wird die Rotation auf 0 Grad um die X-Achse, 270 Grad um die Y-Achse und 0 Grad um die Z-Achse gesetzt.
 *                                  In Unity entspricht dies einer Drehung des Objekts nach links (oder 270 Grad im Uhrzeigersinn).
 *                                  
 *                                  
 *             ***** BEISPIEL *****
 * 
 * Quaternion.Euler(30f, 45f, 60f), bedeutet das:   30 Grad Rotation um die X-Achse (Pitch).
 *                                                  45 Grad Rotation um die Y-Achse (Yaw).
 *                                                  60 Grad Rotation um die Z-Achse (Roll).
 * 
 *                                                       
 */