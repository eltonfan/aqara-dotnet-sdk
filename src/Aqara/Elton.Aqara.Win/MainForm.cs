using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Elton.Aqara.Win
{
    public partial class MainForm : Form
    {
        readonly AqaraClient client = null;
        readonly Timer timerRefresh = null;
        readonly DataTable table = new DataTable();
        public MainForm()
        {
            InitializeComponent();

            string basePath = Path.Combine(
                KnownFolderPaths.KnownFolders.GetPath(KnownFolderPaths.KnownFolder.SkyDrive),
                @"ApplicationData\ConnectedHome\");
            if (!Directory.Exists(basePath))
            {
                Path.Combine(
                    Environment.GetFolderPath(Environment.SpecialFolder.CommonDocuments),
                    @"Elton\ConnectedHome\");
            }
            var configFile = Path.Combine(basePath, "config", "aqara.json");
            AqaraConfig config = null;
            if(File.Exists(configFile))
            {
                config = AqaraConfig.Parse(File.ReadAllText(configFile));
            }
            client = new AqaraClient(config);

            timerRefresh = new Timer();
            timerRefresh.Interval = 1000;
            timerRefresh.Tick += TimerRefresh_Tick;

            var column = new DataColumn() { Unique = true };
            table.Columns.Add("sid", typeof(string));
            table.Columns.Add("short_id", typeof(string));
            table.Columns.Add("name", typeof(string));
            table.Columns.Add("model", typeof(string));
            table.Columns.Add("data", typeof(string));
            table.Columns.Add("timestamp", typeof(string));
            table.Columns.Add("DateUpdated", typeof(DateTime));
            table.PrimaryKey = new DataColumn[] { table.Columns["sid"] };

            dataGridView1.DataSource = table;
        }

        void AddOrUpdate(string sid, string name, DeviceModel model, long short_id, string data, DateTime timestamp)
        {
            if (table.Rows.Contains(sid))
            {
                var row = table.Rows.Find(sid);

                //row["sid"]
                row["short_id"] = short_id.ToString("X4");
                row["name"] = name;
                row["model"] = model?.ToString();
                row["data"] = data;
                row["timestamp"] = $"{DateTime.Now.Subtract(timestamp).TotalSeconds:0}s";
                row["DateUpdated"] = timestamp;
            }
            else
            {
                table.Rows.Add(sid, short_id.ToString("X4"), name, model?.ToString(), data, "刚刚", timestamp);
            }
        }

        private void TimerRefresh_Tick(object sender, EventArgs e)
        {
            var gateway = client.Gateways.Values.FirstOrDefault();
            if(gateway == null)
            {
                labelGatewayIP.Text = "-";
                labelTimestamp.Text = "-";
                labelToken.Text = "-";

                table.Rows.Clear();
            }
            else
            {
                labelGatewayIP.Text = gateway.EndPoint?.ToString();
                labelTimestamp.Text = gateway.LatestTimestamp.ToString();
                labelToken.Text = gateway.Token;

                foreach(var device in gateway.Devices.Values)
                {
                    var sb = new StringBuilder();
                    foreach(var pair in device.States)
                    {
                        sb.Append($"{pair.Key}:{pair.Value.Value} ");
                    }
                    AddOrUpdate(device.Id, device.Name, device.Model, device.ShortId, sb.ToString(), device.LatestTimestamp);
                }
            }
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            Task.Run(() => { client.DoWork(null); });
            timerRefresh.Start();
        }
    }
}
