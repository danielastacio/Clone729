using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mirror : MonoBehaviour
{
    public Color hoverColor, defaultColor;
    private SpriteRenderer mirror;
    public List <Texture2D> mirrorCursor;
    [SerializeField] private int frameCount;
    [SerializeField] private float frameRate;

    private int currentFrame;
    private float frameTimer;
    private void Awake()
    {
        mirror = transform.parent.gameObject.GetComponent<SpriteRenderer>();
        defaultColor = mirror.color;
    }
    private void OnMouseUp()
    {
        Debug.Log("mouse up");

        transform.parent.rotation *= Quaternion.Euler(0, 0, -90);

    }

    private void OnMouseOver()
    {
        mirror.color = hoverColor;
        Cursor.SetCursor(mirrorCursor[currentFrame], Vector2.zero, CursorMode.ForceSoftware);
    }
    private void OnMouseExit()
    {
        Cursor.SetCursor(default, Vector2.zero, CursorMode.ForceSoftware);  
        mirror.color = defaultColor;
    }

    private void Update()
    {
        frameTimer -= Time.deltaTime;
        if(frameTimer <= 0f)
        {
            frameTimer += frameRate;
            currentFrame = (currentFrame + 1) % frameCount;
   
        }
    }
}
