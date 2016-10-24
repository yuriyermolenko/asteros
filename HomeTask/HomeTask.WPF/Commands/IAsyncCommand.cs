using System.Threading.Tasks;
using System.Windows.Input;

namespace HomeTask.WPF.Commands
{
    public interface IAsyncCommand : ICommand
    {
        Task ExecuteAsync(object parameter);
    }
}
