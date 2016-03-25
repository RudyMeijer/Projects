/*
 * Created by SharpDevelop.
 * User: Rudy
 * Date: 25-3-2016
 * Time: 22:21
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace Neuron
{
	/// <summary>
	/// Description of MainForm.
	/// </summary>
	public partial class MainForm : Form
	{
		public MainForm()
		{
			//
			// The InitializeComponent() call is required for Windows Forms designer support.
			//
			InitializeComponent();
			
			//
			// TODO: Add constructor code after the InitializeComponent() call.
			//
		}
		void Button1Click(object sender, EventArgs e)
		{
			var X = new Vector(1,2);
			var W = new Vector(0.1,0.2,0.3);
			var n = new Neuron(X,W);
		}
	}
}
