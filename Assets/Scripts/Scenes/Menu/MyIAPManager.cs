using System;
using UnityEngine;
using UnityEngine.Purchasing;
using UnityEngine.UI;
using UnityEngine.Purchasing.Security;

using Unity.Services.Core;
using Unity.Services.Analytics;
using System.Collections.Generic;

public class MyIAPManager : MonoBehaviour, IStoreListener
{
    private static IStoreController m_StoreController;          // The Unity Purchasing system.
    private static IExtensionProvider m_StoreExtensionProvider; // The store-specific Purchasing subsystems.
    private static UnityEngine.Purchasing.Product test_product = null;

    IGooglePlayStoreExtensions m_GooglePlayStoreExtensions;

    public static string GOLD_50 = "gold50";
    public static string NO_ADS = "noads";
    public static string MYSUB = "mysub";

    public Text myText;

    private Boolean return_complete = true;

    async void Start()
    {
        try
        {
            await UnityServices.InitializeAsync();
            List<string> consentIdentifiers = await AnalyticsService.Instance.CheckForRequiredConsents();
        }
        catch (ConsentCheckException e)
        {
            MyDebug("Consent: :" + e.ToString());  // Something went wrong when checking the GeoIP, check the e.Reason and handle appropriately.
        }

        MyAction += myFunction;
    }

   

    public void InitializePurchasing()
    {
        if (IsInitialized())
        {
            return;
        }

        var builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());

        builder.Configure<IGooglePlayConfiguration>().SetDeferredPurchaseListener(OnDeferredPurchase);
        builder.Configure<IGooglePlayConfiguration>().SetQueryProductDetailsFailedListener(MyAction);

        builder.AddProduct(GOLD_50, ProductType.Consumable);
        builder.AddProduct(NO_ADS, ProductType.NonConsumable);
        builder.AddProduct(MYSUB, ProductType.Subscription);

        UnityPurchasing.Initialize(this, builder);
    }

    private event Action<int> MyAction;

    void myFunction(int myInt)
    {
        MyDebug("Listener = " + myInt.ToString());
    }
    private bool IsInitialized()
    {
        return m_StoreController != null && m_StoreExtensionProvider != null;
    }

    void OnDeferredPurchase(UnityEngine.Purchasing.Product product)
    {
        MyDebug($"Purchase of {product.definition.id} is deferred");
    }

    public void BuySubscription()
    {
        BuyProductID(NO_ADS);
    }

    public void BuyGold50()
    {
        BuyProductID(GOLD_50);
    }

    public void BuyNoAds()
    {
        BuyProductID(NO_ADS);
    }

    public void CompletePurchase()
    {
        if (test_product == null)
            MyDebug("Cannot complete purchase, product not initialized.");
        else
        {
            m_StoreController.ConfirmPendingPurchase(test_product);
            MyDebug("Completed purchase with " + test_product.transactionID.ToString());
        }

    }

    public void ToggleComplete()
    {
        return_complete = !return_complete;
        MyDebug("Complete = " + return_complete.ToString());

    }
    public void RestorePurchases()
    {
        m_StoreExtensionProvider.GetExtension<IAppleExtensions>().RestoreTransactions(result =>
        {
            
            if (result)
            {
                MyDebug("Restore purchases succeeded.");
                Dictionary<string, object> parameters = new Dictionary<string, object>()
                {
                    { "restore_success", true },
                };
                AnalyticsService.Instance.CustomData("myRestore", parameters);
            }
            else
            {
                MyDebug("Restore purchases failed.");
                Dictionary<string, object> parameters = new Dictionary<string, object>()
                {
                    { "restore_success", false },
                };
                AnalyticsService.Instance.CustomData("myRestore", parameters);
            }

            AnalyticsService.Instance.Flush();
        });

    }

    void BuyProductID(string productId)
    {
        if (IsInitialized())
        {
            UnityEngine.Purchasing.Product product = m_StoreController.products.WithID(productId);

            if (product != null && product.availableToPurchase)
            {
                MyDebug(string.Format("Purchasing product:" + product.definition.id.ToString()));
                m_StoreController.InitiatePurchase(product);
            }
            else
            {
                MyDebug("BuyProductID: FAIL. Not purchasing product, either is not found or is not available for purchase");
            }
        }
        else
        {
            MyDebug("BuyProductID FAIL. Not initialized.");
        }
    }

    public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
    {
        MyDebug("OnInitialized: PASS");

        m_StoreController = controller;
        m_StoreExtensionProvider = extensions;
        m_GooglePlayStoreExtensions = extensions.GetExtension<IGooglePlayStoreExtensions>();
      
    }

    public void OnInitializeFailed(InitializationFailureReason error)
    {
        // Purchasing set-up has not succeeded. Check error for reason. Consider sharing this reason with the user.
        MyDebug("OnInitializeFailed InitializationFailureReason:" + error);
    }

    public void OnInitializeFailed(InitializationFailureReason error, string message)
    {
        throw new NotImplementedException();
    }

    public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs args)
    {
        test_product = args.purchasedProduct;

        //var validator = new CrossPlatformValidator(GooglePlayTangle.Data(), AppleTangle.Data(), Application.identifier);
        //var result = validator.Validate(args.purchasedProduct.receipt);
        //MyDebug("Validate = " + result.ToString());

        if (m_GooglePlayStoreExtensions.IsPurchasedProductDeferred(test_product))
        {
            //The purchase is Deferred.
            //Therefore, we do not unlock the content or complete the transaction.
            //ProcessPurchase will be called again once the purchase is Purchased.
            return PurchaseProcessingResult.Pending;
        }
        if (return_complete)
        {
            MyDebug(string.Format("ProcessPurchase: Complete. Product:" + args.purchasedProduct.definition.id + " - " + test_product.transactionID.ToString()));
            return PurchaseProcessingResult.Complete;
        }
        else
        {
            MyDebug(string.Format("ProcessPurchase: Pending. Product:" + args.purchasedProduct.definition.id + " - " + test_product.transactionID.ToString()));
            return PurchaseProcessingResult.Pending;
        }

    }

    public void OnPurchaseFailed(UnityEngine.Purchasing.Product product, PurchaseFailureReason failureReason)
    {
        MyDebug(string.Format("OnPurchaseFailed: FAIL. Product: '{0}', PurchaseFailureReason: {1}", product.definition.storeSpecificId, failureReason));
    }

    public void ListPurchases()
    {
        foreach (UnityEngine.Purchasing.Product item in m_StoreController.products.all)
        {
            if (item.hasReceipt)
            {
                MyDebug("In list for  " + item.receipt.ToString());
            }
            else
                MyDebug("No receipt for " + item.definition.id.ToString());
        }
    }
    private void MyDebug(string debug)
    {
        Debug.Log(debug);
        myText.text += "\r\n" + debug;
    }

}
