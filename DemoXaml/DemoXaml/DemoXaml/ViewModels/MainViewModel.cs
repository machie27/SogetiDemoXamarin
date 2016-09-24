using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Microsoft.ProjectOxford.Vision;
using Microsoft.ProjectOxford.Vision.Contract;
using Plugin.Media;
using Plugin.Media.Abstractions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace DemoXaml.ViewModels
{

    //https://github.com/jamesmontemagno/MediaPlugin -- media plugin info
    //https://www.microsoft.com/cognitive-services/en-us/computer-vision-api/documentation cognitive services
    // https://developer.xamarin.com/guides/xamarin-forms/controls/  xaml components


    public class MainViewModel : ViewModelBase
    {
        #region constructor
        public MainViewModel(INavigation navigation)
        {
            WelcomeText = "test";
            Task.Run(async () => await CrossMedia.Current.Initialize());
        }
        #endregion

        #region properties
        private string _welcomeText;
        public string WelcomeText
        {
            get { return _welcomeText; }
            set { Set(() => WelcomeText, ref _welcomeText, value); }
        }

        private string _descriptionText;
        public string DescriptionText
        {
            get { return _descriptionText; }
            set { Set(() => DescriptionText, ref _descriptionText, value); }
        }

        private ImageSource _imageTaken;
        public ImageSource ImageTaken
        {
            get { return _imageTaken; }
            set { Set(() => ImageTaken, ref _imageTaken, value); }
        }

        private MediaFile Media;
        #endregion

        #region methods
        private RelayCommand _takePhotoCommand;
        public RelayCommand TakePhotoCommand
        {
            get
            {
                return _takePhotoCommand ?? (_takePhotoCommand = new RelayCommand(async () => { await InitiateCamera(); }));
            }
        }


        private async Task InitiateCamera()
        {
            if (CrossMedia.Current.IsCameraAvailable && CrossMedia.Current.IsTakePhotoSupported)
            {
                var mediaOptions = new Plugin.Media.Abstractions.StoreCameraMediaOptions
                {
                    Directory = "Photos",
                    Name = $"{DateTime.UtcNow}.jpg",
                    PhotoSize = PhotoSize.Medium
                };
                Media = await CrossMedia.Current.TakePhotoAsync(mediaOptions);
                ImageTaken = ImageSource.FromStream(() => { return Media.GetStream(); });

                try
                {
                    var result = await GetImageDescription(Media.GetStream());
                    if (result != null)
                    {
                        foreach (var caption in result.Description.Captions)
                        {
                            DescriptionText = caption.Text;
                        }
                    }
                }
                catch (Exception e)
                {

                    DescriptionText = e.Message;
                }
            }
            else
            {
                DescriptionText = "taking a photo is not available on your device";
            }
        }

        public async Task<AnalysisResult> GetImageDescription(Stream imageStream)
        {
            //I know I used my token -- please do not use for your own apps!
            VisionServiceClient visionClient = new VisionServiceClient("c3b0a2e374db48358e440385bb625a5b");
            VisualFeature[] features = { VisualFeature.Tags, VisualFeature.Categories, VisualFeature.Description };
            return await visionClient.AnalyzeImageAsync(imageStream, features.ToList(), null);
        }

        #endregion

    }
}
