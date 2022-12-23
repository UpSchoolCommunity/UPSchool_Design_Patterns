
using Microsoft.AspNetCore.Mvc;

namespace Command_Design_Pattern.Commands
{
    public interface ITableActionCommand
    {
        IActionResult Execute();
    }
}
