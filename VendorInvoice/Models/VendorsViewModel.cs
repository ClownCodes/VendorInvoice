using VendorInvoices.Entities;

namespace VendorInvoice.Models
{
    public class VendorsViewModel
    {
        public List<Vendor> Vendors { get; set; }
        public int LastDeletedVendorId { get; set; }
    }
}
