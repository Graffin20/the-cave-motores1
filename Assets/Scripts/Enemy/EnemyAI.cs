using System.Collections;
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

    [Header("Combat")]
    public Transform[] attackPoints;
    public float attackRange = 1.5f;
    public LayerMask playerLayer;

    [Header("UI y Game Over")]
    public GameObject loseScreen;
    public GameObject winScreen;
    public float winScreenDelay = 3f;
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
        if (walkEvent != null) walkEvent.Invoke();

        if (audioSource != null && stats.footstepSounds != null && stats.footstepSounds.Length > 0)
        {
            AudioClip randomStep = stats.footstepSounds[Random.Range(0, stats.footstepSounds.Length)];
            audioSource.PlayOneShot(randomStep);
        }
    }

    public void PlayAttackSound()
    {
        if (audioSource != null && stats.sfxAttacks != null && stats.sfxAttacks.Length > 0)
        {
            AudioClip randomAttack = stats.sfxAttacks[Random.Range(0, stats.sfxAttacks.Length)];
            audioSource.PlayOneShot(randomAttack);
        }
    }

    public void PlayDeathSound()
    {
        if (audioSource != null && stats.sfxDeath != null && stats.sfxDeath.Length > 0)
        {
            AudioClip randomDeath = stats.sfxDeath[Random.Range(0, stats.sfxDeath.Length)];
            audioSource.PlayOneShot(randomDeath);
        }
    }

    public void TakeHit(bool isSpecialAmmo = false)
    {
        if (isSpecialAmmo)
        {
            if (Agent != null) Agent.enabled = false;
            Collider col = GetComponent<Collider>();
            if (col != null) col.enabled = false;
            if (anim != null) anim.SetTrigger("Die");
            StartCoroutine(ShowWinScreenDelayed());
        }
        else
        {
            if (ReloadLevel.isPhase2Active)
            {
                gotShot = true;
                if (shotreceived != null)
                {
                    shotreceived.Invoke();
                }
            }
            else
            {
                ChangeState(StateID.Fleeing);
            }
        }
    }

    public void DealDamage()
    {
        if (attackPoints == null || attackPoints.Length == 0)
        {
            return;
        }

        foreach (Transform point in attackPoints)
        {
            if (point == null) continue;

            Collider[] hitEnemies = Physics.OverlapSphere(point.position, attackRange, playerLayer);

            foreach (Collider hit in hitEnemies)
            {
                if (hit.CompareTag("Player"))
                {
                    if (loseScreen != null)
                    {
                        loseScreen.SetActive(true);
                        Time.timeScale = 0f;
                        Cursor.lockState = CursorLockMode.None;
                        Cursor.visible = true;
                    }
                }
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (attackPoints == null || attackPoints.Length == 0) return;

        Gizmos.color = Color.red;
        foreach (Transform point in attackPoints)
        {
            if (point != null)
            {
                Gizmos.DrawWireSphere(point.position, attackRange);
            }
        }
    }

    private IEnumerator ShowWinScreenDelayed()
    {
        yield return new WaitForSeconds(winScreenDelay);

        if (winScreen != null)
        {
            winScreen.SetActive(true);
            Time.timeScale = 0f;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }
}