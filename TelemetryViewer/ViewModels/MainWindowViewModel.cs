using Microsoft.Win32;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using TelemetryViewer.Infastructure.Handlers;
using TelemetryViewer.Infrastructure.Commands;
using TelemetryViewer.Models;
using TelemetryViewer.ViewModels.Base;

namespace TelemetryViewer.ViewModels
{
    public class MainWindowViewModel : ViewModel
    {
        private string _title = "";
        public string Title
        {
            get => _title;
            set => Set(ref _title, value);

        }

        private string _titleFrame = "Кадры";
        public string TitleFrame
        {
            get => _titleFrame;
            set => Set(ref _titleFrame, value);
        }

        private string _titleHistogram = "Гситограмма";
        public string TitleHistogram
        {
            get => _titleHistogram;
            set => Set(ref _titleHistogram, value);
        }

        private int _currentFrameIndex = 1;
        public int CurrentFrameIndex
        {
            get => _currentFrameIndex;
            set
            {
                Set(ref _currentFrameIndex, value);
                OnPropertyChanged(nameof(SelectedFrame));
            }
        }

        private int _framesCount;
        public int FramesCount
        {
            get => _framesCount;
            set => Set(ref _framesCount, value);
        }

        public ObservableCollection<FrameModel> Frames { get; set; } = new ObservableCollection<FrameModel>();

        public FrameModel SelectedFrame => Frames.ElementAtOrDefault(CurrentFrameIndex - 1);
         

        #region Commands
        public ICommand OpenFileCommand { get; private set; }
        private async void OnOpenCommandExecuted(object parameter)
        {
            var openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Файлы KDR (*.kdr)|*.kdr";

            if (openFileDialog.ShowDialog() == true)
            {
                Title = openFileDialog.SafeFileName;
                Frames.Clear();

                var frameModels = await KdrParser.ParseFileAsync(openFileDialog.FileName);
                Frames = new ObservableCollection<FrameModel>(frameModels);
                CurrentFrameIndex = 1;
                FramesCount = Frames.Count;
            }
        }

        private bool OnOpenCommandExecute(object arg)
        {
            return true;
        }

        #endregion

        public MainWindowViewModel()
        {
            OpenFileCommand = new LambdaCommand(OnOpenCommandExecuted, OnOpenCommandExecute);
        }
    }
}