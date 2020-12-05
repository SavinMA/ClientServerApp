
using CreateTextFiles;

using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Client
{
    public partial class BaseViewModel : PropertyChangedBase
    {
        public ObservableCollection<FileModel> Files { get; private set; } = new ObservableCollection<FileModel>();

        #region Properties 

        private int _filesCount = 100;
        /// <summary>
        /// Количество файлов
        /// </summary>
        public int FilesCount
        {
            get { return _filesCount; }
            set
            {
                _filesCount = value;
                OnPropertyChanged();
            }
        }

        private bool _accessToFiles = true;
        /// <summary>
        /// Доступность к файлам
        /// </summary>
        public bool AccessToFiles
        {
            get { return _accessToFiles; }
            set
            {
                _accessToFiles = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #region Commands
        private ICommand _generateFilesCommand;

        public ICommand GenerateFilesCommand
        {
            get
            {
                return _generateFilesCommand ?? (_generateFilesCommand = new RelayCommand(GenerateFiles, _ => AccessToFiles));
            }
        }
        #endregion

        #region Methods

        private async void GenerateFiles(object _)
        {
            AccessToFiles = false;
            await Task.Run(() =>
            {
                FileGenerator.GenerateFiles(FilesCount);

                Files = new ObservableCollection<FileModel>
                (
                    Directory.EnumerateFiles(FileGenerator.FilesPath).Select(f => new FileModel(Path.GetFileName(f), File.ReadAllText(f)))
                );
            });
            AccessToFiles = true;

            OnPropertyChanged(nameof(Files));
        }

        #endregion
    }
}
