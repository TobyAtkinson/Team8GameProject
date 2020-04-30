using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CheckpointManager : MonoBehaviour
{
    public int Checkpointat = 0;

    void Awake()
    {
        DontDestroyOnLoad(this);

    }
}
