using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatsController : MonoBehaviour
{
    [SerializeField]
    GameObject[] lifeIcons;
    
    private int livesLeft = 3, maxLives = 5;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ReduceLife()
    {
        livesLeft = Mathf.Clamp(--livesLeft, 0, maxLives);
        lifeIcons[livesLeft].gameObject.SetActive(false);
    }

    public void AddLife()
    {
        livesLeft = Mathf.Clamp(++livesLeft, 0, maxLives);
        lifeIcons[livesLeft-1].gameObject.SetActive(true);
    }


}
