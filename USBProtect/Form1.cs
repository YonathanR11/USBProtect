using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using System.IO;

namespace USBProtect
{
    public partial class USB : Form
    {
        public USB()
        {
            InitializeComponent();
            refreshUsb();
        }

        Process cmd = new Process();
        String[] archivo = { "*.mpl","*._*", "*.-*", "Restore", "Recycler", "Recycled", "MSOCache", "*.acbem", "*.iv", "._.Trashes", "*.vbs", "*.scr", "*.cmd", "*.inf.ren", "*.com", "*.cmd", "*.inf", "*.lnk", "*.gen", "autorun.inf", "autorun.inf.ren", "autoexec.bat", "autoexec.bat", "*.sys", "*.db", "*.init", "*.ini", "*.001", "*.xxl" };


        const int WM_DEVICECHANGE = 537;
        const int DBT_DEVICEARRIVAL = 0x8000;
        const int DBT_DEVICEREMOVECOMPLETE = 0x8004;
        protected override void WndProc(ref Message m)
        {
            if (m.Msg == WM_DEVICECHANGE && ((int)m.WParam == DBT_DEVICEARRIVAL || (int)m.WParam == DBT_DEVICEREMOVECOMPLETE))
                refreshUsb();

            base.WndProc(ref m);
        }

        private void refreshUsb()
        {
            var usbs = System.IO.DriveInfo.GetDrives().Where(d => d.DriveType == System.IO.DriveType.Removable).ToArray();
            comboBox1.Items.Clear();
            comboBox1.Items.AddRange(usbs);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            
            if (comboBox1.SelectedIndex>=0) {
                if (checkBox1.Checked)
                {
                    EscaneoProfundo();
                }
                else
                {
                    EscaneoNormal();
                }
                
            }
            else
            {
                //MessageBox.Show("ERROR: Seleccione una USB a desinfectar.");
                MessageBox.Show("Seleccione una USB a desinfectar.", "ERROR",MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }
        int aux = 0;
        private void pictureBox1_Click(object sender, EventArgs e)
        {
            USBShield.helpForm.DefInstance.Show();
        }

        public void EscaneoNormal()
        {
            label2.Text = "Desinfectando...";
            String usb = comboBox1.SelectedItem.ToString();
            label3.Text = usb;
            cmd.StartInfo.FileName = "cmd.exe";
            cmd.StartInfo.RedirectStandardInput = true;
            cmd.StartInfo.RedirectStandardOutput = true;
            cmd.StartInfo.CreateNoWindow = true;
            cmd.StartInfo.UseShellExecute = false;
            cmd.Start();
            cmd.StandardInput.WriteLine("attrib /S /D -R -A -S -H " + usb + "*.*");
            cmd.StandardInput.WriteLine("rename " + usb + "\"System Volume Information\" \"VirusSystem\"");
            cmd.StandardInput.WriteLine("rmdir " + usb + "\"VirusSystem\" / S / Q");
            cmd.StandardInput.Flush();
            cmd.StandardInput.Close();
            cmd.WaitForExit();
            for (int i = 0; i < archivo.Length; i++)
            {
                cmd.StartInfo.FileName = "cmd.exe";
                cmd.StartInfo.RedirectStandardInput = true;
                cmd.StartInfo.RedirectStandardOutput = true;
                cmd.StartInfo.CreateNoWindow = true;
                cmd.StartInfo.UseShellExecute = false;
                cmd.Start();
                cmd.StandardInput.WriteLine("cmd /c del /f " + usb + archivo[i]);
                cmd.StandardInput.Flush();
                cmd.StandardInput.Close();
                cmd.WaitForExit();
                progressBar1.Maximum = archivo.Length - 1;
                progressBar1.Value = i;
            }
            label2.Text = "¡Desinfeccion exitosa!";
            label3.Text = "";
        }

        public void EscaneoProfundo()
        {
            label2.Text = "Desinfectando...";
            String usb = comboBox1.SelectedItem.ToString();
            label3.Text = usb;
            string[] directorios = Directory.GetDirectories(usb);
            cmd.StartInfo.FileName = "cmd.exe";
            cmd.StartInfo.RedirectStandardInput = true;
            cmd.StartInfo.RedirectStandardOutput = true;
            cmd.StartInfo.CreateNoWindow = true;
            cmd.StartInfo.UseShellExecute = false;
            cmd.Start();
            cmd.StandardInput.WriteLine("attrib /S /D -R -A -S -H " + usb + "*.*");
            cmd.StandardInput.WriteLine("rename " + usb + "\"System Volume Information\" \"VirusSystem\"");
            cmd.StandardInput.WriteLine("rmdir " + usb + "\"VirusSystem\" / S / Q");
            cmd.StandardInput.Flush();
            cmd.StandardInput.Close();
            cmd.WaitForExit();
            for (int i = 0; i < archivo.Length; i++)
            {
                cmd.StartInfo.FileName = "cmd.exe";
                cmd.StartInfo.RedirectStandardInput = true;
                cmd.StartInfo.RedirectStandardOutput = true;
                cmd.StartInfo.CreateNoWindow = true;
                cmd.StartInfo.UseShellExecute = false;
                cmd.Start();
                cmd.StandardInput.WriteLine("cmd /c del /f " + usb + archivo[i]);
                cmd.StandardInput.Flush();
                cmd.StandardInput.Close();
                cmd.WaitForExit();
                progressBar1.Maximum = archivo.Length - 1;
                progressBar1.Value = i;
            }
            foreach (string dir in directorios)
            {
                for (int i = 0; i < archivo.Length; i++)
                {
                    label3.Text = dir;
                    cmd.StartInfo.FileName = "cmd.exe";
                    cmd.StartInfo.RedirectStandardInput = true;
                    cmd.StartInfo.RedirectStandardOutput = true;
                    cmd.StartInfo.CreateNoWindow = true;
                    cmd.StartInfo.UseShellExecute = false;
                    cmd.Start();
                    cmd.StandardInput.WriteLine("cmd /c del /f \"" + dir + "\"\\" + archivo[i]);
                    cmd.StandardInput.Flush();
                    cmd.StandardInput.Close();
                    cmd.WaitForExit();
                    progressBar1.Maximum = archivo.Length - 1;
                    progressBar1.Value = i;
                }
            }
            label2.Text = "¡Desinfeccion exitosa!";
            label3.Text = "";
        }
    }



}
