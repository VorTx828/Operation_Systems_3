using System;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace OSPract3
{
    public class Client : IUser
    {
        private TcpClient _tcpClient;
        private CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();

        public Client(TcpClient client)
        {
            _tcpClient = client;
            new Thread(ListenToServer).Start();
        }

        public async void Send(string message)
        {
            try
            {
                var requestData = Encoding.UTF8.GetBytes(message);
                await _tcpClient.GetStream().WriteAsync(requestData, 0, requestData.Length);
                Logger.Log(Editor.WrapMessage(message));
            }
            catch (Exception ex)
            {
                Logger.Log($"Error sending message: {ex.Message}");
            }
        }

        public void Quit()
        {
            _cancellationTokenSource.Cancel(); // Signal to stop listening
            _tcpClient.Dispose();
        }

        private async void ListenToServer()
        {
            try
            {
                var stream = _tcpClient.GetStream();
                var responseData = new byte[512]; // Buffer size can be adjusted
                while (_tcpClient.Connected && !_cancellationTokenSource.Token.IsCancellationRequested)
                {
                    var response = new StringBuilder();
                    int bytes;
                    do
                    {
                        bytes = await stream.ReadAsync(responseData, 0, responseData.Length);
                        response.Append(Encoding.UTF8.GetString(responseData, 0, bytes));
                    }
                    while (bytes > 0 && stream.DataAvailable);

                    if (response.Length > 0)
                    {
                        Logger.Log(Editor.WrapMessage(response.ToString()));
                    }
                }
            }
            catch (IOException ex)
            {
                Logger.Log($"IOException in ListenToServer: {ex.Message}");
            }
            catch (Exception ex)
            {
                Logger.Log($"Exception in ListenToServer: {ex.Message}");
            }
            finally
            {
                _tcpClient.Close(); // Ensure the client is closed on exit
            }
        }
    }
}
