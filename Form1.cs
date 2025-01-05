using System.Net.Sockets;
using System.Net;

namespace OSPract3
{
    public partial class Form1 : Form
    {
        IUser? chat;

        public Form1()
        {
            InitializeComponent();
            Logger.Label = Label_Status;
            FormClosed += (_, _) =>
            {
                chat?.Quit();
                Logger.Close();
            };
        }

        private async void Button_Connect_Click(object sender, EventArgs e)
        {
            if (chat != null)
                return;

            Logger.Log("Connecting");
            chat = await Editor.Connect();
            if (chat is Server)
            {
                Logger.Log("Server connected");
            }
            else
            {
                Logger.Log("Client connected");
            }
            Button_Connect.Enabled = false;
        }

        private void Button_Send_Click(object sender, EventArgs e)
        {
            chat?.Send(TextBox_Message.Text);
        }
    }
}
