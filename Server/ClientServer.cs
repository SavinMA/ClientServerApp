using Common;

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Server
{
    public class ClientServer : IDisposable
    {
        #region Fields
        private const int port = 50001;
        private const int delayTime = 2048;

        private readonly TcpListener listener;
        private readonly CancellationTokenSource cancellationTokenSource;
        private readonly ConcurrentDictionary<Guid, ClientTcp> Clients = new ConcurrentDictionary<Guid, ClientTcp>();
        
        private readonly int maxProcessCount = 5;

        private bool disposedValue;
        private long currentProcessCount = 0;

        #endregion

        public ClientServer(int maxProcess)
        {
            maxProcessCount = maxProcess;
            cancellationTokenSource = new CancellationTokenSource();

            IPAddress.TryParse("127.0.0.1", out IPAddress iPAddress);
            listener = new TcpListener(iPAddress, port);
        }

        /// <summary>
        /// Прослушивание сервером клиентов по порту
        /// </summary>
        /// <returns></returns>
        public async Task StartListen()
        {
            try
            {
                listener.Start();

                while (!cancellationTokenSource.IsCancellationRequested)
                {
                    TcpClient client = await listener.AcceptTcpClientAsync();
                    ClientTcp clientObject = new ClientTcp();
                    clientObject.Initialize(client);

                    if (Clients.TryAdd(clientObject.Guid, clientObject))
                    {
                        clientObject.MessageRecieved += ClientObjectOnMessageRecieved;
                        clientObject.Disconnected += ClientObjectOnDisconnected;
                        _ = Task.Run(clientObject.Read);
                    }
                }
            }
            finally
            {
                listener?.Stop();
            }
        }

        /// <summary>
        /// Обработка обрыва связи с клиентом
        /// </summary>
        /// <param name="guid">ид клиента</param>
        private void ClientObjectOnDisconnected(Guid guid)
        {
            Clients.TryRemove(guid, out ClientTcp _);
        }

        /// <summary>
        /// Обработка сообщения от клиента
        /// </summary>
        /// <param name="clientGuid">ид клиента</param>
        /// <param name="message">сообщение</param>
        private async void ClientObjectOnMessageRecieved(Guid clientGuid, string message)
        {
            ConsoleWriter.WriteMessage(message);

            var request = JsonWorker.JsonDeserialize<Request>(message);
            if (request is null)
            {
                ConsoleWriter.WriteError(message);
                return;
            }

            if (Interlocked.Increment(ref currentProcessCount) <= maxProcessCount && request != null)
            {
                var result = await CheckForPalindrom(request.Text);

                _ = Task.Run(() => Send(clientGuid, new Responce(EResponceState.Ok, request.FileName, result)));

                Interlocked.Decrement(ref currentProcessCount);
            }
            else
            {
                Interlocked.CompareExchange(ref currentProcessCount, maxProcessCount, maxProcessCount + 1);
                await Send(clientGuid, new Responce(EResponceState.Bad, request.FileName));
            }
        }

        /// <summary>
        /// Отправка результата по опредленному клиенту
        /// </summary>
        /// <param name="guid">ид клиента</param>
        /// <param name="responce">ответ</param>
        /// <returns></returns>
        public async Task Send(Guid guid, Responce responce)
        {
            if(Clients.TryGetValue(guid, out ClientTcp client))
            {
                var text = JsonWorker.JsonSerialize(responce);
                ConsoleWriter.WriteMessage(text);
                await client.Send(text);
            }
        }

        /// <summary>
        /// Проверка строки на палиндром с ожиданием
        /// </summary>
        /// <param name="text">входная строка</param>
        /// <returns>Результат: палиндром или нет</returns>
        public async Task<bool> CheckForPalindrom(string text)
        {
            var result = ActionForFile.IsPalindrom(text);

            await Task.Delay(delayTime);

            return result;
        }

        /// <summary>
        /// Остановка сервера
        /// </summary>
        public void Stop()
        {
            cancellationTokenSource?.Cancel();
            listener?.Stop();
        }

        #region IDisposable
        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                Stop();
                Clients.Clear();
                if (disposing)
                {
                    cancellationTokenSource?.Dispose();
                }

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
