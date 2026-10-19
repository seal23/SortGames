using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class LevelInitializer : MonoBehaviour
{
    [Header("Cấu hình lọ")]
    public List<Potion> potions; // Kéo 5 lọ V1-V5 vào đây
    public Potion tempPotion;    // Kéo lọ Temp vào đây

    [Header("Dữ liệu nước")]
    public List<Material> waterMaterials; // Danh sách 5 Material màu khác nhau

    [Header("UI References")]
    public TextMeshProUGUI statusText;

    void Start()
    {
        InitializeLevel();
    }

    public void InitializeLevel()
    {
        // 1. Tạo danh sách số từ 1 đến 5 và xáo trộn
        List<int> numbers = new List<int> { 1, 2, 3, 4, 5 };
        Shuffle(numbers);

        // 2. Xáo trộn danh sách Material để màu sắc không cố định với số
        List<Material> shuffledMats = new List<Material>(waterMaterials);
        Shuffle(shuffledMats);

        // 3. Gán giá trị và màu sắc cho 5 lọ đầu tiên
        for (int i = 0; i < potions.Count; i++)
        {
            potions[i].value = numbers[i];
            potions[i].hasWater = true;

            // Gán Material ngẫu nhiên từ danh sách đã xáo trộn
            if (i < shuffledMats.Count)
            {
                potions[i].SetWaterMaterial(shuffledMats[i]);
            }

            potions[i].UpdateVisual();
        }

        // 4. Thiết lập lọ Temp luôn rỗng khi bắt đầu
        if (tempPotion != null)
        {
            tempPotion.value = 0;
            tempPotion.hasWater = false;
            tempPotion.UpdateVisual();
        }

        if (statusText != null) statusText.text = "Hãy chọn một lọ để bắt đầu";
        Debug.Log("Màn chơi đã được khởi tạo ngẫu nhiên!");
    }

    // Thuật toán xáo trộn Fisher-Yates đơn giản
    void Shuffle<T>(List<T> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            T temp = list[i];
            int randomIndex = Random.Range(i, list.Count);
            list[i] = list[randomIndex];
            list[randomIndex] = temp;
        }
    }
}