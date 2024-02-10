using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

public class SceneScript : MonoBehaviour {

    public GameObject positive;
    public GameObject neutral;
    public GameObject negative;

    public float pressure = 10.0f; // in Pascals (Pa)
    public float startTemp = 293.15f; // in Kelvins (K) = 20C
    public float volume = 2.5f; // in meters (m)
    public float range = 10.0f;
    private float numAtoms = 0;

    // Start is called before the first frame update
    void Start() {

        numAtoms = (int) ( ( pressure * volume ) / ( 8.31446261815324f * startTemp ) * 6.022f * MathF.Pow(10,10)); // * MathF.Pow(10,13)

        Debug.Log($"#atoms: {numAtoms}");

    }

    // Update is called once per frame
    void Update() {

        InstantiateAtoms();
        
    }

    void InstantiateAtoms() {

        GameObject[] currentAtoms = GameObject.FindGameObjectsWithTag("atom");

        int neutralAtoms = 0;

        foreach (var atom in currentAtoms) {
            
            if (atom.GetComponent<AtomBehaviorScript>().charge == 0 && atom.layer != 3 ) {
                neutralAtoms++;
            }

        }

        for (int i = 0; i < (int)pressure*10 - neutralAtoms; i++) {

            GameObject newAtom = Instantiate(neutral, new Vector3(NextFloat(-range,range), NextFloat(-range,range), NextFloat(-range,range) ), transform.rotation);
            newAtom.GetComponent<Rigidbody>().velocity = new Vector3(NextFloat(-range,range), NextFloat(-range,range), NextFloat(-range,range));
        }

    }

    static float NextFloat(float min, float max){
        System.Random random = new System.Random();
        double val = (random.NextDouble() * (max - min) + min);
        return (float)val;
    }
}
