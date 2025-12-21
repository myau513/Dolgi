using DebtTracker.BusinessLogic;
using DebtTracker.DataAccessLayer;
using DebtTracker.Presenter;
using Ninject;
using Presenter;
using System;
using System.Data.Entity;
using System.Windows.Forms;

namespace DebtTracker.WUI
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            var kernel = new StandardKernel(new SimpleConfigModule());

            var model = kernel.Get<IDebtModel>();
            var view = new MainForm();
            var factory = new WinFormsViewFactory();

            new MainDebtPresenter(view, model, factory);

            Application.Run(view);
        }

    }
}