using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    public float moveSpeed = 5f;
    GameObject currentFloor;
    [SerializeField] int Hp;
    [SerializeField] GameObject HpBar;
    [SerializeField] Text scoreText;
    [SerializeField] GameObject replayButton;
    int score;
    float scoreTime;

    void Start()
    {
        Hp = 10;   
        score = 0;
        scoreTime = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKey(KeyCode.A)){
            transform.Translate(-moveSpeed*Time.deltaTime, 0, 0);
            GetComponent<SpriteRenderer>().flipX = true;
            GetComponent<Animator>().SetBool("run", true);
        }
        else if(Input.GetKey(KeyCode.D)){
            transform.Translate(moveSpeed*Time.deltaTime, 0, 0);
            GetComponent<SpriteRenderer>().flipX = false;
            GetComponent<Animator>().SetBool("run", true);
        }
        else{
            GetComponent<Animator>().SetBool("run", false);
        }
        UpdateScore();
    }
    
    void OnCollisionEnter2D(Collision2D other) {
        if(other.gameObject.tag == "safe"){
            if(other.contacts[0].normal == new Vector2(0, 1f)){
                Debug.Log("safe");
                currentFloor = other.gameObject;
                ModifyHp(1);
                other.gameObject.GetComponent<AudioSource>().Play();
            }
            
        }
        else if (other.gameObject.tag == "danger"){
            if(other.contacts[0].normal == new Vector2(0, 1f)){
                Debug.Log("danger");
                currentFloor = other.gameObject;
                ModifyHp(-2);
                GetComponent<Animator>().SetTrigger("hurt");
                other.gameObject.GetComponent<AudioSource>().Play();
            }       
        }
        else if(other.gameObject.tag == "ceiling"){
            Debug.Log("撞到天花板");
            currentFloor.GetComponent<BoxCollider2D>().enabled = false;
            ModifyHp(-2);
            GetComponent<Animator>().SetTrigger("hurt");
            other.gameObject.GetComponent<AudioSource>().Play();
        }
    }
    void OnTriggerEnter2D(Collider2D other) {
        if(other.gameObject.tag == "death"){
            Debug.Log("Loss");
            Die();
        }
    }

    void ModifyHp(int num){
        Hp=Hp+num;
        if(Hp > 10){
            Hp = 10;
        }
        else if(Hp <= 0){
            Hp = 0;
            Die();
        }
        UpdateHp();
    }

    void UpdateHp(){
        int i;
        for(i=0;i<HpBar.transform.childCount;i++){
            if(Hp > i){
                HpBar.transform.GetChild(i).gameObject.SetActive(true);
            }
            else{
                HpBar.transform.GetChild(i).gameObject.SetActive(false);
            }
        }
    }

    void UpdateScore(){
        scoreTime += Time.deltaTime;
        if(scoreTime > 2f){
            score+=1;
            scoreTime = 0f;
            scoreText.text = "地下" + score.ToString() + "層";
        }
    }

    void Die(){
        GetComponent<AudioSource>().Play();
        Time.timeScale = 0f;
        replayButton.SetActive(true);
    }

    public void Replay(){
        Time.timeScale = 1f;
        SceneManager.LoadScene("SampleScene");
    }
}
