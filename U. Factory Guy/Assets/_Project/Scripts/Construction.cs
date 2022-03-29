using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class Construction : MonoBehaviour
{
    [SerializeField] private Resource _producedResource;
    [SerializeField] private OutputStorage _outputStorage;
    [SerializeField] private InputStorage _inputStorage;
    [SerializeField] private GameObject _emptyInputWarning;
    [SerializeField] private GameObject _overloadedOutputWarning;

    private int _inputResourceCount;
    private float _productionTime;

    private void Awake()
    {
        _outputStorage.Initialize(_inputStorage);
    }

    private void OnEnable()
    {
        _outputStorage.TryStartOnResourceTaken += ProduceResources;
        _inputStorage.OnResourcesAdded += ProduceResources;
    }

    private void OnDisable()
    {
        _outputStorage.TryStartOnResourceTaken -= ProduceResources;
        _inputStorage.OnResourcesAdded -= ProduceResources;
    }

    private void ProduceResources(List<Resource> resources)
    {
        _productionTime = _producedResource.ResourceProductionTime;
        StartCoroutine(WaitForProduction(resources));
    }

    private IEnumerator WaitForProduction(List<Resource> inputResources)
    {
        for (int i = inputResources.Count - 1; i >= 0; i--)
        {
            print(inputResources.Count);
            _inputResourceCount++;
            _emptyInputWarning.SetActive(false);

            yield return new WaitForSeconds(_productionTime);

            Vector3 spawnPositionOffset = new Vector3(0, 0.1f * _outputStorage.StoredResources.Count, 0);
            var product = Instantiate(_producedResource, _outputStorage.transform.position + spawnPositionOffset, Quaternion.identity);
            product.transform.SetParent(_outputStorage.transform);

            _outputStorage.StoredResources.Add(product);
            DestroyResourceWhenDone(inputResources[i]);

            if (_outputStorage.StoredResources.Count >= _outputStorage.OutputCapacity)
            {
                _overloadedOutputWarning.SetActive(true);
                StopAllCoroutines();
            }
            else
            {
                _overloadedOutputWarning.SetActive(false);
            }
        }
        inputResources.Clear();
    }

    private void DestroyResourceWhenDone(Resource resourceToDestroy)
    {
        Destroy(resourceToDestroy.gameObject);
        _inputStorage.InputResources.Remove(resourceToDestroy);
        _inputResourceCount--;
        if (_inputResourceCount > 0) return;
        _emptyInputWarning.SetActive(true);
    }
}
