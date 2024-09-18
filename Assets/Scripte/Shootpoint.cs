using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shootpoint : MonoBehaviour
{
    public Transform shootPoint;        // Referenz für die Zuordnung in Unity zu shootPoint
    public GameObject bulletPrefab;     // Referenz für die Zuordnung in Unity unter bulletPrefab

    public float shootForce = 25f;      // Fügt eine Feuerkraft hinzu
    public float cooldownTime = 3f;     // Cooldown-Zeit zwischen Schüssen
    private float nextFireTime = 0f;    // Zeitpunkt für den nächsten Schuss

    public int maxMana = 100;           // Maximales Mana des Spielers
    public int currentMana;             // Aktuelles Mana des Spielers
    public int manaCostPerShot = 10;    // Mana-Kosten für jeden Schuss
    public float manaRegenRate = 5f;    // Wie viel Mana pro Sekunde regeneriert wird

    private void Start()
    {
        currentMana = maxMana;          // Setzt das Mana des Spielers auf Maximum zu Beginn
    }

    // Update wird einmal pro Frame aufgerufen
    void Update()
    {
        // Mana regenerieren
        if (currentMana < maxMana)
        {
            currentMana += Mathf.FloorToInt(manaRegenRate * Time.deltaTime);  // Mana mit Zeit regenerieren
            currentMana = Mathf.Min(currentMana, maxMana); // Mana darf nicht über das Maximum hinausgehen
        }

        // Überprüfen, ob die linke Maustaste gedrückt wird, der Cooldown abgelaufen ist und genug Mana vorhanden ist
        if (Input.GetMouseButtonDown(0) && Time.time >= nextFireTime && currentMana >= manaCostPerShot)
        {
            // Schuss abfeuern
            GameObject newBullet = Instantiate(bulletPrefab);
            newBullet.transform.position = shootPoint.transform.position;

            Rigidbody rb = newBullet.GetComponent<Rigidbody>();
            rb.AddForce(shootPoint.transform.forward * shootForce, ForceMode.Impulse);

            // Zerstört das "newBullet" nach 2 Sekunden
            Destroy(newBullet, 2);

            // Setzt die nächste erlaubte Schusszeit auf die aktuelle, plus den Cooldown
            nextFireTime = Time.time + cooldownTime;

            // Mana-Kosten abziehen
            currentMana -= manaCostPerShot;

            // Füge das Feuerball-Verhalten hinzu (Bewegung und Kollision)
            newBullet.AddComponent<Fireball>();  // Hänge das Fireball-Skript an das neue Projektil
        }
    }
}

public class Fireball : MonoBehaviour
{
    public float fireballSpeed = 10f;

    void Start()
    {
        // Stelle sicher, dass der Feuerball sich bewegt, falls keine externe Kraft hinzugefügt wurde
        Rigidbody rb = GetComponent<Rigidbody>();
        rb.velocity = transform.forward * fireballSpeed;
    }

    // Trigger wird ausgelöst, wenn der Feuerball mit einem Objekt kollidiert
    private void OnTriggerEnter(Collider other)
    {
        // Prüfen, ob der getroffene Collider den Tag "Enemy" hat
        if (other.CompareTag("Enemy"))
        {
            // Gegner wird zerstört
            Destroy(other.gameObject);

            // Feuerball zerstören, nachdem er den Gegner getroffen hat
            Destroy(gameObject);
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
 ***************************************************************************************************************************************************                              
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