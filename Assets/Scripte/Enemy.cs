using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float speed = 2f;   // Geschwindigkeit des Gegners
    public int health = 3;     // Lebenspunkte des Gegners
    public bool isFrozen = false; // Status, ob der Gegner eingefroren ist
    private Transform player;  // Referenz auf den Spieler

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player").transform;  // Findet den Spieler anhand seines Tags
    }

    // Update is called once per frame
    void Update()
    {
        // Gegner bewegt sich nur, wenn er nicht gefroren ist
        if (!isFrozen)
        {
            MoveTowardsPlayer();
        }
    }

    // Methode zum Bewegen des Gegners in Richtung des Spielers
    void MoveTowardsPlayer()
    {
        /* Berechnet die Richtung zum Spieler und bewegt den Gegner
         * mit einer konstanten Geschwindigkeit auf ihn zu.
         */
        Vector3 direction = (player.position - transform.position).normalized;
        transform.position += direction * speed * Time.deltaTime;
    }

    // Methode, die aufgerufen wird, wenn der Gegner Schaden nimmt
    public void TakeDamage(int damage)
    {
        health -= damage;  // Verringert die Lebenspunkte des Gegners
        if (health <= 0)
        {
            Destroy(gameObject);  // Zerstört den Gegner, wenn seine Lebenspunkte 0 erreichen
        }
    }

    // Methode, um den Gegner für eine bestimmte Zeit einzufrieren
    public void Freeze()
    {
        isFrozen = true;  // Setzt den Gegner in den "Frozen"-Status
        StartCoroutine(Thaw());  // Startet die Koroutine zum Auftauen
    }

    // Koroutine, um den Gegner nach einer bestimmten Zeit wieder aufzutauen
    IEnumerator Thaw()
    {
        yield return new WaitForSeconds(3f);  // Wartet 3 Sekunden
        isFrozen = false;  // Gegner ist nicht mehr eingefroren
    }
}
