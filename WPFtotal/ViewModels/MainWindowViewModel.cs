using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using WPFtotal.Models;
using WPFtotal.Commands;

namespace WPFtotal.ViewModels
{
    // Функционал класса ViewModel

    class MainWindowViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string PropertyName = null) //Связь  с событием объектов View Model - вызов делегата
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(PropertyName)); //Активация события с проверкой на null
        }

        #region Делегаты комманд, поля с переменными

        private readonly Resultcs calculation;
        private RelayCommand<string> digitButtonCommand;
        private RelayCommand<string> operationButtonCommand;
        private RelayCommand<string> singlOperationButton;

        private bool newDisplayRequired = false; //флаг

        #endregion

        #region Конструктор с начальными параметрами

        public MainWindowViewModel()
        {
            this.calculation = new Resultcs();
            this.display = "0";
            this.FirstOperand = string.Empty;
            this.SecondOperand = string.Empty;
            this.Operation = string.Empty;
            this.lastOperation = string.Empty;
        }

        #endregion

        #region Свойсива взаимосвязи с объектом

        public string FirstOperand
        {
            get => calculation.FirstOperand;
            set
            {
                calculation.FirstOperand = value;
                OnPropertyChanged();
            }
        }

        public string SecondOperand
        {
            get => calculation.SecondOperand;
            set
            {
                calculation.SecondOperand = value;
                OnPropertyChanged();
            }
        }

        public string Operation
        {
            get => calculation.Operation;
            set
            {
                calculation.Operation = value;
                OnPropertyChanged();
            }
        }

        private string lastOperation;
        public string LastOperation
        {
            get => lastOperation;
            set
            {
                lastOperation = value;
                OnPropertyChanged();
            }
        }

        public string Result
        {
            get => calculation.Result;
        }

        private string display;
        public string Display
        {
            get => display;
            set
            {
                display = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #region Комманды

        // Свойства get set для команды OperationButtonPressCommand (для клавиш с операциями + - и т.д)
        public ICommand OperationButtonPressCommand
        {
            get
            {
                if (operationButtonCommand == null)
                {
                    operationButtonCommand = new RelayCommand<string>(
                        OperationButtonPress, CanOperationButtonPress);
                }
                return operationButtonCommand;
            }
        }

        // Свойства get set для команды CanOperationButtonPressCommand (для клавиш с операциями + - и т.д)
        private static bool CanOperationButtonPress(string number)
        {
            return true;
        }

        // Свойства get set для команды SingularOperationButtonPressCommand (одинарные операции для квадрата куба т.д)
        public ICommand SingularOperationButtonPressCommand
        {
            get
            {
                if (singlOperationButton == null)
                {
                    singlOperationButton = new RelayCommand<string>(
                         SingularOperationButtonPress, CanSingularOperationButtonPress);
                }
                return singlOperationButton;
            }
        }

        // Свойства get set для команды CanSingularOperationButtonPressCommand (одинарные операции для квадрата куба т.д)
        private static bool CanSingularOperationButtonPress(string number)
        {
            return true;
        }

        // Свойства get set для команды DigitButtonPressCommand (цифровые клавиши)
        public ICommand DigitButtonPressCommand
        {
            get
            {
                if (digitButtonCommand == null)
                {
                    digitButtonCommand = new RelayCommand<string>(
                        DigitButtonPress, CanDigitButtonPress);
                }
                return digitButtonCommand;
            }
        }

        // Свойства get set для команды СanDigitButtonPressCommand (цифровые клавиши)
        private static bool CanDigitButtonPress(string button)
        {
            return true;
        }


        // Соответствие кнопки отображению на дисплее
        public void DigitButtonPress(string button)
        {
            switch (button)
            {
                case "C": // При нажатии кнопки С
                    Display = "0";
                    FirstOperand = string.Empty; // Пустые строки
                    SecondOperand = string.Empty; // Пустые строки
                    Operation = string.Empty; // Пустые строки
                    LastOperation = string.Empty; // Пустые строки
                    break;
                case "Del": // При нажатии кнопки стрелка забой
                    if (display.Length > 1) // определяем длину символов и при соотв. условию после нажатия 
                        Display = display.Substring(0, display.Length - 1); // удаляем 1 символ
                    else Display = "0"; // если символ только 1, то отобразаем после нажатия 0
                    break;
                case "+/-": // замена + на -
                    if (display.Contains("-") || display == "0")
                    {
                        Display = display.Remove(display.IndexOf("-"), 1);
                    }
                    else Display = "-" + display;
                    break;
                case ".": // отображения цифр при наборе, в т.ч. десетичных
                    if (newDisplayRequired)
                    {
                        Display = "0.";
                    }
                    else
                    {
                        if (!display.Contains("."))
                        {
                            Display = display + ".";
                        }
                    }
                    break;
                default: // отображение цифровых кнопок и выражений при множеств. нажатии
                    if (display == "0" || newDisplayRequired)
                        Display = button;
                    else
                        Display = display + button;
                    break;
            }
            newDisplayRequired = false;
        }

        // Операции с 2 орерандами (+-*/)
        public void OperationButtonPress(string operation)
        {
            try
            {
                if (FirstOperand == string.Empty || LastOperation == "=")
                {
                    FirstOperand = display;
                    LastOperation = operation;
                }
                else
                {
                    SecondOperand = display;
                    Operation = lastOperation;
                    calculation.CalculateResult();
                    LastOperation = operation;
                    Display = Result;
                    FirstOperand = display;
                }
                newDisplayRequired = true;
            }
            catch (Exception)
            {
                Display = Result == string.Empty ? "Ошибка!" : Result;
            }
        }

        //Операции с 1 операндом (квпдрат, куб, корень, 1/х)
        public void SingularOperationButtonPress(string operation)
        {
            try
            {
                FirstOperand = Display;
                Operation = operation;
                calculation.CalculateResult();
                LastOperation = "=";
                Display = Result;
                FirstOperand = display;
                newDisplayRequired = true;
            }
            catch (Exception)
            {
                Display = Result == string.Empty ? "Ошибка!" : Result;
            }
        }

        #endregion
    }
}