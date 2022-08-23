using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Invoices.Web.API.Models;
using Microsoft.EntityFrameworkCore;

namespace Invoices.Web.API
{
    public class InvoicesContext : DbContext
    {
        public InvoicesContext(DbContextOptions<InvoicesContext> options) : base(options)
        {

        }

        public DbSet<Invoice>? Invoices { get; set; } = null;
    }
}
