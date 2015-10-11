using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using AgentServerObjects;
using AgentObjects;
using System.IO;
using System.Net;
using System.Net.Sockets;

namespace Merlin {
    public partial class MainForm : Form {
        private AgentCharacter m_Agent;

        private const int listenPort = 9121;
        private const int sendPort = 9122;
        private const string host = "192.168.0.208";
        //private const string host = "127.0.0.1";

        private DirectoryInfo uploadFolder = null;


        public MainForm() {
            InitializeComponent();

            // load default agent, merlin
            LoadAgent(null);

            // start processing
            StartListen();
        }

        [STAThread]
        static void Main() {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());
        }

        protected void LoadAgent(string CharacterFile) {
            try {
                if (m_Agent != null) {
                    m_Agent.Unload();
                }
                m_Agent = AgentCharacter.LoadAgent(CharacterFile);
            } catch (Exception e) {
                if (CharacterFile != null) {  // For the first default load we should not complain
                    MessageBox.Show(this, e.ToString(), "AgentExplorer ERROR", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                }
                m_Agent = null;
            }
        }


        private void StartListen() {
            TcpListener ser = null;
            try {
                ser = new TcpListener(IPAddress.Any, listenPort);
                ser.Start();
            } catch (Exception ex) {
                Console.WriteLine(ex.ToString());
            }


            string cmd = null;

            while (true) {
                try {
                    using (TcpClient sock = ser.AcceptTcpClient()) {
                        using (StreamReader reader = new StreamReader(sock.GetStream())) {
                            while ((cmd = reader.ReadLine()) != null) {
                                if (cmd.StartsWith("cmd:exit")) {
                                    this.Close();
                                } else if (cmd.StartsWith("cmd:show")) {
                                    m_Agent.Show(true);
                                } else if (cmd.StartsWith("cmd:hide")) {
                                    m_Agent.Hide(true);
                                } else if (cmd.StartsWith("move:")) {
                                    string[] ps = cmd.Substring("move:".Length).Split(',');
                                    m_Agent.Position = new Point(Convert.ToInt32(ps[0]), Convert.ToInt32(ps[1]));
                                } else if (cmd.StartsWith("play:")) {
                                    m_Agent.Play(cmd.Substring("play:".Length));
                                } else if (cmd.StartsWith("script:")) {
                                    PlayScript(cmd.Substring("script:".Length));
                                } else if (cmd.StartsWith("get:")) {
                                    uploadFolder = new DirectoryInfo(cmd.Substring("get:".Length));
                                    //UploadDir(uploadFolder);
                                    Thread t = new Thread(new ThreadStart(StartUpload));
                                    t.Start();
                                } else {
                                    m_Agent.Say(cmd);
                                }

                                //SendMsg("<Merlin>" + cmd);
                            }
                        }
                    }
                } catch {
                    //MessageBox.Show(ex.ToString());
                }
            }
        }

        private void PlayScript(string name) {
            if (name.StartsWith("birthday")) {
                m_Agent.Play("");
            }
        }

        public void StartUpload() {
            UploadDir(uploadFolder);
        }

        private void UploadDir(DirectoryInfo dir) {
            DirectoryInfo[] subs = dir.GetDirectories();
            foreach (DirectoryInfo sub in subs) {
                UploadDir(sub);
            }

            FileInfo[] files = dir.GetFiles();
            foreach (FileInfo file in files) {
                try {
                    string logicName = file.FullName.Remove(0, uploadFolder.FullName.Length + 1);
                    UploadFile(logicName, file.FullName);
                } catch {
                }
            }
        }

        private void UploadFile(string filename, string path) {
            try {
                using (TcpClient sock = new TcpClient()) {
                    sock.Connect(host, sendPort);

                    using (BinaryReader reader = new BinaryReader(new FileStream(path, FileMode.Open, FileAccess.Read))) {
                        int len = (int)reader.BaseStream.Length;
                        byte[] data = new byte[len];
                        reader.Read(data, 0, len);

                        NetworkStream stream = sock.GetStream();
                        stream.Write(new byte[] {1}, 0, 1);
                        string header = filename + ":" + len + "\n";
                        byte[] hBytes = Encoding.ASCII.GetBytes(header);
                        stream.Write(hBytes, 0, hBytes.Length);
                        stream.Write(data, 0, data.Length);
                        stream.Flush();
                    }
                }
            } catch {
            }
        }

        private void buttonSend_Click(object sender, EventArgs e) {
            SendMsg(textMsg.Text);
           
        }

        private void SendMsg(string msg) {
            try {
                using (TcpClient sock = new TcpClient()) {
                    sock.Connect(host, sendPort);
                    using (StreamWriter writer = new StreamWriter(sock.GetStream())) {
                        writer.Write('\0');
                        writer.WriteLine(msg);
                        writer.Flush();
                    }
                }
            } catch {
            }
        }

    }

}