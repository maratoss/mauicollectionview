using System.Reactive;
using System.Reactive.Linq;
using ReactiveUI;

namespace Codenames;

public class UploadPhotoViewModel : INPC
{
    private readonly MauiMediaDevice _mediaDevice;
    private bool _isLoading;
    private string? _previewPic;
    private ReactiveCommand<Unit, Unit>? _capturePhotoCommand;

    public UploadPhotoViewModel()
    {
        _mediaDevice = new MauiMediaDevice();
    }

    public event Action<UploadPhotoViewModel, bool> PhotoTaken;

    public event Action<UploadPhotoViewModel> Deleted;

    public event Action<UploadPhotoViewModel, bool> Loading;

    public bool IsLoading
    {
        get => _isLoading;
        private set
        {
            if (SetField(ref _isLoading, value))
            {
                Loading?.Invoke(this, _isLoading);
            }
        }
    }

    public string? PreviewPic
    {
        get => _previewPic;
        private set => SetField(ref _previewPic, value);
    }

    public ReactiveCommand<Unit, Unit> CapturePhotoCommand =>
        _capturePhotoCommand ??= ReactiveCommand.CreateFromTask(
            CapturePhotoAsync,
            this.WhenAnyValue(x => x.IsLoading).Select(isLoading => !isLoading));

    private async Task CapturePhotoAsync()
    {
        var result = await _mediaDevice.CapturePhotoAsync();
        if (!result.Success)
            return;

        IsLoading = true;
        PreviewPic = result.PicUrl;
    }

    private void DeletePhoto()
    {
        //var response = await _serverService.DropUserPhotoAsync(_cts.Token);
        PreviewPic = null;

        Deleted?.Invoke(this);
    }
}