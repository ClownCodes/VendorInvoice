using Microsoft.AspNetCore.Mvc;
using VendorInvoice.DataAccess;
using VendorInvoices.Entities;

namespace VendorInvoice.Controllers
{
    public class ValidationController : Controller
    {
        public ValidationController(VendorInvoiceDbContext vendorInvoiceDbContext)
        {
            _vendorInvoiceDbContext = vendorInvoiceDbContext;
        }

        /// <summary>
        /// This Action, which will be called by the jquery validation code,
        /// will pass the current value of the EmailAddress field and
        /// the action will return a JSON response indicating "true" if the email 
        /// address is not in use (i.e. is available to use) and a message indicating
        /// it's in use if it's already found in the DB for a customer.
        /// </summary>
        public IActionResult CheckPhone([Bind(Prefix = "Vendor.VendorPhone")] string vendorPhone)
        {
            Console.WriteLine($"In check email action for email: {vendorPhone}");

            string msg = CheckIfPhoneExistsInDb(vendorPhone);
            if (string.IsNullOrEmpty(msg))
            {
                TempData["newPhone"] = true;
                return Json(true);
            }
            else
            {
                return Json(msg);
            }
        }

        private string CheckIfPhoneExistsInDb(string vendorPhone)
        {
            string msg = "";
            if (!string.IsNullOrEmpty(vendorPhone))
            {
                Vendor vendor = _vendorInvoiceDbContext.Vendors.FirstOrDefault(v => v.VendorPhone == vendorPhone);
                if (vendor != null)
                    msg = $"The phone number \"{vendorPhone}\" is already in use.";
            }

            return msg;
        }

        private VendorInvoiceDbContext _vendorInvoiceDbContext;
    }
}
