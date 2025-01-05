using System;
using System.Configuration;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace OSPract3
{
    public static class Editor
    {
        public static async Task<IUser> Connect()
        {
            TcpClient tcpClient = new();
            try
            {
                await tcpClient.ConnectAsync(Ip, Port);
                if (tcpClient.Connected)
                {
                    Logger.Log($"Connected to server at {Ip}:{Port}");
                    return new Client(tcpClient);
                }
                else
                {
                    throw new Exception("Client not connected");
                }
            }
            catch (SocketException ex)
            {
                Logger.Log($"SocketException: {ex.Message}");
                return new Server();
            }
            catch (Exception ex)
            {
                Logger.Log($"Exception: {ex.Message}");
                throw; // Rethrow the exception for further handling
            }
        }

        public static string WrapMessage(string message)
            => $"Message received (DateTime:: {DateTime.Now.ToShortDateString()} - {DateTime.Now.ToLongTimeString()}): {message}";

        public static string Ip
        {
            get
            {
                var ip = ConfigurationManager.AppSettings.Get("ip");
                if (string.IsNullOrWhiteSpace(ip) || !IPAddress.TryParse(ip, out _))
                {
                    throw new ConfigurationErrorsException("Invalid IP address in configuration.");
                }
                return ip;
            }
        }

        public static int Port
        {
            get
            {
                var portString = ConfigurationManager.AppSettings.Get("port");
                if (!int.TryParse(portString, out int port) || port <= 0 || port > 65535)
                {
                    throw new ConfigurationErrorsException("Invalid port number in configuration.");
                }
                return port;
            }
        }
    }
}
