using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(Animator))]
public class PlayerController : MonoBehaviour
{
    [SerializeField] private InputAction _moveAction;
    [SerializeField] private float _moveSpeed;
    [SerializeField] private InputAction _jumpAction;
    [SerializeField] private float _jumpSpeed;
    [SerializeField] private LayerMask _platformsLayerMask;

    public bool MoveEnabled { get; set; }

    private Rigidbody2D _rigidbody;
    private BoxCollider2D _boxCollider;
    private Animator _animator;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _boxCollider = GetComponent<BoxCollider2D>();
        _animator = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        _moveAction.Enable();
        _jumpAction.Enable();
    }

    private void OnDisable()
    {
        _moveAction.Disable();
        _jumpAction.Disable();
    }

    private void Update()
    {
        if (transform.position.y < -8)
        {
            gameObject.SetActive(false);
            GameManager.ShowGameOverWindow();
            return;
        }

        var isGrounded = GetStandingPlatform() != null;
        var velocityX = 0f;
        var velocityY = _rigidbody.velocity.y;

        if (MoveEnabled)
        {
            velocityX = _moveAction.ReadValue<float>() * _moveSpeed;

            if (isGrounded)
            {
                velocityY = _jumpAction.ReadValue<float>() * _jumpSpeed;
            }
        }

        if (velocityX > 0) transform.rotation = new Quaternion(0, 0, 0, 1);
        if (velocityX < 0) transform.rotation = new Quaternion(0, 180, 0, 1);

        _animator.SetFloat("velocityX", Mathf.Abs(velocityX));
        _animator.SetBool("isGrounded", isGrounded);

        _rigidbody.velocity = new Vector2(velocityX, velocityY);
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("Coin"))
        {
            Destroy(collider.gameObject);
            GameManager.AddCoin();
        }

        if (collider.CompareTag("Spike"))
        {
            MoveEnabled = false;
            GameManager.ShowGameOverWindow();
        }

        if (collider.CompareTag("Finish"))
        {
            MoveEnabled = false;
            _animator.SetTrigger("victory");
        }
    }

    public Transform GetStandingPlatform()
    {
        var contactFilter = new ContactFilter2D
        {
            layerMask = _platformsLayerMask,
            useLayerMask = true
        };
        var hits = new RaycastHit2D[1];
        
        _boxCollider.Cast(Vector2.down, contactFilter, hits, .1f);

        return hits[0].transform;
    }

    private void OnVictoryAnimationExit()
    {
        GameManager.GoToNextLevel();   
    }
}