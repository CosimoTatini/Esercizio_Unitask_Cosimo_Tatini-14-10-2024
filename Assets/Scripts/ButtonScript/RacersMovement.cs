using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class RacersMovement : MonoBehaviour
{
    private bool _shouldMove = false;
    private float _randomspeed;

    public static UnityEvent<string> onRaceFinish = new UnityEvent<string>();
    private static bool _raceEnded = false;

    private void Start()
    {
        _randomspeed = Random.Range(10f, 20f);
    }

    public void StartMoving()
    {
        _shouldMove = true;
    }
    private void Update()
    {
        if (_shouldMove && !_raceEnded)
        {
            transform.Translate(Vector3.forward* _randomspeed * Time.deltaTime);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
       if (other.gameObject.CompareTag ("Finish") && !_raceEnded)
       {
            _raceEnded = true;
            _shouldMove = false;
            string racerName = gameObject.name;
            onRaceFinish.Invoke(racerName);
       }
    }
    public void StopMoving()
    {
        _shouldMove = false;
    }
}
