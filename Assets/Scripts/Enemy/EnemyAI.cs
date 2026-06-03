using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Audio;
using UnityEngine.Events;

public enum StateID { Waiting, AtWindow, Fleeing, Follow }

public class EnemyAI : MonoBehaviour
{
    public UnityEvent shotreceived;
    public UnityEvent walkEvent;

    public AudioSource audioSource;

    [Header("Configuration")]
    public EnemyStats stats;

    [Header("Environment References")]
    public Transform[] windowPoints;
    public Transform[] escapePoints;

    [Header("Flags")]
    public bool isPlayerInside = false;
    public bool gotShot = false;
    public bool firstspawn = false;

    [Header("Animation")]
    public Animator anim;

    public NavMeshAgent Agent { get; private set; }

    private Dictionary<StateID, EnemyState> _states = new Dictionary<StateID, EnemyState>();
    private EnemyState _currentState;

    private void Awake()
    {
        Agent = GetComponent<NavMeshAgent>();

        _states.Add(StateID.Waiting, new StateWaiting());
        _states.Add(StateID.AtWindow, new StateAtWindow());
        _states.Add(StateID.Fleeing, new StateFleeing());
        _states.Add(StateID.Follow, new StateFollow());
    }

    private void Start()
    {
        _currentState = _states[StateID.Waiting];
        _currentState.Enter(this);
    }

    private void Update()
    {
        if (_currentState != null)
        {
            EnemyState newState = _currentState.Update(this);

            if (newState != null && newState != _currentState)
            {
                _currentState.Exit(this);

                _currentState = newState;
                _currentState.Enter(this);
            }
        }
    }

    public EnemyState GetState(StateID id)
    {
        return _states[id];
    }

    public void StartSpawns()
    {
        isPlayerInside = true;
    }

    public void StartRoar()
    {
        if (audioSource != null && stats.sfxSpawn != null)
        {
            audioSource.clip = stats.sfxSpawn;
            audioSource.Play();
        }
    }

    public void ChangeState(StateID id)
    {
        EnemyState newState = GetState(id);

        if (newState != null && newState != _currentState)
        {
            if (_currentState != null)
            {
                _currentState.Exit(this);
            }

            _currentState = newState;
            _currentState.Enter(this);
        }
    }

    public void StopRoar()
    {
        if (audioSource != null)
        {
            audioSource.Stop();
        }
    }

    public void PlayWalkSound()
    {
        if (walkEvent != null)
        {
            walkEvent.Invoke();
        }
    }

    public void TakeHit()
    {
        gotShot = true;

        if (shotreceived != null)
        {
            shotreceived.Invoke();
        }
    }
}