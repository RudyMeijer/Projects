/*
 * Created by SharpDevelop.
 * User: Rudy
 * Date: 25-3-2016
 * Time: 23:20
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using Lib;

namespace NeuralNetwork
{
	/// <summary>
	/// Description of Neuron.
	/// </summary>
	public class Neuron
	{
		Vector X, W;
		public Neuron(Vector X, Vector W)
		{
			this.X = X;
			this.W = W;
		}
		public double Execute()
		{
			return Sigmoid(X * W);
		}

		double Sigmoid(double z)
		{
			return 1 / (1 + Math.Exp(-z));
		}
	}
}
