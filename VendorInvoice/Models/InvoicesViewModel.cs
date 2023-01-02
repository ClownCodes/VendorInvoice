using VendorInvoices.Entities;

namespace VendorInvoice.Models
{
    public class InvoicesViewModel
    {
        public Vendor ActiveVendor { get; set; }

        public List<Invoice> Invoices { get; set; }

        public Invoice NewInvoice { get; set; }

        public List<PaymentTerms> PaymentTerms { get; set; }

        public Invoice? ActiveInvoice { get; set; }

        public List<InvoiceLineItem> LineItems { get; set; }

        public InvoiceLineItem NewLineItem { get; set; }
        public char LowerBound { get; set; }
        public char UpperBound { get; set; }
    }
}
