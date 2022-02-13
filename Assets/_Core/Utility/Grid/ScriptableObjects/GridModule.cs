using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GridModule", menuName = "Core/Grid/Grid Module", order = 0)]
public class GridModule : ScriptableObject
{
    Dictionary<Vector2Int, GridCell> _grid = new Dictionary<Vector2Int, GridCell>();
    Vector2Int _cache = Vector2Int.zero;
    Vector2 _origin;
    Vector2 _gridToWorld;

    public event System.Action GridCreated;

    public Vector2Int size = Vector2Int.zero;

    public void CreateGrid(GridSettings settings, Transform parent)
    {
        size.x = settings.maxWidth;
        size.y = settings.maxHeight;

        _origin = settings.origin - ((size - Vector2Int.one) * (settings.cellSize + settings.padding)) / 2.0f;
        _gridToWorld = settings.cellSize + settings.padding;

        GameObject cell;
        for (int x = 0; x < size.x; x++)
        {
            for (int y = 0; y < size.y; y++)
            {
                _cache.x = x;
                _cache.y = y;
                cell = Instantiate(settings.cell, (_cache * _gridToWorld + _origin), Quaternion.identity, parent);
                cell.transform.localScale = settings.cellSize;

                _grid.Add(_cache, new GridCell(_cache, cell));
            }
        }

        GridCreated?.Invoke();
    }
    
    public bool ValidateCell(Vector2Int position) => _grid.ContainsKey(position);
    
    public GridCell GetCell(Vector2Int position) => _grid[position];
    
    public void ReleaseGrid()
    {
        _grid.Clear();
    }
}
