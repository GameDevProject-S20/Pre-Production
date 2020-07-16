using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using System;

namespace SIEvents
{
    // Transaction condition. Listens for buy / sell events.
    // Selling with subtract from buying and vice versa.
    public class TransactionCondition : Condition
    {
        public enum TransactionTypeEnum { BUY, SELL };

        private readonly string ItemName;
        private readonly TransactionTypeEnum transactionType;
        private readonly int requiredCount;
        private int currentCount;
        private readonly int? ReqLocId;

        protected UnityAction<Events.TransactionEvents.Details> transactionAction;

        public TransactionCondition(string _description, string _itemName, int _requiredCount, TransactionTypeEnum _transactionType, int? requiredLocationId = null)
         : base(_description)
        {
            ItemName = _itemName;
            currentCount = 0;
            requiredCount = _requiredCount;
            transactionType = _transactionType;
            ReqLocId = requiredLocationId;
        }

        public override void AllowProgression()
        {
            if (transactionAction == null)
            {
                // Wanted to add a list of listeners to Condition class, but it involves a lot of type generics that make the code messy
                transactionAction = new UnityAction<Events.TransactionEvents.Details>((Events.TransactionEvents.Details details) => Handler(details));
            }
            EventManager.Instance.OnTransaction.AddListener(transactionAction);
        }

        public override void DisallowProgression()
        {
            EventManager.Instance.OnTransaction.RemoveListener(transactionAction);
        }

        /// <summary>
        /// Used to translate between Transaction Event types defined in the Event System and in this Condition
        /// </summary>
        /// <param name="from">The seller</param>
        /// <param name="to">The buyer</param>
        /// <returns>Either BUY or SELL, from the player's perspective</returns>
        protected TransactionTypeEnum GetTransactionType(Events.TransactionEvents.Entity from, Events.TransactionEvents.Entity to)
        {
            if (from == to) throw new ArgumentException("From and To cannot be the same source");
            return (to == Events.TransactionEvents.Entity.PLAYER) ? TransactionTypeEnum.BUY : TransactionTypeEnum.SELL;
        }

        // As is, you could feasibly immediately buy the item back from the store after completing the quest
        protected virtual void Handler(Events.TransactionEvents.Details details)
        {
            if (details.ItemName == ItemName && GetTransactionType(details.From, details.To) == transactionType)
            {
                currentCount += details.ItemCount;
                if (currentCount >= requiredCount)
                {
                    Satisfy();
                }
            }
        }

        public override string ToString()
        {
            return string.Format("{0} {1}{2}: {3}/{4}", (transactionType == TransactionTypeEnum.BUY) ? "Buy" : "Sell", ItemName, (ReqLocId.HasValue) ? " " + TownManager.Instance.GetTownById(ReqLocId.Value).Name : "", currentCount, requiredCount);
        }
    }
}