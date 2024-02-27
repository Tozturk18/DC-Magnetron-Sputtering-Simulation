using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class ElectricFields : MonoBehaviour {

    //public float simulationSpeed = 1.0f;
    public GameObject[] electrodes;
    public GameObject[] magnets;

    private GameObject[] atoms;
    public Vector3 forceVector;

    // Start is called before the first frame update
    void Start() {
        
        /*yield return new WaitForSeconds(1f);
        atoms = GameObject.FindGameObjectsWithTag("atom");*/
    }

    // Update is called once per frame
    void Update() {
        
        GenField();

    }

    void GenField() {

        atoms = GameObject.FindGameObjectsWithTag("atom");

        Vector3 avgVelocity = new Vector3(0,0,0);
        float avgMass = 0.0f;

        foreach (var atom in atoms) {

            forceVector = Vector3.zero;

            float force = 0.0f;
            float charge = atom.GetComponent<AtomBehaviorScript>().charge;

            float dx = 0.0f, dy = 0.0f, dz = 0.0f, radius = 0.0f;

            GameObject[] others = atoms.Where( val => val != atom ).ToArray();
            foreach (var other in others) {

                float otherCharge = other.GetComponent<AtomBehaviorScript>().charge;

                dx = atom.transform.position.x - other.transform.position.x;
                dy = atom.transform.position.y - other.transform.position.y;
                dz = atom.transform.position.z - other.transform.position.z;

                radius = MathF.Sqrt( MathF.Pow(dx, 2) + MathF.Pow(dy, 2) + MathF.Pow(dz, 2) );

                if (radius != 0.0f) { 

                    force = (float)( 1 / ( 4 * Mathf.PI * 55.26349406 * MathF.Pow(10, -5)  ) ) * ( charge * otherCharge / Mathf.Pow( radius, 2 ) ); 

                    dx /= radius;
                    dy /= radius;
                    dz /= radius;

                    forceVector += new Vector3(MathF.Round(force * dx), MathF.Round(force * dy), MathF.Round(force * dz));

                }

            }

            foreach (var electrode in electrodes) {
                
                float voltage = electrode.GetComponent<PlatesScript>().voltage;

                GameObject VCC = electrode.GetComponent<PlatesScript>().VCC;
                GameObject GND = electrode.GetComponent<PlatesScript>().GND;

                float VCCArea = VCC.transform.localScale.x * VCC.transform.localScale.z;
                float GNDArea = GND.transform.localScale.x * GND.transform.localScale.z;

                dx = VCC.transform.position.x - GND.transform.position.x;
                dy = VCC.transform.position.y - GND.transform.position.y;
                dz = VCC.transform.position.z - GND.transform.position.z;

                radius = MathF.Sqrt( MathF.Pow(dx, 2) + MathF.Pow(dy, 2) + MathF.Pow(dz, 2) );

                for (int i = (int)(transform.position.x-VCC.transform.localScale.x/2); i < VCC.transform.localScale.x/2; i++) {

                    for (int j = (int)(transform.position.z-VCC.transform.localScale.z/2); j < VCC.transform.localScale.z/2; j++) {

                        //float VCCDistance = Vector3.Distance(atom.transform.position, VCC.transform.position);

                        dx = atom.transform.position.x - i;
                        dy = atom.transform.position.y - VCC.transform.position.y;
                        dz = atom.transform.position.z - j;

                        float VCCDistance = MathF.Sqrt( MathF.Pow(dx, 2) + MathF.Pow(dy, 2) + MathF.Pow(dz, 2) );
                        

                        //force = (float)( ( (-charge * i * j * voltage) / (4 * MathF.PI * radius) ) * ( 1 / MathF.Pow(VCCDistance, 2) + 1 / MathF.Pow(GNDDistance, 2) ) );

                        force = (float) ( ( (charge * VCCArea * voltage) / (4 * MathF.PI * radius) ) * ( 1 / MathF.Pow(VCCDistance, 2) ) );

                        dx /= VCCDistance;
                        dy /= VCCDistance;
                        dz /= VCCDistance;

                        forceVector += new Vector3(MathF.Round(force * dx), MathF.Round(force * dy), MathF.Round(force * dz));

                        //float GNDDistance = Vector3.Distance(atom.transform.position, GND.transform.position);

                        dx = atom.transform.position.x - i;
                        dy = atom.transform.position.y - GND.transform.position.y;
                        dz = atom.transform.position.z - j;

                        float GNDDistance = MathF.Sqrt( MathF.Pow(dx, 2) + MathF.Pow(dy, 2) + MathF.Pow(dz, 2) );

                        force = (float) ( ( (-charge * GNDArea * voltage) / (4 * MathF.PI * radius) ) * ( 1 / MathF.Pow(GNDDistance, 2) ) );

                        dx /= GNDDistance;
                        dy /= GNDDistance;
                        dz /= GNDDistance;

                        forceVector += new Vector3(MathF.Round(force * dx), MathF.Round(force * dy), MathF.Round(force * dz));
                    }

                }

            }

            int k = 0;
            foreach (var magnet in magnets) {

                //Debug.Log($"Magnet: {magnet.name}, Type: {magnet.GetComponent<MagnetScript>().magnetType}");

                float permeability = magnet.GetComponent<MagnetScript>().permeability;

                switch (magnet.GetComponent<MagnetScript>().magnetType) {
                    case 0:

                        Debug.Log("Cylinder Magnet");

                        Vector3 sphere = Cartesian_to_Spherical(atom.transform.position - magnet.transform.position); 
                        

                        Debug.Log("Position: " + (atom.transform.position - magnet.transform.position) + ", Spherical: " + sphere);

                        sphere.Normalize();

                        Debug.Log("Normalize: " +  sphere);

                        break;
                    
                    case 1:

                        Debug.Log("O-Ring Magnet");

                        sphere = Cartesian_to_Spherical(atom.transform.position - magnet.transform.position); 

                        Debug.Log("Position: " + atom.transform.position + ", Spherical: " + sphere);

                        sphere.Normalize();

                        Debug.Log("Normalize: " +  sphere);

                        break;


                    default:
                        break;
                }
                

                k++;
            }

            Force(atom);

            avgVelocity += atom.GetComponent<Rigidbody>().velocity;
            avgMass += atom.GetComponent<Rigidbody>().mass;
        }

        avgVelocity /= atoms.Length;
        avgMass /= atoms.Length;

        float avgVel = ( MathF.Abs(avgVelocity.x) + MathF.Abs(avgVelocity.y) + MathF.Abs(avgVelocity.z) ) / 3;

        // Used temperature kinetic energy proportionality
        // https://en.wikipedia.org/wiki/Kinetic_theory_of_gases
        float temp = (float) ( (1.0f / 3.0f) * ( avgMass * MathF.Pow(10, -4) * MathF.Pow(avgVel * 100, 2) ) / 1.380649f );

        //Debug.Log($"Temp: {temp}K, avgMass: {avgMass}, avgVel: {avgVel}, #atoms: {atoms.Length}");

    }

    private void Force(GameObject atom) {

        atom.GetComponent<Rigidbody>().AddForce(forceVector);

        

        

    }

    private Vector3 Cartesian_to_Spherical(float x, float y, float z) {

        return new Vector3( Mathf.Sqrt( Mathf.Pow(x,2) * Mathf.Pow(y,2) * Mathf.Pow(z,2) ), Mathf.Atan2(y,x), Mathf.Acos( z / Mathf.Sqrt( Mathf.Pow(x,2) * Mathf.Pow(y,2) * Mathf.Pow(z,2) ) ) );

    }

    private Vector3 Cartesian_to_Spherical(Vector3 v) {

        float x = v.x;
        float y = v.y;
        float z = v.z;

        return new Vector3( Mathf.Sqrt( Mathf.Pow(x,2) * Mathf.Pow(y,2) * Mathf.Pow(z,2) ), Mathf.Atan2(y,x), Mathf.Acos( z / Mathf.Sqrt( Mathf.Pow(x,2) * Mathf.Pow(y,2) * Mathf.Pow(z,2) ) ) );

    }

    private Vector3 Spherical_to_Cartesian(float radius, float theta, float phi) {

        return new Vector3( radius * Mathf.Sin(phi) * Mathf.Cos(theta), radius * Mathf.Sin(phi) * Mathf.Sin(theta), radius * Mathf.Cos(phi) );

    }

}
