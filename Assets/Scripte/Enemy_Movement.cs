using UnityEngine;
using System.Collections;

public class Enemy_Movement : MonoBehaviour
{
    public float speed = 2f;
    private bool isFrozen = false; // Variable, um den Frozen-Status des Gegners zu speichern
    private float frozenTimer = 0f; // Timer für den Frozen-Status
    public float frozenDuration = 5f; // Dauer, wie lange der Gegner eingefroren bleibt

    public float respawnDelay = 2f; // Verzögerung bis zur Wiederbelebung
    public float respawnRadius = 1f; // Radius um den Spieler, in dem der Gegner neu erscheinen kann

    private Vector3 initialPosition; // Die Position, an der der Gegner gespawnt wurde
    private Quaternion initialRotation; // Die Rotation, an der der Gegner gespawnt wurde

    private void Start()
    {
        // Speichern der initialen Position und Rotation
        initialPosition = transform.position;
        initialRotation = transform.rotation;
    }

    void Update()
    {
        // Frost-Status-Timer aktualisieren
        if (isFrozen)
        {
            frozenTimer -= Time.deltaTime;
            if (frozenTimer <= 0)
            {
                isFrozen = false; // Frozen-Status zurücksetzen
            }
        }

        // Nur weitermachen, wenn der Spieler existiert und der Gegner nicht eingefroren ist
        if (Wizard_Movement.instance != null && !isFrozen)
        {
            // Berechne die Richtung zum Spieler
            Vector3 direction = Wizard_Movement.instance.transform.position - transform.position;
            direction.y = 0;  // Ignoriere die vertikale Bewegung
            direction.Normalize();

            // Bewege den Gegner in Richtung des Spielers
            transform.position += direction * speed * Time.deltaTime;

            // Lasse den Gegner den Spieler anschauen
            transform.LookAt(Wizard_Movement.instance.transform.position);
        }
    }

    // Kollisionserkennung mit anderen Objekten
    private void OnCollisionEnter(Collision collision)
    {
        // Prüfen, ob der Gegner mit dem Feuerball kollidiert
        if (collision.gameObject.CompareTag("Fireball"))
        {
            StartCoroutine(DestroyAndRespawn()); // Gegner wird zerstört und nach Verzögerung wieder gespawnt
        }
    }

    // Methode, um den Gegner einzufrieren
    public void Freeze()
    {
        isFrozen = true;
        frozenTimer = frozenDuration; // Setzt den Timer für den Frozen-Status
    }

    // Coroutine zum Zerstören und Wiederbeleben des Gegners
    private IEnumerator DestroyAndRespawn()
    {
        // Gegner deaktivieren
        Debug.Log("Gegner wird deaktiviert.");
        gameObject.SetActive(false);

        // Warte für die spezifizierte Verzögerung
        yield return new WaitForSeconds(respawnDelay);

        // Zufällige Position innerhalb des respawnRadius um den Spieler
        Vector3 spawnPosition = GetRandomSpawnPosition();
        Debug.Log("Neupositionierung des Gegners auf: " + spawnPosition);
        transform.position = spawnPosition;

        // Gegner zurücksetzen und wieder aktivieren
        transform.rotation = initialRotation;
        gameObject.SetActive(true);
        Debug.Log("Gegner wieder aktiviert.");
    }

    // Methode zur Berechnung einer zufälligen Position innerhalb eines bestimmten Radius um den Spieler
    private Vector3 GetRandomSpawnPosition()
    {
        if (Wizard_Movement.instance == null)
        {
            // Rückfall, wenn die Instanz des Wizard_Movement nicht gefunden wurde
            Debug.LogWarning("Wizard_Movement Instanz nicht gefunden. Rückfall zur Initialposition.");
            return initialPosition;
        }

        Vector3 playerPosition = Wizard_Movement.instance.transform.position;

        // Zufällige Richtung und Distanz
        Vector3 randomDirection = Random.insideUnitSphere * respawnRadius;
        randomDirection.y = 0; // Nur horizontale Positionierung
        Vector3 spawnPosition = playerPosition + randomDirection;

        return spawnPosition;
    }
}