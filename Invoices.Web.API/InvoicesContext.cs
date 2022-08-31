using Invoices.Web.API.Models;
using Microsoft.EntityFrameworkCore;

namespace Invoices.Web.API
{
    public class InvoicesContext : DbContext
    {
        public InvoicesContext(DbContextOptions<InvoicesContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //modelBuilder.Entity<Invoice>()
            //    .HasMany(items => items.InvoiceItems);
        }

        public DbSet<Customer> Customers { get; set; }
        public DbSet<Invoice> Invoices { get; set; }
        public DbSet<InvoiceItem> InvoiceItems { get; set; }
    }
}
