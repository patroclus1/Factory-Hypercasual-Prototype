using System;
using System.Collections.Generic;
using UnityEngine;

public class InputStorage : MonoBehaviour
{
    [SerializeField] private ResourceTypes _inputResourceType;
    private List<Resource> _inputResources = new List<Resource>();
    public event Action<List<Resource>> OnResourcesAdded;
    public List<Resource> InputResources
    {
        get { return _inputResources; }
    }
    public ResourceTypes InputResourceType => _inputResourceType;

    public void AddResourcesForProduction(List<Resource> resources)
    {
        _inputResources.AddRange(resources);
        OnResourcesAdded?.Invoke(_inputResources);
    }
}
