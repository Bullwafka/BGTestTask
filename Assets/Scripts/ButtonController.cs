using UnityEngine;

public class ButtonController : MonoBehaviour
{
    [SerializeField] private PlayerMove _player;
    [SerializeField] private GameObject _startButton;

    private bool _hold;
    private float _holdTime;

    private void Update()
    {
        if(_hold)
        {
            _holdTime += Time.deltaTime;

            if(_holdTime >= 2)
            {
                OnButtonUp();
                _holdTime = 0;
                _hold = false;
            }
        }
    }
    public void OnButtonDown()
    {
        _player.ApplyProtection();
        _hold = true;
    }

    public void OnButtonUp()
    {
        _player.RemoveProtection();
        _hold = false;
        _holdTime = 0;
    }

    public void OnStartButtonClick()
    {
        _player.EnabelMove = true;
        _startButton.SetActive(false);
    }
}

