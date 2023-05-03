using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameUIEffects : MonoBehaviour
{
    Animator animator;
    public GameObject fullMeterEffectObject;
    ParticleSystem fullMeterEffect;

    public GameObject distanceBoard;
    Animator distanceAnimator;
    public TextMeshProUGUI distanceText;

    public GameObject gameManager;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        fullMeterEffect = fullMeterEffectObject.GetComponent<ParticleSystem>();

        distanceAnimator = distanceBoard.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayFullMeterParticles()
    {
        fullMeterEffect.Play();
        AudioManager.Instance.PlaySFX2("Full Meter Particle", 1f, 0.5f);
        gameManager.GetComponent<GameManager>().FullMeterBonusGems();
    }

    public void PlayFullMeterAnimation()
    {
        animator.SetTrigger("Full Meter");
    }

    public void DisplayDistance(int distance)
    {
        distanceText.text = distance.ToString() + "M";
        distanceAnimator.SetTrigger("Display Distance");
    }
}
