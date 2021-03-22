using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DamageOverlay : MonoBehaviour
{
    public Image overlayImage;

    private float r;
    private float g;
    private float b;
    private float a;

    private static float SHOW_ALPHA_VALUE = 0.34f;
    private static int OVERLAY_TIME = 1;

    void Start()
    {
        r = overlayImage.color.r;
        g = overlayImage.color.g;
        b = overlayImage.color.b;
        a = overlayImage.color.a;
    }

    private void Update()
    {
        AdjustColor();
    }

    public void Show()
    {
        a = SHOW_ALPHA_VALUE;
    }

    public void Hide()
    {
        a = 0.0f;
    }

    public void AdjustColor()
    {
        Color newColor = new Color(r, g, b, a);
        overlayImage.color = newColor;
    }

    public IEnumerator ShowOverlay()
    {
        Show();
        yield return new WaitForSeconds(OVERLAY_TIME);
        Hide();
    }
}
