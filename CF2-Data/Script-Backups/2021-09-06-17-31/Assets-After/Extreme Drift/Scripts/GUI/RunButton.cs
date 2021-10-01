using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class RunButton : MonoBehaviour
{
    public KeyCode keyPress;
    void Update()
    {
        if (ControlFreak2.CF2Input.GetKeyDown(keyPress))
            transform.GetComponent<Button>().onClick.Invoke();
    }
}

