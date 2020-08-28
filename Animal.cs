using System.Collections;
using UnityEngine;

public class Animal : MonoBehaviour
{
    public int Health { get; set; }
    public bool IsAfraid { get; set; } = false;

    public virtual void HealthChange() { }

    public virtual IEnumerator Afraid() { yield return new WaitForSeconds(5); }
}

