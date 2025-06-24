﻿namespace MiniAccountManagementSystem.Entity
{
    public class Voucher
    {
        public int VoucherId { get; set; }
        public string VoucherType { get; set; }
        public DateTime? VoucherDate { get; set; }
        public string ReferenceNo { get; set; }
        public List<VoucherEntries> Entries { get; set; } = new(); // Safe default init
    }

}
