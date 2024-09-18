using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wizard_Stats : MonoBehaviour
{
    public int healthPoints = 10;  // Anzahl der Lebenspunkte des Wizards
    public int manaPoints = 100;   // Anzahl der Mana-Punkte des Wizards

    // Start is called before the first frame update
    void Start()
    {
        // Initialisiere Stats oder führe Setup durch, wenn nötig
    }

    // Update is called once per frame
    void Update()
    {
        // Hier könnte man regenerierende Fähigkeiten oder ähnliche Dinge umsetzen
    }

    // Methode, um Schaden zu nehmen
    public void TakeDamage(int damage)
    {
        healthPoints -= damage;  // Verringert die Lebenspunkte des Wizards um den Schadenswert
        if (healthPoints <= 0)
        {
            Die();  // Wenn die Lebenspunkte 0 erreichen, stirbt der Wizard
        }
    }

    // Methode, die aufgerufen wird, wenn der Wizard stirbt
    void Die()
    {
        // Zerstört das GameObject oder leitet den Game Over Bildschirm ein
        Debug.Log("Wizard ist gestorben!");
        Destroy(gameObject);  // Zerstört den Wizard
    }
}
