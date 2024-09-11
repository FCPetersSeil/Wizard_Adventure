using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Wizard_Steuerung : MonoBehaviour
{

    public Rigidbody rb;    // Referenz auf den Rigidbody des Charakters, notwendig fuer physikalische Berechnungen
    public AudioSource audioSource; // Referenz auf die Audioquelle, die den Sprung-Sound abspielen wird
    //*** AUSKOMMENTIERT *** public GameObject fireballPrefab;   // Referenz auf das Feuerball-Prefab

    public float speed = 5f;    // Geschwindigkeit des Charakters
    public float jumpForce = 5f;    // Sprungkraft des Charakters
    //*** AUSKOMMENTIERT *** public float fireballForce = 10f;   // Geschwidigkeit des Abgefeuerten Feuerballs

    private bool isGrounded = true; // Boolesche Variable, die anzeigt, ob der Charakter auf dem Boden steht

    UnityEngine.Vector3 startPos;   //Speichert die Startposition des Charakters



    // Start wird einmalig beim Start des Spiels aufgerufen
    private void Start()
    {
        startPos = transform.position;  // Speichern der aktuellen Position als Startposition
    }





    // Update wird einmal pro Frame aufgerufen
    void Update()
    {
        //
        // Charakter Movement with Rigidbody
        //
        // Erfassen der horizontalen und vertikalen Eingaben des Spielers
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");
        // Die Position wird mithilfe des Rigidbodys geaendert
        rb.MovePosition(rb.position + (new Vector3(x, 0, z) * speed * Time.deltaTime));


        //
        //  Charakter fall (fallen)
        // 
        if (transform.position.y < -5)  // Ueberprueft ob der Charakter auf der Y-Achse unter -5 faellt
        {
            transform.position = startPos;  // Setzt den Charakter wieder auf die startPos
        }


        //
        //  Charakter Jump
        //
        // Ueberpruefen, ob die Leertaste gedrueckt wurde und der Charakter auf dem Boden steht
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded == true)
        {
            // Anwenden einer Kraft nach oben, um den Charakter springen zu lassen
            rb.AddForce(UnityEngine.Vector3.up * jumpForce, ForceMode.Impulse);

            isGrounded = false; // Setzen von isGrounded auf false, um zu verhindern, dass der Charakter mehrfach springt

            audioSource.Play(); // Abspielen des Sprung-Sounds
        }


        //
        // Charakter Rotation
        //
        if (x < 0)
        {
            transform.localRotation = Quaternion.Euler(0f, 270f, 0f);   // Nach links schauen
        }
        else if (x > 0)
        {
            transform.localRotation = Quaternion.Euler(0f, 90f, 0f);    // Nach rechts schauen
        }
        else if (z > 0)
        {
            transform.localRotation = Quaternion.Euler(0f, 0f, 0f);     // Nach vorne schauen
        }
        else if (z < 0)
        {
            transform.localRotation = Quaternion.Euler(0f, 180f, 0f);   // Nach hinten schauen
        }


        //************ AUSKOMMENTIERT *************
        //  Shoot Fireball
        //
        //if (Input.GetMouseButtonDown(0))
        //{
        //    ShootFireball();
        //}
        //*****************************************
    }


    //**************************************** AUSKOMMENTIERT *********************************************
    //  Fireball
    //
    //private void ShootFireball()
    //{
    //    if (fireballPrefab != null)
    //    {
    //        GameObject fireball = Instantiate(fireballPrefab, transform.position, transform.rotation);
    //        Rigidbody fireballRb = fireball.GetComponent<Rigidbody>();
    //        if (fireballRb != null)
    //        {
    // Berechnung der Richtung in die der Feuerball geschossen wird
    //            Vector3 fireballDirection = transform.forward;
    //            fireballRb.AddForce(fireballDirection * fireballForce, ForceMode.Impulse);
    //        }
    //    }
    //*****************************************************************************************************



    //
    // Charakter Kollision
    // 
    private void OnCollisionEnter(Collision collision)
    {
        // Setzen von isGrounded auf true, um zu ermoeglichen, dass der Charakter wieder springen kann
        isGrounded = true;
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