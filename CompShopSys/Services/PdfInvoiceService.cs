using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using TechShopMS.Models;
using System.Globalization;

public class PdfInvoiceService
{
    public void GenerateInvoice(Sale sale, Customer customer, string outputPath)
    {
        var phpCulture = new CultureInfo("en-PH");
        phpCulture.NumberFormat.CurrencySymbol = "₱";

        QuestPDF.Settings.License = LicenseType.Community;

        Document.Create(container =>
        {
            container.Page(page =>
            {
                page.Size(PageSizes.A4);
                page.Margin(2, Unit.Centimetre);

                // Header
                page.Header()
                    .Row(row =>
                    {
                        row.RelativeItem().Column(column =>
                        {
                            column.Item().Text("INVOICE").Bold().FontSize(20);
                            column.Item().Text($"No: {sale.InvoiceNumber}");
                            column.Item().Text($"Date: {sale.SaleDate:yyyy-MM-dd HH:mm}");
                        });

                        row.RelativeItem().Column(column =>
                        {
                            column.Item().Text("Bill To:").Bold();
                            column.Item().Text($"{customer.FirstName} {customer.LastName}");
                            column.Item().Text(customer.Email);
                            column.Item().Text(customer.ContactNumber);
                        });
                    });

                // Items Table
                page.Content()
                    .PaddingVertical(1, Unit.Centimetre)
                    .Table(table =>
                    {
                        table.ColumnsDefinition(columns =>
                        {
                            columns.RelativeColumn(3); // Product
                            columns.ConstantColumn(2, Unit.Centimetre); // Qty
                            columns.ConstantColumn(3, Unit.Centimetre); // Price
                            columns.ConstantColumn(3, Unit.Centimetre); // Total
                        });

                        table.Header(header =>
                        {
                            header.Cell().Text("Product").Bold();
                            header.Cell().AlignRight().Text("Qty").Bold();
                            header.Cell().AlignRight().Text("Price").Bold();
                            header.Cell().AlignRight().Text("Total").Bold();
                        });

                        foreach (var item in sale.Items)
                        {
                            table.Cell().Text(item.ProductName);
                            table.Cell().AlignRight().Text(item.Quantity.ToString());
                            table.Cell().AlignRight().Text(item.UnitPrice.ToString("C", phpCulture));
                            table.Cell().AlignRight().Text((item.Quantity * item.UnitPrice).ToString("C", phpCulture));
                        }
                    });

                // Footer (Totals)
                page.Footer()
                    .AlignRight()
                    .Column(column =>
                    {
                        column.Item().Text($"Subtotal: {sale.TotalAmount.ToString("C", phpCulture)}");
                        column.Item().Text($"Amount Paid: {sale.AmountPaid.ToString("C", phpCulture)}");
                        column.Item().Text($"Change Due: {sale.Change.ToString("C", phpCulture)}").Bold();
                    });
            });
        })
        .GeneratePdf(outputPath);
    }
}