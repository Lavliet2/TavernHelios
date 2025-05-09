using ClosedXML.Excel;
using System.Collections.Generic;
using System.IO;

namespace TavernHelios.Utils.Reports
{
    public class ExcelReportGenerator : IReportGenerator
    {
        private readonly string _title;
        private readonly List<string> _headers;

        public ExcelReportGenerator(string title, List<string> headers)
        {
            _title = title;
            _headers = headers;
        }

        public MemoryStream GenerateReport(List<List<string>> data)
        {
            return GenerateMultiTableReport(new List<(string, List<List<string>>)> { (_title, data) });
        }

        public MemoryStream GenerateMultiTableReport(List<(string Title, List<List<string>> Data)> tables)
        {
            var stream = new MemoryStream();
            using var workbook = new XLWorkbook();

            foreach (var (title, data) in tables)
            {
                var safeTitle = SanitizeSheetName(title);
                var worksheet = workbook.Worksheets.Add(safeTitle);

                // Заголовки
                for (int i = 0; i < _headers.Count; i++)
                {
                    worksheet.Cell(1, i + 1).Value = _headers[i];
                    worksheet.Cell(1, i + 1).Style.Font.Bold = true;
                    worksheet.Cell(1, i + 1).Style.Fill.BackgroundColor = XLColor.LightGray;
                }

                // Данные
                for (int row = 0; row < data.Count; row++)
                {
                    for (int col = 0; col < _headers.Count; col++)
                    {
                        worksheet.Cell(row + 2, col + 1).Value = data[row][col];
                    }
                }

                worksheet.Columns().AdjustToContents();
            }

            workbook.SaveAs(stream);
            stream.Position = 0;
            return stream;
        }

        private string SanitizeSheetName(string name)
        {
            var invalidChars = new[] { ':', '\\', '/', '?', '*', '[', ']' };
            foreach (var ch in invalidChars)
                name = name.Replace(ch, '-');

            return name.Length > 31 ? name.Substring(0, 31) : name;
        }

        public string GetFileExtension() => "xlsx";
        public string GetMimeType() => "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
    }
}
