using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using System.IO;
using System.Diagnostics;

namespace TechShopMS.Services
{
   public  class InvoiceGenService
    {
        private const string SequenceFilePath = "Data/InvoiceSequence.txt";

        public string GenerateInvoiceNumber()
        {
            int nextNumber = 1;

            if (File.Exists(SequenceFilePath))
            {
                Debug.WriteLine("Sequence file exists, reading last number.");
                string content = File.ReadAllText(SequenceFilePath);
                if (int.TryParse(content, out int lastNumber))
                {
                    nextNumber = lastNumber + 1;
                }
            }

            File.WriteAllText(SequenceFilePath, nextNumber.ToString());

            // Format: INV-20250528-00123
            string formatted = $"INV-{DateTime.Now:yyyyMMddss}-{nextNumber:D5}";
            return formatted;
        }

    }
}
