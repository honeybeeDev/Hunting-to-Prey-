using UnityEngine;
using UnityEngine.UI;

public class Shoot : MonoBehaviour
{
    [SerializeField] private GameObject _aimUI;
    [SerializeField] private GameObject _bulletPoint;
    [SerializeField] private GameObject _bullet;
    [SerializeField] private ParticleSystem _shootVFX;
    [SerializeField] private AudioSource _shootSound;
    [SerializeField] private AudioSource _reloadSound;
    [SerializeField] private Text _bulletValueText;
    [SerializeField] private Slider _aimPlusSlider;

    public static GameObject BulletClone { get; private set; }

    private RaycastHit _hit;

    private Camera _camera;
    private int _bulletSpeed = 1000;
    private int _bulletValue = 10;
    private bool _aiming;

    private void Start()
    {
        _camera = Camera.main;
        _bulletValueText.text = _bulletValue + "/10";
    }

    public void StartShoot()
    {
        if(_bulletValue > 0)
        {
            if (Physics.Raycast(_camera.transform.position, _camera.transform.forward, out _hit))
            {
                if (_hit.collider.GetComponentInParent<Animal>())
                {
                    _hit.collider.GetComponentInParent<Animal>().HealthChange();
                }
            }

            _shootVFX.Play();
            _shootSound.Play();

            Vector3 startPos = _bulletPoint.transform.position;
            BulletClone = Instantiate(_bullet, startPos, transform.rotation);
            BulletClone.GetComponent<Rigidbody>().AddForce(transform.forward * _bulletSpeed, ForceMode.Impulse);
            Destroy(BulletClone, 5);

            _bulletValue -= 1;
            _bulletValueText.text = _bulletValue + "/10";
        }
        else
        {
            Debug.Log("Нет пуль.");
            _reloadSound.Play();
            _bulletValue = 10;
            _bulletValueText.text = _bulletValue + "/10";
        }
    }

    private void Update()
    {
        //if (bulletClone != null)
        //{
        //    Time.timeScale = 0.4f;
        //    _camera.transform.position = bulletClone.transform.position;
        //}

        if (_aiming)
        {
            _camera.fieldOfView = _aimPlusSlider.value;
        }
    }

    public void Aiming()
    {
        _aimUI.SetActive(true);
        _aiming = true;
    }
    
    public void AimingOff()
    {
        _aimUI.SetActive(false);
        _camera.fieldOfView = 60;
        _aiming = false;
    }

    public void Reload()
    {
        if (_bulletValue <= 9)
        {
            _reloadSound.Play();
            _bulletValue = 10;
            _bulletValueText.text = _bulletValue + "/10";
        }
        else if(_bulletValue == 10)
        {
            Debug.Log("Уже 10/10");
        }
    }
}