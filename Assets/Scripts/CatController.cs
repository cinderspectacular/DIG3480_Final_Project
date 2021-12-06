using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatController : MonoBehaviour
{
    GameObject ruby;
    RubyController2 rubyController2;
    public bool caught;
    private float timer;

    void Start()
    {
        
    }


    void OnCollisionEnter2D(Collision2D other)
    {
        if(other.gameObject.CompareTag("RubyController"))
        {
            ruby = other.gameObject;
            rubyController2 = ruby.GetComponent<RubyController2>();

            rubyController2.catCount += 1;
            rubyController2.SetCatText();

            if(rubyController2.catCount%3 == 0)
            {
                rubyController2.speed = 3.0f;
            }
            
            Destroy(this.gameObject);
        }
    }
}
