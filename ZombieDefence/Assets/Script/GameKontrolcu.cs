using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityStandardAssets.Characters.FirstPerson;

public class GameKontrolcu : MonoBehaviour
{
    int aktifsira;
    float health = 100;

    [Header("SAĞLIK AYARLARI")]
    public Image HealthBar;

    [Header("SİLAH AYARLARI")]
    public GameObject[] silahlar;
    public AudioSource degisimSesi;
    public GameObject Bomba;
    public GameObject BombaPoint;
    public Camera benimCam;


    [Header("DUSMAN AYARLARI")]
    public GameObject[] dusmanlar;
    public GameObject[] cikisNoktalari;
    public GameObject[] hedefNoktalar;
    public float DusmancikmaSuresi;
    public TextMeshProUGUI KalanDusman_text;
    public int Baslangic_dusman_sayisi;
    public static int Kalan_dusman_sayisi;

    [Header("DİĞER AYARLAR")]
    public GameObject GameOverCanvas;
    public GameObject KazandinCanvas;
    public GameObject PauseCanvas;
    public TextMeshProUGUI saglik_sayisi_text;
    public TextMeshProUGUI bomba_sayisi_text;
    public AudioSource Itemyok;
    public AudioSource Oyunicises;

    public static bool OyunDurduMu;
    void Start()
    {
      
        Baslangicislemler();
    }
   
    void Baslangicislemler()
    {

        OyunDurduMu = false;


        PlayerPrefs.SetInt("Taramali_Mermi", 570);
        PlayerPrefs.SetInt("Pompali_Mermi", 550);
        PlayerPrefs.SetInt("Magnum_Mermi", 530);
        PlayerPrefs.SetInt("Sniper_Mermi", 520);

        PlayerPrefs.SetInt("saglik_sayisi", 11);
        PlayerPrefs.SetInt("bomba_sayisi", 25);

        if (!PlayerPrefs.HasKey("OyunBasladimi"))
        {
            PlayerPrefs.SetInt("Taramali_Mermi", 70);
            PlayerPrefs.SetInt("Pompali_Mermi", 50);
            PlayerPrefs.SetInt("Magnum_Mermi", 30);
            PlayerPrefs.SetInt("Sniper_Mermi", 20);

            PlayerPrefs.SetInt("saglik_sayisi", 1);
            PlayerPrefs.SetInt("bomba_sayisi", 5);

            PlayerPrefs.SetInt("OyunBasladimi", 1);

        }

        KalanDusman_text.text = Baslangic_dusman_sayisi.ToString();
        Kalan_dusman_sayisi = Baslangic_dusman_sayisi;

        saglik_sayisi_text.text = PlayerPrefs.GetInt("saglik_sayisi").ToString();
        bomba_sayisi_text.text = PlayerPrefs.GetInt("bomba_sayisi").ToString();


        aktifsira = 0;
        Oyunicises = GetComponent<AudioSource>();
        Oyunicises.Play();

        StartCoroutine(DusmanCikar());


      

    }
   
    IEnumerator DusmanCikar()
    {

        while (true)
        {
            yield return new WaitForSeconds(DusmancikmaSuresi);

            if (Baslangic_dusman_sayisi != 0)
            {
                int dusman = Random.Range(0, 5);
                int cikisnoktasi = Random.Range(0, 2);
                int hedefnoktasi = Random.Range(0, 2);

                GameObject Obje = Instantiate(dusmanlar[dusman], cikisNoktalari[cikisnoktasi].transform.position, Quaternion.identity);
                Obje.GetComponent<Dusman>().HedefBelirle(hedefNoktalar[hedefnoktasi]);
                Baslangic_dusman_sayisi--;
            }


        }

    }
   
    public void Dusman_sayisi_guncelle()
    {
        Kalan_dusman_sayisi--;

        if (Kalan_dusman_sayisi<=0)
        {
            KazandinCanvas.SetActive(true);
            KalanDusman_text.text = "0";
            Time.timeScale = 0;

    
            OyunDurduMu = true;

            Cursor.visible = true;
            GameObject.FindWithTag("Player").GetComponent<FirstPersonController>().m_MouseLook.lockCursor = false;
            Cursor.lockState = CursorLockMode.None;


        }
        else
        {
            KalanDusman_text.text = Kalan_dusman_sayisi.ToString();
        }
       
    }
   
    void Update()
    {


        if (Input.GetKeyDown(KeyCode.Alpha1) && !OyunDurduMu)
        {
            Silahdegistir(0);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2) && !OyunDurduMu)
        {
            Silahdegistir(1);
        }
        if (Input.GetKeyDown(KeyCode.Alpha3) && !OyunDurduMu)
        {
            Silahdegistir(2);
        }
        if (Input.GetKeyDown(KeyCode.Alpha4) && !OyunDurduMu)
        {
            Silahdegistir(3);
        }
        if (Input.GetKeyDown(KeyCode.Q) && !OyunDurduMu)
        {
            QTusuVersiyonuSilahdegistir();

        }
        if (Input.GetKeyDown(KeyCode.G) && !OyunDurduMu)
        {

            BombaAt();
        }

        if (Input.GetKeyDown(KeyCode.E) && !OyunDurduMu)
        {
            Saglik_doldur();

        }

       
        if (Input.GetKeyDown(KeyCode.Escape) && !OyunDurduMu)
        {
            Pause();

        }
    }
   
    public void DarbeAl(float darbegucu)
    {
        health -= darbegucu;
        HealthBar.fillAmount = health / 100;
        if (health <= 0)
        {
            GameOver();

        }

    }

    void BombaAt()
    {
        if (PlayerPrefs.GetInt("bomba_sayisi") != 0)
        {

            GameObject obje = Instantiate(Bomba, BombaPoint.transform.position, BombaPoint.transform.rotation);
            Rigidbody rg = obje.GetComponent<Rigidbody>();
            Vector3 acimiz = Quaternion.AngleAxis(90, benimCam.transform.forward) * benimCam.transform.forward;
            rg.AddForce(acimiz * 250f);


            PlayerPrefs.SetInt("bomba_sayisi", PlayerPrefs.GetInt("bomba_sayisi") - 1);
            bomba_sayisi_text.text = PlayerPrefs.GetInt("bomba_sayisi").ToString();
        }
        else
        {
            Itemyok.Play();
        }


    }
    public void Saglik_doldur()
    {

        if (PlayerPrefs.GetInt("saglik_sayisi")!=0 && health!= 100)
        {
            health = 100;
            HealthBar.fillAmount = health / 100;

            PlayerPrefs.SetInt("saglik_sayisi", PlayerPrefs.GetInt("saglik_sayisi") - 1);
            saglik_sayisi_text.text = PlayerPrefs.GetInt("saglik_sayisi").ToString();
        }
        else
        {
            // müzik koyabilirsin dostum
        }
       
    }

    public void Saglik_Al()
    {
         PlayerPrefs.SetInt("saglik_sayisi", PlayerPrefs.GetInt("saglik_sayisi") + 1);
        saglik_sayisi_text.text = PlayerPrefs.GetInt("saglik_sayisi").ToString();
    }
    public void Bomba_Al()
    {
        PlayerPrefs.SetInt("bomba_sayisi", PlayerPrefs.GetInt("bomba_sayisi") + 1);
        bomba_sayisi_text.text = PlayerPrefs.GetInt("bomba_sayisi").ToString();
    }
    
    void Silahdegistir(int siraNumarasi)
    {
        degisimSesi.Play();
        foreach (GameObject silah in silahlar)
        {
            silah.SetActive(false);

        }
        aktifsira = siraNumarasi;
        silahlar[siraNumarasi].SetActive(true);

    }
   
    void QTusuVersiyonuSilahdegistir()
    {

        degisimSesi.Play();

        foreach (GameObject silah in silahlar)
        {
            silah.SetActive(false);
        }
        if (aktifsira == 3)
        {
            aktifsira = 0;
            silahlar[aktifsira].SetActive(true);

        }
        else
        {
            aktifsira++;
            silahlar[aktifsira].SetActive(true);

        }


    }
    
    public void BastanBasla()
    {

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
       
        Time.timeScale = 1;
        OyunDurduMu = true;
        Cursor.visible = true;
        GameObject.FindWithTag("Player").GetComponent<FirstPersonController>().m_MouseLook.lockCursor = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

   

    public void Pause()
    {
        PauseCanvas.SetActive(true);
        Time.timeScale = 0;
        OyunDurduMu = true;


        Cursor.visible = true;
        GameObject.FindWithTag("Player").GetComponent<FirstPersonController>().m_MouseLook.lockCursor = false;
        Cursor.lockState = CursorLockMode.None;
    }
   
    

    void GameOver()
    {

        GameOverCanvas.SetActive(true);
        Time.timeScale = 1;

        OyunDurduMu = true;

        Cursor.visible = true;
        GameObject.FindWithTag("Player").GetComponent<FirstPersonController>().m_MouseLook.lockCursor = false;
        Cursor.lockState = CursorLockMode.None;
    }

    public void DevamEt()
    {
        PauseCanvas.SetActive(false);
        Time.timeScale = 1;
        OyunDurduMu = false;

        Cursor.visible = false;
        GameObject.FindWithTag("Player").GetComponent<FirstPersonController>().m_MouseLook.lockCursor = true;
        Cursor.lockState = CursorLockMode.Locked;
    }
    

       public void anaMenu()
    {
        SceneManager.LoadScene(0);
        Time.timeScale = 1;
    }
}
