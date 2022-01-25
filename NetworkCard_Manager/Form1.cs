using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net.NetworkInformation;
using DevExpress.XtraEditors;
using System.Diagnostics;

namespace NetworkCard_Manager
{
    public partial class frmMain : Form
    {
        public frmMain()
        {
            InitializeComponent();
        }

        private void frmMain_Load(object sender, EventArgs e)
        {
            NetworkInterface[] adapters = NetworkInterface.GetAllNetworkInterfaces();

            foreach (var adapter in adapters)
            {
                CreateControlGroup(adapter);

            }
        }

        private void CreateControlGroup(NetworkInterface adapter)
        {
            GroupBox gr = new GroupBox();
            gr.Width = flowLayoutPanel1.Width / 2 - 10; /// flowLayoutPanel1.Width - 10;


            LabelControl lbl = new LabelControl();
            lbl.Text = adapter.Name;
            lbl.Font = new Font("Arial", 12, FontStyle.Bold);
            lbl.Top = 10;
            lbl.Left = 10;
            gr.Controls.Add(lbl);

            ToggleSwitch toggle = new ToggleSwitch() { Name = adapter.Name, Width = gr.Width - 20, IsOn = adapter.OperationalStatus == OperationalStatus.Up };
            toggle.Font = new Font("Arial", 12, FontStyle.Bold);
            toggle.Top = 30;
            toggle.Left = 10;
            toggle.Tag = adapter;
            toggle.Toggled += Toggle_Toggled;

            gr.Controls.Add(toggle);
            flowLayoutPanel1.Controls.Add(gr);
        }

        private void Toggle_Toggled(object sender, EventArgs e)
        {
            ToggleSwitch toggle = (ToggleSwitch)sender;
            NetworkInterface adapter = (NetworkInterface)toggle.Tag;

            if (toggle.IsOn)
                EnableAdapter(adapter.Name);
            else
                DisableAdapter(adapter.Name);

        }
        static void EnableAdapter(string interfaceName)
        {
            ProcessStartInfo psi = new ProcessStartInfo("netsh", "interface set interface \"" + interfaceName + "\" enable");
            Process p = new Process();
            p.StartInfo = psi;
            p.Start();
        }

        static void DisableAdapter(string interfaceName)
        {
            ProcessStartInfo psi = new ProcessStartInfo("netsh", "interface set interface \"" + interfaceName + "\" disable");
            Process p = new Process();
            p.StartInfo = psi;
            p.Start();
        }
    }
}
