using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Invoices.Web.API.Models;

namespace Invoices.Console.Client.Models
{
    public class InvoiceDTO
    {
        public int InvoiceId { get; set; }

        public string InvoiceNumber { get; set; }

        public List<InvoiceItem> InvoiceItems { get; set; }

        public Customer Customer { get; set; }

        public decimal Total { get; set; }
    }

    public class InvoiceItemsDTO
    {
        public int ItemId { get; set; }

        public string ItemName { get; set; }

        public decimal ItemPrice { get; set; }
    }

    public class CustomerDTO
    {
        public int Customerid { get; set; }

        public string CustomerName { get; set; }
    }
}
