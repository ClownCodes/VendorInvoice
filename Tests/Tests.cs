using Microsoft.AspNetCore.Mvc.ViewEngines;
using VendorInvoices.Entities;

namespace Tests
{
    public class Tests
    {
        [Fact]
        public void InvoiceLineItemsTotalAmountTest1()
        {
            // Arrange:
            Invoice invoice = new Invoice()
            {
                InvoiceId = 1,
                InvoiceDate = new DateTime(2022, 8, 5),
                InvoiceLineItems = new List<InvoiceLineItem>() { }
            };

            invoice.InvoiceLineItems.Add(new InvoiceLineItem()
            {
                Description = "Part 123",
                Amount = 50,
                InvoiceId = 1
            });

            invoice.InvoiceLineItems.Add(new InvoiceLineItem()
            {
                Description = "Part 123",
                Amount = 50.1,
                InvoiceId = 1
            });

            // Act:
            double totalAmount = invoice.InvoiceLineItems.Sum(i => i.Amount).GetValueOrDefault();

            // Assert:
            Assert.Equal(100.1, totalAmount);
        }

    }
}