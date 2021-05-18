using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class scoreCounter : MonoBehaviour
{
    public Text ScoreText;
    public Transform player;
    private float timer;
    private float oldPlayerPos;
    public float score;
    // Start is called before the first frame update
    void Start()
    {
        oldPlayerPos = player.position.x;
    }

    // Update is called once per frame

    void Update () {
        timer += (int)(Time.timeSinceLevelLoad * 1000f) % 1000;;
        //Debug.Log(timer);
        if (timer > 1f && player.position.x > oldPlayerPos) {

            score += 1;

            //Update the text if the score changed.
            ScoreText.text = score.ToString();

            //Reset the timer to 0.
            timer = 0;
            //previous player position reset
            oldPlayerPos = player.position.x;
        }
        //Debug.Log(score);
    }
}
