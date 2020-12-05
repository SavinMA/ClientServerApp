using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    public class FileModel : PropertyChangedBase
    {
        public FileModel(string fileName, string text)
        {
            FileName = fileName;
            Text = text;
        }

        public string FileName { get; }
        public string Text { get; }

        private bool isPolinom = false;
        public bool IsPolinom
        {
            get => isPolinom;
            set
            {
                isPolinom = value;
                OnPropertyChanged();
            }
        }

        private EFileState _state = EFileState.None;
        public EFileState State
        {
            get { return _state; }
            set
            {
                _state = value;
                OnPropertyChanged();
            }
        }
    }

    public enum EFileState
    {
        None,
        Sended,
        RecievedBad,
        RecievedGood
    }
}
