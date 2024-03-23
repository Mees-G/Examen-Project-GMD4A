using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class SelectedButtonOutlineManager : MonoBehaviour
{

    public static SelectedButtonOutlineManager INSTANCE;


    public Input input;
    private GameObject previousSelected;
    private Outline previousOutline;


    private void Awake()
    {
        input = new Input();
        INSTANCE = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        input.UI.Any.performed += OnAnyPressed;
    }

    private void OnAnyPressed(InputAction.CallbackContext context)
    {
        if (EventSystem.current.currentSelectedGameObject == null && context.control.device is Gamepad)
        {
            GameSceneManager.FindObjectOfType<Button>().Select(); ;
        }
    }

    void Update()
    {
        GameObject selected = EventSystem.current.currentSelectedGameObject;
        if (selected != null)
        {
            if (previousSelected != selected)
            {
                Outline outline = selected.GetComponent<Outline>();

                if (previousOutline != null)
                    previousOutline.enabled = false;

                if (outline == null)
                    outline = selected.AddComponent<Outline>();


                outline.enabled = true;
                outline.effectDistance = new Vector2(4, 4);
                outline.effectColor = Color.black;

                previousSelected = selected;
                previousOutline = outline;
            }
        }
    }
}
