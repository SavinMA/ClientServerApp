using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    public partial class BaseViewModel 
    {
        #region Methods
        private void ResetStatistic()
        {
            SendedFilesCount = 0;
            BadFilesCount = 0;
            IsPolinomFilesCount = 0;
            IsNotPolinomFilesCount = 0;
        }

        private void ChangeStatistic()
        {
            SendedFilesCount = Files.Count(f => f.State == EFileState.Sended);
            BadFilesCount = Files.Count(f => f.State == EFileState.RecievedBad);
            IsPolinomFilesCount = Files.Count(f => f.State == EFileState.RecievedGood && f.IsPolinom);
            IsNotPolinomFilesCount = Files.Count(f => f.State == EFileState.RecievedGood && !f.IsPolinom);
        }
        #endregion

        #region Properties
        private int _sendedFilesCount;
        private int _badFilesCount;
        private int _isPolinomFilesCount;
        private int _isNotPolinomFilesCount;

        /// <summary>
        /// Количество отправленных файлов
        /// </summary>
        public int SendedFilesCount
        {
            get { return _sendedFilesCount; }
            set
            {
                if (_sendedFilesCount == value) 
                    return;

                _sendedFilesCount = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Количество файлов, по которым пришел ответ, что сервер их не обработал
        /// </summary>
        public int BadFilesCount
        {
            get { return _badFilesCount; }
            set
            {
                if (_badFilesCount == value)
                    return;

                _badFilesCount = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Количество обработанных файлов, полиномов
        /// </summary>
        public int IsPolinomFilesCount
        {
            get { return _isPolinomFilesCount; }
            set
            {
                if (_isPolinomFilesCount == value)
                    return;

                _isPolinomFilesCount = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Количество обработанных файлов, не полиномов
        /// </summary>
        public int IsNotPolinomFilesCount
        {
            get { return _isNotPolinomFilesCount; }
            set
            {
                if (_isNotPolinomFilesCount == value)
                    return;

                _isNotPolinomFilesCount = value;
                OnPropertyChanged();
            }
        }
        #endregion
    }
}
