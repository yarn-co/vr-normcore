
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class FPSDisplay : MonoBehaviour
{
    public int FPS { get; private set; }
    public TextMeshPro FPSText;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        FPS = (int)(1f / Time.deltaTime);

        if (Time.frameCount % 50 == 0) {

            FPSText.text = FPS.ToString();
        }
    }
}
