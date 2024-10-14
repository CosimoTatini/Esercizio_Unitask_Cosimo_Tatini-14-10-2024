using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class CountdownManager : MonoBehaviour

{
    [SerializeField] private TextMeshProUGUI _countdowntext;
    public UnityEvent onCountDownEnd;
    public TextMeshProUGUI _winnertext;
    public RacersMovement[] _racers;

    private CancellationTokenSource _cancellationToken;

    private void OnEnable()
    {
        RacersMovement.onRaceFinish.AddListener(HandleRaceFinish);
    }

    private void OnDisable()
    {
        RacersMovement.onRaceFinish.RemoveListener(HandleRaceFinish);
    }

    public async void StartCountdown()
    {
      if (_cancellationToken!= null)
      {
        _cancellationToken.Cancel();
      }
        _cancellationToken = new CancellationTokenSource();
        await Runcountdown(10, _cancellationToken.Token);
    }

    private async UniTask Runcountdown(int duration, CancellationToken token)
    {
        for (int i = duration;i>=0; i--)
        {
            _countdowntext.text = i.ToString();
            await UniTask.Delay(1000, cancellationToken: token);
            
        }
        if (token.IsCancellationRequested)
        {
            return;
        }
        _countdowntext.text = "GO";
        onCountDownEnd?.Invoke();
    }

    private void HandleRaceFinish(string winner)
    {
        _winnertext.text = $"{winner} wins!";
        
        foreach (RacersMovement racer in _racers)
        {
            racer.StopMoving();
        }
    }
}
