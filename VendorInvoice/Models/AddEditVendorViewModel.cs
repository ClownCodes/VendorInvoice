using VendorInvoices.Entities;

namespace VendorInvoice.Models
{
    public class AddEditVendorViewModel
    {
        public Vendor Vendor { get; set; }
        public char LowerBound { get; set; }
        public char UpperBound { get; set; }
    }
}
