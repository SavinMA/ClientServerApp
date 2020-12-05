using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Common
{
    public delegate void OnMessageHandler(Guid clientGuid, string message);
    public delegate void OnDisconnectHandler(Guid guid);

    /// <summary>
    /// TCP Клиент
    /// </summary>
    public class ClientTcp : IDisposable
    {
        #region Fields
        private CancellationTokenSource cancellationToken;
        private TcpClient client;
        private NetworkStream stream;
        private bool disposedValue;
        #endregion

        public ClientTcp()
        {
        }

        #region Properties

        /// <summary>
        /// Глобальный идентификатор
        /// </summary>
        public Guid Guid { get; private set; }

        /// <summary>
        /// Состояние подключения
        /// </summary>
        public bool IsConnected => client != null && stream != null && client.Connected;

        #endregion

        #region Events
        public event OnMessageHandler MessageRecieved;
        public event OnDisconnectHandler Disconnected;
        #endregion

        #region Methods

        /// <summary>
        /// Инициализация сервера
        /// </summary>
        /// <param name="tcpClient"></param>
        public void Initialize(TcpClient tcpClient)
        {
            Guid = Guid.NewGuid();
            cancellationToken = new CancellationTokenSource();
            client = tcpClient;
            if (client.Connected)
                stream = client.GetStream();
        }

        /// <summary>
        /// Попытка подключения к серверу по заданным параметрам
        /// </summary>
        /// <param name="ip">хост</param>
        /// <param name="port">порт</param>
        /// <returns>Результат подключения</returns>
        public async Task<bool> Connect(string ip, int port)
        {
            try
            {
                if (IsConnected)
                    return false;

                Initialize(new TcpClient());
                await client.ConnectAsync(ip, port);
                stream = client.GetStream();
                _ = Task.Run(Read);

                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Отключение от сервера
        /// </summary>
        /// <returns></returns>
        public async Task Disconnect()
        {
            if (!IsConnected)
                return;

            await Task.Run(() =>
            {
                cancellationToken.Cancel();
                client.Close();
            });
        }

        /// <summary>
        /// Задача чтения из потока
        /// </summary>
        /// <returns></returns>
        public async Task Read()
        {
            byte[] data = new byte[64];
            while (!cancellationToken.IsCancellationRequested)
            {
                StringBuilder builder = new StringBuilder();
                do
                {
                    var bytes = await stream.ReadAsync(data, 0, data.Length, cancellationToken.Token);
                    builder.Append(Encoding.UTF8.GetString(data, 0, bytes));
                }
                while (stream.DataAvailable && !cancellationToken.Token.IsCancellationRequested);

                var message = builder.ToString();

                if (string.IsNullOrWhiteSpace(message))
                {
                    Disconnected?.Invoke(Guid);
                    _ = Disconnect();
                    Dispose();
                }
                else
                {
                    MessageRecieved?.Invoke(Guid, message);
                }
            }
        }

        /// <summary>
        /// Отправка сообщения на сервер
        /// </summary>
        /// <param name="message">текст сообщения</param>
        /// <returns></returns>
        public async Task Send(string message)
        {
            var data = Encoding.UTF8.GetBytes(message);
            await stream.WriteAsync(data, 0, data.Length);
        }

        #endregion

        #region IDisposable

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    cancellationToken?.Dispose();
                    stream?.Dispose();
                    client?.Dispose();
                }

                stream = null;
                disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        #endregion
    }
}
