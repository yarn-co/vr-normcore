using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]

public class Jump : MonoBehaviour

{
    [SerializeField] private InputActionReference jumpButton;
    [SerializeField] private float normalJumpForce = 500.0f;
    [SerializeField] private float highJumpForce = 5000.0f;
    private bool enemiesDefeated = false;
    private bool isOnPlatform = false;
    public TMP_Text powerUpText;

    private Rigidbody _body;

    /*    private bool IsGrounded => Physics.Raycast(
            new Vector2(transform.position.x, transform.position.y + 2.0f),
            Vector3.down, 2.0f);
    */
    private bool IsGrounded
    {
        get
        {
            RaycastHit[] hits;
            Ray ray = new Ray(transform.position + Vector3.up * 0.1f, Vector3.down);
            hits = Physics.RaycastAll(ray, 0.2f);
            foreach (RaycastHit hit in hits)
            {
                if (hit.collider.gameObject != this.gameObject) // Ensure the raycast is not hitting player's own collider
                {
                    Debug.Log("Hit: " + hit.collider.gameObject.name);
                    return true;
                }
            }
            return false;
        }
    }

    private void Start()
    {
        _body = GetComponent<Rigidbody>();
        jumpButton.action.performed += Jumping;
    }

    private void Jumping(InputAction.CallbackContext obj)
    {
        if (!IsGrounded) return;
        if (isOnPlatform && enemiesDefeated == true)
        {
            _body.AddForce(Vector3.up * highJumpForce);
        }
        else
        {
            _body.AddForce(Vector3.up * normalJumpForce);
        }       
    }

    private void Update()
    {
        int remainingEnemies = GameObject.FindGameObjectsWithTag("Enemy").Length;
        if (remainingEnemies == 0 && enemiesDefeated == false)
        {
            enemiesDefeated = true;
            StartCoroutine(DisplayPowerUpText());
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("EnemyPlatform"))
        {
            isOnPlatform = true;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("EnemyPlatform"))
        {
            isOnPlatform = false;
        }
    }

    IEnumerator DisplayPowerUpText()
    {
        Debug.Log("Starting delay");
        yield return new WaitForSeconds(3.0f);
        Debug.Log("Showing text");
        powerUpText.text = "<color=red>Power Up!</color> Your jump is now more powerful when standing on this platform.";
        yield return new WaitForSeconds(10.0f);
        powerUpText.text = "";
    }
}



/*{
    [SerializeField] private InputActionReference jumpButton;
    [SerializeField] private float jumpHeight = 2.0f;
    [SerializeField] private float gravityValue = -9.81f;

    private CharacterController _characterController;
    private Vector3 _playerVelocity;

    private void Awake() => _characterController = GetComponent<CharacterController>();

    private void OnEnable() => jumpButton.action.performed += Jumping;

    private void OnDisable() => jumpButton.action.performed -= Jumping;

    private void Jumping(InputAction.CallbackContext obj)
    {
        if (!_characterController.isGrounded) return;
        _playerVelocity.y += Mathf.Sqrt(f: jumpHeight * -3.0f * gravityValue);
    }

    private void Update()
    {
        if (_characterController.isGrounded && _playerVelocity.y < 0)
        {
            _playerVelocity.y = 0f;
        }

        Debug.Log(_playerVelocity.y);

        _playerVelocity.y += gravityValue * Time.deltaTime;
        _characterController.Move(_playerVelocity * Time.deltaTime);
    }
}*/