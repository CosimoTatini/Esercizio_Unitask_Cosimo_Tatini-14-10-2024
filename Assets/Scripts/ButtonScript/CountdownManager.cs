using Cysharp.Threading.Tasks;
using System.Threading;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class CountdownManager : MonoBehaviour
{
    public UnityEvent onCountDownEnd;
    public RacersMovement[] _racers;
    [SerializeField] private TextMeshProUGUI _countdownText;
    public TextMeshProUGUI _winnerText;

    private CancellationTokenSource _cancellationToken = new CancellationTokenSource();

    private void OnEnable()
    {
        RacersMovement.onRaceFinish.AddListener(HandleRaceFinish);
    }

    private void OnDisable()
    {
        RacersMovement.onRaceFinish.RemoveListener(HandleRaceFinish);
    }

    public void StartCountdown()
    {
        if (_cancellationToken != null)
        {
            _cancellationToken.Cancel();
            _cancellationToken.Dispose();
        }

        _cancellationToken = new CancellationTokenSource();

        RunCountdown(10, _cancellationToken.Token).Forget();
    }

    private async UniTask RunCountdown(int duration, CancellationToken token)
    {
        for (int i = duration; i >= 0; i--)
        {
            _countdownText.text = i.ToString();

            if (token.IsCancellationRequested)
            {
                // return;
                //potrebbe essere meglio utilizzare questa chiamata
                //il return non è una cosa così utile in questo caso e in più non è buona pratica usare return
                //in una funzione asincrona. potrebbe causare problemi di sincronizzazione
                await UniTask.CompletedTask;
            }
            
            await UniTask.Delay(1000, cancellationToken: token);
        }

        _countdownText.text = "GO";
        onCountDownEnd?.Invoke();
    }

    private void HandleRaceFinish(string winner)
    {
        _winnerText.text = $"{winner} wins!";
        
        foreach (RacersMovement racer in _racers)
        {
            racer.StopMoving();
        }
    }
}
