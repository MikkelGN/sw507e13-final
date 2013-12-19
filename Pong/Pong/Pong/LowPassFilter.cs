using System;

namespace StepAnalyzer
{
	public static class LowPassFilter
	{
		public static float LowPassFiterReturn (float input, float output)
		{
			float alpha = 0.3F;
			//On = On-1 + α(In – On-1);
			return output + alpha*(input - output);
		}
	}
}