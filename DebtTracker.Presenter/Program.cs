using Ninject;
using System;
using System.Text;
using System.Windows.Forms;
using DebtTracker.BusinessLogic;
using DebtTracker.Dto;
using DebtTracker.Presentation.Contracts;
using DebtTracker.WUI;
using DebtTracker.ConsoleApp;

namespace DebtTracker.Presenter
{
    public class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;

            while (true)
            {
                Console.WriteLine("=== ВЫБОР ИНТЕРФЕЙСА ===");
                Console.WriteLine("1. Windows Forms");
                Console.WriteLine("2. Консоль");
                Console.WriteLine("0. Выход");
                Console.Write("Выберите команду\n");

                var key = Console.ReadKey(true).Key;

                switch (key)
                {
                    case ConsoleKey.D1:
                        RunWinForms();
                        break;

                    case ConsoleKey.D2:
                        Console.Clear();
                        RunConsole();
                        break;

                    case ConsoleKey.D0:
                        return;
                }
            }
        }

        private static void RunWinForms()
        {

            var kernel = new StandardKernel(new SimpleConfigModule());
            var model = kernel.Get<IDebtModel>();

            IMainDebtView view = new MainForm();
            IViewFactory factory = new WinFormsViewFactory();

            var presenter = new MainDebtPresenter(view, model, factory);

            presenter.Run();
        }

        private static void RunConsole()
        {
            var kernel = new StandardKernel(new SimpleConfigModule());
            var model = kernel.Get<IDebtModel>();

            IConsoleMainDebtView view = new ConsoleDebtView();
            IViewFactory factory = new ConsoleViewFactory(); 

            var presenter = new MainDebtPresenter(view, model, factory);

            presenter.Run(); 
        }
    }
}
