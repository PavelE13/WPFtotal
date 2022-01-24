using System;

namespace WPFtotal.Models
{
    public class Resultcs
    {
        #region Объявление result

        private string result;

        #endregion

        #region Конструкторы

        // Для 2-х операндов
        public Resultcs(string firstOperand, string secondOperand, string operation)
        {
            ValidateOperand(firstOperand);
            ValidateOperand(secondOperand);
            ValidateOperation(operation);

            FirstOperand = firstOperand;
            SecondOperand = secondOperand;
            Operation = operation;
            result = string.Empty;
        }

        // Для 1-го операнда
        public Resultcs(string firstOperand, string operation)
        {
            ValidateOperand(firstOperand);
            ValidateOperation(operation);

            FirstOperand = firstOperand;
            SecondOperand = string.Empty;
            Operation = operation;
            result = string.Empty;
        }

        // Для результата расчета =
        public Resultcs()
        {
            FirstOperand = string.Empty;
            SecondOperand = string.Empty;
            Operation = string.Empty;
            result = string.Empty;
        }

        #endregion

        #region Свойства

        public string FirstOperand { get; set; }
        public string SecondOperand { get; set; }
        public string Operation { get; set; }
        public string Result { get { return result; } }

        #endregion

        #region Методы

        public void CalculateResult() // Метод  вычисления при нажатии на разл. клавиш
        {
            ValidateData();

            try
            {
                switch (Operation)
                {
                    case ("+"):
                        result = (Convert.ToDouble(FirstOperand) + Convert.ToDouble(SecondOperand)).ToString();
                        break;

                    case ("-"):
                        result = (Convert.ToDouble(FirstOperand) - Convert.ToDouble(SecondOperand)).ToString();
                        break;

                    case ("*"):
                        result = (Convert.ToDouble(FirstOperand) * Convert.ToDouble(SecondOperand)).ToString();
                        break;

                    case ("/"):
                        result = (Convert.ToDouble(FirstOperand) / Convert.ToDouble(SecondOperand)).ToString();
                        break;

                    case ("bdiv"):
                        result = (1 / Convert.ToDouble(FirstOperand)).ToString();
                        break;

                    case ("cube"):
                        result = Math.Pow(Convert.ToDouble(FirstOperand), 3).ToString();
                        break;

                    case ("square"):
                        result = Math.Pow(Convert.ToDouble(FirstOperand), 2).ToString();
                        break;

                    case ("squareroot"):
                        result = Math.Sqrt(Convert.ToDouble(FirstOperand)).ToString();
                        break;
                    case ("GkaltokWh"):
                        result = (Convert.ToDouble(FirstOperand)*1163.000308195081671).ToString();
                        break;
                }
            }
            catch (Exception)
            {
                result = "Ошибка при расчете";
                throw;
            }
        }

        private void ValidateOperand(string operand) // Метод  проверки типа операндов и вывод ошибки
        {
            try
            {
                Convert.ToDouble(operand);
            }
            catch (Exception)
            {
                result = "Не верное число: " + operand;
                throw;
            }
        }

        private void ValidateOperation(string operation) // Метод  проверки вида операций и вывод ошибки
        {
            switch (operation)
            {
                case "/":
                case "*":
                case "-":
                case "+":
                case "bdiv":
                case "cube":
                case "square":
                case "squareroot":
                case "GkaltokWh":
                    break;
                default:
                    result = "Не известная операция: " + operation;
                    throw new ArgumentException("Не известная операция: " + operation, "операция");
            }
        }

        private void ValidateData()
        {
            switch (Operation)
            {
                case "/":
                case "*":
                case "-":
                case "+":
                    ValidateOperand(FirstOperand); // отсыл на проверку операнда 1 для операций / * - +
                    ValidateOperand(SecondOperand); // отсыл на проверку операнда 2 для операций / * - +
                    break;
                case "bdiv":
                case "cube":
                case "square":
                case "squareroot":
                case "GkaltokWh":
                    ValidateOperand(FirstOperand); // отсыл на проверку единственного операнда для операций квадрат, куб, корень 1/х, gkal в кВт
                    break;
                /*default:
                    result = "Не известная операция: " + Operation; // вызов ошибки для операндов
                    throw new ArgumentException("Не известная операция: " + Operation, "операция");*/
            }
        }
        
        #endregion
    }
}
