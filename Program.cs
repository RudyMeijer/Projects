/*
 * Created by SharpDevelop.
 * User: Rudy
 * Date: 25-3-2016
 * Time: 22:21
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using static System.Console;

namespace NeuralNetwork
{
	/// <summary>
	/// Class with program entry point.
	/// </summary>
	internal sealed class Program
	{
		/// <summary>
		/// Program entry point.
		/// </summary>
		[STAThread]
		private static void Main(string[] args)
		{
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);
			//.TestEventHandler();
			TestCodingInterview();
			Application.Run(new MainForm());
		}

		private static void TestCodingInterview()
		{
			int[] data = { 1, 2, 3 };
			int i = 1;
			data[i++] = data[i] + 10;
			WriteLine(String.Join("+", data) + "=" + data.Sum());
		}

		private static void TestEventHandler()
		{
			var f = new Form();
			var b = new Button();

			f.Controls.Add(b);
			b.Text = "Click me";
			b.Click += B_Click;
			InsertEventHandler(b, B_Click1);

			var p = new PropertyGrid();
			p.SelectedObject = new Button();
			f.Controls.Add(p);
			p.PropertyValueChanged += P_PropertyValueChanged;
			InsertPropEventHandler(p, P_PropertyValueChanged1);
			Application.Run(f);
		}

		private static bool InsertPropEventHandler(PropertyGrid control, Action<object, PropertyValueChangedEventArgs> p_PropertyValueChanged1)
		{
			var bf = BindingFlags.NonPublic | BindingFlags.Instance;
			var events = (EventHandlerList)typeof(Component)
				 .GetProperty("Events", bf)
				 .GetValue(control, null);


			var head = typeof(EventHandlerList)
				.GetField("head", bf)
				.GetValue(events);
			var handler = (PropertyValueChangedEventHandler)(head.GetType()
				.GetField("handler", bf)
				.GetValue(head));
			//
			// Insert handler @ top of invocation list.
			//
			if (handler.GetInvocationList().Count() == 1)
			{
				control.PropertyValueChanged -= handler;
				control.PropertyValueChanged += new PropertyValueChangedEventHandler(p_PropertyValueChanged1);
				control.PropertyValueChanged += handler;
			}
			return handler.GetInvocationList().Count() == 2;
		}

		private static void P_PropertyValueChanged(object s, PropertyValueChangedEventArgs e)
		{
			Console.WriteLine($"Property {e.ChangedItem.Label} changed from {e.OldValue} to {e.ChangedItem.Value}.");
		}

		private static void P_PropertyValueChanged1(object s, PropertyValueChangedEventArgs e)
		{
			Console.WriteLine($"Property1 {e.ChangedItem.Label} changed from {e.OldValue} to {e.ChangedItem.Value}.");
		}


		private static bool InsertEventHandler(Control control, Action<object, EventArgs> b_Click1)
		{

			var events = (EventHandlerList)typeof(Component)
				 .GetProperty("Events", BindingFlags.NonPublic | BindingFlags.Instance)
				 .GetValue(control, null);

			var key = typeof(Control)
				.GetField("EventClick", BindingFlags.NonPublic | BindingFlags.Static)
				.GetValue(null);

			var handlers = (EventHandler)events[key];
			//
			// Insert handler.
			//
			if (events[key].GetInvocationList().Count() == 1)
			{
				control.Click -= handlers;
				control.Click += new EventHandler(b_Click1);
				control.Click += handlers;
			}
			return handlers != null && handlers.GetInvocationList().Any();
		}

		private static void B_Click(object sender, EventArgs e)
		{
			Console.WriteLine("B_Click");
		}
		private static void B_Click1(object sender, EventArgs e)
		{
			Console.WriteLine("B_Click1");
		}

	}
}
