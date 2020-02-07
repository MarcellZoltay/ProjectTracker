using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ProjectTracker.BLL.Services.Implementations;
using ProjectTracker.BLL.Services.Interfaces;

namespace Test.ProjectTracker.BLL
{
    [TestClass]
    public class ExcelServiceTest
    {
        [TestMethod]
        public void ReadExcelTest()
        {
            IExcelService excelService = new ExcelService();

            var result = excelService.ReadExcelFile("C:\\Users\\Zoltay Marcell\\Desktop\\export.xlsx");

            Assert.IsTrue(result.Count == 147);
            Assert.IsTrue(result[0].Count == 5);
        }
    }
}
