using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bomba : MonoBehaviour
{
    public float guc;
    public float menzil;
    public float yukariguc;
    public ParticleSystem patlamaEfeckt;
    AudioSource patlamaSesi;


    void Start()
    {
        patlamaSesi = GetComponent<AudioSource>();

       
    }

   
   

    private void OnCollisionEnter(Collision collision)
    {
      

        if (collision != null)
        {



            Destroy(gameObject, .5f);


            Patlama();
        }
    }
    void Patlama()
    {
        Vector3 patlamapozisyonu = transform.position;
        Instantiate(patlamaEfeckt, transform.position, transform.rotation);
        patlamaSesi.Play();
        Collider[] colliders = Physics.OverlapSphere(patlamapozisyonu, menzil);

        foreach (Collider hit in colliders)
        {
            Rigidbody rb = hit.GetComponent<Rigidbody>();
            if (hit != null && rb)
            {



                if (hit.gameObject.CompareTag("Dusman"))
                {
                    hit.transform.gameObject.GetComponent<Dusman>().oldun();

                }


                
                
                rb.AddExplosionForce(guc, patlamapozisyonu, menzil, .5f, ForceMode.Impulse);
            }

        }

    }

}
