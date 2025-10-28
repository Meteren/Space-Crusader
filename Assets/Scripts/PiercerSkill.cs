using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;

public class PiercerSkill : MonoBehaviour
{
    [Header("Piercer Attributes")]
    [SerializeField] private float fillSpeed;
    [SerializeField] private AudioClip soundEffect;

    [HideInInspector] private Slider slider;

    [SerializeField] private LayerMask hitDetectionLayer;

    [SerializeField] private GameObject pierceEffect;

    [SerializeField] private GameObject releaseText;

    [HideInInspector] public float damageAmount = 50;


    //---
    PlayerController pController;

    Animator effectAnimator;

    GameObject instantiatedPierceEffect;
    //---

    private void Start()
    {
        slider = GetComponent<Slider>();
    }

    private void Update()
    {
        if(TouchManager.instance.activeTouchesCount > 0)
            slider.value += fillSpeed * Time.deltaTime;
        else
        {
            if(slider.value >= 1f)
            {
                SoundManager.instance.PlaySFX(soundEffect,1f);
                if(pController == null)
                    pController = FindFirstObjectByType<PlayerController>();

                instantiatedPierceEffect = Instantiate(pierceEffect);
                instantiatedPierceEffect.transform.position = pController.transform.position;
                effectAnimator = instantiatedPierceEffect.GetComponentInChildren<Animator>();
                Transform effectTransform = instantiatedPierceEffect.GetComponentInChildren<Transform>();
                //effectTransform.localScale *= CameraScaler.scaledRatio;

                List<RaycastHit2D> mainLine = Physics2D.RaycastAll(pController.transform.position,new Vector2(0,1), Mathf.Infinity, hitDetectionLayer).ToList();
                Debug.DrawRay(pController.transform.position, new Vector2(0, 1) * 10,Color.red,3f);
                List<RaycastHit2D> rightLine = Physics2D.RaycastAll(
                    pController.transform.position + new Vector3(pController.boundarySize.x / 2, 0, 0),
                    new Vector2(0, 1), Mathf.Infinity, hitDetectionLayer).ToList();
                Debug.DrawRay(pController.transform.position + new Vector3(pController.boundarySize.x / 2, 0, 0), new Vector2(0, 1) * 10, Color.red, 3f);
                List<RaycastHit2D> leftLine = Physics2D.RaycastAll(
                   pController.transform.position + new Vector3(pController.boundarySize.x / 2, 0, 0),
                   new Vector2(0, 1), Mathf.Infinity, hitDetectionLayer).ToList();
                Debug.DrawRay(pController.transform.position - new Vector3(pController.boundarySize.x / 2, 0, 0), new Vector2(0, 1) * 10, Color.red, 3f);

                mainLine.AddRange(rightLine);
                mainLine.AddRange(leftLine);

                slider.value = 0;

                for(int i = 0; i < mainLine.Count; i++)
                {
                    if (mainLine[i].transform.TryGetComponent<Asteroid>(out Asteroid asteroid))
                        asteroid.OnDamage(this);
                }

            }
            slider.value -= fillSpeed * Time.deltaTime;
        }

        if(effectAnimator != null)
        {
            AnimatorStateInfo stateInfo = effectAnimator.GetCurrentAnimatorStateInfo(0);

            if (stateInfo.IsName("skillAnim"))
            {
                if (stateInfo.normalizedTime >= 1)
                {
                    Destroy(instantiatedPierceEffect);
                }
            }
        
        }


        if (slider.value >= 1f)
            releaseText.SetActive(true);
        else
            releaseText.SetActive(false);


        slider.value = Mathf.Clamp01(slider.value);

    }
}
