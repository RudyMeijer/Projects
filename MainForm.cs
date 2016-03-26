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
using Lib;

namespace NeuralNetwork
{
	/// <summary>
	/// Description of MainForm.
	/// </summary>
	public partial class MainForm : Form
	{
		public MainForm()
		{
			InitializeComponent();
		}
		void Button1Click(object sender, EventArgs e)
		{
			var X = new Vector(1, 1, 1);
			var W = new Vector(3,-2,-2);
			var N = new Neuron(X,W);
			var Y = N.Execute();
			label1.Text = Y.ToString();
		}
	}
}
