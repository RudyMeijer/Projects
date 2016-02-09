using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using AxCVIMAGELib;
using AxCVDISPLAYLib;
using Cvb;
using IrisVision;
using IrisVision.IPL;
using System.Threading;
using System.Diagnostics;

namespace TestTracker
{
    public partial class Form1 : Form
    {
        #region FIELDS
        private SharedImg imgOut;
        private Point startPoint;
        private AppSettings appSet;
        private CaliperTool caliperTool;
        private bool Initialized;
        private Foundation.FBLOB handle;
        private string statusMsg;
        private Tracker tracker;
        private int ImageWidth;
        private int ImageHeight;
        private int cnt;
        private Tracker tracker1;
        private int cnt1;
        private int deltaFinish;
        #endregion
        #region CONSTRUCTOR
        public Form1()
        {
            InitializeComponent();
            this.Text = ProductName + " V" + ProductVersion;
            IV.TraceError(1000, "Starting {0}", Text);
            appSet = AppSettings.Load(propertyGrid1);
            InitializeStartUp();
            InitializeAppSettings();
            InitializeTracker();
            InitializeDriver();
            Initialized = true;
            Status("Ready");
        }

        #endregion
        #region INITIALIZE
        private void InitializeTracker()
        {
            deltaFinish = (int)numericUpDown2.Value;
            tracker = new Tracker(appSet.TrackParams);
            tracker1 = new Tracker(appSet.TrackParams);
            tracker1.FinishLine = tracker.FinishLine + deltaFinish;
        }
        private void InitializeStartUp()
        {
            /// <summary>
            /// Process commandline arguments.
            /// </summary>
            var cmdLine = Environment.CommandLine.ToUpper();
            if (cmdLine.Contains("DELAY")) Thread.Sleep(10000);//ms
            InitializeCheckBoxes(cmdLine);
        }
        private void InitializeCheckBoxes(string cmdLine)
        {
            foreach (var c in GetAll(this, typeof(CheckBox)))
            {
                var chkbox = c as CheckBox;
                if (cmdLine.Contains(chkbox.Text.ToUpper())) chkbox.Checked = true;
                if (cmdLine.Contains("-" + chkbox.Text.ToUpper())) chkbox.Checked = false;
            }
        }
        public IEnumerable<Control> GetAll(Control control, Type type)
        {
            var controls = control.Controls.Cast<Control>();
            return controls.SelectMany(ctrl => GetAll(ctrl, type)).Concat(controls).Where(c => c.GetType() == type);
        }
        private void InitializeAppSettings()
        {
            //if (caliperTool != null) caliperTool.PixelsPerMM = appSet.PixelsPerMM;
            numericUpDown1.Value = appSet.Threshold;
            propertyGrid1.HelpVisible = appSet.HelpVisible;
            InitializeTracker();
            if (appSet.Threshold == 128) // Set defaults.
            {
                appSet.Threshold = 220;
                appSet.InvertImage = true;
                appSet.ROI = new Rectangle(0, 0, 1020, 510);
            }

        }
        private void InitializeDriver()
        {
            /// <summary>
            /// Load CVB image driver and attach display 1.
            /// </summary>
            IV.Track();
            if (appSet.DriverPath.EndsWith("xml")) { MessageBox.Show("You can't load a configuration file (xml) as driver. Please use Load Configuration via menu.", this.Text); return; }
            if (axCVimage1.LoadImage(appSet.DriverPath))
            {
                axCVimage1.Grab = chkLive.Checked;
                axCVdisplay1.Image = axCVimage1.Image;
                ImageWidth = axCVimage1.ImageWidth;
                ImageHeight = axCVimage1.ImageHeight;
                DoExecute();
            }
            else IV.TraceError(1020, "Error loading Driver {0}", appSet.DriverPath);
        }
        private void InitializeCaliper()
        {
            /// <summary>
            /// Intialize a caliperToolbox which show a red measurement line on display 2. 
            /// </summary>
            caliperTool = new CaliperTool(axCVdisplay2);
            //caliperTool.PixelsPerMM = appSet.PixelsPerMM;
            if (!caliperTool.Load("Calipers.xml"))
            {
                caliperTool.Add("line", CaliperTypes.Line, 1);
                caliperTool.Save();
            }
        }
        #endregion
        #region METHODES
        private void DoExecute()
        {
            Overlay.Clear(axCVdisplay1);
            ShowFinishLine();
            //
            // Binarize Image.
            //
            BinarizeImage();
            //
            // Execute FBlob
            //
            ExecuteFBlob(axCVdisplay2.Image);
            var pigs = ShowBlobs();
            //
            // Execute tracker.
            //
            cnt += tracker.Execute(pigs);
            cnt1 += tracker1.Execute(pigs);
            //
            // Show results.
            //
            label1.Text = String.Format("{0} {1}",cnt,cnt1);
            label2.Text = String.Format("Quality = {0:f0} %", tracker.QualityFactor * 100);
            //
            // Save images.
            //
            if (chkSave.Checked) SaveImage();
        }
        #endregion
        #region General Methodes
        private void BinarizeImage()
        {
            //
            // Preserve zoom factor.
            //
            var pz = new PreserveZoom(axCVdisplay2);
            //
            // Binarize Image.
            //
            axCVdisplay2.Image = Conversion.BinarizeImage(axCVimage1.Image, appSet.Threshold, ref imgOut, appSet.InvertImage);
            pz.Restore();
        }
        private Foundation.FBLOB ExecuteFBlob(Cvb.Image.IMG img)
        {
            //
            // Intialize Blob.
            //
            var AOI = appSet.ROI;
            Cvb.Image.ReleaseObj(handle);
            handle = Foundation.FBlobCreate(img, 0);
            Foundation.FBlobSetSkipBinarization(handle, true);
            Foundation.FBlobSetLimitArea(handle, appSet.MinimumSize, appSet.MaximumSize);
            Foundation.FBlobSetArea(handle, 0, AOI.Left, AOI.Top, AOI.Right, AOI.Bottom);
            Foundation.FBlobSetObjectTouchBorder(handle, Foundation.BlobTouchBorderFilter.FBLOB_BORDER_NONE);
            Foundation.FBlobExec(handle);
            return handle;
        }
        private void ShowFinishLine()
        {
            var f = (int)appSet.TrackParams.FinishLine;
            var r = (int)appSet.TrackParams.ActieRadius;
            Overlay.DrawLine("finish", f, 0, f, ImageHeight);
            Overlay.DrawLine("actieradius", f - r, 0, f - r, ImageHeight, Color.AliceBlue);
            Overlay.DrawLine("actieradius", f + r, 0, f + r, ImageHeight, Color.AliceBlue);
            f += deltaFinish;
            Overlay.DrawLine("finish", f, 0, f, ImageHeight);
            Overlay.DrawLine("actieradius", f - r, 0, f - r, ImageHeight, Color.AliceBlue);
            Overlay.DrawLine("actieradius", f + r, 0, f + r, ImageHeight, Color.AliceBlue);
        }
        private List<Point> ShowBlobs()
        {
            int cnt, size; int x, y, dx, dy;
            Foundation.FBlobGetNumBlobs(handle, out cnt);
            Overlay.axCVdisplay = axCVdisplay1;
            var pigs = new List<Point>();
            for (int i = 0; i < cnt; i++)
            {
                Foundation.FBlobGetBlobSize(handle, i, out size);
                Foundation.FBlobGetBoundingBox(handle, i, out x, out y, out dx, out dy);
                //Overlay.DrawRectangle("Blobsize " + size + " px.", new Rectangle(x, y, dx, dy), Color.Red);
                Foundation.FBlobGetCenter(handle, i, out x, out y);
                if (Overlay.Enabled) Overlay.DrawLabel(x.ToString(), x, y);
                pigs.Add(new Point(x, y));
            }
            Overlay.Refresh();
            return pigs;
        }
        private void SaveImage()
        {
            try
            {
                //
                //  Create Directory structure:
                //
                //  var path  = String.Format("{0}\\Camera {1}\\{2:yyyy-MM-dd}", appSet.ImagePath, Camera.GetCameraPort(axCVimage) + 1, DateTime.Now);
                var path = Path.GetDirectoryName(appSet.ImagePath);
                if (!Directory.Exists(path)) Directory.CreateDirectory(path);
                var fileName = String.Format("{0}\\{1:HH.mm.ss.fff}.bmp", path, DateTime.Now);
                if (appSet.ImagePath.EndsWith(".bmp")) fileName = appSet.ImagePath;
                var res = axCVimage1.SaveImage(fileName);
                Status((res) ? "Save image to {0}" : "Error saving file {0}", fileName);
            }
            catch (Exception ex) { Status(ex.ToString()); }
        }
        private void Status(string msg, params object[] args)
        {
            toolStripStatusLabel1.Text = String.Format(msg, args);
        }
        private String GetToolStripStatusDirectoryName(object sender)
        {
            //
            // 1) Find \\
            // 2) If driveletter is entered then return driveletter. 
            //
            var dirName = "";
            var status = (sender as ToolStripItem).Text;
            int p = status.IndexOf('\\');
            if (p >= 1 && status[p - 1] == ':') p -= 2; // return driveletter.
            if (p >= 0) dirName = status.Substring(p);
            return dirName;
        }
        #endregion
        #region FORMEVENTS
        private void LoadConfigMenuItem_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                var fileName = openFileDialog1.FileName;
                appSet = AppSettings.Load(propertyGrid1, fileName);
                InitializeAppSettings();
                InitializeDriver();
                if (!appSet.DriverPath.EndsWith("vin")) btnSnap_Click(null, null);
                this.Text = ProductName + " V" + ProductVersion + " configuration " + fileName;
            }

        }
        private void SaveConfigMenuItem_Click(object sender, EventArgs e)
        {
            if (saveFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                appSet.SaveAs(saveFileDialog1.FileName);
            }
        }
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (chkLive.Checked)
            {
                chkLive.Checked = false;
                System.Threading.Thread.Sleep(100); // Wait for last event.
            }
            Properties.Settings.Default.Save();
            if (caliperTool != null) caliperTool.Save();
        }
        private void chkLive_CheckedChanged(object sender, EventArgs e)
        {
            axCVimage1.Grab = chkLive.Checked;
            axCVimage2.Grab = chkLive.Checked;
            axCVdisplay1.ManualDisplayRefresh = chkLive.Checked;
        }
        private void btnSnap_Click(object sender, EventArgs e)
        {
            axCVimage1.Snap();
            axCVimage2.Snap();
            axCVdisplay1.Refresh();
            axCVdisplay2.Refresh();
            DoExecute();
        }
        private void axCVimage1_ImageSnaped(object sender, EventArgs e)
        {
            IV.TraceInformation(3005, "{0} {1} 2D image snapped. {2}", Text, Environment.MachineName, statusMsg);

            Stopwatch s = Stopwatch.StartNew();
            DoExecute();
            statusMsg = toolStripStatusLabel2.Text = String.Format(" Process time: {0} ms, mem = {1} MB.", s.ElapsedMilliseconds, IV.GetPrivateMemory());
        }
        private void axCVimage2_ImageSnaped(object sender, EventArgs e)
        {
            axCVdisplay2.Refresh();
            //DoExecute(axCVimage2);
        }
        private void btnExecute_Click(object sender, EventArgs e)
        {
            Stopwatch s = Stopwatch.StartNew();
            DoExecute();
            string msg = string.Format(" Elapsed time: {0} ms.", s.ElapsedMilliseconds);
            Status(msg);
        }
        private void axCVdisplay2_MouseDownEvent(object sender, _DCVdisplayEvents_MouseDownEvent e)
        {
            int _x = 0;
            int _y = 0;
            axCVdisplay2.ClientToImage(e.x, e.y, ref _x, ref _y);
            axCVdisplay2.StatusUserText = " x: " + _x.ToString() + " y: " + _y.ToString();
            startPoint = new Point(_x, _y);
        }
        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            if (Initialized)
            {
                chkSave.Checked = false;
                appSet.Threshold = (int)(sender as NumericUpDown).Value;
                appSet.Refresh();

                DoExecute();
            }
        }
        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void axCVdisplay1_RectDraged(object sender, EventArgs e)
        {
            var axCVdisplay = (sender as AxCVdisplay);
            Double x0 = 0, y0 = 0, x1 = 0, y1 = 0, x2 = 0, y2 = 0;
            axCVdisplay.GetSelectedArea(ref x0, ref y0, ref x1, ref y1, ref x2, ref y2);
            appSet.ROI = Conversion.FillRect(x0, y0, x1, y1, x2, y2);
            appSet.Refresh();
            DoExecute();
        }
        private void toolStripStatusLabel1_Click(object sender, EventArgs e)
        {
            var path = appSet.ImagePath;// GetToolStripStatusDirectoryName(sender);
            Process.Start("Explorer", path);
        }
        private void propertyGrid1_PropertyValueChanged(object s, PropertyValueChangedEventArgs e)
        {
            chkSave.Checked = false;
            //if (e.ChangedItem.Label == "PixelsPerMM" && caliperTool != null) caliperTool.PixelsPerMM = appSet.PixelsPerMM;
            if (e.ChangedItem.Label == "DriverPath")
            {
                var path = (string)e.ChangedItem.Value;
                if (path.Length == 0 || path[0] == '.')
                {
                    path = Environment.ExpandEnvironmentVariables("%cvb%Drivers\\GenIcam.vin");
                    appSet.DriverPath = path;
                    appSet.Refresh();
                }
                if (File.Exists(appSet.DriverPath))
                {
                    if (!Properties.Settings.Default.DriverPath.Contains(appSet.DriverPath))
                        Properties.Settings.Default.DriverPath.Add(appSet.DriverPath);
                    InitializeDriver();
                }
            }
            else
            {
                InitializeAppSettings();
                DoExecute();
            }
        }
        private void splitContainer1_Panel2_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Right)
            {
                appSet.SupervisorMode = true;
                appSet.Refresh();
            }
        }
        private void chkLabels_CheckedChanged(object sender, EventArgs e)
        {
            Overlay.Enabled = chkLabels.Checked;
            DoExecute();
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            cnt1=cnt = 0; InitializeTracker(); DoExecute();
        }
        #endregion

        private void numericUpDown2_ValueChanged(object sender, EventArgs e)
        {
            deltaFinish = (int)numericUpDown2.Value;
			tracker1.FinishLine = tracker.FinishLine + deltaFinish;
			DoExecute();
        }
    }
}
