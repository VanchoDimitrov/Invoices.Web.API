using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Invoices.Web.API.Models
{
    public class Invoice
    {
        [Key]
        public int InvoiceId { get; set; }
        public string InvoiceNumber { get; set; }
        public Customer Customer { get; set; }
        public decimal Total { get; set; }
        public List<InvoiceItem> InvoiceItems { get; set; } = new List<InvoiceItem>();
    }
}
