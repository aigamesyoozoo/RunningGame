﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{

    public IEnumerator Shake(float duration, float magnitude, int times)
    {
        for (int i = 0; i < times; i++)
        {
            Vector3 originalPos = transform.localPosition;

            float elapsed = 0.0f;

            while (elapsed < duration)
            {
                float x = Random.Range(-1f, 1f) * magnitude;

                transform.localPosition = new Vector3(x, originalPos.y, originalPos.z);

                elapsed += Time.deltaTime;

                yield return null;
            }
            transform.localPosition = originalPos;

            yield return new WaitForSeconds(0.7f);
        }
    }
}
