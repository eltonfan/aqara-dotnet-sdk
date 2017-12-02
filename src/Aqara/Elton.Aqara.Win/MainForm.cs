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
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            Task.Run(() => { client.DoWork(null); });
        }
    }
}
