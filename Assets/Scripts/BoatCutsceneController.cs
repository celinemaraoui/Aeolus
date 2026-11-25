using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.XR.Interaction.Toolkit.Locomotion;
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

public class BoatCutsceneController : MonoBehaviour
{
    [Header("Movement Toward Turbine")]
    [SerializeField] private Transform targetDestination;
    [SerializeField] private float boatSpeed = 4f;
    [SerializeField] private float turnSpeed = 2f;
    [SerializeField] private float arrivalDistance = 3f;

    [Header("Realistic Wave Bobbing (Up/Down + Pitch/Roll)")]
    [SerializeField] private bool enableWaveMotion = true;
    [SerializeField] private float bobHeight = 0.45f;
    [SerializeField] private float bobFrequency = 1.5f;
    [SerializeField] private float pitchAngle = 3f;
    [SerializeField] private float rollAngle = 4f;

    [Header("Water Splash on Dip")]
    [SerializeField] private ParticleSystem[] splashParticles;
    [SerializeField] private float dipThreshold = -0.25f;
    [SerializeField] private float splashCooldown = 0.75f;

    [Header("Player & Movement Settings")]
    [SerializeField] private Transform playerRig;
    [SerializeField] private Transform seatOrDeckAnchor;
    [SerializeField] private bool keepPlayerParentedToBoat = true;
    [SerializeField] private bool lockMovementDuringCutscene = false;
    [SerializeField] private GameObject[] locomotionObjectsToDisable;
    [SerializeField] private Behaviour[] locomotionComponentsToDisable;
    [SerializeField] private bool disableAllXRLocomotionProviders = false;

    [Header("Skip Cutscene")]
    [SerializeField] private bool allowSkipWithO = true;
    [SerializeField] private bool snapBoatToTargetOnSkip = true;

    [Header("Optional Scene Transition")]
    [SerializeField] private bool loadSceneOnArrival = false;
    [SerializeField] private string nextSceneName = "Turbine";

    private bool isCutsceneRunning;
    private bool skipRequested;
    private float baseY;
    private float lastSplashTime = -99f;
    private bool wasBelowDipThreshold;

    void Start()
    {
        baseY = transform.position.y;
        StartBoatCutscene();
    }

    void Update()
    {
        if (!isCutsceneRunning || !allowSkipWithO) return;

        if (PressedSkipKey())
        {
            skipRequested = true;
        }
    }

    private bool PressedSkipKey()
    {
#if ENABLE_INPUT_SYSTEM
        if (Keyboard.current != null && Keyboard.current.oKey.wasPressedThisFrame)
        {
            return true;
        }
#endif
#if ENABLE_LEGACY_INPUT_SYSTEM
        if (Input.GetKeyDown(KeyCode.O))
        {
            return true;
        }
#endif
        return false;
    }

    public void StartBoatCutscene()
    {
        if (isCutsceneRunning) return;
        isCutsceneRunning = true;
        skipRequested = false;
        baseY = transform.position.y;

        if (keepPlayerParentedToBoat && playerRig != null && seatOrDeckAnchor != null)
        {
            playerRig.SetParent(seatOrDeckAnchor);
            playerRig.localPosition = Vector3.zero;
            playerRig.localRotation = Quaternion.identity;
        }

        SetMovementLocked(lockMovementDuringCutscene);
        StartCoroutine(MoveBoatRoutine());
    }

    public void SkipCutscene()
    {
        if (!isCutsceneRunning) return;
        skipRequested = true;
    }

    private void SetMovementLocked(bool locked)
    {
        if (locomotionObjectsToDisable != null)
        {
            foreach (GameObject obj in locomotionObjectsToDisable)
            {
                if (obj != null)
                {
                    obj.SetActive(!locked);
                }
            }
        }

        if (locomotionComponentsToDisable != null)
        {
            foreach (Behaviour comp in locomotionComponentsToDisable)
            {
                if (comp != null)
                {
                    comp.enabled = !locked;
                }
            }
        }

        if (disableAllXRLocomotionProviders && playerRig != null)
        {
            LocomotionProvider[] providers = playerRig.GetComponentsInChildren<LocomotionProvider>(true);
            foreach (LocomotionProvider provider in providers)
            {
                if (provider != null)
                {
                    provider.enabled = !locked;
                }
            }
        }
    }

    private void TriggerSplashIfDipped(float yOffset)
    {
        bool isBelowThreshold = yOffset <= dipThreshold;

        if (isBelowThreshold && !wasBelowDipThreshold && Time.time - lastSplashTime >= splashCooldown)
        {
            lastSplashTime = Time.time;

            if (splashParticles != null)
            {
                foreach (ParticleSystem ps in splashParticles)
                {
                    if (ps != null)
                    {
                        ps.Play();
                    }
                }
            }
        }

        wasBelowDipThreshold = isBelowThreshold;
    }

    private IEnumerator MoveBoatRoutine()
    {
        float waveTime = 0f;
        Quaternion currentHeadingRot = transform.rotation;

        while (targetDestination != null && !skipRequested)
        {
            Vector3 toTarget = targetDestination.position - transform.position;
            toTarget.y = 0f;

            if (toTarget.sqrMagnitude <= arrivalDistance * arrivalDistance)
            {
                break;
            }

            if (toTarget.sqrMagnitude > 0.001f)
            {
                Quaternion desiredHeading = Quaternion.LookRotation(toTarget.normalized, Vector3.up);
                currentHeadingRot = Quaternion.Slerp(currentHeadingRot, desiredHeading, turnSpeed * Time.deltaTime);
            }

            Vector3 forwardStep = (currentHeadingRot * Vector3.forward) * (boatSpeed * Time.deltaTime);
            transform.position += forwardStep;
            baseY += forwardStep.y;

            if (enableWaveMotion)
            {
                waveTime += Time.deltaTime * bobFrequency;
                float yOffset = Mathf.Sin(waveTime) * bobHeight;
                float pitch = Mathf.Sin(waveTime * 0.85f) * pitchAngle;
                float roll = Mathf.Cos(waveTime * 0.7f) * rollAngle;

                Vector3 pos = transform.position;
                pos.y = baseY + yOffset;
                transform.position = pos;

                transform.rotation = currentHeadingRot * Quaternion.Euler(pitch, 0f, roll);

                TriggerSplashIfDipped(yOffset);
            }
            else
            {
                transform.rotation = currentHeadingRot;
            }

            yield return null;
        }

        if (skipRequested && snapBoatToTargetOnSkip && targetDestination != null)
        {
            transform.position = new Vector3(targetDestination.position.x, baseY, targetDestination.position.z);
            transform.rotation = currentHeadingRot;
        }

        FinishCutscene();
    }

    private void FinishCutscene()
    {
        SetMovementLocked(false);

        if (playerRig != null)
        {
            playerRig.SetParent(null);
        }

        isCutsceneRunning = false;

        if (loadSceneOnArrival && !string.IsNullOrEmpty(nextSceneName))
        {
            SceneManager.LoadScene(nextSceneName);
        }
    }
}