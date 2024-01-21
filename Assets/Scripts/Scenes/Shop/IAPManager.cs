using Data;
using UnityEngine;
using UnityEngine.Purchasing;

namespace Scenes.Shop
{
    public class IAPManager:MonoBehaviour
    {
        public void OnPurchaseComplete(Product product)
        {
            switch (product.definition.id)
            {
                case "com.cubeminer.removeads":
                    ShopAdsButtonScript.RemoveAds();
                    break;
            }
        }

        public void OnPurchaseFailed(Product product, PurchaseFailureReason reason)
        {
            if (product.definition.id == "com.cubeminer.removeads" && reason == PurchaseFailureReason.DuplicateTransaction)
            {
                ShopAdsButtonScript.RemoveAds();
            }
        }
        
        
    }
}