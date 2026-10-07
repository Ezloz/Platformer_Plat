using System.Collections;
using UnityEngine;

public class RainbowMaterialScript : MonoBehaviour
{
    [SerializeField] private Material material;   

    [SerializeField] private Color[] colors;

    void Start()
    {
        StartCoroutine(Cycle());
    }

    IEnumerator Cycle()
    {
        int i = 0;
        int length = colors.Length;
        while(true) {
            for (float interpolant = 0f; interpolant < 1f; interpolant += 0.01f)
            {
                material.color = Color.Lerp(colors[i % length], colors[(i + 1) % length], interpolant);
                yield return null;
            
            }
            i++;
        }
         
    }
}
