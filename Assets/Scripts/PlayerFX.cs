using System.Collections;
using Unity.VectorGraphics;
using UnityEngine;

public class PlayerFX : MonoBehaviour
{
    private TrailRenderer trailRenderer => GetComponent<TrailRenderer>();

    private void Start()
    {
        StopEmiting();
    }
    public void EmitTrail(float time, float emiting_time)
    {
        trailRenderer.time = time;
        trailRenderer.emitting = true;
        StartCoroutine(EndEmission(emiting_time));
    }

    private IEnumerator EndEmission(float emiting_time)
    {
        yield return new WaitForSeconds(emiting_time);
        trailRenderer.emitting = false;
    }

    public void StopEmiting()
    {
        trailRenderer.emitting = false;
    }



}
