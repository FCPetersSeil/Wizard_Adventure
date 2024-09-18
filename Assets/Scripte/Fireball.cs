using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Feuerballd : MonoBehaviour
{

    public Vector3 moveDirection;
    public float speed = 10f;
    public float lifeTime = 2f;



    // Start is called before the first frame update
    void Start()
    {
        /*  Zerstört das Objekt nach einer Betimmten Zeit
         *  
         *  Destroy = "Zerstör" (gameObject = "Das Object", lifeTime = "nach 2f");
         */  
        Destroy(gameObject, lifeTime);
    }

    // Update is called once per frame
    void Update()
    {
        /*  Bewegt den Feuerball in die Richtung
         *  
         *  transform.Translate = "eine Methode die das GameObject im Raum verschiebt"
         *  
         *  (moveDirection = "gibt die Richtung an, Vector3" * speed = "wie schnell sich das Object bewegen soll" * Time.deltaTime = "Ist Wichtig! Für die frame-unabhängige Bewegung)
         */  
        transform.Translate(moveDirection * speed * Time.deltaTime);   
    }
}






/*             ***** INFORMATION *****
 *
 * moveDirection * speed * Time.deltaTime:  Dies ist der Vektor, der an Translate übergeben wird und 
 *                                          die Verschiebung des GameObjects beschreibt.
 *                                          
 * moveDirection:   Ein Vektor (Vector3), der die Richtung der Bewegung angibt (z.B. `(1, 0, 0)` für die X-Achse).
 * 
 * Geschwindigkeit: Ein skalarer Wert (`float`), der die Geschwindigkeit der Bewegung angibt. Höhere Werte bedeuten eine schnellere Bewegung.
 * 
 * Time.deltaTime:  Die Zeit seit dem letzten Bild. Wichtig für eine gleichmäßige Bewegung, unabhängig von der Framerate.
 * 
 * 
 * Der Ausdruck (moveDirection * speed * Time.deltaTime);   berechnet die Verschiebung des GameObjects für den aktuellen Frame
 *                                                          und sorgt für eine konsistente Bewegung bei unterschiedlichen Frameraten.
 */
          