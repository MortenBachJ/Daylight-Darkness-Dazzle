using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class displayScore : MonoBehaviour
{
    public Text finalScore;
    float score;
    // Start is called before the first frame update
    void Start()
    {
        score = PlayerMovement.finalScore;
        finalScore.text = score.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
