using Microsoft.AspNetCore.Mvc;


namespace Command_Design_Pattern.Commands
{
    public class CreateExcellTableActionCommand<T> : ITableActionCommand
    {
        private readonly ExcellFile<T> _excellFile;

        public CreateExcellTableActionCommand(ExcellFile<T> excellFile)
        {
            _excellFile = excellFile;
        }

        public IActionResult Execute()
        {
            var excelMemoryStream = _excellFile.Create();

            return new FileContentResult(excelMemoryStream.ToArray(), _excellFile.FileType) { FileDownloadName = _excellFile.FileName };
        }
    }
}
