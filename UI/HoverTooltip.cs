using UnityEngine;
using UnityEngine.EventSystems; // Required for hover events
using TMPro; // Use UnityEngine.UI for legacy text
namespace MagmaLabs.UI{
public class HoverTooltip : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public GameObject tooltipText; // Assign your text object in the inspector

    public void OnPointerEnter(PointerEventData eventData)
    {
        tooltipText.SetActive(true); // Show text
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        tooltipText.SetActive(false); // Hide text
    }
}
}
