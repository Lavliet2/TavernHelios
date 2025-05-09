using System;
using System.Collections.Generic;

namespace TavernHelios.Utils.Reports
{
    public static class ReportFactory
    {
        public static IReportGenerator CreateReportGenerator(
            string format, string title, List<string> headers)
        {
            return format.ToLower() switch
            {
                "pdf" => new PdfReportGenerator(title, headers),
                "excel" => new ExcelReportGenerator(title, headers),
                _ => throw new ArgumentException($"Формат '{format}' не поддерживается.")
            };
        }
    }
}
