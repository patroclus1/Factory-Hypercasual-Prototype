using System.Collections.Generic;
using UnityEngine;
using System;

public class OutputStorage : MonoBehaviour
{
    private List<Resource> _storedResources = new List<Resource>();
    [SerializeField] private int _outputCapacity;
    public event Action<List<Resource>> TryStartOnResourceTaken;
    private InputStorage _inputStorage;
    public int OutputCapacity => _outputCapacity;
    public List<Resource> StoredResources 
    { 
        set { _storedResources = value; } 
        get { return _storedResources; }
    }

    public void TakeResourcesFromStorage(List<Resource> resourcesTaken)
    {
        for (int i = 0; i < resourcesTaken.Count; i++)
        {
            _storedResources.Remove(resourcesTaken[i]);

            if (_storedResources.Count < _outputCapacity)
            {
                print(_storedResources.Count);
                TryStartOnResourceTaken?.Invoke(_inputStorage.InputResources);
            }
        }
    }

    public void Initialize(InputStorage input)
    {
        _inputStorage = input;
    }
}
