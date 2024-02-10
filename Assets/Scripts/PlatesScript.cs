using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatesScript : MonoBehaviour {


    public GameObject VCC;
    public GameObject GND;
    public GameObject negative;
    public float voltage = 5000.0f;
    private float distance = 0.0f;
    private int threshold = 0;

    // Start is called before the first frame update
    void Start() {

        distance = Vector3.Distance(VCC.transform.position, GND.transform.position);
        
    }

    // Update is called once per frame
    void Update() {

        threshold = (int)Random.Range(0, distance/voltage * 1000);

        //Debug.Log(threshold);

        if ( threshold == 0 ) {
            Instantiate(negative, new Vector3(GND.transform.position.x + Random.Range(-GND.transform.localScale.x/2,GND.transform.localScale.x/2), GND.transform.position.y+GND.transform.localScale.y, GND.transform.position.z + Random.Range(-GND.transform.localScale.z/2,GND.transform.localScale.z/2)), GND.transform.rotation);
        }
        
    }
}
