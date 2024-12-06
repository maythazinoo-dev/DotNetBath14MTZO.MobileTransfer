using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DotNetBath14MTZO.MobileTransfer.Feature.MobileTransfer
{
    [Table("Tbl_User")]
    public class UserModel
    {
        [Key]
        public string? UserId { get; set; }
        public string? UserName { get; set; }
        public string? MobileNo { get; set; }
        public decimal? UserBalance { get; set; }
        public string? UserPassword { get; set; }
       
    }

    public class TransactionModel
    {
        [Key]
        public string? TransactionId { get; set; }
        public string? FromMobileNo { get; set; }
        public string? ToMobileNo { get; set; }
        public decimal? Amount { get; set; }
        public DateTime? TransactionDate { get; set; }
        public string? Notes { get; set; }

    }

    public class MobileResponseModel
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
    }
}
