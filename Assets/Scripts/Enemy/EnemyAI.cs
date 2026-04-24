using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

public enum StateID { Waiting, AtWindow, Fleeing }

public class EnemyAI : MonoBehaviour
{

    public UnityEvent shotreceived;

    [Header("Configuración")]
    public EnemyStats stats;

    [Header("Referencias de Entorno")]
    public Transform[] windowPoints;
    public Transform[] escapePoints;

    [Header("Banderas")]
    public bool isPlayerInside = false;
    public bool gotShot = false;
    public bool firstspawn = false;

    [Header("Animación")]
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

    public void TakeHit()
    {
        gotShot = true;
        shotreceived.Invoke();
    }
}