using TMPro;
using UnityEngine;

public class EndGameShow : MonoBehaviour
{
    [SerializeField] private TMP_Text endText;

    private void Start()
    {
        endText.enabled = false;
    }
  
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!endText.enabled)
            endText.enabled = true;
    }
}
