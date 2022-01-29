using System.Collections;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    private void OnEnable()
    {
        Ball.ballAbsorbed += ShakeCamera;
    }

    private void OnDisable()
    {
        Ball.ballAbsorbed -= ShakeCamera;
    }

    private void ShakeCamera()
    {
        StartCoroutine(ShakeCo(0.1f, 0.05f));
    }

    IEnumerator ShakeCo(float duration, float magnitude)
    {
        Vector3 originalPos = Vector3.zero;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            float xOffset = Random.Range(-0.5f, 0.5f) * magnitude;
            float yOffset = Random.Range(-0.5f, 0.5f) * magnitude;

            transform.localPosition = new Vector3(xOffset, yOffset, originalPos.z);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.localPosition = originalPos;
    }
}
