using System;
using UnityEngine;

public class BoardTile : MonoBehaviour
{

    
    [SerializeField] private Transform foodStandardVisual;

    [SerializeField] private Transform foodReflectedVisual;

    [SerializeField] private Transform foodPoisonedVisual;

    [SerializeField] private Transform tileBlockedVisual;

    public bool ContainsFoodStandard
    {
        get => containsFoodStandard;
        set
        {
            containsFoodStandard = value;
            foodStandardVisual.gameObject.SetActive(value);
        }
    }
    private bool containsFoodStandard;
    
    public bool ContainsFoodReflected
    {
        get => containsFoodReflected;
        set
        {
            containsFoodReflected = value;
            foodReflectedVisual.gameObject.SetActive(value);
        }
    }
    private bool containsFoodReflected;
    
    
    public bool ContainsFoodPoisoned
    {
        get => containsFoodPoisoned;
        set
        {
            containsFoodPoisoned = value;
            foodPoisonedVisual.gameObject.SetActive(value);
        }
    }
    private bool containsFoodPoisoned;

    public bool IsBlocked
    {
        get => isBlocked;
        set
        {
            isBlocked = value;
            tileBlockedVisual.gameObject.SetActive(value);
        }
    }

    private bool isBlocked;

    public bool IsEmpty => !IsBlocked && !ContainsFoodStandard && !ContainsFoodReflected && !ContainsFoodPoisoned;

    public void SetEmpty()
    {
        IsBlocked = false;
        ContainsFoodPoisoned = false;
        ContainsFoodReflected = false;
        ContainsFoodStandard = false;
    }
    
    private void Awake()
    {
        foodStandardVisual.gameObject.SetActive(false);
        foodReflectedVisual.gameObject.SetActive(false);
        foodPoisonedVisual.gameObject.SetActive(false);
        IsBlocked = false;
    }
}
