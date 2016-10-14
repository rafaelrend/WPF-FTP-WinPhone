using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System.Windows.Media;
using WinPhoneFtp.FtpService;
using System.Threading;

namespace RendFTP
{
    public partial class PageServer : PhoneApplicationPage
    {
        public PageServer()
        {
            InitializeComponent();
        }

        //Variável para o tamanho dos inputs, to com preguiça de ficar repetindo ela.  //Pra usar ela no XAML é Binding {Binding InputSize}
        int InputSize = 72;

        public static PhoneApplicationFrame RootFrame { get; private set; }
        private void btTestConnection_Click(object sender, RoutedEventArgs e)
        {
            if (!campoPreenchido(txtServer, "Informe o servidor"))
                return;

            if (!campoPreenchido(txtPort, "Informe a porta"))
                return;

            try
            {
                int cd = Convert.ToInt32(txtPort.Text);
            }
            catch
            {
                setaMensagem("Porta deve ser um número!", "2");
                return;
            }


            if (!campoPreenchido(txtUser, "Informe o usuário"))
                return;


            if (!campoPreenchido(txtUser, "Informe a senha"))
                return;

           // OpenFTP.FTP f = new OpenFTP.FTP();

            try
            {

                conect();
               // f.Connect(txtServer.Text, Convert.ToInt32(txtPort.Text),
                //     txtUser.Text, txtPass.Text);

                //f.Disconnect();

            }
            catch (Exception exp)
            {
                setaMensagem("Erro ao conectar: " + exp.Message, "2");
            }

        }
        public async System.Threading.Tasks.Task conect()
        {
            FtpClient Client = new FtpClient(txtServer.Text, txtPort.Text, Deployment.Current.Dispatcher);

            Client.FtpConnected += Client_FtpConnected;
            Client.FtpAuthenticationFailed += Client_FtpAuthenticationFailed;
            //if (opcao == 1)
                await Client.ConnectAsync();
            //else
             //   await Client.DisconnectAsync();

        }
        void Client_FtpAuthenticationFailed(object sender, EventArgs e)
        {
            setaMensagem("Erro ao conectar ", "2");
        }

        void Client_FtpConnected(object sender, EventArgs e)
        {
            setaMensagem("Conectado com sucesso!", "1");
        }

        private bool campoPreenchido(TextBox txt, string msg)
        {
            if (txt.Text.Trim() == String.Empty)
            {
                setaMensagem(msg, "1");
                txt.Focus();
                return false;
            }
            return true;
        }

        private void setaMensagem(string msg, string tipo)
        {

             Brush Red = new SolidColorBrush(System.Windows.Media.Colors.Red);
             Brush Blue = new SolidColorBrush(System.Windows.Media.Colors.Blue);

            txtMsgRetorno.Text = msg;

            if (tipo == "2")
                txtMsgRetorno.Foreground = Red;



            if (tipo == "1")
                txtMsgRetorno.Foreground = Blue;
        }
    }
}