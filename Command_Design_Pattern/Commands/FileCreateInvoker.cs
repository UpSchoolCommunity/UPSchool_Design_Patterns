using DocumentFormat.OpenXml.Office2010.ExcelAc;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace Command_Design_Pattern.Commands
{
    public class FileCreateInvoker
    {
        private  ITableActionCommand _tableActionCommand;

        private List<ITableActionCommand> tableActionCommandList = new List<ITableActionCommand>();

        public void SetCommand(ITableActionCommand tableActionCommand) 
        {
            _tableActionCommand = tableActionCommand;
        }

        public void AddCommand(ITableActionCommand tableActionCommand) 
        {
            tableActionCommandList.Add(tableActionCommand);
        }


        public IActionResult CreateFile() 
        {
            return _tableActionCommand.Execute();
        }

        public List<IActionResult> CreateFiles()
        {
      
           return tableActionCommandList.Select(x => x.Execute()).ToList();
        }


    }
}
