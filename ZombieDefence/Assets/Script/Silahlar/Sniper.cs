﻿using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class Sniper : MonoBehaviour
{
    Animator animatorum;

    [Header("AYARLAR")]
    public bool atesEdebilirmi;
    float İceridenAtesEtmeSikligi;
    public float disaridanAtesEtmeSikligi;
    public float menzil;
    public GameObject Cross;
    public GameObject Scope;


    [Header("SESLER")]
    public AudioSource AtesSesi;
    public AudioSource SarjorDegirme;
    public AudioSource MermiBittiSes;
    public AudioSource MermiAlmaSesi;


    [Header("EFEKTLER")]
    public ParticleSystem AtesEfekt;
    public ParticleSystem Mermiİzi;
    public ParticleSystem KanEfekti;

    [Header("DİGERLERİ")]
    public Camera benimCam;
    float camFieldPov;
    public float YaklasmaPov;

    [Header("SİLAH AYARLAR")]
    int ToplamMermiSayisi;
    public int SarjorKapasitesi;
    int KalanMermi;
    public string Silahin_adi;
    public TextMeshProUGUI ToplamMermi_Text;
    public TextMeshProUGUI KalanMermi_Text;
    public float DarbeGucu;

    public bool Kovan_ciksinmi;
    public GameObject Kovan_Cikis_noktasi;
    public GameObject Kovan_objesi;

    public Mermi_Kutusu_olustur Mermi_Kutusu_olustur_yonetim;

    void Start()
    {
        ToplamMermiSayisi = PlayerPrefs.GetInt(Silahin_adi + "_Mermi");
        Baslangic_mermi_doldur();
        SarjordoldurmaTeknikFonksiyon("NormalYaz");
        animatorum = GetComponent<Animator>();
        camFieldPov = benimCam.fieldOfView;
    }

    void Update()
    {

        if (Input.GetKey(KeyCode.Mouse0))
        {

            if (atesEdebilirmi && Time.time > İceridenAtesEtmeSikligi && KalanMermi != 0)
            {
                if (!GameKontrolcu.OyunDurduMu)
                {

                    AtesEt();
                    İceridenAtesEtmeSikligi = Time.time + disaridanAtesEtmeSikligi;

                }

            }

            if (KalanMermi == 0)
            {
                MermiBittiSes.Play();
            }



        }

        if (Input.GetKey(KeyCode.R))
        {
            if (KalanMermi < SarjorKapasitesi && ToplamMermiSayisi != 0)
            {
                animatorum.Play("sarjordegistir");

            }
        }


        if (Input.GetKeyDown(KeyCode.E))
        {
            MermiAL();
        }


        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            KameraYaklastirceScopeCikar(true);
           
        }
        if (Input.GetKeyUp(KeyCode.Mouse1))
        {
            KameraYaklastirceScopeCikar(false);
           
        }
        

    }

    void KameraYaklastirceScopeCikar(bool durum)
    {

        if (durum)
        {
            Cross.SetActive(false);
            benimCam.cullingMask = ~ (1 << 8);
            animatorum.SetBool("zoomyap", durum);
            benimCam.fieldOfView = YaklasmaPov;
            Scope.SetActive(true);
            
        }
        else
        {
            Scope.SetActive(false);
            benimCam.cullingMask = -1;
            animatorum.SetBool("zoomyap", durum);
            benimCam.fieldOfView = camFieldPov;
            Cross.SetActive(true);
        }
        

    }



    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Mermi"))
        {
            MermiKaydet(other.transform.gameObject.GetComponent<MermiKutusu>().Olusan_Silahin_Turu, other.transform.gameObject.GetComponent<MermiKutusu>().Olusan_Mermi_sayisi);
            Mermi_Kutusu_olustur_yonetim.NoktalarinKaldirma(other.transform.gameObject.GetComponent<MermiKutusu>().Noktasi);
            Destroy(other.transform.parent.gameObject);

        }

        if (other.gameObject.CompareTag("Can_kutusu"))
        {

            Mermi_Kutusu_olustur_yonetim.GetComponent<GameKontrolcu>().Saglik_Al();
            Health_Kutusu_olustur.Health_kutusu_varmi = false;
            Destroy(other.transform.gameObject);

        }

        if (other.gameObject.CompareTag("Bomba_Kutusu"))
        {

            Mermi_Kutusu_olustur_yonetim.GetComponent<GameKontrolcu>().Bomba_Al();
            Bomba_Kutusu_olustur.Bomba_kutusu_varmi = false;
            Destroy(other.transform.gameObject);

        }
    }

   

    void AtesEt()
    {
        AtesEtmeTeknikİslemleri();

        RaycastHit hit;

        if (Physics.Raycast(benimCam.transform.position, benimCam.transform.forward, out hit, menzil))
        {

            if (hit.transform.gameObject.CompareTag("Dusman"))
            {
                Instantiate(KanEfekti, hit.point, Quaternion.LookRotation(hit.normal));

                hit.transform.gameObject.GetComponent<Dusman>().DarbeAl(DarbeGucu);

            }
            else if (hit.transform.gameObject.CompareTag("DevrilebilirObje"))
            {

                Rigidbody rg = hit.transform.gameObject.GetComponent<Rigidbody>();
                rg.AddForce(-hit.normal * 50f);
                Instantiate(Mermiİzi, hit.point, Quaternion.LookRotation(hit.normal));
            }
            else
            {
                Instantiate(Mermiİzi, hit.point, Quaternion.LookRotation(hit.normal));
            }




        }



    }

    void MermiAL()
    {

        RaycastHit hit;

        if (Physics.Raycast(benimCam.transform.position, benimCam.transform.forward, out hit, 4))
        {

            if (hit.transform.gameObject.CompareTag("Mermi"))
            {

                MermiKaydet(hit.transform.gameObject.GetComponent<MermiKutusu>().Olusan_Silahin_Turu, hit.transform.gameObject.GetComponent<MermiKutusu>().Olusan_Mermi_sayisi);
                Mermi_Kutusu_olustur_yonetim.NoktalarinKaldirma(hit.transform.gameObject.GetComponent<MermiKutusu>().Noktasi);
                Destroy(hit.transform.parent.gameObject);


            }

        }


    }

    void Baslangic_mermi_doldur()
    {

        if (ToplamMermiSayisi <= SarjorKapasitesi)
        {
            KalanMermi = ToplamMermiSayisi;
            ToplamMermiSayisi = 0;
            PlayerPrefs.SetInt(Silahin_adi + "_Mermi", 0);
        }
        else
        {
            KalanMermi = SarjorKapasitesi;
            ToplamMermiSayisi -= SarjorKapasitesi;
            PlayerPrefs.SetInt(Silahin_adi + "_Mermi", ToplamMermiSayisi);

        }

    }
    void SarjordoldurmaTeknikFonksiyon(string tur)
    {
        switch (tur)
        {
            case "MermiVar":

                if (ToplamMermiSayisi <= SarjorKapasitesi)
                {
                    int OlusanToplamDeger = KalanMermi + ToplamMermiSayisi;

                    if (OlusanToplamDeger > SarjorKapasitesi)
                    {
                        KalanMermi = SarjorKapasitesi;
                        ToplamMermiSayisi = OlusanToplamDeger - SarjorKapasitesi;
                        PlayerPrefs.SetInt(Silahin_adi + "_Mermi", ToplamMermiSayisi);

                    }
                    else
                    {
                        KalanMermi += ToplamMermiSayisi;
                        ToplamMermiSayisi = 0;
                        PlayerPrefs.SetInt(Silahin_adi + "_Mermi", 0);
                    }


                }
                else
                {
                    ToplamMermiSayisi -= SarjorKapasitesi - KalanMermi;
                    KalanMermi = SarjorKapasitesi;
                    PlayerPrefs.SetInt(Silahin_adi + "_Mermi", ToplamMermiSayisi);
                }

                ToplamMermi_Text.text = ToplamMermiSayisi.ToString();
                KalanMermi_Text.text = KalanMermi.ToString();
                break;

            case "MermiYok":
                if (ToplamMermiSayisi <= SarjorKapasitesi)
                {
                    KalanMermi = ToplamMermiSayisi;

                    ToplamMermiSayisi = 0;
                    PlayerPrefs.SetInt(Silahin_adi + "_Mermi", 0);
                }
                else
                {
                    ToplamMermiSayisi -= SarjorKapasitesi;
                    PlayerPrefs.SetInt(Silahin_adi + "_Mermi", ToplamMermiSayisi);
                    KalanMermi = SarjorKapasitesi;
                }


                ToplamMermi_Text.text = ToplamMermiSayisi.ToString();
                KalanMermi_Text.text = KalanMermi.ToString();
                break;

            case "NormalYaz":
                ToplamMermi_Text.text = ToplamMermiSayisi.ToString();
                KalanMermi_Text.text = KalanMermi.ToString();
                break;

        }

    }

    void Sarjordegistir()
    {
        SarjorDegirme.Play();
        if (KalanMermi < SarjorKapasitesi && ToplamMermiSayisi != 0)
        {
            if (KalanMermi != 0)
            {
                SarjordoldurmaTeknikFonksiyon("MermiVar");
            }
            else
            {
                SarjordoldurmaTeknikFonksiyon("MermiYok");
            }

        }

    }

    void AtesEtmeTeknikİslemleri()
    {
        if (Kovan_ciksinmi)
        {
            GameObject obje = Instantiate(Kovan_objesi, Kovan_Cikis_noktasi.transform.position, Kovan_Cikis_noktasi.transform.rotation);
            Rigidbody rb = obje.GetComponent<Rigidbody>();
            rb.AddRelativeForce(new Vector3(-10f, 1, 0) * 60);

        }

        AtesSesi.Play();
        AtesEfekt.Play();
        animatorum.Play("ateset");


        KalanMermi--;
        KalanMermi_Text.text = KalanMermi.ToString();


    }

    void MermiKaydet(string silahturu, int mermisayisi)
    {
        /*  PlayerPrefs.SetInt("Pompali_Mermi", 50);
             PlayerPrefs.SetInt("Magnum_Mermi", 30);
             PlayerPrefs.SetInt("Sniper_Mermi", 20);*/
        MermiAlmaSesi.Play();

        switch (silahturu)
        {
            case "Taramali":
                PlayerPrefs.SetInt("Taramali_Mermi", PlayerPrefs.GetInt("Taramali_Mermi") + mermisayisi);
                break;

            case "Pompali":
                PlayerPrefs.SetInt("Pompali_Mermi", PlayerPrefs.GetInt("Pompali_Mermi") + mermisayisi);
                break;

            case "Magnum":
                PlayerPrefs.SetInt("Magnum_Mermi", PlayerPrefs.GetInt("Magnum_Mermi") + mermisayisi);
                break;

            case "Sniper":
                ToplamMermiSayisi += mermisayisi;
                PlayerPrefs.SetInt(Silahin_adi + "_Mermi", ToplamMermiSayisi);
                SarjordoldurmaTeknikFonksiyon("NormalYaz");
                break;


        }

    }

   
    
}
