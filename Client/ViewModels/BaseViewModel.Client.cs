using Common;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Client
{
    public partial class BaseViewModel : PropertyChangedBase
    {
        public ClientTcp Client { get; } = new ClientTcp();

        private string port = "50001";
        private string ip = "127.0.0.1";

        private bool isConnected;
        private int _delayFromSends = 50;
        private bool isWait = false;

        #region Properties

        /// <summary>
        /// Порт сервера
        /// </summary>
        public string Port
        {
            get => port;
            set
            {
                port = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Хост клиента
        /// </summary>
        public string IP
        {
            get => ip;
            set
            {
                ip = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Статус подключения к хосту
        /// </summary>
        public bool IsConnected
        {
            get => isConnected;
            set
            {
                isConnected = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Задержка между отправками текста из файлов
        /// </summary>
        public int DelayFromSends
        {
            get { return _delayFromSends; }
            set
            {
                _delayFromSends = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #region Commands

        private ICommand _connectCommand;
        private ICommand _disconnectCommand;
        private ICommand _sendCommand;

        #region Commands.Connect

        /// <summary>
        /// Команда подключения к серверу
        /// </summary>
        public ICommand ConnectCommand
        {
            get
            {
                return _connectCommand ?? (_connectCommand = new RelayCommand(Connect, CanConnect));
            }
        }

        public async void Connect(object _)
        {
            if (await Client.Connect(IP, int.Parse(Port)))
            {
                IsConnected = Client.IsConnected;
                Client.MessageRecieved += ClientOnMessageRecieved;
                Client.Disconnected += ClientOnDisconnected;
            }
            else
            {
                IsConnected = false;
            }
        }

        public bool CanConnect(object _)
        {
            var clientValid = !(Client != null && Client.IsConnected);
            var ipValid = IPAddress.TryParse(IP, out IPAddress _);
            var portValid = int.TryParse(Port, out int port) && port == 50001;
            return clientValid && ipValid && portValid && AccessToFiles;
        }

        #endregion

        #region Commands.Disconnect

        /// <summary>
        /// Команда отключения
        /// </summary>
        public ICommand DisconnectCommand
        {
            get
            {
                return _disconnectCommand ?? (_disconnectCommand = new RelayCommand(Disconnect, _ => IsConnected && !isWait));
            }
        }

        private async void Disconnect(object _)
        {
            Client.Disconnected -= ClientOnDisconnected;
            Client.MessageRecieved -= ClientOnMessageRecieved;
            IsConnected = false;
            await Client.Disconnect();
        }

        #endregion

        #region Commands.Send
        /// <summary>
        /// Команда отправки
        /// </summary>
        public ICommand SendCommand
        {
            get
            {
                return _sendCommand ?? (_sendCommand = new RelayCommand(Send, CanSend));
            }
        }

        private async void Send(object _)
        {
            AccessToFiles = false;
            isWait = true;

            Files.AsParallel().ForAll(file => file.State = EFileState.None);
            ResetStatistic();

            foreach (var file in Files)
            {
                file.State = EFileState.Sended;
                var request = new Request() { FileName = file.FileName, Text = file.Text };
                var text = JsonWorker.JsonSerialize(request);
                _ = Task.Run(() => Client.Send(text));

                await Task.Delay(DelayFromSends);
            }

            AccessToFiles = true;
            isWait = false;
        }

        private bool CanSend(object _)
        {
            return IsConnected
                && Files.Any()
                && !isWait
                && (DelayFromSends > 0 || DelayFromSends < 10000);
        }
        #endregion

        #endregion

        /// <summary>
        /// Обработка сообщения от сервера
        /// </summary>
        /// <param name="clientGuid">ид клиента</param>
        /// <param name="message">сообщение</param>
        private void ClientOnMessageRecieved(Guid clientGuid, string message)
        {
            var responce = JsonWorker.JsonDeserialize<Responce>(message);

            if (responce is null)
                return;

            var file = Files.FirstOrDefault(f => f.FileName == responce.FileName);
            if (file != null)
            {
                file.State = responce.State == EResponceState.Bad ? EFileState.RecievedBad : EFileState.RecievedGood;
                file.IsPolinom = responce.IsPolinom;
            }

            ChangeStatistic();
        }

        private void ClientOnDisconnected(Guid guid)
        {
            IsConnected = false;
            Client.Disconnected -= ClientOnDisconnected;
        }
    }
}
