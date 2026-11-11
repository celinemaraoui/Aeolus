using System.Collections;
using UnityEngine;
using UnityEngine.Playables;

public class BoxDropZone : MonoBehaviour
{
    [Header("Drop-Off Settings")]
    [SerializeField] private string boxTag = "Box";

    [Header("Cutscene References")]
    [SerializeField] private GameObject cutsceneManagerObject;
    [SerializeField] private float cutsceneDuration = 5.0f;

    [Header("Vessel Spawn References")]
    [SerializeField] private Transform xrOriginTransform;
    [SerializeField] private Transform vesselSpawnPoint;

    private bool hasPlayedCutscene = false;

    private void OnTriggerEnter(Collider other)
    {
        if (hasPlayedCutscene) return;

        if (other.CompareTag(boxTag) || other.GetComponent<BoxManager>() != null)
        {
            hasPlayedCutscene = true;
            Debug.Log("Box delivered! Playing one-time cutscene and teleporting...");

            Destroy(other.gameObject);
            StartCoroutine(DropOffCutsceneRoutine());
        }
    }

    private IEnumerator DropOffCutsceneRoutine()
    {
        if (cutsceneManagerObject != null)
        {
            cutsceneManagerObject.SetActive(true);

            PlayableDirector director = cutsceneManagerObject.GetComponentInChildren<PlayableDirector>();
            if (director != null)
            {
                director.Evaluate();
                director.Play();
            }
        }

        yield return new WaitForSeconds(cutsceneDuration);

        if (cutsceneManagerObject != null)
        {
            cutsceneManagerObject.SetActive(false);
        }

        if (xrOriginTransform != null && vesselSpawnPoint != null)
        {
            CharacterController cc = xrOriginTransform.GetComponent<CharacterController>();
            if (cc != null) cc.enabled = false;

            xrOriginTransform.position = vesselSpawnPoint.position;
            xrOriginTransform.rotation = vesselSpawnPoint.rotation;

            if (cc != null) cc.enabled = true;

            Debug.Log("Cutscene finished, timeline disabled, and player spawned in the vessel!");
        }
    }
}