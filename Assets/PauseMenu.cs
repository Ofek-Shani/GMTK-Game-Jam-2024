using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("escape"))
        {
            transform.GetChild(0).gameObject.SetActive(true);
        }
    }

    public void ClosePauseMenu(){
        transform.GetChild(0).gameObject.SetActive(false);
    }

    public void BackToLevelSelect(){
        SceneManager.LoadScene("Level Select");
    }
}
