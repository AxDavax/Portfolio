namespace ECommerce.ClientPortal.ViewModels.Core;

/// <summary>
/// Minimal, non-reactive ViewModel used for debugging or isolating
/// rendering/state issues. Contains no SetProperty, no StateChanged,
/// and no reactive pipeline.
/// </summary>
public class SafeVM
{
    public bool IsProcessing { get; set; }
}