using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Powerup : MonoBehaviour
{
    RubyController2 rc2;
    void Start()
    {
        
    }

    void Update()
    {
        transform.Rotate(0.0f, 0.0f, 6.0f*12*Time.deltaTime);
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        rc2 = other.gameObject.GetComponent<RubyController2>();
        Instantiate(rc2.healthPrefab, rc2.transform.position, Quaternion.identity);
        rc2.speed = 9.0f;

        Destroy(this.gameObject);
    }
}
