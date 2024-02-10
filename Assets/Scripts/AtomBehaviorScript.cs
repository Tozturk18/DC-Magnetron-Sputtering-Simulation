using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AtomBehaviorScript : MonoBehaviour {

    public int charge = 0;

    public Rigidbody thisRigidbody;
    public float bounce = 100.0f;

    public GameObject positive;
    public GameObject neutral;
    public GameObject negative;
    public GameObject deposit;

    private Vector3 initPos;
    private Vector3 initVel;

    // Start is called before the first frame update
    void Start() {
        initPos = transform.position;
    }

    // Update is called once per frame
    /*void Update() {
        
    }*/

    void OnCollisionEnter(Collision collision) {

        GameObject collided = collision.gameObject;
        
        if (collided.CompareTag("atom")) {

            if (charge == 0 && collided.GetComponent<AtomBehaviorScript>().charge == -1 && gameObject.layer != 3) {

                GameObject outPositive = Instantiate(positive, transform.position + new Vector3( Random.Range(0.1f,1) + 1, Random.Range(0.1f,1) + 1, Random.Range(0.1f,1) + 1 ),transform.rotation);
                outPositive.GetComponent<Rigidbody>().velocity = thisRigidbody.velocity + collided.GetComponent<Rigidbody>().velocity;
                outPositive.GetComponent<AtomBehaviorScript>().initVel = thisRigidbody.velocity + collided.GetComponent<Rigidbody>().velocity;

                GameObject outNegative = Instantiate(negative, transform.position + new Vector3( -Random.Range(0.1f,1) - 1, -Random.Range(0.1f,1) - 1, -Random.Range(0.1f,1) - 1 ),transform.rotation);
                outNegative.GetComponent<Rigidbody>().velocity = thisRigidbody.velocity + collided.GetComponent<Rigidbody>().velocity;

                Destroy(gameObject);

                return;
            }

            if (charge == 1 && collided.GetComponent<AtomBehaviorScript>().charge == -1) {

                GameObject outNeutral = Instantiate(neutral, transform.position, transform.rotation);
                outNeutral.GetComponent<Rigidbody>().velocity = thisRigidbody.velocity + collided.GetComponent<Rigidbody>().velocity;

                Destroy(collided);
                Destroy(gameObject);

                return;
            }

        } else if (charge == -1 && collided.CompareTag("anode")) {

            Destroy(gameObject);

            return;

        }/* else if (collided.CompareTag("wall")) {

            //thisRigidbody.AddForce(collision.contacts[0].normal * bounce);

            //Debug.Log("Collision");

        }*/

    }

    private void OnTriggerEnter(Collider other) {

        if (charge == 1) {

            Vector3 inVelocity = thisRigidbody.velocity;
        
            GameObject deposition = Instantiate(deposit, transform.position, transform.rotation);
            deposition.GetComponent<Rigidbody>().velocity = new Vector3(inVelocity.x, -inVelocity.y, inVelocity.z);

            Destroy(gameObject);

            return;

        }

    }

}
