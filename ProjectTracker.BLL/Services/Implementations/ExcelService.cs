using ProjectTracker.BLL.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectTracker.BLL.Services.Implementations
{
    public class ExcelService : IExcelService
    {
        public List<List<string>> ReadExcelFile(string path)
        {
            var excel = new Microsoft.Office.Interop.Excel.Application();
            var workbook = excel.Workbooks.Open(path);
            var worksheet = workbook.Worksheets[1];

            var rows = new List<List<string>>();

            int i = 1;
            var cellInRowIsEmpty = false;
            var cellInFirstColumnIsEmpty = false;

            while (!cellInFirstColumnIsEmpty)
            {
                int j = 1;
                cellInRowIsEmpty = false;

                while (!cellInRowIsEmpty)
                {
                    var cellValue = worksheet.Cells[i, j].Value2;

                    if(cellValue != null)
                    {
                        if (j == 1)
                            rows.Add(new List<string>());

                        rows.ElementAt(i-1).Add(cellValue.ToString());
                    }
                    else
                    {
                        cellInRowIsEmpty = true;

                        if (j == 1)
                            cellInFirstColumnIsEmpty = true;
                    }

                    j++;
                }

                i++;
            }

            workbook.Close();

            return rows;
        }
    }
}
