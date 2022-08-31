
using System.ComponentModel.DataAnnotations;

namespace Invoices.Web.API.Models;

public class Customer
{
    [Key] 
    public int Customerid { get; set; }
    public string CustomerName { get; set; }

    public ICollection<Invoice> Invoices { get; set; } = new List<Invoice>();
}