using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace TavernHelios.Utils.Reports
{
    public class PdfReportGenerator : IReportGenerator
    {
        private readonly string _title;
        private readonly List<string> _headers;

        public PdfReportGenerator(string title, List<string> headers)
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

            Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Margin(30);
                    page.Content().Column(col =>
                    {
                        foreach (var (title, data) in tables)
                        {
                            col.Item().Text(title).FontSize(16).Bold().AlignCenter();
                            col.Item().PaddingVertical(10).Element(BuildTable(data));
                        }
                    });
                });
            }).GeneratePdf(stream);

            stream.Position = 0;
            return stream;
        }

        private Action<IContainer> BuildTable(List<List<string>> data)
        {
            return container =>
            {
                container.Table(table =>
                {
                    table.ColumnsDefinition(columns =>
                    {
                        for (int i = 0; i < _headers.Count; i++)
                            columns.RelativeColumn();
                    });

                    // Заголовки
                    table.Header(header =>
                    {
                        foreach (var headerText in _headers)
                        {
                            header.Cell().Background(Colors.Grey.Lighten2).Padding(5)
                                .Text(headerText).Bold().FontSize(12).AlignCenter();
                        }
                    });

                    // Данные
                    foreach (var row in data)
                    {
                        foreach (var cell in row)
                        {
                            table.Cell().Padding(5)
                                .Text(string.IsNullOrEmpty(cell) ? "—" : cell).FontSize(10).AlignCenter();
                        }
                    }
                });
            };
        }

        public string GetFileExtension() => "pdf";
        public string GetMimeType() => "application/pdf";
    }
}
