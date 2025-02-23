using System.Collections.Generic;
using System.IO;

namespace TavernHelios.Utils.Reports
{
    public interface IReportGenerator
    {
        MemoryStream GenerateReport(List<List<string>> data);
        string GetFileExtension();
        string GetMimeType();
    }
}
