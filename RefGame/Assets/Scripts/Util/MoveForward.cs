using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveForward : MonoBehaviour
{
    public float sensitivity = 1f;

    bool isStarted = false;

    private void OnEnable()
    {
        GameManager.OnGameStarted += OnGameStarted;
        GameManager.OnGameEnded += OnGameEnded;
    }

    private void OnDisable()
    {
        GameManager.OnGameStarted -= OnGameStarted;
        GameManager.OnGameEnded -= OnGameEnded;
    }

    private void OnGameEnded()
    {
        isStarted = false;
    }

    private void OnGameStarted()
    {
        isStarted = true;
    }

    void Update()
    {
        if (!isStarted)
            return;

        transform.Translate(Vector3.forward * sensitivity * Time.deltaTime);
    }
}
