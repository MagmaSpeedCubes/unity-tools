using System;

using UnityEngine;
using UnityEngine.Events;


using MagmaLabs;

namespace MagmaLabs.Economy{
    public class ItemShop : MonoBehaviour
    {
        [SerializeField] private Item item;
        [SerializeField] private string currency;
        [SerializeField] private int price;

        public UnityEvent onInsufficientFunds;
        public UnityEvent onPurchaseSuccessful;
        public UnityEvent onPurchaseFailed;
        
        public void Buy()
        {
            Buy(1);
        }

        public void Buy(int amount)
        {
            try
            {
                int buyerFunds = SaveManager.instance.LoadInt(currency);
                if(buyerFunds >= price)
                {
                    buyerFunds -= price;
                    SaveManager.instance.SaveInt(currency, buyerFunds);
                    Item newItem = item.Copy();
                    newItem.id += SaveManager.instance.playerID;

                    SaveManager.instance.SaveString(newItem.ToString(), newItem.name);



                    onPurchaseSuccessful.Invoke();

                }
                else
                {
                    onInsufficientFunds.Invoke();
                }
            }catch(Exception e)
            {
                onPurchaseFailed.Invoke();
            }

        }

    }
}
