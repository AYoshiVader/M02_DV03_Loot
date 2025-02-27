using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBehaviour : MonoBehaviour
{
    public float moveSpeed = 10f;
    public float rotateSpeed = 75f;
    public float jumpVelocity = 5f;

    public float distanceToGround = 0.1f;
    public LayerMask groundLayer;

    public GameObject bullet;
    public float bulletSpeed = 100f;

    private float vInput;
    private float hInput;
    public bool doJump = false;
    public bool shoot = false;

    private Rigidbody _rb;
    private CapsuleCollider _col;
    private GameBehaviour _gameManager;

    // Start is called before the first frame update
    void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _col = GetComponent<CapsuleCollider>();
        _gameManager = GameObject.Find("Game Manager").GetComponent<GameBehaviour>();
    }

    // Update is called once per frame
    void Update()
    {
        vInput = Input.GetAxis("Vertical") * moveSpeed;
        hInput = Input.GetAxis("Horizontal") * rotateSpeed;
        if (!doJump) {
            doJump = Input.GetKeyDown(KeyCode.Space);
        }
        if (!shoot)
        {
            shoot = Input.GetMouseButtonDown(0);
        }

        /*
        this.transform.Translate(Vector3.forward * vInput * Time.deltaTime);
        this.transform.Rotate(Vector3.up * hInput * Time.deltaTime);
        */


    }

    void FixedUpdate()
    {
        if (IsGrounded() && doJump)
        {
            _rb.AddForce(Vector3.up * jumpVelocity, ForceMode.Impulse);
            doJump = false;
        }

        if (shoot)
        {
            GameObject newBullet = Instantiate(bullet, this.transform.position + this.transform.right, this.transform.rotation) as GameObject;
            Rigidbody bulletRB = newBullet.GetComponent<Rigidbody>();
            bulletRB.velocity = this.transform.forward * bulletSpeed;
            shoot = false;
        }

        Vector3 rotation = Vector3.up * hInput;

        Quaternion angleRot = Quaternion.Euler(rotation * Time.fixedDeltaTime);

        _rb.MovePosition(this.transform.position + this.transform.forward * vInput * Time.fixedDeltaTime);

        _rb.MoveRotation(_rb.rotation * angleRot);
    }

    private bool IsGrounded()
    {
        Vector3 capsuleBottom = new Vector3(_col.bounds.center.x, _col.bounds.min.y, _col.bounds.center.z);
        bool grounded = Physics.CheckCapsule(_col.bounds.center, capsuleBottom, distanceToGround, groundLayer, QueryTriggerInteraction.Ignore);
        return grounded;
    }

    void OnCollisionEnter(Collision collision)
    {
        // 4
        if (collision.gameObject.name == "Enemy")
        {
            // 5
            _gameManager.HP -= 1;
        }
    }
}
