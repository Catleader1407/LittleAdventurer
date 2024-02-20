using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    public float HorizontalInput;
    public float VerticalInput;
    public bool MouseButtonDown;
    public bool spaceKeyDown;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!MouseButtonDown && Time.timeScale != 0)
        {
            MouseButtonDown = Input.GetMouseButtonDown(0);
        }

        if (!spaceKeyDown && Time.timeScale != 0)
        {
            spaceKeyDown = Input.GetKeyDown(KeyCode.Space);
        }

        HorizontalInput = Input.GetAxisRaw("Horizontal");
        VerticalInput = Input.GetAxisRaw("Vertical");
    }
    private void OnDisable()
    {
        clearCache();
    }

    public void clearCache()
    {
        MouseButtonDown = false;
        spaceKeyDown = false;
        HorizontalInput = 0;
        VerticalInput = 0;
    }
}
