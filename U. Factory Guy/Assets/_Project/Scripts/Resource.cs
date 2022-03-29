using UnityEngine;

public class Resource : MonoBehaviour
{
    [SerializeField] private float _resourceProductionTime;
    [SerializeField] private ResourceTypes _resourceType;

    public ResourceTypes ResourceType => _resourceType;

    public float ResourceProductionTime => _resourceProductionTime;
}
