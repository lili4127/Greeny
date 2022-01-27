using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class AlbedoPanel : MonoBehaviour
{
    private TextMeshProUGUI albedoText;

    private void Awake()
    {
        albedoText = GetComponentInChildren<TextMeshProUGUI>();
        albedoText.text = "Albedo is a term used in planetary science synonymous with reflection. It represents the amount of incoming radiation returned from a surface. Earths albedo is roughly 30% meaning 30 percent of incoming radiation hitting the surface of our planet is being reflected and not absorbed. This percentage is mainly thanks to the clouds that Earth has in its atmosphere.";
    }

    private void OnEnable()
    {
        
    }

    public void NextFrame()
    {
        albedoText.text = "Albedo differs by color with lighter surfaces being more reflective and darker ones being less reflective. Looking at the colors white, tan, green, and blue which can represent landscapes such as snow, sand, grass, and water on Earths surface, reflectivity decreases in that order. As polar ice caps begin to melt due to climate change, our planet is losing its most valuable reflective material causing it to absorb even more energy leading to global warming.";
    }
}
