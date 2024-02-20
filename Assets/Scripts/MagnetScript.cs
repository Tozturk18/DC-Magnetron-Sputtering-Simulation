using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagnetScript : MonoBehaviour {

    public int magnetType = 0;
    public float remanence_field = 0.0f;
    public float dipole_moment;
    private float permiability = 4*Math.PI()*pow(10,-7) (Tesla * meter / Ampere);

    // Start is called before the first frame update
    void Start() {
        dipole_moment = remanence_field * VolumeOfMesh(gameobject.mesh) / permiability;
    }

    /*
    // Update is called once per frame
    void Update() {
        
    }*/

    float VolumeOfMesh(Mesh mesh) {
        float volume = 0;
        Vector3[] vertices = mesh.vertices;
        int[] triangles = mesh.triangles;
        for (int i = 0; i < mesh.triangles.Length; i += 3) {
            Vector3 p1 = vertices[triangles[i + 0]];
            Vector3 p2 = vertices[triangles[i + 1]];
            Vector3 p3 = vertices[triangles[i + 2]];
            volume += SignedVolumeOfTriangle(p1, p2, p3);
        }
        return Mathf.Abs(volume);
    }
}
