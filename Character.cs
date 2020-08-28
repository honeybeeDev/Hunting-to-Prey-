using UnityEngine;

public class Character : MonoBehaviour
{
    public static Character Singleton { get; private set; }

    private JoyStickController _joyController;
    
    [SerializeField] private GameObject _pauseMenu;
    
    private Camera _camera;
    private Rigidbody _rigidbody;

    private float _speed = 30;
    private float _speedInAir = 15;
    private bool _isGround;

    private void Awake()
    {
        Singleton = this;
    }

    private void Start()
    {
        RenderSettings.skybox = SceneLoader.Skybox;

        _rigidbody = GetComponent<Rigidbody>();
        _camera = Camera.main;
        _joyController = GameObject.FindGameObjectWithTag("JoyStick").GetComponent<JoyStickController>();
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void Move()
    {
        if (_isGround)
        {
            _rigidbody.AddForce(_joyController.Vertical() * _camera.transform.forward * _speed);
            _rigidbody.AddForce(_joyController.Horizontal() * _camera.transform.right * _speed);
        }
        else
        {
            _rigidbody.AddForce(_joyController.Vertical() * _camera.transform.forward * _speedInAir);
            _rigidbody.AddForce(_joyController.Horizontal() * _camera.transform.right * _speedInAir);
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.collider.tag == "Ground")
        {
            _isGround = false;
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.collider.tag == "Ground")
        {
            _isGround = true;
        }
    }
 
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "DeadWater")
        {
            Time.timeScale = 0f;
            _pauseMenu.SetActive(true);
        }
    }
}
