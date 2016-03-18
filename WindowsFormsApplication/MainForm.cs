﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApplication
{
    public partial class MainForm : Form
    {
        [DllImport("user32.dll", EntryPoint = "FindWindow", CharSet = CharSet.Auto)]
        private extern static IntPtr FindWindow(string classname, string captionName);

        [DllImport("user32.dll", EntryPoint = "FindWindowEx", CharSet = CharSet.Auto)]
        private extern static IntPtr FindWindowEx(IntPtr parent, IntPtr child, string classname, string captionName);

        [DllImport("user32.dll")]
        static extern IntPtr SendMessage(IntPtr hWnd, UInt32 Msg, IntPtr wParam, [MarshalAs(UnmanagedType.LPStr)] string lParam);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool SetForegroundWindow(IntPtr hWnd);
        public MainForm()
        {
            InitializeComponent();
            Initialize();
            getWebBrowser();

        }
        public WebBrowser getWebBrowser()
        {
            return this.webBrowser;
        }

        private void Initialize()
        {
            this.webBrowser.Navigate("http://www.jsds.gov.cn/index/caLogin.html");
            //this.webBrowser.Navigate("https://www.jsds.gov.cn/index/sbLogin.do");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void webBrowser_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            HtmlDocument doc = this.webBrowser.Document;   //把当前的webBrowser1显示的文档实例
            HtmlElementCollection elemColl = doc.GetElementsByTagName("a");
            //MessageBox.Show(  webBrowser.DocumentText );
           // MessageBox.Show(elemColl.Count.ToString() );
            foreach (HtmlElement elem in elemColl)
            {
                string elemName = elem.GetAttribute("href");
                if(elemName.Equals("http://www.jsds.gov.cn/index/caLogin.html#"))
                {
                    //MessageBox.Show("find!");
                    elem.InvokeMember("click"); //点击登陆
                    this.timer1.Enabled = true;
                    //Thread.Sleep(3000);
                    //模拟输入 密码
                    //IntPtr hwndCalc = FindWindow("#32770", null); //查找窗口句柄
                    //if (hwndCalc != IntPtr.Zero)//找到
                    //{
                    //    SetForegroundWindow(hwndCalc);
                    //    System.Windows.Forms.SendKeys.SendWait("{TAB}");
                    //    System.Windows.Forms.SendKeys.SendWait("{TAB}");
                    //    System.Windows.Forms.SendKeys.SendWait("123456");
                    //    System.Windows.Forms.SendKeys.SendWait("{TAB}");
                    //    System.Windows.Forms.SendKeys.SendWait("{ENTER}");
                    //}
                    //else
                    //{
                    //   // MessageBox.Show("没有启动 ");
                    //}
                }         

            }
            //if (doc.All.Count != 0)
            //{

            //    for (int i = 0; i < doc.All.Count; i++)          //循环查找这个对象的每一个元素
            //    {
            //        if (doc.All[i].GetAttribute("class") == "link-login")
            //        {
            //            doc.All[i].InvokeMember("click");
            //            break;
            //        }

            //    }
            //}
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            IntPtr mwh1 = IntPtr.Zero;
            mwh1 = FindWindow(null, "数字证书登录");
            if (mwh1 != IntPtr.Zero)
            {
                IntPtr edit = FindWindowEx(mwh1, IntPtr.Zero, "Edit", null);
              //  IntPtr button = FindWindowEx(mwh1, IntPtr.Zero, "Button", "确定");
                if (edit != IntPtr.Zero)
                {
                    //MessageBox.Show("find edit");
                    SendMessage(edit, 0xC, IntPtr.Zero, "123456");
                    SendKeys.SendWait("{Enter}");
                    SendKeys.Flush();
                    this.timer1.Dispose();    //关闭定时器
                }
            }
        }
    }
}
