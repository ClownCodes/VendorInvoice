using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace VendorInvoices.Entities
{
    public class Vendor
    {
        public int VendorId { get; set; }

        [Required(ErrorMessage = "Please enter a name.")]
        public string Name { get; set; } = null!;

        [Required(ErrorMessage = "Please enter an address.")]
        public string? Address1 { get; set; }

        public string? Address2 { get; set; }

        [Required(ErrorMessage = "Please enter a city.")]
        public string? City { get; set; } = null!;

        [Required(ErrorMessage = "Please enter a province or state.")]
        [RegularExpression("(?i)^[a-z]{2}$", ErrorMessage = "Province/state must be 2 letter code.")]
        public string? ProvinceOrState { get; set; } = null!;

        [Required(ErrorMessage = "Please enter a zip or postal code.")]
        [RegularExpression(@"[0-9]{5}|[0-9]{5}-[0-9]{4}|(?i)^[a-z][0-9][a-z]\s?[0-9][a-z][0-9]$", ErrorMessage = "Zip/postal code must be in a valid US or Canadian format.")] //? | bitwise 
        public string? ZipOrPostalCode { get; set; } = null!;

        [Required(ErrorMessage = "Please enter a phone number.")]
        [Remote("CheckPhone", "Validation")]
        [RegularExpression("^[0-9]{3}-?[0-9]{3}-?[0-9]{4}$", ErrorMessage = "Phone number must be in valid 10-digit format.")]
        public string? VendorPhone { get; set; }

        [Required(ErrorMessage = "Please enter a last name.")]
        public string? VendorContactLastName { get; set; }

        [Required(ErrorMessage = "Please enter a first name.")]
        public string? VendorContactFirstName { get; set; }

//        [Required(ErrorMessage = "Please enter an email address.")]
        [EmailAddress(ErrorMessage = "Please enter a valid email address")]
        public string? VendorContactEmail { get; set; }

        public bool IsDeleted { get; set; } = false;

        public ICollection<Invoice>? Invoices { get; set; }
    }
}
