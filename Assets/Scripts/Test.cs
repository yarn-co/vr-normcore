using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class Test : MonoBehaviour
{
    Rigidbody rb;
    bool jumped = false;

    public TextMeshPro message;

    // Start is called before the first frame update
    void Start()
    {
       rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            //Destroy(gameObject);

            rb.AddForce(Vector3.up * 500);
        }

        if(Input.GetKeyDown(KeyCode.R))
        {
            Reset();
        }
    }

    private void OnMouseDown(){
        
        Destroy(gameObject);
    }

    public void Reset()
    {
        SceneManager.LoadScene("HandTestScene");
    }

    public void Jump(){
        rb.AddForce(Vector3.up * 500);

        if(!jumped){
            jumped = true;

            message.text = "Great, now make Moonbox jump off the ledge.";
        }
    }

    private void OnCollisionEnter(Collision collision)
    {    
        if(collision.gameObject.tag == "Enemy")
        {
            Destroy(collision.gameObject);
        }

        if(collision.gameObject.tag == "Floor")
        {
            message.text = "Yay, you killed Moonbox!";

            //Destroy(gameObject);
        }
    }
}
