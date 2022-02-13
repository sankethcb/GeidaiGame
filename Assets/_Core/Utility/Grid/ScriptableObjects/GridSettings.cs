using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GridSettings", menuName = "Core/Grid/Grid Settings", order = 1)]
public class GridSettings : ScriptableObject
{
    [SerializeField]
    int _maxWidth;
    public int maxWidth => _maxWidth;

    [SerializeField]
    int _maxHeight;
    public int maxHeight => _maxHeight;

    [SerializeField]
    private Vector2 _cellSize = Vector2.one;
    public Vector2 cellSize => _cellSize;

    [SerializeField]
    private Vector2 _padding = Vector2.zero;
    public Vector2 padding => _padding;

    [SerializeField]
    private Vector2 _origin = Vector2.zero;
    public Vector2 origin => _origin;

    [SerializeField]
    private GameObject _cell;
    public GameObject cell => _cell;
}
