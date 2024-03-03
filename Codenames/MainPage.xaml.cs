using System.Collections.ObjectModel;
using System.Reactive.Linq;

namespace Codenames;

public partial class MainPage : ContentPage
{
    public MainPage()
    {
        InitializeComponent();
        PicUploaderModels = new ObservableCollection<UploadPhotoViewModel>
        {
            new UploadPhotoViewModel(),
            new UploadPhotoViewModel(),
            new UploadPhotoViewModel(),
            
            new UploadPhotoViewModel(),
            new UploadPhotoViewModel(),
            new UploadPhotoViewModel(),
            
            new UploadPhotoViewModel(),
            new UploadPhotoViewModel(),
            new UploadPhotoViewModel(),
        };

        foreach (var viewModel in PicUploaderModels)
        {
            viewModel.Loading += ViewModelOnLoading;
        }

        BindingContext = this;
    }

    private async void ViewModelOnLoading(UploadPhotoViewModel uploader, bool isLoading)
    {
        if (!isLoading)
        {
            return;
        }
        
        await SyncItems(uploader);
    }

    public ObservableCollection<UploadPhotoViewModel> PicUploaderModels { get; private set; }
    
    private async Task SyncItems(UploadPhotoViewModel uploader)
    {
        int emptyLoaderIndex = -1;
        for (int i = 0; i < PicUploaderModels.Count; i++)
        {
            if (PicUploaderModels[i].PreviewPic == null)
            {
                emptyLoaderIndex = i;
                break;
            }
        }

        var currentIndex = PicUploaderModels.IndexOf(uploader);
        if (currentIndex != emptyLoaderIndex && emptyLoaderIndex != -1)
        {
            await Task.Delay(500);
            PicUploaderModels.Move(currentIndex, emptyLoaderIndex);
        }
    }

    private async void OnTakePhotoClicked(object? sender, TappedEventArgs e)
    {
        var model = (UploadPhotoViewModel)(sender as BindableObject)?.BindingContext!;
        await model.CapturePhotoCommand.Execute();
    }
}