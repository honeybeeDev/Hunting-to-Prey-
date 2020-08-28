using UnityEngine;
using Vector3 = UnityEngine.Vector3;

public class Auto : MonoBehaviour
{
    private GameObject _character;

    public static bool CharacterSideDown { get; private set; }

    public bool IsGas { get; set; }
    public bool IsStop { get; set; }
    public bool IsRight { get; set; }
    public bool IsLeft { get; set; }

    [SerializeField] private GameObject _sideDownButton;
    [SerializeField] private GameObject _cameraPosition;
    [SerializeField] private GameObject _characterStandUpPosition;
    [SerializeField] private GameObject _autoController;
    [SerializeField] private GameObject _characterController;

    private Camera _camera;
    private Rigidbody _rigidbody;

    private float _speed = 60f;
    private float _angleSpeed = 50;


    private void Start()
    {
        _camera = Camera.main;
        _rigidbody = GetComponent<Rigidbody>();
        _character = Character.Singleton.gameObject;
    }

    private void Update()
    {
        if (!CharacterSideDown)
        {
            if (Vector3.Distance(Character.Singleton.gameObject.transform.position, transform.position) < 10)
            {
                _sideDownButton.SetActive(true);
            }
            else
            {
                _sideDownButton.SetActive(false);
            }
        }

        if (IsGas)
        {
            Gas();
        }

        if (IsStop)
        {
            Stop();
        }

        if(IsGas && IsLeft)
        {
            Gas();
            Left();
        }

        if (IsGas && IsRight)
        {
            Gas();
            Right();
        }
    }

    public void SideDown()
    {
        if (CharacterSideDown)
        {
            CharacterSideDown = false;
            _character.SetActive(true);
            _character.transform.position = _characterStandUpPosition.transform.position;
            _characterController.SetActive(true);
            _autoController.SetActive(false);
            _sideDownButton.SetActive(true);
            
        }
        else
        {
            CharacterSideDown = true;
            _character.SetActive(false);
            _camera.transform.parent = _cameraPosition.transform;
            _camera.transform.localPosition = Vector3.zero;
            _camera.transform.localRotation = Quaternion.Euler(Vector3.zero);
            _characterController.SetActive(false);
            _autoController.SetActive(true);
        }
    }

    private void Gas()
    {
        _rigidbody.AddForce(transform.forward * _speed);
    }

    private void Stop()
    {
        _rigidbody.AddForce(-transform.forward * _speed);
    }

    private void Left()
    {
        transform.Rotate(Vector3.up, -_angleSpeed * Time.deltaTime);
    }

    private void Right()
    {
        transform.Rotate(Vector3.up, _angleSpeed * Time.deltaTime);
    }
}
