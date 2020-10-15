using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;

namespace ADCP
{
    public partial class FrmDownload : Form
    {
        public FrmDownload(RTI.AdcpSerialPort adcpsp)
        {
            InitializeComponent();
            sp = adcpsp;
            sp.Reconnect();
            OnDSDIR();
            textBoxDirectory.Text = Environment.CurrentDirectory;

            download = new DownloadDelegate(OnDownload);
            displaybar = new DisplayProgressBarDelegate(OnDisplay);

            timer.Interval = 1000;
            timer.Elapsed += new System.Timers.ElapsedEventHandler(timer_Elapsed);

            toolStripProgressBar1.Maximum = 100;
        }

        #region parameters
        private RTI.AdcpSerialPort sp;
        public bool bDownload = false;

        private delegate void DownloadDelegate(string dirName,string fileName); //定义一个委托，用于记录并解析数据
        DownloadDelegate download;

        private delegate void DisplayProgressBarDelegate(int iflag);
        DisplayProgressBarDelegate displaybar;

        List<float> fileSize=new List<float>(); //记录数据大小
        System.Timers.Timer timer = new System.Timers.Timer();

        private int iFlag = 0;
     
        #endregion

        private void btnDownLoad_Click(object sender, EventArgs e)
        {
            try
            {
                //数据下载
                if (sp.IsOpen())
                { 
                    for (int i = 0; i < checkedListBoxFiles.Items.Count; i++)
                    {
                        if (checkedListBoxFiles.GetItemChecked(i))
                        {
                            iFlag = i;
                            string strfilename = checkedListBoxFiles.Items[i].ToString();
                            try
                            {
                                MessageBox.Show("Downloading " + strfilename);
                                this.BeginInvoke(download, textBoxDirectory.Text, strfilename);

                                toolStripProgressBar1.Value = 0;
                                toolStripProgressBar1.Visible = true;
                                timer.Start();
                            }
                            catch
                            {
                                bDownload = false;
                                toolStripStatusLabel1.Text = Resource1.String301;
                            }
                        }
                        //toolStripProgressBar1.Visible = false;
                    }
                }
                //else
                //    MessageBox.Show("Please Connect Instrument.");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void timer_Elapsed(object sender,EventArgs e)
        {
            try
            {
                this.BeginInvoke(displaybar, iFlag);
            }
            catch
            {
            }
        }

        private void OnDisplay(int iflag)
        {
            try
            {
                string strfilename = checkedListBoxFiles.Items[iflag].ToString();
                float currentFileSize = new System.IO.FileInfo(textBoxDirectory.Text + "\\" + strfilename).Length;
                float maxFileSize = fileSize[iflag] * 1024 * 1024;

                toolStripProgressBar1.Value = (int)(currentFileSize / maxFileSize * 100);
                toolStripStatusLabel1.Text = Resource1.String299;

                if (currentFileSize >= maxFileSize)
                {
                    toolStripProgressBar1.Visible = false;
                    bDownload = true;
                    toolStripStatusLabel1.Text = Resource1.String300;

                    timer.Stop();
                }
            }
            catch
            {
                toolStripProgressBar1.Visible = false;
                bDownload = true;
                toolStripStatusLabel1.Text = Resource1.String300;

                timer.Stop();
            }

        }

        private void OnDownload(string dirName,string fileName)
        {
            sp.XModemDownload(dirName, fileName, false, false);
        }

        private void OnDSDIR()
        {
            try
            {
                if (sp.IsOpen())
                {
                    sp.ClearInputBuffer();
  
                    if (sp.IsBreakState())
                    {
                        sp.SendBreak();
                    }
                    //显示内存空间，DSDIR命令
                    List<string> cmd = new List<string>();
                    cmd.Add(RTI.Commands.AdcpCommands.CMD_DSDIR);
                 
                    if (sp.SendCommands(cmd))
                    {
                        textBoxDSDIR.Text = sp.ReceiveBufferString;
                    }
                    else
                    {
                        textBoxDSDIR.Text = Resource1.String302;
                    }
                 
                    #region 解析DSDIR命令,提取数据ENS或RAW的名称及大小
                  
                    checkedListBoxFiles.Items.Clear();
                    string strBuffer=sp.ReceiveBufferString;
                    string[] lines = strBuffer.Split('\n');
                    for (int i = 0; i < lines.Length; i++)
                    {
                        if (lines[i].Contains("ENS") || lines[i].Contains("RAW"))
                        {
                            try
                            {
                                char[] delimiters = { ' ' };
                                string[] strname = lines[i].Trim().Split(delimiters, StringSplitOptions.RemoveEmptyEntries);
                                checkedListBoxFiles.Items.Add(strname[0]);

                                float filesize = float.Parse(strname[strname.Length - 1]);
                                fileSize.Add(filesize);
                                
                            }
                            catch
                            {
                            }
                        }
                    }

                    #endregion
                  

                      /*
                    #region 将SD卡中的数据文件列出
                    RTI.Commands.AdcpDirListing listing = new RTI.Commands.AdcpDirListing();
                    listing = sp.GetDirectoryListing();
                    float ftotal = listing.TotalSpace;
                    float fused = listing.UsedSpace;
               
                    checkedListBoxFiles.Items.Clear();
                    foreach (RTI.Commands.AdcpEnsFileInfo fileInfo in listing.DirListing)
                    {
                        //checkedListBoxFiles.Items.Add(fileInfo.FileName + "      " + fileInfo.FileSize.ToString("0.0000") + "\r\n");
                        if (fileInfo.FileName != "")
                            checkedListBoxFiles.Items.Add(fileInfo.FileName);
                    }
                    #endregion
                       * */
                     

                        if (checkedListBoxFiles.Items.Count < 1)
                            btnDownLoad.Enabled = false;
                }
                else
                    MessageBox.Show("Please Connect Instrument.");

            }
            catch(Exception ex)
            {
                this.Close();
                MessageBox.Show(ex.Message);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            try
            {
                if (sp.IsOpen())
                {
                    sp.CancelDownload();
                }
            }
            catch
            {
            }
          
        }

        private void btnDirectory_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog folderDlg = new FolderBrowserDialog();
            if (DialogResult.OK == folderDlg.ShowDialog())
            {
                textBoxDirectory.Text = folderDlg.SelectedPath;
            }
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            try
            {
                if (sp.IsOpen())
                {
                    sp.Disconnect();
                }
            }
            catch
            {
            }
            base.OnFormClosing(e);
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            try
            {
                sp.Disconnect();
            }
            catch
            {
            }
            this.Close();
        }

        private void checkedListBoxFiles_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            //实现单选功能
            if (e.CurrentValue == CheckState.Checked) return;
            for (int i = 0; i < ((CheckedListBox)sender).Items.Count; i++)
            {
                ((CheckedListBox)sender).SetItemChecked(i, false);
            }
            e.NewValue = CheckState.Checked;
        }
    }
}
