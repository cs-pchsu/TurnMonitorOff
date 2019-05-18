using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Win32;

using System.Runtime.InteropServices;
using System.Diagnostics; //to DllImport


namespace TurnMonitorOff
{
    public partial class Form1 : Form
    {
        [DllImport("user32.dll")]
        private static extern int SendMessage(int hWnd, int hMsg, int wParam, int lParam);

        [DllImport("user32.dll")]
        public static extern bool LockWorkStation();

        public int WM_SYSCOMMAND = 0x0112;
        public int SC_MONITORPOWER = 0xF170; 
        const string subkey = @"System\CurrentControlSet\Control\Power";
        const int TURN_ON = 1;
        const int TURN_OFF = 0;

        public Form1()
        {
            InitializeComponent();
            LinkLabel.Link link = new LinkLabel.Link();
            link.LinkData = "http://pc-hsu.blogspot.tw/2015/09/windows.html";
            linkLabel1.Links.Add(link);
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            get_reg_status();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            DialogResult myResult = MessageBox.Show("是否關閉顯示器 ? ", "關閉顯示器", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (myResult == DialogResult.Yes)
            {
                SendMessage(this.Handle.ToInt32(), WM_SYSCOMMAND, SC_MONITORPOWER, 2);//DLL function
                LockWorkStation();
            }
        }

        private void get_reg_status()
        {
            int status = TURN_ON;
            try
            {
                RegistryKey mainKey = Registry.LocalMachine.OpenSubKey(subkey);
                status = (int)mainKey.GetValue("CsEnabled");
                if (status == TURN_ON)
                    this.textBox1.Text = "休眠已啟動 !\r\n若還未生效，請嘗試重新開機。";
                else
                    this.textBox1.Text = "休眠已關閉 !\r\n若還未生效，請嘗試重新開機。";
            }
            catch (Exception e)
            {
                this.textBox1.Text = "發生錯誤，未能取得資訊。";
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {

            DialogResult myResult = MessageBox.Show("是否啟動休眠 ? ", "需要有管理者權限", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (myResult == DialogResult.Yes)
            {
                try
                {
                    RegistryKey mainKey = Registry.LocalMachine.OpenSubKey(subkey, true);
                    mainKey.SetValue("CsEnabled", TURN_ON);
                }
                catch (Exception ee)
                {
                    MessageBox.Show("操作此功能需要有管理者權限");
                    return;
                }
                get_reg_status();
                MessageBox.Show("啟動休眠 ! 重新啟動電腦，才會生效");
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            DialogResult myResult = MessageBox.Show("是否關閉休眠 ? ", "需要有管理者權限", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (myResult == DialogResult.Yes)
            {
                try
                {
                    RegistryKey mainKey = Registry.LocalMachine.OpenSubKey(subkey, true);
                    mainKey.SetValue("CsEnabled", TURN_OFF);
                }
                catch (Exception ee)
                {
                    MessageBox.Show("操作此功能需要有管理者權限");
                    return;
                }
                get_reg_status();
                MessageBox.Show("關閉休眠 ! 重新啟動電腦，才會生效");
            }
            
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start(e.Link.LinkData as string);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            DialogResult myResult = MessageBox.Show("是否關閉電腦 ? ", "關閉電腦", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (myResult == DialogResult.Yes)
            {
                try
                {
                    System.Diagnostics.Process.Start("shutdown.exe", "/s /t 0");
                }
                catch (Exception ee)
                {
                    MessageBox.Show("操作失敗");
                    return;
                }
                MessageBox.Show("關閉電腦 ! 成功 !");
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            DialogResult myResult = MessageBox.Show("是否重啟電腦 ? ", "重啟電腦", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (myResult == DialogResult.Yes)
            {
                try
                {
                    System.Diagnostics.Process.Start("shutdown.exe", "-r -t 0");
                }
                catch (Exception ee)
                {
                    MessageBox.Show("操作失敗");
                    return;
                }
                MessageBox.Show("重新啟動電腦 ! 成功 !");
            }
        }
    }
}
