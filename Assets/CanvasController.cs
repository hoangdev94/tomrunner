using UnityEngine;

public class CanvasController : MonoBehaviour
{
    public void GoToShop()
    {
        GameManager.Instance.GoToShop();
    }
    public void ShopGoToMenu()
    {
        GameManager.Instance.ShopToMenu();
    }
    public void Setting()
    {
        GameManager.Instance.Setting();
    }
    public void CloseSetting()
    {
        GameManager.Instance.CloseSetting();
    }
    public void BuyItems()
    {
        GameManager.Instance.BuyItems();
    }
    public void UseItem()
    {
        GameManager.Instance.UseItems();
    }
    public void GotoMenu()
    {
        GameManager.Instance.GoToMenu();
    }
    public void PlayAgain()
    {
        GameManager.Instance.RestartGame();
    }
}
