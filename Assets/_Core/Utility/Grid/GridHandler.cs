using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridHandler : MonoBehaviour
{
    [SerializeField]
    GridSettings _settings;
    [SerializeField]
    GridModule _module;

    void Start()
    {
        _module.CreateGrid(_settings, transform);
    }

    void OnDisable() 
    {
        _module.ReleaseGrid();
    }
}
