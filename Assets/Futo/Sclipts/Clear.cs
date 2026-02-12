using System.Collections.Generic;
using UnityEngine;

public class Clear : MonoBehaviour
{
    private FutoSceneManager _manager;
    private int _count = 0;

    private void Start()
    {
        _manager = FindAnyObjectByType<FutoSceneManager>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            other.gameObject.SetActive(false);
            _count++;
            if(_count >= 2)
            {
                _manager.SceneChange(3);
            }
        }
    }
}
