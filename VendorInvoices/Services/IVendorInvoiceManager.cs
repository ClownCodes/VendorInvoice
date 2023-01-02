using VendorInvoices.Entities;

namespace VendorInvoices.Services
{
    public interface IVendorInvoiceManager
    {
        public List<Vendor> GetAllVendorsAlphabetically(char lowerBound, char upperBound);
        public Vendor? GetVendorById(int vendorId); 
        public void UpdateVendor(Vendor vendor);
        public void AddNewVendor(Vendor vendor);
        public Vendor DeleteVendorById(int vendorId);


        public List<Invoice> GetInvoicesByVendorId(int vendorId);
        public List<PaymentTerms> GetAllPaymentTerms();
        public void AddNewInvoice(Invoice invoice);


        public Invoice? GetFirstInvoiceByVendorId(int vendorId);
        public Invoice GetInvoiceById(int invoiceId);
        public List<InvoiceLineItem> GetInvoiceLineItemsByInvoiceId(int invoiceId);
        public void AddNewInvoiceLineItem(InvoiceLineItem invoiceLineItem);

        public void UndoDeleteById(int vendorId);
    }
}
