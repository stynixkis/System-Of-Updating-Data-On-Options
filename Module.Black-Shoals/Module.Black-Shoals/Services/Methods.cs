namespace Module.Black_Shoals.Service
{
    public static class Methods
    {
        /// <summary>
        /// Кумулятивная функция распределения стандартного нормального закона
        /// Вычисляет вероятность P(Z ≤ value) для Z ~ N(0,1)
        /// Использует аппроксимацию Харта с точностью 1.5×10⁻⁷
        /// </summary>
        /// <param name="value">z-score стандартного нормального распределения</param>
        /// <returns></returns>
        public static double StandardNormalCDF(double value)
        {
            double coefficient1 = 0.254829592;
            double coefficient2 = -0.284496736;
            double coefficient3 = 1.421413741;
            double coefficient4 = -1.453152027;
            double coefficient5 = 1.061405429;
            double approximationConstant = 0.3275911;

            int sign = 1;
            if (value < 0)
                sign = -1;

         
            value = Math.Abs(value) / Math.Sqrt(2.0);

            double temp = 1.0 / (1.0 + approximationConstant * value);
            double errorFunction = 1.0 - (((((coefficient5 * temp + coefficient4) * temp) + coefficient3)
                * temp + coefficient2) * temp + coefficient1) * temp * Math.Exp(-value * value);

            return 0.5 * (1.0 + sign * errorFunction);
        }
        /// <summary>
        /// Производная кумулятивной функции распределения стандартного нормального закона 
        /// Вычисляет плотность вероятности стандартного нормального распределения в точке value
        /// </summary>
        /// <param name="value">Точка, в которой вычисляется плотность вероятности</param>
        /// <returns></returns>
        public static double StandardNormalCDFDerivative(double value)
        {
            double valueOne = 1 / (Math.Sqrt(2 * Math.PI));
            double valueTwo = Math.Exp(Math.Pow(-value, 2) / 2);
            return valueOne * valueTwo;
        }
    }
}
