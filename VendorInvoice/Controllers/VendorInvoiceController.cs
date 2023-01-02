using Microsoft.AspNetCore.Mvc;
using System.Collections;
using VendorInvoice.Models;
using VendorInvoices.Entities;
using VendorInvoices.Services;

namespace VendorInvoice.Controllers
{
    public class VendorInvoiceController : Controller
    {
        private IVendorInvoiceManager _vendorInvoiceManager;
        public VendorInvoiceController(IVendorInvoiceManager vendorInvoiceManager)
        {
            _vendorInvoiceManager = vendorInvoiceManager;
        }

        [HttpGet("/vendors/{lowerBound}-{upperBound}")]
        public IActionResult GetAllVendorsAlphabetically(char lowerBound, char upperBound)
        {
            List<Vendor> vendors = _vendorInvoiceManager.GetAllVendorsAlphabetically(lowerBound, upperBound);
            ViewData["GroupSelected"] = $"{lowerBound}{upperBound}";
            return View("Vendors", vendors);
        }

        [HttpGet("/vendors/{id}/{lowerBound}-{upperBound}/edit-request")]
        public IActionResult GetVendorById(int id, char lowerBound, char upperBound)
        {
            Vendor vendor = _vendorInvoiceManager.GetVendorById(id);
            AddEditVendorViewModel editVendorViewModel = new AddEditVendorViewModel()
            {
                Vendor = vendor,
                LowerBound = lowerBound,
                UpperBound = upperBound
            };
            return View("EditVendor", editVendorViewModel);
        }

        [HttpPost("/vendors/edit")]
        public IActionResult ProcessEditRequest(AddEditVendorViewModel addEditVendorViewModel)
        {
            if (ModelState.IsValid)
            {
                _vendorInvoiceManager.UpdateVendor(addEditVendorViewModel.Vendor);
                return RedirectToAction("GetAllVendorsAlphabetically", new { lowerBound = addEditVendorViewModel.LowerBound, upperBound = addEditVendorViewModel.UpperBound });
            }
            else
            {
                ModelState.AddModelError("", "There were errors in the form - please fix them and try again.");
                return View("EditVendor", addEditVendorViewModel);
            }
        }

        [HttpGet("/vendors/{lowerBound}-{upperBound}/add-request")]
        public IActionResult GetAddVendorRequest(char lowerBound, char upperBound)
        {
            AddEditVendorViewModel addEditVendorViewModel = new AddEditVendorViewModel()
            {
                LowerBound = lowerBound,
                UpperBound = upperBound
            };
            return View("AddVendor", addEditVendorViewModel);
        }

        [HttpPost("/vendors/add")]
        public IActionResult AddNewVendor(AddEditVendorViewModel addEditVendorViewModel)
        {
            if (ModelState.IsValid)
            {
                _vendorInvoiceManager.AddNewVendor(addEditVendorViewModel.Vendor);
                return RedirectToAction("GetAllVendorsAlphabetically", new { lowerBound = 'a', upperBound = 'e' }); //!!!!!!!!!!!!!!!!!!!!!!!!!
            }
            else
            {
                ModelState.AddModelError("", "There were errors in the form - please fix them and try again.");
                return View("AddVendor", addEditVendorViewModel);
            }
        }

        [HttpGet("/vendors/{lowerBound}-{upperBound}/{vendorId}/invoice/{invoiceId}/line-itmes")]
        public IActionResult GetInvoicsByVendorId(int vendorId, int invoiceId, char lowerBound, char upperBound)
        {
            Vendor activeVendor = _vendorInvoiceManager.GetVendorById(vendorId);
            Invoice? activeInvoice;
            if (invoiceId == -1)
            {
                activeInvoice = _vendorInvoiceManager.GetFirstInvoiceByVendorId(vendorId);
            }
            else
            {
                activeInvoice = _vendorInvoiceManager.GetInvoiceById(invoiceId);
            }
            List<Invoice> invoices = _vendorInvoiceManager.GetInvoicesByVendorId(vendorId);
            List<PaymentTerms> paymentTerms = _vendorInvoiceManager.GetAllPaymentTerms();
            List<InvoiceLineItem> invoiceLineItems = new List<InvoiceLineItem>();
            if (activeInvoice != null)
            {
                invoiceLineItems = _vendorInvoiceManager.GetInvoiceLineItemsByInvoiceId(activeInvoice.InvoiceId);
            }
            InvoicesViewModel invoicesViewModel = new InvoicesViewModel()
            {
                ActiveVendor = activeVendor,
                ActiveInvoice = activeInvoice,
                Invoices = invoices,
                NewInvoice = new Invoice(),
                PaymentTerms = paymentTerms,
                LineItems = invoiceLineItems,
                LowerBound = lowerBound,
                UpperBound = upperBound
            };
            return View("Invoices", invoicesViewModel);
        }

        [HttpPost("/vendors/{lowerBound}-{upperBound}/{vendorId}/invoices/add")]
        public IActionResult AddInvoiceToVendorById(int vendorId, InvoicesViewModel invoicesViewModel, char lowerBound, char upperBound)
        {
            List<Invoice> invoices = _vendorInvoiceManager.GetInvoicesByVendorId(vendorId);
            invoicesViewModel.NewInvoice.VendorId = vendorId;
            invoices.Add(invoicesViewModel.NewInvoice);
            _vendorInvoiceManager.AddNewInvoice(invoicesViewModel.NewInvoice);
            return RedirectToAction("GetInvoicsByVendorId", new { vendorId = vendorId, invoiceId = invoicesViewModel.NewInvoice.InvoiceId, lowerBound = lowerBound, upperBound = upperBound });
        }

        [HttpPost("/invoices/{lowerBound}-{upperBound}/{id}/line-items/add")]
        public IActionResult AddInvoiceLineItemsByInvoiceId(int id, char lowerBound, char upperBound, InvoicesViewModel invoicesViewModel)
        {
            List<InvoiceLineItem> invoiceLineItems = _vendorInvoiceManager.GetInvoiceLineItemsByInvoiceId(id);
            invoicesViewModel.NewLineItem.InvoiceId = id;
            invoiceLineItems.Add(invoicesViewModel.NewLineItem);
            _vendorInvoiceManager.AddNewInvoiceLineItem(invoicesViewModel.NewLineItem);
            Invoice invoice = _vendorInvoiceManager.GetInvoiceById(id);
            return RedirectToAction("GetInvoicsByVendorId", new { vendorId = invoice.VendorId, invoiceId = id, lowerBound = lowerBound, upperBound = upperBound });
        }

        [HttpGet("/vendors/{lowerBound}-{upperBound}/{id}/delete")]
        public IActionResult ProcessDeleteRequestById(int id, char lowerBound, char upperBound)
        {
            var vendor = _vendorInvoiceManager.DeleteVendorById(id);

            HttpContext.Session.SetString("LastDeletedVendorId", id.ToString());

            TempData["LastActionMessage"] = $"The vendor \"{vendor.Name}\" was deleted.";

            return RedirectToAction("GetAllVendorsAlphabetically", new { lowerBound = lowerBound, upperBound = upperBound });
        }
        
        [HttpGet("/vendors/{lowerBound}-{upperBound}/{id}/undo-delete")]
        public IActionResult ProcessUndoDeleteRequestById(char lowerBound, char upperBound, int id)
        {
            _vendorInvoiceManager.UndoDeleteById(id);
            return RedirectToAction("GetAllVendorsAlphabetically", new { lowerBound = lowerBound, upperBound = upperBound });
        }
    }
}
