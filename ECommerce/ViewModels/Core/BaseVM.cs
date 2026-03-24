using System.ComponentModel;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using Portfolio.ECommerce.Blazor.Expressions;

namespace Portfolio.ECommerce.Blazor.ViewModels.Core
{
    /// <summary>
    /// A base view model that fires Property Changed events as needed
    /// </summary>
    public class BaseVM : INotifyPropertyChanged
    {
        /// <summary>
        /// Fired when a property value changes
        /// </summary>
        public event PropertyChangedEventHandler? PropertyChanged;

        /// <summary>
        /// Let's razor component to be notify of stateChanged
        /// </summary>
        public Action? StateChanged { get; set; }

        /// <summary>
        /// Called to fire a <see cref="PropertyChanged"/> event
        /// </summary>
        /// <param name="propertyName">The name of the property that changed</param>
        public void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

            StateChanged?.Invoke();
        }

        /// <summary>
        /// Sets the property if the value has changed, and fires OnPropertyChanged
        /// </summary>
        protected bool SetProperty<T>(ref T storage, T value, [CallerMemberName] string propertyName = "")
        {
            if (EqualityComparer<T>.Default.Equals(storage, value))
                return false;

            storage = value;
            OnPropertyChanged(propertyName);
            return true;
        }

        #region Command Helpers

        /// <summary>
        /// Runs a command if the updating flag is not set
        /// If the flag is true (indicating the function is already running) then the action is not run
        /// If the flas is false (indicating no running function) then the action is run
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
            }
        }

        #endregion
    }
}