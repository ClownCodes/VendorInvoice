using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using VendorInvoice.DataAccess;
using VendorInvoices.Entities;
using VendorInvoices.Services;

namespace VendorInvoice.Services
{
    public class VendorInvoiceManager : IVendorInvoiceManager
    {
        private VendorInvoiceDbContext _vendorInvoiceDbContext;
        public VendorInvoiceManager(VendorInvoiceDbContext vendorInvoiceDbContext)
        {
            _vendorInvoiceDbContext = vendorInvoiceDbContext;
        }

        public List<Vendor> GetAllVendorsAlphabetically(char lowerBound, char upperBound)
        {
            //List<Vendor> vendors = _vendorInvoiceDbContext.Vendors
            //.Where(v => v.Name.ToLower().Substring(0, 1).CompareTo(lowerBound) >= 0
            //&& v.Name.ToLower().Substring(0, 1).CompareTo(upperBound) <= 0)
            //.OrderBy(v => v.Name).ToList();
            List<Vendor> vendors = _vendorInvoiceDbContext.Vendors
                .Where(v => v.Name.ToLower().FirstOrDefault() >= lowerBound
                && v.Name.ToLower().FirstOrDefault() <= upperBound)
                .OrderBy(v => v.Name).ToList();
            return vendors;
        }

        public Vendor? GetVendorById(int vendorId)
        {
            return _vendorInvoiceDbContext.Vendors.Find(vendorId);
        }

        public void UpdateVendor(Vendor vendor)
        {
            _vendorInvoiceDbContext.Vendors.Update(vendor);
            _vendorInvoiceDbContext.SaveChanges();
        }

        public void AddNewVendor(Vendor vendor)
        {
            _vendorInvoiceDbContext.Vendors.Add(vendor);
            _vendorInvoiceDbContext.SaveChanges();

        }

        public Vendor DeleteVendorById(int vendorId)
        {
            Vendor vendor = _vendorInvoiceDbContext.Vendors.Find(vendorId);
            vendor.IsDeleted = true;
            _vendorInvoiceDbContext.Vendors.Update(vendor);
            _vendorInvoiceDbContext.SaveChanges();
            return vendor;
        }


        public List<Invoice> GetInvoicesByVendorId(int vendorId)
        {
            List<Invoice> invoices = _vendorInvoiceDbContext.Invoices.Where(i => i.VendorId == vendorId).OrderBy(i => i.InvoiceId).ToList();
            return invoices;
        }

        public List<PaymentTerms> GetAllPaymentTerms()
        {
            return  _vendorInvoiceDbContext.PaymentTerms.ToList();
        }

        public void AddNewInvoice(Invoice invoice)
        {
            _vendorInvoiceDbContext.Invoices.Add(invoice);
            _vendorInvoiceDbContext.SaveChanges();
        }
        public Invoice? GetFirstInvoiceByVendorId(int vendorId)
        {
            return _vendorInvoiceDbContext.Invoices.Where(i => i.VendorId == vendorId).FirstOrDefault();
        }

        public Invoice GetInvoiceById(int invoiceId)
        {
            return _vendorInvoiceDbContext.Invoices.Find(invoiceId);
        }
        public List<InvoiceLineItem> GetInvoiceLineItemsByInvoiceId(int invoiceId)
        {
            List<InvoiceLineItem> invoiceLineItems = _vendorInvoiceDbContext.InvoiceLineItems.Where(ili => ili.InvoiceId == invoiceId)
                .OrderBy(ili => ili.InvoiceLineItemId).ToList();
            return invoiceLineItems;
        }

        public void AddNewInvoiceLineItem(InvoiceLineItem invoiceLineItem)
        {
            _vendorInvoiceDbContext.InvoiceLineItems.Add(invoiceLineItem);
            _vendorInvoiceDbContext.SaveChanges();
        }

        public void UndoDeleteById(int vendorId)
        {
            Vendor vendor = _vendorInvoiceDbContext.Vendors.Find(vendorId);
            vendor.IsDeleted = false;
            _vendorInvoiceDbContext.Vendors.Update(vendor);
            _vendorInvoiceDbContext.SaveChanges();
        }
    }
}
