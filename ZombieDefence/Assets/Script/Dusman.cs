using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Dusman : MonoBehaviour
{
    NavMeshAgent ajan;
    GameObject Hedef;
    public float health;
    public float DusmanDarbeGucu;
    GameObject anaKontrolcum;
    Animator Animatorum;
    


    void Start()
    {
        Animatorum = GetComponent<Animator>();
         ajan = GetComponent<NavMeshAgent>();
        anaKontrolcum = GameObject.FindWithTag("AnaKontrolcum");
       

    }

    public void HedefBelirle(GameObject objem)
    {
        Hedef = objem;

    }
   
    void Update()
    {
       ajan.SetDestination(Hedef.transform.position);

        
    }

    public void DarbeAl(float darbegucu)
    {
        health -= darbegucu;
        if (health <= 0)
        {
            oldun();

            gameObject.tag = "Untagged";
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Korumam_gerekli"))
        {         

            anaKontrolcum.GetComponent<GameKontrolcu>().DarbeAl(DusmanDarbeGucu);
            oldun();
        }
    }

    public void oldun()
    {
        anaKontrolcum.GetComponent<GameKontrolcu>().Dusman_sayisi_guncelle();

         Animatorum.SetTrigger("Olme");
         Destroy(gameObject,5f);

    }
}
