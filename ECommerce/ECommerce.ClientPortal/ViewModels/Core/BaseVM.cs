using System.Linq.Expressions;
using ECommerce.ClientPortal.Expressions;

namespace ECommerce.ClientPortal.ViewModels.Core;

/// <summary>
/// A base view model that fires Property Changed events as needed
/// </summary>
public abstract class BaseVM
{
    /// <summary>
    /// Let's razor component to be notify of stateChanged
    /// </summary>
    public event Action? StateChanged;

    protected void NotifyStateChanged() => StateChanged?.Invoke();

    /// <summary>
    /// Sets the property if the value has changed, and fires NotifyStateChanged
    /// </summary>
    protected bool SetProperty<T>(ref T storage, T value, Action? onChanged = null)
    {
        if (EqualityComparer<T>.Default.Equals(storage, value))
            return false;

        storage = value;
        onChanged?.Invoke();
        NotifyStateChanged();
        return true;
    }

    #region Command Helpers

    /// <summary>
    /// Runs a command if the updating flag is not set
    /// If the flag is true (indicating the function is already running) then the action is not run
    /// If the flag is false (indicating no running function) then the action is run
    /// Once the action is finished if it was run, then the flag is reset to false
    /// </summary>
    /// <param name="updatingFlag">The boolean property flag defining if the command is already running</param>
    /// <param name="action">The action to run if the command is not already running</param>
    /// <returns></returns>
    protected async Task RunCommandAsync(Expression<Func<bool>> updatingFlag, Func<Task> action)
    {
        // Check if the flag property is true (meaning the function is already running)
        if (updatingFlag.GetPropertyValue())
            return;

        // Set the property flag to true to indicate we are running
        updatingFlag.SetPropertyValue(true);

        try
        {
            // Run the passed in action
            await action();
        }
        finally
        {
            // Set the property flag back to false now it's finished
            updatingFlag.SetPropertyValue(false);
            NotifyStateChanged();
        }
    }

    #endregion
}