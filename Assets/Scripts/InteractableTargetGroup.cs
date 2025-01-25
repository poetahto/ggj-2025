using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

[RequireComponent(typeof(CinemachineTargetGroup))]
public class InteractableTargetGroup : MonoBehaviour
{
    public Transform playerTransform;
    
    private CinemachineTargetGroup _targetGroup;
    private List<TargetInfo> _targetInfo;
    
    private void Start()
    {
        _targetGroup = GetComponent<CinemachineTargetGroup>();
        _targetInfo = new List<TargetInfo>();

        foreach (Interactable interactable in FindObjectsByType<Interactable>(FindObjectsSortMode.None))
        {
            _targetGroup.AddMember(interactable.transform, 0, interactable.radius);
            
            _targetInfo.Add(new TargetInfo {
                Interactable = interactable,
                TargetIndex = _targetGroup.FindMember(interactable.transform),
            });
        }
        
        gameObject.SetActive(false);
    }

    private void Update()
    {
        foreach (TargetInfo info in _targetInfo)
        {
            float distance = Vector3.Distance(info.Interactable.transform.position, playerTransform.position);
            float weight = Mathf.Lerp(0, info.Interactable.maxWeight, Mathf.Clamp01(info.Interactable.range - distance));
            _targetGroup.m_Targets[info.TargetIndex].weight = weight;
        }
    }
    
    private struct TargetInfo
    {
        public Interactable Interactable;
        public int TargetIndex;
    }
}
