using UnityEngine;
using TMPro;
using System.Collections;

public class SimpleSwap : MonoBehaviour
{
    private Potion selected;
    private bool isAnimating = false;

    [Header("UI References")]
    public TextMeshProUGUI statusText; 

    [Header("Cấu hình vị trí đổ")]
    public Vector3 pourOffset = new Vector3(0.4f, 1.5f, 0f); // Chỉnh trong Inspector
    public float animationSpeed = 0.5f;

    [Header("Chọn 2 lọ A, B")]
    public Potion potionA;
    public Potion potionB;
    
    private int firstValueA, firstValueB;
    void Start()
    {
        if (statusText != null) statusText.text = "Hãy chọn lọ nguồn (có nước)";
        firstValueA = potionA.value;
        firstValueB = potionB.value;
    }



    void Update()
    {
        if (isAnimating) return;

        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                Potion clicked = hit.collider.GetComponent<Potion>();

                if (clicked != null)
                {
                    if (selected == null && clicked.hasWater)
                    {
                        selected = clicked;
                        // Nhấc nhẹ lên để báo hiệu đang chọn
                        if (selected.bottleModel != null)
                            selected.bottleModel.transform.localPosition += Vector3.up * 0.5f;

                        if (statusText != null)
                            statusText.text = $"Đã chọn {selected.varName}. Hãy chọn lọ đích rỗng.";
                    }
                    else if (selected != null && !clicked.hasWater)
                    {
                        StartCoroutine(PourAnimation(selected, clicked));
                        selected = null;
                    }
                    else if (selected != null)
                    {
                        if (selected.bottleModel != null)
                            selected.bottleModel.transform.localPosition -= Vector3.up * 0.5f;
                        selected = null;
                        if (statusText != null) statusText.text = "Đã hủy chọn. Hãy chọn lại.";
                    }
                }
            }
        }
    }

    IEnumerator PourAnimation(Potion source, Potion target)
    {
        isAnimating = true;

        if (statusText != null)
        {
            statusText.text = $"Thực hiện: {target.varName} = {source.varName}";
            statusText.color = Color.yellow; // Đổi màu để làm nổi bật dòng lệnh
        }

        Transform movingPart = source.bottleModel.transform;

        // Lưu vị trí gốc (trừ đi khoảng nhấc lên 0.5f lúc nãy)


        Vector3 originalPos = source.transform.position;
        Quaternion originalRot = source.transform.rotation;


        // VỊ TRÍ ĐÍCH = Vị trí lọ rỗng + Offset để miệng lọ khớp nhau
        Vector3 targetPos = target.transform.position + pourOffset;

        // 1. Di chuyển Model đến điểm đổ
        float elapsed = 0;
        while (elapsed < animationSpeed)
        {
            movingPart.position = Vector3.Lerp(originalPos, targetPos, elapsed / animationSpeed);
            elapsed += Time.deltaTime;
            yield return null;
        }

        // 2. Xoay nghiêng Model
        elapsed = 0;
        Quaternion targetRot = Quaternion.Euler(0, 0, 90f);
        while (elapsed < 0.3f)
        {
            movingPart.rotation = Quaternion.Lerp(originalRot, targetRot, elapsed / 0.3f);
            elapsed += Time.deltaTime;
            yield return null;
        }

        // 3. Đổ nước
        target.value = source.value; // Chuyển con số
        target.SetWaterMaterial(source.GetWaterMaterial());
        target.hasWater = true;
        source.hasWater = false;
        target.UpdateVisual();
        source.UpdateVisual();
        source.value = 0; // Reset giá trị lọ cũ

        yield return new WaitForSeconds(0.5f);

        // 4. Trở về vị trí cũ
        elapsed = 0;
        while (elapsed < animationSpeed)
        {
            movingPart.rotation = Quaternion.Lerp(targetRot, originalRot, elapsed / animationSpeed);
            movingPart.position = Vector3.Lerp(movingPart.position, originalPos, elapsed / animationSpeed);
            elapsed += Time.deltaTime;
            yield return null;
        }

        movingPart.position = originalPos;
        movingPart.rotation = originalRot;

        if (statusText != null)
        {
            statusText.color = Color.white;
            statusText.text = "Tiếp tục bước tiếp theo...";
        }

        isAnimating = false;

        CheckWinCondition();
    }

    void CheckWinCondition()
    {
        if ((potionA.value == firstValueB) && (potionB.value == firstValueA))
        {
            statusText.text = "<color=green>CHÚC MỪNG! BẠN ĐÃ HOÁN ĐỔI THÀNH CÔNG!</color>";
        }
    }
}