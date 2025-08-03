using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CavancTest : MonoBehaviour
{
    private void Awake()
    {
        UIManager.Instance.ShowPanel<GamePanel>();
    }
}
