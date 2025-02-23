//using OfficeOpenXml;
//using OfficeOpenXml.Core.ExcelPackage;
//using System;
//using System.Collections.Generic;
//using System.IO;

//namespace TavernHelios.Utils.Reports
//{
//    public class ExcelReportGenerator<T> : IReportGenerator<T>
//    {
//        private readonly string _sheetName;
//        private readonly List<string> _headers;
//        private readonly List<Func<T, string>> _columnSelectors;

//        public ExcelReportGenerator(string sheetName, List<string> headers, List<Func<T, string>> columnSelectors)
//        {
//            _sheetName = sheetName;
//            _headers = headers;
//            _columnSelectors = columnSelectors;
//        }

//        public MemoryStream GenerateReport(List<T> data)
//        {
//            using var package = new ExcelPackage();
//            var sheet = package.Workbook.Worksheets.Add(_sheetName);

//            // Заголовки
//            for (int i = 0; i < _headers.Count; i++)
//                sheet.Cells[1, i + 1].Value = _headers[i];

//            // Данные
//            for (int row = 0; row < data.Count; row++)
//                for (int col = 0; col < _columnSelectors.Count; col++)
//                    sheet.Cells[row + 2, col + 1].Value = _columnSelectors[col](data[row]);

//            var stream = new MemoryStream(package.GetAsByteArray());
//            stream.Position = 0;
//            return stream;
//        }

//        public string GetFileExtension() => "xlsx";
//        public string GetMimeType() => "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
//    }
//}
