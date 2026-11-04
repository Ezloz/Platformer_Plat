using System;
using TMPro;
using UnityEngine;

public class EndGameShow : MonoBehaviour
{
    [SerializeField] private TMP_Text endText;

    private string finalDisplayTime;
    private float start_time;
    private float end_time;
    private float final_time;

    private void Start()
    {
        endText.enabled = false;
        start_time = Time.time;
    }
  
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!endText.enabled)
        {
            end_time = Time.time;
            final_time = end_time - start_time;
            endText.enabled = true;
            var min = (int)(final_time / 60);
            var sec = (int)(final_time % 60);
            finalDisplayTime = min + "min" + sec + "s";
            endText.text += "\nTime : " + finalDisplayTime;
        }
        

    }
}
