using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace rebbit01
{

    public partial class frmRebbit : Form
    {
        public string QuerySong;
        SoundParser pars;
        WebClient wb = new WebClient();
        string TrackName;
        public frmRebbit()
        {
            InitializeComponent();
        }

        private void btnDownload_Click(object sender, EventArgs e)
        {
      

            try
             {
                /* int i = 0;

                 do
                 {
                     getSelectedSong(treeView1.Nodes[i]);
                     i++;
                 } while (treeView1.Nodes[i] != treeView1.Nodes[i].LastNode);*/

                foreach (TreeNode sound in treeView1.Nodes)
                {
                    getSelectedSong(sound);

                }

             }
             catch (ArgumentOutOfRangeException exc)
             {
                 MessageBox.Show(exc.Message);
             }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
           
        }
        private async void getSelectedSong(TreeNode a)
        {
            SoundParser pars = new SoundParser();

            wb.DownloadProgressChanged += DownloadProgressChanged;
            wb.DownloadFileCompleted += DownloadFileCompleted;


            foreach (SoundNode childNode in a.Nodes) //не зацикливаеться ли от родителя к 1 ребенку и обратно?
            {
                if (childNode.Checked)
                {
                    string url = pars.getMp3Link(childNode.getAttr());

                    if (url == null) { MessageBox.Show("Для даннй композиции нет ссылки на скачивание"); }
                    else
                    {
                        Uri pl = new Uri(url);
                        try
                        {
                            TrackName = childNode.getName() + ".mp3";
                            TrackName = TrackName.Replace(" ", "_");
                            TrackName = TrackName.Replace(":", "_");
                            await wb.DownloadFileTaskAsync(pl, musDir.Text + "\\" + TrackName);

                        }
                        catch (Exception e)
                        {

                            MessageBox.Show("В процессе работы приложения возникло " + e.Message);
                        }

                    }
                    a = childNode; //посетить ребенка

                    getSelectedSong(a);
                }

            }

        }
        private void DownloadFileCompleted(object sender, System.ComponentModel.AsyncCompletedEventArgs e)
        {
            progressDownload.Value = 0;
          toolStripStatusLabel1.Text = "Файл успешно скачан";
        }

        private void DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            progressDownload.Value = (int)e.ProgressPercentage;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                musDir.Text = folderBrowserDialog1.SelectedPath;
                iniSetting iniSet = new iniSetting();

               
                iniSet.pathDir = musDir.Text;


                // выкидываем класс iniSet целиком в файл setting.xml
                using (Stream writer = new FileStream("setting.xml", FileMode.Create))
                {
                    XmlSerializer serializer = new XmlSerializer(typeof(iniSetting));
                    serializer.Serialize(writer, iniSet);
                }
            }
        }

        private void frmRebbit_Load(object sender, EventArgs e)
        {
           

            using (Stream stream = new FileStream("setting.xml", FileMode.OpenOrCreate))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(iniSetting));

                // в тут же созданную копию класса iniSettings под именем iniSet
                iniSetting iniSet = (iniSetting)serializer.Deserialize(stream);

                // 
                musDir.Text =  iniSet.pathDir;

            }
        }

        private void txtQuery_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (txtQuery.Text.Length > 0)
                {
                    QuerySong = txtQuery.Text;
                    pars = new SoundParser(QuerySong);

                    treeView1.Nodes.Add(pars.BuildNode());
                }
                else
                {
                    MessageBox.Show("Введите в поле поиска название песни,\n исполнителя или жанр.", "Пустой запрос", MessageBoxButtons.OK,
            MessageBoxIcon.Information);
                }


            }

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (txtQuery.Text.Length > 0)
            {
                QuerySong = txtQuery.Text;
                pars = new SoundParser(QuerySong);

                treeView1.Nodes.Add(pars.BuildNode());
            }
            else
            {
                MessageBox.Show("Введите в поле поиска название песни,\n исполнителя или жанр.", "Пустой запрос", MessageBoxButtons.OK,
        MessageBoxIcon.Information);
            }

        }
    }
        
    }
