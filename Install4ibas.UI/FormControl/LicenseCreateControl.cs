﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace Install4ibas.UI
{
    public partial class LicenseCreateControl : ChildControl
    {
        public LicenseCreateControl()
        {
            InitializeComponent();
        }

        protected override void SetButtonsVisibleStyle()
        {
            this.ButtonsVisibleStyle = ButtonsVisibleStyle.Finish;
        }

        public override void LoadAppSetting()
        {
            this.txt_SiteName.Text = this.ShellControl.installService.AppSetting.SiteName;
            this.txt_WebInfo.Text = this.ShellControl.installService.AppSetting.Licenses.ToString();
        }

        private void btn_ChooseFolder_Click(object sender, EventArgs e)
        {
            OpenFileDialog fileDialog = new OpenFileDialog();
            fileDialog.Multiselect = true;
            fileDialog.Title = "请选择文件";
            fileDialog.Filter = "txt文件|license.txt";
            if (fileDialog.ShowDialog() == DialogResult.OK)
            {
                txt_Path.Text = fileDialog.FileName;
            }
            txt_LicenseInfo.Text = readtxt(txt_Path.Text);
            this.splitContainer2.Panel1Collapsed = false;
            this.splitContainer2.Panel2Collapsed = false;
            this.splitContainer2.SplitterDistance = this.splitContainer2.Width / 2;
        }
        private string readtxt(string path)
        {
            var tmp = new StringBuilder();

            System.IO.StreamReader fileReader;
            using (fileReader = System.IO.File.OpenText(path))
            {
                while (!fileReader.EndOfStream)
                {
                    var line = fileReader.ReadLine();
                    if (string.Equals(line, "DATAS:")) break;
                    tmp.AppendLine(line);
                }
            }
            return tmp.ToString();
        }
        private void btn_Import_Click(object sender, EventArgs e)
        {
            try
            {
                string InstallDiraddress = this.MyAppSetting.InstallDiraddress;
                DirectoryInfo TheFolder = new DirectoryInfo(InstallDiraddress);
                //遍历文件夹
                foreach (DirectoryInfo NextFolder in TheFolder.GetDirectories())
                {
                    if (File.Exists(txt_Path.Text))
                    {
                        string copypath = Path.Combine(NextFolder.FullName, Path.GetFileName(txt_Path.Text));
                        File.Copy(txt_Path.Text, copypath, true);
                    }
                }
                MessageBox.Show(string.Format("导入成功"));
            }
            catch (Exception error)
            {
                MessageBox.Show(string.Format("导入失败，因为：{0}", error.Message));
            }
        }

    }
}
