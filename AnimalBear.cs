using System.Collections;
using UnityEngine;

public class AnimalBear : Animal
{
    private Animator _bearAnimator;
    private Transform _transform;
    private Rigidbody _rigidbody;

    private float _bearTalkSpeed = 0.5f;
    private float _bearRunSpeed = 5f;

    private void Start()
    {
        Health = Random.Range(2, 4);

        _transform = GetComponent<Transform>();
        _bearAnimator = GetComponent<Animator>();
        _rigidbody = GetComponent<Rigidbody>();

        _transform.position = new Vector3(Random.Range(300, 730), 100,
                                            Random.Range(300, 730));
        _transform.rotation = Quaternion.Euler(0, Random.Range(-180, 180), 0);
    }

    private void Update()
    {
        if(Health > 0)
        {
            if (Vector3.Distance(Character.Singleton.gameObject.transform.position, _transform.position) > 50 && !IsAfraid)
            {
                _transform.Translate(Vector3.forward * _bearTalkSpeed * Time.deltaTime);
                _bearAnimator.Play("walk");
            }

            if (Vector3.Distance(Character.Singleton.gameObject.transform.position, _transform.position) > 50 && IsAfraid)
            {
                _transform.Translate(Vector3.forward * _bearRunSpeed * Time.deltaTime);
                _bearAnimator.Play("run");
            }

            if (Vector3.Distance(Character.Singleton.gameObject.transform.position, _transform.position) < 50)
            {
                StopCoroutine(Afraid());
                IsAfraid = false;
                Quaternion targetRotation = Quaternion.LookRotation(Character.Singleton.gameObject.transform.position - _transform.position);
                _transform.rotation = Quaternion.Slerp(_transform.rotation, targetRotation, Time.deltaTime);
                _transform.Translate(Vector3.forward * _bearRunSpeed * Time.deltaTime);
                _bearAnimator.Play("run");
            }

            if (Shoot.BulletClone != null && Vector3.Distance(Shoot.BulletClone.transform.position, _transform.position) < 50)
            {
                StartCoroutine(Afraid());
            }
        }
    }
    public override void HealthChange()
    {
        Health -= 1;

        if (Health <= 0)
        {
            _bearAnimator.Play("die");
            _rigidbody.isKinematic = true;
        }
    }

    public override IEnumerator Afraid()
    {
        IsAfraid = true;
        yield return new WaitForSeconds(10);
        IsAfraid = false;
    }
}
