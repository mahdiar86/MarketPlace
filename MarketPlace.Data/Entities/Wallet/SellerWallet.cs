using MarketPlace.Data.Entities.Common;
using MarketPlace.Data.Entities.Store;
using System.ComponentModel.DataAnnotations;

namespace MarketPlace.Data.Entities.Wallet
{
    public class SellerWallet : BaseEntity
    {
        #region Properties

        public long SellerId { get; set; }

        public int Price { get; set; }

        public TransactionType TransactionType { get; set; }

        [Display(Name = "توضیحات")]
        public string Description { get; set; }

        #endregion

        #region Relations

        public Seller Seller { get; set; }

        #endregion
    }

    public enum TransactionType 
    {
        [Display(Name = "واریز")]
        Deposit,
        [Display(Name = "برداشت")]
        Withrawal
    }
}
