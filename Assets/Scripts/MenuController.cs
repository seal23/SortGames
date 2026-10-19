using UnityEngine;
using UnityEngine.SceneManagement; // Bắt buộc để chuyển Scene

public class MenuController : MonoBehaviour
{

    // Hàm mở màn chơi Hoán Đổi (Index 1 trong Build Settings)
    public void LoadLevelSwap()
    {
        SceneManager.LoadScene("LevelSwap");
    }

    // Hàm mở màn chơi Sắp Xếp (Index 2 trong Build Settings)
    public void LoadLevelSort()
    {
        SceneManager.LoadScene("HumanSort");
    }

    // Hàm quay về Menu chính (Dùng cho nút Back trong mỗi Level)
    public void BackToMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    // Hàm thoát Game
    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Game đã thoát!");
    }
}