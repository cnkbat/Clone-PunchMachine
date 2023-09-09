using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class Obstacle : DamagableObject , IDamagable, IInteractable
{
    [Header("Health")]
    [SerializeField] float maxHealth;
    float currentHealth;

    [Header("Health Bar")]
    [SerializeField] Image fillImage;
    [SerializeField] GameObject healthBar;
    [Header("Animation")]
    [SerializeField] List<GameObject> parts;
    [SerializeField] List<float> healthPercentages;

    void Start()
    {
        currentHealth = maxHealth;
        if(transform.parent.GetComponent<Gate>())
        {
            transform.parent.gameObject.layer = LayerMask.NameToLayer("ObstacledGate");
        }

        fillImage.fillAmount = (float)currentHealth/ (float)maxHealth;
        healthBar.SetActive(false);
    }

    public void Interact()
    {
        Player.instance.KnockbackPlayer();
        if(transform.parent.GetComponent<Gate>())
        {
            transform.parent.gameObject.layer = LayerMask.NameToLayer("ObstacledCard");
        }
        transform.gameObject.layer = LayerMask.NameToLayer("CantCollidePlayer");
    }

    private void HitEffect()
    {
        CheckValues(healthPercentages[0],parts[0]);
        CheckValues(healthPercentages[1],parts[1]);
        CheckValues(healthPercentages[2],parts[2]);
        CheckValues(healthPercentages[3],parts[3]);
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        HitEffect();

        healthBar.SetActive(true);
        fillImage.fillAmount = (float)currentHealth/ (float)maxHealth;

        if(currentHealth <= 0)
        {

            if(transform.parent.GetComponent<Gate>())
            {
                transform.parent.gameObject.layer = LayerMask.NameToLayer("Default");
            }

            Destroy(gameObject);
        }
    }

    private void CheckValues(float healthPercentage, GameObject obstaclePart)
    {
        if(currentHealth <= maxHealth * healthPercentage && obstaclePart.transform.parent == gameObject.transform)
        {
            hitPoints.Remove(obstaclePart.transform);
            Destroy(obstaclePart);

        }
    }
}
