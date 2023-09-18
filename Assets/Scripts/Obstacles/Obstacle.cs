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

        transform.gameObject.layer = LayerMask.NameToLayer("CantCollidePlayer");

        Debug.Log("inter");
    }

    private void HitEffect()
    {
        for (int i = 0; i < parts.Count; i++)
        {
            CheckValues(healthPercentages[i],parts[i]);
        } 
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
            Debug.Log(obstaclePart.name);
           /* hitPoints.Remove(obstaclePart.transform);
            healthPercentages.Remove(parts.IndexOf(obstaclePart));
            parts.Remove(obstaclePart);*/
            obstaclePart.SetActive(false);

        }
    }
}
