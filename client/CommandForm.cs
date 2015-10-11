using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.Net;
using System.Net.Sockets;
using System.IO;

namespace client {
    public partial class CommandForm : Form {
        private const int listenPort = 9122;
        private const int sendPort = 9121;
        private Thread listener = null;

        public CommandForm() {
            InitializeComponent();

            listener = new Thread(new ThreadStart(this.MsgReceiver));
            listener.Start();
        }

        [STAThread]
        static void Main() {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new CommandForm());
        }

        private void buttonSend_Click(object sender, EventArgs e) {
            using (TcpClient sock = new TcpClient()) {
                sock.Connect(textMerlinHost.Text, sendPort);
                using (StreamWriter writer = new StreamWriter(sock.GetStream())) {
                    writer.WriteLine(textMsg.Text);
                }
            }

            textReceiveBox.AppendText(textMsg.Text);
        }

        private void MsgReceiver() {
            TcpListener ser = null;

            try {
                ser = new TcpListener(IPAddress.Any, listenPort);
                ser.Start();
            } catch (Exception ex) {
                MessageBox.Show(ex.ToString());
                return;
            }


            byte[] buf = new byte[1024];
            StringBuilder msg = new StringBuilder();
            string header = "";

            while (true) {
                try {
                    using (TcpClient sock = ser.AcceptTcpClient()) {
                        NetworkStream stream = sock.GetStream();
                        if (stream.CanRead) {
                            int n = 0;
                            msg.Length = 0;
                            if (stream.Read(buf, 0, 1) == 1) {
                                if (buf[0] == 1) { // file
                                    header = "";
                                    while (stream.Read(buf, 0, 1) == 1 && buf[0] != '\n') {
                                        header += Encoding.ASCII.GetString(buf, 0, 1);
                                    }

                                    string fileInfo = header.Remove(0, "<file>".Length);
                                    string[] ps = fileInfo.Split(':');
                                    string[] dirs = ps[0].Split('\\');
                                    int len = Convert.ToInt32(ps[1]);

                                    int i = 0;
                                    string filename = Application.StartupPath;
                                    while (i < dirs.Length - 1) {
                                        filename += "\\" + dirs[i];
                                        i++;
                                    }

                                    Directory.CreateDirectory(filename);
                                    filename += "\\" + dirs[dirs.Length - 1];

                                    using (BinaryWriter writer = new BinaryWriter(new FileStream(filename, FileMode.OpenOrCreate, FileAccess.Write))) {
                                        while (stream.DataAvailable) {
                                            len = stream.Read(buf, 0, buf.Length);
                                            writer.Write(buf, 0, len);
                                            writer.Flush();
                                        }
                                    }
                                } else {
                                }
                            }
                        }
                        

                        /*
                        using (StreamReader reader = new StreamReader(sock.GetStream())) {
                            cmd = reader.ReadLine();
                            if (cmd != null) {
                                if (cmd.StartsWith("<file>")) {
                                    string fileInfo = cmd.Remove(0, "<file>".Length);
                                    string[] ps = fileInfo.Split(':');
                                    string[] dirs = ps[0].Split('\\');
                                    int len = Convert.ToInt32(ps[1]);

                                    int i = 0;
                                    string filename = Application.StartupPath;
                                    while (i < dirs.Length - 1) {
                                        filename += "\\" + dirs[i];
                                        i++;
                                    }

                                    Directory.CreateDirectory(filename);
                                    filename += "\\" + dirs[dirs.Length - 1];

                                    string str = reader.ReadToEnd();

                                    using (StreamWriter writer = new StreamWriter(new FileStream(filename, FileMode.OpenOrCreate, FileAccess.Write))) {
                                        writer.Write(str);
                                    }
                                } else {
                                    textReceiveBox.AppendText(cmd);
                                }
                            }
                        }
                         * */
                    }
                } catch (Exception ex) {
                    MessageBox.Show(ex.ToString());
                }
            }
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e) {
            if (listener != null)
                listener.Abort();

            this.Close();
        }

        private void playInsert_Click(object sender, EventArgs e) {

        }


        
    }

}