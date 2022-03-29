using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    [SerializeField] private Transform _backpackTransform;
    [SerializeField] private int _backpackCapacity;
    [SerializeField] private float _interactionDuration = 0.5f;

    private List<Resource> _resourcesInBackpack = new List<Resource>();
    private List<Resource> _resourcesProcessed = new List<Resource>();

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent(out Resource baseResource) && _resourcesInBackpack.Count < _backpackCapacity)
        {
            other.enabled = false;
            StartCoroutine(PickupBaseResource(baseResource));
        }

        if (other.gameObject.TryGetComponent(out InputStorage inputStorage))
        {
            StartCoroutine(SendResourceToInput(inputStorage));
        }

        if (other.gameObject.TryGetComponent(out OutputStorage outputStorage))
        {
            StartCoroutine(TakeResourcesFromOutput(outputStorage));
        }
    }

    private IEnumerator PickupBaseResource(Resource resource)
    {
        _resourcesInBackpack.Add(resource);

        resource.transform.SetParent(_backpackTransform);
        StartCoroutine(LerpResourceMovement(resource, _backpackTransform, _resourcesInBackpack));
        yield return null;
    }

    private IEnumerator SendResourceToInput(InputStorage input)
    {
        foreach (Resource sentResource in _resourcesInBackpack)
        {
            if (sentResource.ResourceType == input.InputResourceType)
            {
                _resourcesProcessed.Add(sentResource);

                sentResource.transform.SetParent(input.transform);
                StartCoroutine(LerpResourceMovement(sentResource, input.transform, input.InputResources));
            }
        }
        input.AddResourcesForProduction(_resourcesProcessed);
        _resourcesInBackpack.RemoveAll(item => _resourcesProcessed.Contains(item));
        _resourcesProcessed.Clear();
        yield return null;
    }

    private IEnumerator TakeResourcesFromOutput(OutputStorage output)
    {
        foreach (var storedResource in output.StoredResources)
        {
            if (_resourcesInBackpack.Count < _backpackCapacity)
            {
                storedResource.transform.SetParent(_backpackTransform);
                _resourcesProcessed.Add(storedResource);
                _resourcesInBackpack.Add(storedResource);
                StartCoroutine(LerpResourceMovement(storedResource, _backpackTransform, _resourcesInBackpack));
                yield return null;
            }
        }
        output.TakeResourcesFromStorage(_resourcesProcessed);
        _resourcesProcessed.Clear();
    }

    private IEnumerator LerpResourceMovement(Resource resource, Transform endPoint, List<Resource> resourceStack)
    {
        float elapsedTime = 0;

        Vector3 positionOffset = new Vector3(0, 0.1f * resourceStack.Count, 0);

        var startPoint = resource.transform.position;
        while (elapsedTime < _interactionDuration)
        {
            resource.transform.position = Vector3.Lerp(startPoint, endPoint.position + positionOffset, elapsedTime / _interactionDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        resource.transform.position = endPoint.position + positionOffset;
    }
}
