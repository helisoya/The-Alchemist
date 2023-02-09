using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartDialog : MonoBehaviour
{
    [SerializeField] private string dialogToLaunch;
    void Start()
    {
        DialogSystem.instance.StartDialog(dialogToLaunch);
    }
}
