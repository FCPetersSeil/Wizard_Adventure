using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class Wizard_Movement : MonoBehaviour
{
    public static Wizard_Movement instance;          // Ändert den Spieler zu einem Singleton

    public Rigidbody rb;                            // Referenz auf den Rigidbody des Charakters, notwendig für physikalische Berechnungen
    public AudioSource audioSource;                 // Referenz auf die Audioquelle, die den Sprung-Sound abspielen wird

    public float speed = 5f;                        
    public float jumpForce = 5f;                    
    public int health = 100;                        
    public int maxMana = 100;                       
    public int currentMana;                         
    public float manaRegenRate = 5f;                // Rate der Mana-Regeneration pro Sekunde
    public int manaCostPerFireball = 10;            // Mana-Kosten pro Feuerball

    public float invulnerabilityDuration = 2f;      // Dauer der Unverwundbarkeit in Sekunden

    // Frostangriff Variablen
    public bool isFrostAttackActive = false;        // Variable, die anzeigt, ob der Frostangriff aktiv ist
    public float frostCooldown = 5f;                // Cooldown zwischen Frostangriffen
    private float frostTimer = 0f;                  // Timer, um den Cooldown zu überwachen

    private bool isGrounded = true;                 // Gibt an, ob der Charakter auf dem Boden steht
    private bool isInvulnerable = false;            // Gibt an, ob der Spieler momentan unverwundbar ist
    private float invulnerabilityTimer = 0f;

    UnityEngine.Vector3 startPos;   // Speichert die Startposition des Charakters

    private void Awake()            // Stellt sicher, dass es nur eine Instanz verfügbar ist
    {
        if (instance == null)
        {
            instance = this;        // Weist die Instanz zu, wenn die nicht existiert
        }
        else
        {
            Destroy(gameObject);    // Zerstört das Objekt, wenn es bereits vorhanden ist
        }
    }

    // Start wird einmalig beim Start des Spiels aufgerufen
    private void Start()
    {
        startPos = transform.position;  // Speichern der aktuellen Position als Startposition
        currentMana = maxMana;          // Setzt das Mana zu Beginn auf das Maximum
    }

    // Update wird einmal pro Frame aufgerufen
    void Update()
    {
        // Mana automatisch regenerieren
        RegenerateMana();

        // Frostangriff aktivieren, wenn die rechte Maustaste gedrückt wird
        if (Input.GetMouseButton(1) && frostTimer <= 0f)                        // Rechte Maustaste
        {
            isFrostAttackActive = true;
        }
        else
        {
            isFrostAttackActive = false;
        }

        // Frostangriff hat einen Cooldown, der nach jedem Einsatz zurückgesetzt wird
        if (isFrostAttackActive)
        {
            frostTimer = frostCooldown;          // Setzt den Timer auf den Cooldown-Wert
        }

        
        if (frostTimer > 0f)                    // Cooldown-Timer für Frostangriff
        {
            frostTimer -= Time.deltaTime;
        }

        // Wenn der Frostangriff aktiv ist, überprüfe ob die Maus über einem Gegner ist
        if (isFrostAttackActive)
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.CompareTag("Enemy"))
                {
                                                                                
                    hit.collider.GetComponent<Enemy_Movement>().Freeze();           // Rufe die Freeze-Methode des Gegners auf
                }
            }
        }

        //
        // Charakter Movement with Rigidbody
        //
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");
        rb.MovePosition(rb.position + (new Vector3(x, 0, z) * speed * Time.deltaTime));

        //
        // Charakter fall (fallen)
        // 
        if (transform.position.y < -5)
        {
            transform.position = startPos;                          // Setzt den Charakter wieder auf die startPos
        }

        //
        // Charakter Jump
        //
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded == true)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            isGrounded = false;
            audioSource.Play();                                     // Abspielen des Sprung-Sounds
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

        //
        // Unverwundbarkeits-Timer aktualisieren
        //
        if (isInvulnerable)
        {
            invulnerabilityTimer -= Time.deltaTime;
            if (invulnerabilityTimer <= 0)
            {
                isInvulnerable = false;
            }
        }
    }

    //
    // Methode zum Schießen (Schuss wird nur abgefeuert, wenn genug Mana vorhanden ist)
    //
    public bool TryShootFireball()
    {
        if (currentMana >= manaCostPerFireball)
        {
            currentMana -= manaCostPerFireball;     // Mana reduzieren
            return true;                            // Feuerball kann abgeschossen werden
        }
        return false;                               // Nicht genug Mana, Feuerball kann nicht abgeschossen werden
    }

    //
    // Mana regenerieren
    //
    private void RegenerateMana()
    {
        if (currentMana < maxMana)
        {
            currentMana += Mathf.RoundToInt(manaRegenRate * Time.deltaTime); // Mana pro Sekunde regenerieren
            currentMana = Mathf.Clamp(currentMana, 0, maxMana); // Sicherstellen, dass Mana nicht über das Maximum steigt
        }
    }

    //
    // Charakter Kollision
    // 
    private void OnCollisionEnter(Collision collision)
    {
        // Setzen von isGrounded auf true, um zu ermöglichen, dass der Charakter wieder springen kann
        isGrounded = true;

        // Überprüfen, ob der Gegner den Spieler berührt
        if (collision.gameObject.CompareTag("Enemy") && !isInvulnerable)
        {
            TakeDamage(10); // Beispielwert für Schaden
            isInvulnerable = true;
            invulnerabilityTimer = invulnerabilityDuration;
        }
    }

    //
    // Methode, um Schaden zu nehmen
    //
    public void TakeDamage(int damage)
    {
        health -= damage;
        Debug.Log("Spieler hat Schaden erlitten! Aktuelle Gesundheit: " + health);

        if (health <= 0)
        {
            Debug.Log("Spieler ist gestorben!");
        }
    }
}




/*             ***** INFORMATION *****
 *  
 * Unterschied zwischen transform.position & rb.position
 *  
 * transform.position:  Ändert die Position eines Objekts direkt, unabhängig von der Physik. 
 *  
 * (RigidBody)rb.position:  Ändert die Position eines Objekts über den "RigidBody", der die Physik berücksichtigt.
 *                          
 * Warum wird VOID verwendet?   Wird verwendet, wenn eine Methode keinen Wert zurückgibt. Sie führt nur eine Aktion
 *                              aus oder reagiert auf Ereignisse, wie z.B. "OnCollisionEnter" in Unity.
 *                              
 *                              
 * transform.localRotation = Quaternion.Euler(0f, 270f, 0f);:   Diese Zeile setzt die lokale Rotation des Transform-Objektes.
 * 
 * transform:   In Unity ist transform eine Komponente eines GameObjects, die dessen Position, Rotation und Skalierung im Raum beschreibt.
 * 
 * localRotation:   localRotation bezieht sich auf die Rotation des GameObjects relativ zu seiner übergeordneten Transform-Komponente.
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