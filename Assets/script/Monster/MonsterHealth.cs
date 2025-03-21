using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterHealth : MonoBehaviour
{
    [SerializeField]
    public float maxHealth;
    public float currentHealth;
    [SerializeField]
    private Animator animator;
    [SerializeField]
    private float DIES;
    public SpriteRenderer spriteRenderer;
    public bool flip;
    public float DalayDamage;
    public Renderer objectRenderer;
    public Color flashColor = Color.red;
    private Color originalColor;

    
    // Start is called before the first frame update
    void Start()
    {
        objectRenderer = GetComponent<SpriteRenderer>();
        originalColor = objectRenderer.material.color;
        currentHealth = maxHealth;
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        if(spriteRenderer.flipX == true)
        {
            flip = true;
        }
        else if(spriteRenderer.flipX == false)
        {
            flip = false;
        }
        DalayDamage += Time.deltaTime;
    }

    // Update is called once per frame
    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        Debug.Log("Damage" + damage);
        StartCoroutine(FlashObject());

        if(currentHealth <= 0) 
        {
            
            StartCoroutine(Die());
            
        }

    }

    public void TakeDamageAuraBlade(int damage)
    {
        if(DalayDamage >= 0.2f)
        {
            currentHealth -= damage;
            Debug.Log("Damage" + damage);
            DalayDamage = 0f;
            StartCoroutine(FlashObject());
        }

        if(currentHealth <= 0) 
        {
            StartCoroutine(Die());
        }

    }

        private IEnumerator FlashObject()
    {
            objectRenderer.material.color = flashColor;
            yield return new WaitForSeconds(0.2f);
            objectRenderer.material.color = originalColor;
            yield return new WaitForSeconds(0.2f);
    }




    IEnumerator Die()
    {
        animator.SetTrigger("die");
        Debug.Log("die");
        yield return new WaitForSeconds(DIES);
        Debug.Log("Des enemy");
        Destroy(gameObject);
    }

    //public void Die() 
    //{
    //    Debug.Log("Enemy Die");
    //    Destroy(gameObject);

    //}



}
