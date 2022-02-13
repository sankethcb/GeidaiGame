using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum OccupantTypes
{
    COMBATANT,
    HAZARD
}


public interface IOccupant
{
    GridCell CurrectCell { get; }

    OccupantTypes OccupantType { get; }

    bool Targetable { get; }
}


public class GridCell
{
    GameObject _cell;
    public GameObject Cell => _cell;

    Vector3 _worldPosition;
    public Vector3 WorldPosition => _worldPosition;

    Vector2Int _gridPosition;
    public Vector2Int GridPosition => _gridPosition;

    private List<IOccupant> _occupants = new List<IOccupant>();
    bool Occupied(OccupantTypes occupantType) => _occupants[(int)occupantType] == null;

    public GridCell(Vector2Int position, GameObject cell)
    {
        _gridPosition = position;
        _worldPosition = cell.transform.position;
        _cell = cell;
    }

    public void AddOccupant(IOccupant occupant)
    {
        _occupants.Insert((int)occupant.OccupantType, occupant);
    }

    public IOccupant RemoveOccupant(OccupantTypes occupantType)
    {
        IOccupant occupant = _occupants[(int)occupantType];
        _occupants[(int)occupantType] = null;

        return occupant;
    }

    public void DeleteCell()
    {
        UnityEngine.GameObject.Destroy(_cell);
    }

}