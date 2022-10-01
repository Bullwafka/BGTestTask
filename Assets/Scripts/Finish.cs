using System.Collections;
using UnityEngine.UI;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class Finish : MonoBehaviour
{
    [SerializeField] private ParticleSystem _particleSystem;
    [SerializeField] private TMP_Text _congratulationText;
    [SerializeField] private Image _fadeInOutImage;
    
    private Animator _animator;

    private float _timer;

    private void Start()
    {
        _particleSystem.Stop();
        StartCoroutine(FadeInOut(255, 0));
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent<PlayerMove>(out PlayerMove player))
        {
            _animator = GetComponent<Animator>();
            StartCoroutine(FinishScreenAnimation());
        }
    }
    private IEnumerator FinishScreenAnimation()
    {
        _particleSystem.Play();
        _animator.Play("TextAnim");
        float t = 0;
        while (t < 1)
        {
            t += Time.deltaTime;
            _congratulationText.alpha = Mathf.Lerp(0, 255, t);
            yield return null;
        }

        t = 0;
        while (t < 1)
        {
            t += Time.deltaTime;
            _congratulationText.alpha = Mathf.Lerp(255, 0, t);
            yield return null;
        }

        _animator.StopPlayback();

        yield return StartCoroutine(FadeInOut(0, 255));
        _particleSystem.Stop();

        
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);

    }

    private IEnumerator FadeInOut(float start, float end)
    {
        float t = 0;
        var color = _fadeInOutImage.color;
        float alphaValue;

        while (t < 1)
        {
            t += Time.deltaTime;
            alphaValue = Mathf.Lerp(start, end, t);
            color.a = alphaValue;
            _fadeInOutImage.color = color;
            yield return null;
        }
    }
}
