namespace MiniAccountManagementSystem.Entity
{
    public class Account
    {
        public int AccountID { get; set; }
        public string AccountName { get; set; }
        public int? ParentAccountID { get; set; }
        public string AccountType { get; set; }
    }
}
