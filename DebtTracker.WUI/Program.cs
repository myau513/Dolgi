using DebtTracker.BusinessLogic;
using DebtTracker.DataAccessLayer;
using Ninject;
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
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            try
            {
                // Создаем DI контейнер
                IKernel ninjectKernel = new StandardKernel(new SimpleConfigModule());

                // Инициализируем базу данных
                InitializeDatabase(ninjectKernel);

                // Получаем сервис
                var debtService = ninjectKernel.Get<IDebtService>();

                // Запускаем главную форму
                Application.Run(new MainForm(debtService));
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при запуске приложения: {ex.Message}\n\n" +
                    $"Тип ошибки: {ex.GetType().Name}\n" +
                    $"Детали: {ex.InnerException?.Message}",
                    "Ошибка запуска", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        static void InitializeDatabase(IKernel kernel)
        {
            try
            {
                using (var context = kernel.Get<DebtContext>())
                {
                    // Проверяем и создаем БД если нужно
                    if (!context.Database.Exists())
                    {
                        MessageBox.Show("База данных не существует. Создаем...",
                            "Информация", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        context.Database.Create();
                        MessageBox.Show("База данных успешно создана!",
                            "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        // Проверяем, что таблицы существуют
                        var canConnect = context.Database.CompatibleWithModel(false);
                        if (!canConnect)
                        {
                            MessageBox.Show("Схема базы данных устарела. Обновляем...",
                                "Информация", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Не удалось инициализировать базу данных: {ex.Message}");
            }
        }
    }
}