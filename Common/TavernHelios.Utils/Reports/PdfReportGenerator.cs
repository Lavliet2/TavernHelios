using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace TavernHelios.Utils.Reports
{
    public class PdfReportGenerator : IReportGenerator
    {
        private readonly string _title;
        private readonly List<string> _headers;
        private readonly BaseFont _baseFont;

        public PdfReportGenerator(string title, List<string> headers)
        {
            _title = title;
            _headers = headers;
            _baseFont = LoadFont();
        }

        private BaseFont LoadFont()
        {
            string assemblyLocation = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) ?? "";
            string fontPath = Path.Combine(AppContext.BaseDirectory, "Reports", "fonts", "arial.ttf");

            if (!File.Exists(fontPath))
            {
                throw new FileNotFoundException($"Файл шрифта не найден: {fontPath}");
            }

            return BaseFont.CreateFont(fontPath, BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
        }

        public MemoryStream GenerateReport(List<List<string>> data)
        {
            var stream = new MemoryStream();

            using (var doc = new Document(PageSize.A4))
            {
                using (var writer = PdfWriter.GetInstance(doc, stream))
                {
                    writer.CloseStream = false;
                    doc.Open();

                    var titleFont = new Font(_baseFont, 16, Font.BOLD);
                    var headerFont = new Font(_baseFont, 12, Font.BOLD);
                    var dataFont = new Font(_baseFont, 10);

                    doc.Add(new Paragraph(_title, titleFont) { Alignment = Element.ALIGN_CENTER });
                    doc.Add(new Paragraph("\n"));

                    var table = new PdfPTable(_headers.Count) { WidthPercentage = 100 };
                    table.SetWidths(new float[] { 2, 2, 2, 2, 2 });

                    foreach (var header in _headers)
                    {
                        var headerCell = new PdfPCell(new Phrase(header, headerFont))
                        {
                            HorizontalAlignment = Element.ALIGN_CENTER,
                            BackgroundColor = new BaseColor(211, 211, 211)
                        };
                        table.AddCell(headerCell);
                    }

                    foreach (var row in data)
                    {
                        foreach (var cellValue in row)
                        {
                            var cellText = cellValue ?? "—";
                            var pdfCell = new PdfPCell(new Phrase(cellText, dataFont))
                            {
                                HorizontalAlignment = Element.ALIGN_CENTER
                            };
                            table.AddCell(pdfCell);
                        }
                    }

                    doc.Add(table);
                }
            }

            stream.Position = 0;
            return stream;
        }

        public MemoryStream GenerateMultiTableReport(List<(string Title, List<List<string>> Data)> tables)
        {
            var stream = new MemoryStream();
            using (var doc = new Document(PageSize.A4, 10, 10, 10, 10))
            {
                using (var writer = PdfWriter.GetInstance(doc, stream))
                {
                    writer.CloseStream = false;
                    doc.Open();

                    var titleFont = new Font(_baseFont, 14, Font.BOLD);
                    var headerFont = new Font(_baseFont, 12, Font.BOLD);
                    var dataFont = new Font(_baseFont, 10);

                    foreach (var (tableTitle, data) in tables)
                    {
                        doc.Add(new Paragraph(tableTitle, titleFont) { Alignment = Element.ALIGN_CENTER });
                        doc.Add(new Paragraph("\n"));

                        var table = new PdfPTable(_headers.Count) { WidthPercentage = 100 };
                        table.SetWidths(new float[] { 2, 2, 2, 2, 2 });

                        foreach (var header in _headers)
                        {
                            var headerCell = new PdfPCell(new Phrase(header, headerFont))
                            {
                                HorizontalAlignment = Element.ALIGN_CENTER,
                                BackgroundColor = new BaseColor(211, 211, 211)
                            };
                            table.AddCell(headerCell);
                        }

                        foreach (var row in data)
                        {
                            foreach (var cell in row)
                            {
                                var pdfCell = new PdfPCell(new Phrase(cell ?? "—", dataFont))
                                {
                                    HorizontalAlignment = Element.ALIGN_CENTER
                                };
                                table.AddCell(pdfCell);
                            }
                        }

                        doc.Add(table);
                        doc.Add(new Paragraph("\n\n"));
                    }
                }
            }

            stream.Position = 0;
            return stream;
        }


        public string GetFileExtension() => "pdf";
        public string GetMimeType() => "application/pdf";
    }
}
