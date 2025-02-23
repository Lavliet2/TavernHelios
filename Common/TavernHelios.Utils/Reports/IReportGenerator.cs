using System.Collections.Generic;
using System.IO;

namespace TavernHelios.Utils.Reports
{
    public interface IReportGenerator
    {
        MemoryStream GenerateReport(List<List<string>> data);
        MemoryStream GenerateMultiTableReport(List<(string Title, List<List<string>> Data)> tables);
        string GetFileExtension();
        string GetMimeType();
    }
}
