using UnityEngine;
using TMPro;

public class Potion : MonoBehaviour
{

    [Header("Thông số lọ")]
    public string varName;
    public int value;
    public bool hasWater;

    [Header("Tham chiếu")]
    public GameObject bottleModel;
    public GameObject bottleFill;
    public TMP_Text labelText;


    private Renderer bottleRenderer;
    private Color originalEmission = Color.black;
    private Color highlightColor = new Color(0.7f, 0.7f, 0.7f); // Màu sáng khi hover

    private void OnValidate()
    {
        UpdateVisual();
    }

    private void Start()
    {
        if (bottleModel != null)
        {
            bottleRenderer = bottleModel.GetComponent<Renderer>();
        }
        UpdateVisual();
    }

    public void UpdateVisual()
    {
        // show/hide water
        if (bottleFill != null)
        {
            bottleFill.SetActive(hasWater);
        }
        if (labelText != null)
        {
            if (hasWater)
            {
                labelText.text = (value > 0) ? $"{varName}\n({value})" : $"{varName}\n ";
            }
            else
            {
                labelText.text = $"{varName}\n(Rỗng)";
            }
        }
    }
       

    public void SetHighlight(bool isOn)
    {
        if (bottleRenderer != null)
        {
            Color color = isOn ? highlightColor : originalEmission;
            bottleRenderer.material.SetColor("_EmissionColor", color);

            // Kích hoạt tính năng Emission nếu chưa có
            if (isOn) bottleRenderer.material.EnableKeyword("_EMISSION");
        }
    }

    // Lấy Material hiện tại của nước
    public Material GetWaterMaterial()
    {
        if (bottleFill != null)
        {
            return bottleFill.GetComponent<Renderer>().sharedMaterial;
        }
        return null;
    }

    // Gán Material mới cho nước
    public void SetWaterMaterial(Material newMat)
    {
        if (bottleFill != null)
        {
            bottleFill.GetComponent<Renderer>().material = newMat;
        }
    }

    private void OnMouseEnter()
    {
        // Chỉ highlight nếu không trong quá trình animation và (là nguồn có nước hoặc là đích đang trống)
        SetHighlight(true);
    }

    private void OnMouseExit()
    {
        SetHighlight(false);
    }

}