using SharpAvi;
using SharpAvi.Output;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SnapAvi
{
	public partial class Form1 : Form
	{
		#region FIELDS
		private static byte[] frameData;
		private static IAviVideoStream stream;
		private int _Width;
		private int _Height;
		private AviWriter writer;
		private bool res;
		#endregion
		#region CONSTRUCTOR + INIT
		public Form1()
		{
			InitializeComponent();
			InitializeDriver(@"%cvb%\tutorial\ClassicSwitch.emu");
			//InitializeAVI(); //done during Save.
		}

		private void InitializeDriver(string fileName)
		{
			if (fileName.Length > 0) res = axCVimage1.LoadImage(fileName);
			if (res)
			{
				axCVdisplay1.Image = axCVimage1.Image;
				_Width = axCVimage1.ImageWidth;
				_Height = axCVimage1.ImageHeight;
			}
		}

		private void InitializeAVI()
		{
			//https://sharpavi.codeplex.com/documentation
			writer = new AviWriter("test.avi")
			{
				FramesPerSecond = 30,
				// Emitting AVI v1 index in addition to OpenDML index (AVI v2)
				// improves compatibility with some software, including 
				// standard Windows programs like Media Player and File Explorer
				EmitIndex1 = true
			};
			stream = writer.AddVideoStream();
			// set standard VGA resolution
			stream.Width = _Width;
			stream.Height = _Height;
			// class SharpAvi.KnownFourCCs.Codecs contains FOURCCs for several well-known codecs
			// Uncompressed is the default value, just set it for clarity
			stream.Codec = KnownFourCCs.Codecs.Uncompressed;
			// Uncompressed format requires to also specify bits per pixel
			stream.BitsPerPixel = BitsPerPixel.Bpp32;
			frameData = new byte[stream.Width * stream.Height * 4];
		}
		#endregion
		#region FORMEVENTS
		private void btnLoad_Click(object sender, EventArgs e)
		{
			res = axCVimage1.LoadImageByDialog();
			InitializeDriver("");
		}

		private void btnSnap_Click(object sender, EventArgs e)
		{
			axCVimage1.Snap();
			axCVimage1_ImageSnaped(null, null);
		}

		private void chkLive_CheckedChanged(object sender, EventArgs e)
		{
			axCVimage1.Grab = chkLive.Checked;
		}
		#endregion
		private void axCVimage1_ImageSnaped(object sender, EventArgs e)
		{
			axCVdisplay1.Refresh();
			if (chkSaveAvi.Checked)
			{
				// fill frame with image data BGR-32 topdown
				FillFrameData(axCVimage1.Image);
				// write data to a frame
				stream.WriteFrame(true, frameData, 0, frameData.Length);
				//stream.Index += 1;
			}
		}

		private unsafe void FillFrameData(Cvb.Image.IMG img)
		{
			var dimension = Cvb.Image.ImageDimension(img);
			IntPtr baseAddress;
			int xInc, yInc;
			var res = Cvb.Utilities.GetLinearAccess(img, 0, out baseAddress, out xInc, out yInc);
			var i = 0;
			for (int y = _Height - 1; y > 0; y--) // Top-Down
			{
				for (int x = 0; x < _Width; x++)
				{
					var pixel = *(byte*)((int)baseAddress + x * xInc + y * yInc);
					frameData[i++] = pixel; // B
					frameData[i++] = pixel; // G
					frameData[i++] = pixel; // R
					frameData[i++] = 255;
				}
			}
		}

		private void chkSaveAvi_CheckedChanged(object sender, EventArgs e)
		{
			if (chkSaveAvi.Checked) InitializeAVI(); else writer.Close();
		}
	}
}
