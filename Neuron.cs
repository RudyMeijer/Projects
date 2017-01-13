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
		Vector Inputs, WeightFactors;
		public Neuron(Vector Inputs, Vector WeightFactors)
		{
			this.Inputs = Inputs;
			this.WeightFactors = WeightFactors;
		}
		public double Execute()
		{
			return Sigmoid(Inputs * WeightFactors);
		}

		double Sigmoid(double z)
		{
			return 1 / (1 + Math.Exp(-z));
		}
	}
}
