using System;
using System.Collections;
using System.Collections.Generic;
using PirateJam.Scripts;
using UnityEngine;
using Yarn.Unity;

public class DialogueManager : MonoBehaviour
{
    [SerializeField] private FMODUnity.EventReference toneExcited, toneDissapointed, toneNeutral;

    private FMOD.Studio.EventInstance excitedInstance, dissapInstance, neutralInstance;

    private InMemoryVariableStorage _storage;
    private DialogueRunner runner;

    private void Start()
    {
        excitedInstance = FMODUnity.RuntimeManager.CreateInstance(toneExcited);
        dissapInstance = FMODUnity.RuntimeManager.CreateInstance(toneDissapointed);
        neutralInstance = FMODUnity.RuntimeManager.CreateInstance(toneNeutral);

        _storage = GetComponent<InMemoryVariableStorage>();
        runner = GetComponent<DialogueRunner>();
    }

    [YarnCommand("end")]
    public void EndDialogue()
    {
        GameManager.Instance.StopDialogue();
        //runner.Stop();
    }

    public void CharacterNoise()
    {
        _storage.TryGetValue("$tone", out string line);
        switch (line)
        {
            case "excited":
            {
                excitedInstance.start();
            }
                break;
            case "disappointed":
                dissapInstance.start();
                break;
            case "neutral":
                neutralInstance.start();
                break;
            default:
                Debug.LogError("Invalid tone!");
                break;
        }
    }
    
    
}
