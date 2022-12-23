using ClosedXML.Excel;
using DocumentFormat.OpenXml.Office2021.DocumentTasks;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;

namespace Command_Design_Pattern.Commands
{
    public class ExcellFile<T>
    {
        private readonly List<T> _list;
        public string FileName => $"{typeof(T).Name}.xlsx";
        public string FileType => "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

        public ExcellFile(List<T> list)
        {
            _list = list;
        }

        public  MemoryStream Create() 
        {
            var wb = new XLWorkbook();
            var ds = new DataSet();

            ds.Tables.Add(GetTable());
            wb.Worksheets.Add(ds);
            var excelMemory = new MemoryStream();
            wb.SaveAs(excelMemory);

            return excelMemory;
        }

        private DataTable GetTable() 
        {
            var table = new DataTable();
            var type = typeof(T);
            type.GetProperties().ToList().ForEach(x =>
            {
                table.Columns.Add(x.Name, x.PropertyType);
            });

            _list.ForEach(x =>
            {
                //listedeki tek bir ürün tüm bilgilerini aldık
                var values = type.GetProperties().Select(propInfo => propInfo.GetValue(x, null)).ToArray();
                table.Rows.Add(values);
            });

            return table;
        }
    }
}
