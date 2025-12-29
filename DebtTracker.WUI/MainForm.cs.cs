using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using DebtTracker.Dto;
using DebtTracker.Presentation.Contracts;

namespace DebtTracker.WUI
{
    /// <summary>
    /// Главная форма WinForms-приложения.
    /// Является View в архитектуре MVP: показывает список долгов,
    /// поднимает события действий пользователя и не содержит бизнес-логики.
    /// </summary>
    public partial class MainForm : Form, IMainDebtView
    {
        /// <summary>
        /// Событие: форма загрузилась и готова к работе.
        /// Используется Presenter'ом для начальной загрузки данных.
        /// </summary>
        public event Action LoadView;

        /// <summary>
        /// Событие: пользователь нажал кнопку "Добавить".
        /// </summary>
        public event Action AddRequested;

        /// <summary>
        /// Событие: пользователь нажал кнопку "Редактировать".
        /// </summary>
        public event Action EditRequested;

        /// <summary>
        /// Событие: пользователь нажал кнопку "Удалить".
        /// </summary>
        public event Action DeleteRequested;

        /// <summary>
        /// Событие: пользователь нажал кнопку "Обновить".
        /// </summary>
        public event Action RefreshRequested;

        /// <summary>
        /// Событие: пользователь запросил долги на завтра.
        /// </summary>
        public event Action TomorrowDebtsRequested;

        /// <summary>
        /// Id выбранного в таблице долга.
        /// Возвращает -1, если ни одна строка не выбрана.
        /// </summary>
        public int SelectedDebtId =>
            debtsDataGridView.SelectedRows.Count == 0
                ? -1
                : (int)debtsDataGridView.SelectedRows[0].Cells["Id"].Value;

        /// <summary>
        /// Инициализирует главную форму и настраивает обработчики кнопок.
        /// </summary>
        public MainForm()
        {
            InitializeComponent();

            addButton.Click += addButton_Click;
            editButton.Click += editButton_Click;
            deleteButton.Click += deleteButton_Click;
            refreshButton.Click += refreshButton_Click;
            tomorrowButton.Click += tomorrowButton_Click;
        }

        /// <summary>
        /// Обработка события загрузки формы.
        /// Поднимает событие LoadView для Presenter'а.
        /// </summary>
        private void MainForm_Load(object sender, EventArgs e)
            => LoadView?.Invoke();

        /// <summary>
        /// Отображает список долгов в таблице DataGridView.
        /// </summary>
        /// <param name="debts">Коллекция DTO с данными по долгам.</param>
        public void ShowDebts(IEnumerable<DebtDto> debts)
        {
            debtsDataGridView.DataSource = debts
                .Select(d => new
                {
                    d.Id,
                    Предмет = d.Subject,
                    Описание = d.Description,
                    Дедлайн = d.Deadline.ToString("yyyy-MM-dd"),
                    Статус = d.Status
                })
                .ToList();
        }

        /// <summary>
        /// Показывает предупреждение о долгах с дедлайном на завтра.
        /// Если долгов нет, ничего не показывает.
        /// </summary>
        /// <param name="debts">Коллекция долгов с завтрашним дедлайном.</param>
        public void ShowTomorrowWarning(IEnumerable<DebtDto> debts)
        {
            if (!debts.Any()) return;

            var text = "=== Завтра дедлайн ===\n\n" +
                       string.Join("\n", debts.Select(d =>
                           $"{d.Subject} ({d.Deadline:yyyy-MM-dd})"));

            MessageBox.Show(text, "Внимание",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        /// <summary>
        /// Показывает обычное информационное сообщение.
        /// </summary>
        /// <param name="message">Текст сообщения.</param>
        public void ShowMessage(string message)
            => MessageBox.Show(message);

        /// <summary>
        /// Показывает сообщение об ошибке в стандартном MessageBox.
        /// </summary>
        /// <param name="message">Текст ошибки.</param>
        public void ShowError(string message)
            => MessageBox.Show(message, "Ошибка",
                MessageBoxButtons.OK, MessageBoxIcon.Error);

        /// <summary>
        /// Обработчик кнопки "Добавить".
        /// Поднимает событие AddRequested.
        /// </summary>
        private void addButton_Click(object sender, EventArgs e)
            => AddRequested?.Invoke();

        /// <summary>
        /// Обработчик кнопки "Редактировать".
        /// Поднимает событие EditRequested.
        /// </summary>
        private void editButton_Click(object sender, EventArgs e)
            => EditRequested?.Invoke();

        /// <summary>
        /// Обработчик кнопки "Удалить".
        /// Поднимает событие DeleteRequested.
        /// </summary>
        private void deleteButton_Click(object sender, EventArgs e)
            => DeleteRequested?.Invoke();

        /// <summary>
        /// Обработчик кнопки "Обновить".
        /// Поднимает событие RefreshRequested.
        /// </summary>
        private void refreshButton_Click(object sender, EventArgs e)
            => RefreshRequested?.Invoke();

        /// <summary>
        /// Обработчик кнопки "Долги на завтра".
        /// Поднимает событие TomorrowDebtsRequested.
        /// </summary>
        private void tomorrowButton_Click(object sender, EventArgs e)
            => TomorrowDebtsRequested?.Invoke();

        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            // В данный момент не используется.
        }
    }
}
