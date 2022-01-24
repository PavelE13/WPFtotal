using System;
using System.Windows.Input;

namespace WPFtotal.Commands
{
    // Используем Параметризированный класс типа Icommand
    public class RelayCommand<T> : ICommand
    {
        #region Конструкторы
        // Для команды с аргументами executeMethod
        public RelayCommand(Action<T> executeMethod)
            : this(executeMethod, null, false)
        {
        }

        // Для команды с аргументами executeMethod, canExecuteMethod, Func
        public RelayCommand(Action<T> executeMethod, Func<T, bool> canExecuteMethod)
            : this(executeMethod, canExecuteMethod, false)
        {
        }

        // Для команды с аргументами executeMethod, canExecuteMethod, isAutomaticRequeryDisabled
        public RelayCommand(Action<T> executeMethod, Func<T, bool> canExecuteMethod, bool isAutomaticRequeryDisabled)
        {
            if (executeMethod == null)
            {
                throw new ArgumentNullException("executeMethod");
            }

            _executeMethod = executeMethod;
            _canExecuteMethod = canExecuteMethod;
            _isAutomaticRequeryDisabled = isAutomaticRequeryDisabled;
        }

        #endregion

        #region Методы

        // Метод, определяющий может ли команда быть выполнена
        public bool CanExecute(T parameter)
        {
            if (_canExecuteMethod != null)
            {
                return _canExecuteMethod(parameter);
            }
            return true;
        }

        ///    Метод выполнения команды
        public void Execute(T parameter)
        {
            if (_executeMethod != null)
            {
                _executeMethod(parameter);
            }
        }

        #endregion

        #region ICommand

        // Реализация ICommand.CanExecuteChanged
        public event EventHandler CanExecuteChanged
        {
            add
            {
                if (!_isAutomaticRequeryDisabled)
                {
                    CommandManager.RequerySuggested += value;
                }
            }
            remove
            {
                if (!_isAutomaticRequeryDisabled)
                {
                    CommandManager.RequerySuggested -= value;
                }
             }
        }

        // Условие: если тип параметра Т или значение его еще не установлено, вернуть CanExecute false, иначе true 
        bool ICommand.CanExecute(object parameter)
        {
            if (parameter == null &&
                typeof(T).IsValueType)
            {
                return (_canExecuteMethod == null);
            }
            return CanExecute((T)parameter);
        }

        void ICommand.Execute(object parameter)
        {
            Execute((T)parameter);
        }

        #endregion

        #region Начальные значения

        private readonly Action<T> _executeMethod = null;
        private readonly Func<T, bool> _canExecuteMethod = null;
        private bool _isAutomaticRequeryDisabled = false;
        #endregion
    }
}