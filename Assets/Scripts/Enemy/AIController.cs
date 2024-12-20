using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine;
using UnityEngine.UIElements;
using System.Security.Cryptography;

namespace Enemy
{
    [RequireComponent(typeof(NavMeshAgent))]
    [RequireComponent(typeof(Animator))]
    public class AIController : MonoBehaviour, IInteraction
    {
        public enum AIState
        {
            Idle,
            Patrol,
            Wander,
            Stun,
            Attack,
            Chase
        }
        #region Variables 
        //the current state - going to need states???
        [SerializeField] private AIState _state = AIState.Idle;
        //Nav Mesh Agent
        [SerializeField] private NavMeshAgent _agent;
        //Animator
        [SerializeField] private Animator _animator;
        //walk speed and a run/chase speed
        [SerializeField] private float _walkSpeed = 2f, _runSpeed = 5;
        //patrolPoints/wayPoints []array of locations
        [SerializeField] private Transform[] _wayPoints;
        //iteration of array
        [SerializeField] private int _currentWayPointIndex = 0;
        // move randomly???
        [SerializeField] private Vector3 _randomPosition;
        //where are you player??
        [SerializeField] private float og_detectionRadius = 5f;
        [SerializeField] private float _detectionRadius = 5f;
        //who are you player??
        [SerializeField] private LayerMask _playerLayer;
        //keep chasing distance
        [SerializeField] private float _chaseDistance = 15f;
        //attack distance
        [SerializeField] private float _attackDistance = 2.5f;
        //time between attacks
        [SerializeField] private float _attackCooldown = 2f;

        [SerializeField] private float _lastAttackTime = 0f;

        [SerializeField] private float _stunDuration = 3f;
        PlayerMove playerMove;
        CharacterController cTroller;
        GameObject playerREF;
        bool pauseCheck;
        [SerializeField] float timer = 0;
        bool idleChoice = false;
        bool isStunned = false;
        [SerializeField] bool isPlayerRunning;
        #endregion
        #region Unity Event Functions
        private void Start()
        {
            playerREF = GameObject.FindWithTag("Player"); // Finds the GameObject with the Tag "Player"
            pauseCheck = playerREF.GetComponent<PlayerInteraction>().gamePaused; // Gets the paused bool value form the PlayerInteraction script
            _agent = GetComponent<NavMeshAgent>();
            _animator = GetComponent<Animator>();
            playerMove = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMove>();
            cTroller = GameObject.FindGameObjectWithTag("Player").GetComponent<CharacterController>();
            _state = AIState.Idle;
            TransitionToState(_state);

            og_detectionRadius = _detectionRadius;
        }
        private void Update()
        {
            if (!pauseCheck) 
            {
                _agent.speed = 1;
                // Check if player is running
                isPlayerRunning = cTroller.GetComponent<PlayerMove>().sprinting;
                if (isPlayerRunning)
                {
                    if (_detectionRadius == og_detectionRadius)
                    {
                        _detectionRadius = _detectionRadius * 2;
                    }
                }
                else
                {
                    _detectionRadius = og_detectionRadius;
                }


                switch (_state)
                {
                    case AIState.Idle:
                        StartCoroutine(Idle());
                        break;
                    case AIState.Patrol:
                        Patrol();
                        break;
                    case AIState.Wander:
                        Wander();
                        break;
                    case AIState.Stun:
                        Stun();
                        break;
                    case AIState.Attack:
                        Attack();
                        break;
                    case AIState.Chase:
                        Chase();
                        break;

                }
            }
            else
            {
                _agent.speed = 0;
            }
        }
        private void LateUpdate()
        {
            if (!pauseCheck)
            {
                float distanceToPlayer = Vector3.Distance(transform.position, GetPlayerPosition());
                if (distanceToPlayer <= _attackDistance)
                {
                    TransitionToState(AIState.Attack);
                }
                if (distanceToPlayer <= _detectionRadius && distanceToPlayer > _attackDistance)
                {
                    TransitionToState(AIState.Chase);
                }
                if (distanceToPlayer > _chaseDistance && _state == AIState.Chase)
                {
                    TransitionToState(AIState.Idle);
                }
                switch (_state)
                {
                    case AIState.Idle:
                        _agent.isStopped = true;
                        _agent.stoppingDistance = 2.5f;
                        _agent.speed = 0;
                        StartCoroutine(Idle());
                        break;
                    case AIState.Patrol:
                        _agent.isStopped = false;
                        _agent.stoppingDistance = 0f;
                        _agent.speed = _walkSpeed;
                        Patrol();
                        break;
                    case AIState.Wander:
                        _agent.isStopped = false;
                        _agent.stoppingDistance = 0f;
                        _agent.speed = _walkSpeed;
                        Wander();
                        break;
                    case AIState.Stun:
                        _agent.destination = this.transform.position;
                        _agent.isStopped = true;
                        _agent.stoppingDistance = 2.5f;
                        _agent.speed = 0;
                        Stun();
                        break;
                    case AIState.Attack:
                        _agent.isStopped = true;
                        _agent.stoppingDistance = 2.5f;
                        _agent.speed = 0;
                        Attack();
                        break;
                    case AIState.Chase:
                        _agent.isStopped = false;
                        _agent.stoppingDistance = 0f;
                        _agent.speed = _runSpeed;
                        Chase();
                        break;
                }
            }
            else
            {
                _agent.speed = 0;
            }
        }
        #endregion

        #region States
        void TransitionToState(AIState newState)
        {
            if (!pauseCheck)
            {
                _state = newState;
            }
            //switch (_state)
            //{
            //    case AIState.Idle:
            //         StartCoroutine(Idle());
            //        break;
            //    case AIState.Patrol:
            //        Patrol();
            //        break;
            //    case AIState.Wander:
            //        Wander();
            //        break;
            //    case AIState.Stun:
            //        Stun();
            //        break;
            //    case AIState.Attack:
            //        Attack();
            //        break;
            //    case AIState.Chase:
            //        Chase();
            //        break;
            //        //default:
            //        //    StartCoroutine(Idle());
            //        //    break;
            //}
        }
        IEnumerator Idle()
        {
            if (!pauseCheck)
            {
                if (isStunned)
                {
                    yield return null;
                }
                PlayAnim("Idle");
                _agent.stoppingDistance = 2.5f;
                _agent.speed = 0;
                timer = Random.Range(3, 10f);
                yield return new WaitForSeconds(timer);
                if (_state == AIState.Idle)
                {
                    int choice = Random.Range(0, 2);
                    if (choice == 0)
                    {
                        _randomPosition = GetRandomPosition();
                        TransitionToState(AIState.Wander);

                    }
                }
                else
                {
                    TransitionToState(AIState.Patrol);
                }
            }
        }


        void Patrol()
        {
            if (!pauseCheck)
            {
                if (isStunned)
                {
                    return;
                }
                _agent.speed = _walkSpeed;
                _agent.stoppingDistance = 0f;

                PlayAnim("Walk");
                if (_wayPoints.Length == 0)
                {
                    TransitionToState(AIState.Idle);
                }
                if (!_agent.pathPending && _agent.remainingDistance <= _agent.stoppingDistance)
                {
                    int choice = Random.Range(0, 6);
                    if (choice == 0)
                    {
                        TransitionToState(AIState.Idle);
                    }
                    else
                    {
                        TransitionToNextWayPoint();
                    }
                }
            }
        }

        void TransitionToNextWayPoint()
        {
            if (!pauseCheck)
            {
                _currentWayPointIndex = (_currentWayPointIndex + 1) % _wayPoints.Length;
                _agent.SetDestination(_wayPoints[_currentWayPointIndex].position);
                _agent.speed = _walkSpeed;
                PlayAnim("Walk");
            }
        }
        void Wander()
        {
            if (!pauseCheck)
            {

                if (isStunned)
                {
                    return;
                }
                _agent.SetDestination(_randomPosition);
                _agent.stoppingDistance = 0f;

                while (_agent.pathStatus == NavMeshPathStatus.PathInvalid)
                {
                    GetRandomPosition();
                    _agent.SetDestination(_randomPosition);
                }
                _agent.speed = _walkSpeed;
                PlayAnim("Walk");
                Debug.Log("1");
                if (_agent.remainingDistance <= 1f)
                {
                    Debug.Log("2");
                    int choice = Random.Range(0, 10);
                    Debug.Log("3: " + choice);

                    if (choice == 0)
                    {
                        Debug.Log("4: Idle");
                        TransitionToState(AIState.Idle);
                    }
                    else
                    {
                        Debug.Log("4: Wander");
                        _randomPosition = GetRandomPosition();
                        Debug.Log("5: Wander");
                        TransitionToState(AIState.Wander);
                    }
                }
            }
        }
        Vector3 GetRandomPosition()
        {
            Vector3 finalPosition = Vector3.zero;
            Vector3 randomDirection = Random.insideUnitSphere * 10;
            randomDirection += transform.position;
            NavMeshHit hit;
            if (NavMesh.SamplePosition(randomDirection, out hit, 10, 1))
            {
                finalPosition = hit.position;
                return finalPosition;

            }
            return this.transform.position;
        }
        void Chase()
        {
            if (!pauseCheck)
            {
                if (isStunned)
                {
                    Debug.Log("Return Coz Stunn");
                    return;
                
                }
                _agent.SetDestination(GetPlayerPosition());
                Debug.Log("Destination");
                PlayAnim("Run");
                Debug.Log("Animation");
                _agent.stoppingDistance = 0.2f;
                Debug.Log("stopp dist");
                _agent.speed = _runSpeed;
                Debug.Log("runSpeed");


                //// Slow down as the enemy approaches the player
                float distanceToPlayer = Vector3.Distance(transform.position, GetPlayerPosition());
                if (distanceToPlayer <= _attackDistance)
                {
                    TransitionToState(AIState.Attack);
                }           
            }
        }

        public void Activate()
        {
            Stun();
        }
        void Stun()
        {
            isStunned = true;
            // Play the stun animation
            PlayAnim("Stun");
            // Disable enemy and stop movement
             
            
            // Start a coroutine to resume after the stun duration
            StartCoroutine(RecoverFromStun());
        }

        IEnumerator RecoverFromStun()
        {
        
            // Wait for the stun duration
            yield return new WaitForSeconds(_stunDuration);


            isStunned = false;

            // Transition back to a previous state
            TransitionToState(AIState.Idle);
        }

        void Attack()
        {
            if (!pauseCheck)
            {
                if (isStunned)
                {
                    return;
                }
                // Stop enemy while attacking to prevent sliding past Player
                _agent.isStopped = true;
                _agent.speed = 0;
                _agent.stoppingDistance = 2f;
                // Trigger attack animation
                PlayAnim("Attack");

                // Once the attack is over, re-enable
                StartCoroutine(ResumeAfterAttack());
            }
        }

        // Coroutine to resume movement after the attack animation
        IEnumerator ResumeAfterAttack()
        {
            // Wait for the attack animation duration or a fixed time
            yield return new WaitForSeconds(1f);

            cTroller.GetComponent<PlayerHealth>().KillPlayer();

            //// After the attack, re-enable movement
            //_agent.isStopped = false;

            //// If the player is still in range, transition to Chase, otherwise go idle or patrol
            //float distanceToPlayer = Vector3.Distance(transform.position, GetPlayerPosition());
            //if (distanceToPlayer > _attackDistance)
            //{
            //    TransitionToState(AIState.Chase);
            //}
            //else
            //{
            //    TransitionToState(AIState.Patrol);
            //}
        }

        public void DealDamageToPlayer()
        {
            // Code for dealing damage to the player (this depends on your game's damage system)
            Debug.Log("Dealing damage to the player");
        }
        #endregion
        void PlayAnim(string trigger)
        {
            _animator.SetTrigger(trigger);
        }
        private Vector3 GetPlayerPosition()
        {
            return GameObject.FindWithTag("Player").transform.position;
        }
    }
}

