using System.Xml.Schema;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagnetScript : MonoBehaviour {

    public int magnetType = 0;
    public float remanence_field = 0.0f;
    public float dipole_moment;
    public float permeability = 4*Mathf.PI * Mathf.Pow(10,-7); // (Tesla * meter / Ampere);

    // Start is called before the first frame update
    void Start() {
        float volume = Mathf.PI * transform.localScale.x * transform.localScale.x * transform.localScale.y / 1000000000;

        dipole_moment = remanence_field * volume / this.permeability;
        
        Debug.Log(dipole_moment);
    }

    /*
    // Update is called once per frame
    void Update() {
        
    }*/
}
