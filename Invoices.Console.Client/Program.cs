
using Invoices.Web.API.Models;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using Invoices.Console.Client.Models;
using Newtonsoft.Json;

public class Program
{
    static HttpClient client = new();

    static void ShowInvoices(Invoice invoice)
    {
        InvoiceDTO invoiceDTO = new InvoiceDTO
        {
            InvoiceId = invoice.InvoiceId,
            InvoiceNumber = invoice.InvoiceNumber,
            Total = invoice.Total,
            Customer = invoice.Customer,
            InvoiceItems = invoice.InvoiceItems,
        };


        // get the total of all items within the Invoice
        var getTotal = GetTotal(invoice);

        Console.WriteLine($"ID {invoiceDTO.InvoiceId} " +
                          $"InvoiceNumber {invoiceDTO.InvoiceNumber} " +
                          $"Customer {invoiceDTO.Customer.CustomerName} " +
                          $"Total: {getTotal}");

        foreach (var items in invoiceDTO.InvoiceItems)
        {
            Console.WriteLine($" Item Name: {items.ItemName} Item Price: {items.ItemPrice}");
        }
    }

    // calculate SUM
    static decimal GetTotal(Invoice invoice)
    {
        InvoiceDTO invoiceDTO = new InvoiceDTO
        {
            InvoiceId = invoice.InvoiceId,
            InvoiceNumber = invoice.InvoiceNumber,
            Total = invoice.Total,
            Customer = invoice.Customer,
            InvoiceItems = invoice.InvoiceItems,
        };

        var itemsTotal = invoiceDTO.InvoiceItems.Select(p => p.ItemPrice).Sum();

        return itemsTotal;
    }

    static async Task<Uri> CreateInvoiceAsync(Invoice invoice)
    {
        var json = JsonConvert.SerializeObject(invoice);
        var content = new StringContent(json, UTF8Encoding.UTF8, "application/json");

        var response = await client.PostAsync("api/Invoices", content);

        //if (response.IsSuccessStatusCode)
        //{
        //    var stringData = await response.Content.ReadAsStringAsync();
        //    var result = JsonConvert.DeserializeObject<object>(stringData);
        //}

        //HttpResponseMessage response = await client.PostAsJsonAsync("api/Invoices", invoice);
        response.EnsureSuccessStatusCode();

        // return URI of the created resource
        return response.Headers.Location;
    }

    static async Task<Invoice> GetInvoiceAsync(string path)
    {
        Invoice invoice = null;

        HttpResponseMessage response = await client.GetAsync(path);

        if (response.IsSuccessStatusCode)
        {
            invoice = await response.Content.ReadFromJsonAsync<Invoice>();
        }

        return invoice;
    }

    static async Task<Invoice> GetInvoiceIdAsync(int invoiceId)
    {
        Invoice invoice = null;

        HttpResponseMessage response = await client.GetAsync($"api/Invoices/{invoiceId}");

        if (response.IsSuccessStatusCode)
        {
            invoice = await response.Content.ReadFromJsonAsync<Invoice>();
        }

        return invoice;
    }


    static async Task<Invoice?> UpdateInvoiceAsync(Invoice invoice)
    {
        HttpResponseMessage response = await client.PutAsJsonAsync($"api/Invoices/{invoice.InvoiceId}", invoice);
        response.EnsureSuccessStatusCode();

        // Deserialize the invoice data
        invoice = await response.Content.ReadFromJsonAsync<Invoice>();
        return invoice;
    }

    static async Task<HttpStatusCode> DeleteInvoiceAsync(int id)
    {
        HttpResponseMessage response = await client.DeleteAsync($"api/Invoices/{id}");
        return response.StatusCode;
    }

    static async Task Main()
    {
        var tasks = RunAsync();

        await Task.WhenAll(tasks);
        Console.ReadLine();
    }
    static async Task RunAsync()
    {
        client.BaseAddress = new Uri("https://localhost:7207/");
        client.DefaultRequestHeaders.Accept.Clear();
        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

        try
        {
            Invoice invoice = new Invoice()
            {
                InvoiceNumber = "Inv 1",
                Customer = new Customer()
                {
                    CustomerName = "Customer 1",
                },
                Total = 165,
                InvoiceItems = new List<InvoiceItem>()
                {
                    new () {ItemName = "Leb", ItemPrice = 45,},
                    new () {ItemName = "Sol", ItemPrice = 55,},
                    new () {ItemName = "Chocolate", ItemPrice = 65,},
                },
            };

            // Create an Invoice
            Console.WriteLine("Creating an Invoice....");

            var url = await CreateInvoiceAsync(invoice);
            Console.WriteLine($"Created invoice at {url}");
            Console.WriteLine("Invoice created....");

            Console.WriteLine();

            // Get the Invoice
            Console.WriteLine("Showing Invoices....");

            var createdInvId = await GetInvoiceAsync(url.PathAndQuery).ConfigureAwait(false);
            var inv = await GetInvoiceIdAsync(createdInvId.InvoiceId).ConfigureAwait(false);
            if (inv != null) ShowInvoices(inv);
            Console.WriteLine("Invoices displayed....");

            Console.WriteLine();

            //// Update the Invoice
            //Console.WriteLine("Updating an Invoice....");

            //invoice.InvoiceId = 1;
            //invoice.InvoiceNumber = "Inv 2";
            //invoice.Customer = "Customer 2";
            //invoice.Total = 200;
            //await UpdateInvoiceAsync(invoice).ConfigureAwait(false);
            //Console.WriteLine("Invoice Updated....");

            //Console.WriteLine();

            //// Get the Updated Invoice
            //Console.WriteLine("Updated Invoice....");

            //invoice = await GetInvoiceAsync(url.PathAndQuery).ConfigureAwait(false);
            //if (invoice != null) ShowInvoices(invoice);
            //Console.WriteLine("Updated Invoices displayed....");

            //Console.WriteLine();

            //// Delete an Invoice
            //Console.WriteLine("Deleting an Invoice....");

            //if (invoice != null)
            //{
            //    var statusCode = await DeleteInvoiceAsync(invoice.InvoiceId).ConfigureAwait(false);
            //    Console.WriteLine($"Invoice Deleted....HTTPS Status Code{statusCode}");
            //}

            //Console.WriteLine();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
    }
}