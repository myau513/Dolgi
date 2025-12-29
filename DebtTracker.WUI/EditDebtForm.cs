using System;
using System.Windows.Forms;
using DebtTracker.Dto;
using DebtTracker.Presentation.Contracts;

namespace DebtTracker.WUI
{
    /// <summary>
    /// Окно редактирования существующего долга.
    /// Реализует интерфейс IEditDebtView и выступает как View в архитектуре MVP:
    /// отображает данные и поднимает события, но не содержит бизнес-логики.
    /// </summary>
    public partial class EditDebtForm : Form, IEditDebtView
    {
        /// <summary>
        /// Событие: форма загрузилась.
        /// Presenter использует его, чтобы загрузить данные долга из модели.
        /// </summary>
        public event Action LoadView;

        /// <summary>
        /// Событие: пользователь нажал кнопку "Сохранить".
        /// </summary>
        public event Action SaveRequested;

        /// <summary>
        /// Событие: пользователь нажал кнопку "Отмена".
        /// </summary>
        public event Action CancelRequested;

        /// <summary>
        /// Идентификатор редактируемого долга.
        /// Передаётся презентеру, чтобы он знал, какой объект обновлять.
        /// </summary>
        public int DebtId { get; }

        /// <summary>
        /// Тема долга.
        /// Привязана к текстовому полю Subject.
        /// </summary>
        public string Subject
        {
            get => subjectTextBox.Text.Trim();
            set => subjectTextBox.Text = value ?? string.Empty;
        }

        /// <summary>
        /// Описание долга.
        /// Привязано к текстовому полю Description.
        /// </summary>
        public string Description
        {
            get => descriptionTextBox.Text.Trim();
            set => descriptionTextBox.Text = value ?? string.Empty;
        }

        /// <summary>
        /// Статус долга (NotStarted / InProgress / Completed).
        /// Привязан к выпадающему списку.
        /// </summary>
        public string Status
        {
            get => statusComboBox.SelectedItem?.ToString();
            set => statusComboBox.SelectedItem = value;
        }

        /// <summary>
        /// Дата дедлайна.
        /// </summary>
        public DateTime Deadline
        {
            get => deadlineDateTimePicker.Value;
            set => deadlineDateTimePicker.Value = value;
        }

        /// <summary>
        /// Создаёт форму редактирования и инициализирует элементы управления.
        /// </summary>
        /// <param name="debtId">Id долга, который нужно отредактировать.</param>
        public EditDebtForm(int debtId)
        {
            InitializeComponent();
            DebtId = debtId;

            InitializeStatusComboBox();

            this.Load += EditDebtForm_Load;

            saveButton.Click += saveButton_Click;
            cancelButton.Click += cancelButton_Click;
        }

        /// <summary>
        /// При загрузке формы вызывает событие LoadView,
        /// чтобы Presenter получил данные из модели и заполнил поля.
        /// </summary>
        private void EditDebtForm_Load(object sender, EventArgs e)
        {
            LoadView?.Invoke();
        }

        /// <summary>
        /// Заполняет ComboBox возможными статусами и задаёт значение по умолчанию.
        /// </summary>
        private void InitializeStatusComboBox()
        {
            statusComboBox.Items.AddRange(new[]
            {
                "NotStarted",
                "InProgress",
                "Completed"
            });

            statusComboBox.SelectedIndex = 0;
        }

        /// <summary>
        /// Показывает обычное информационное сообщение.
        /// </summary>
        public void ShowMessage(string message)
            => MessageBox.Show(message);

        /// <summary>
        /// Показывает сообщение об ошибке.
        /// </summary>
        public void ShowError(string message)
            => MessageBox.Show(message, "Ошибка",
                MessageBoxButtons.OK, MessageBoxIcon.Error);

        /// <summary>
        /// Открывает форму как диалоговое окно.
        /// </summary>
        public new void Show() => ShowDialog();

        /// <summary>
        /// Закрывает форму.
        /// </summary>
        public new void Close() => base.Close();

        /// <summary>
        /// Нажатие "Сохранить" → поднимаем событие для Presenter'а.
        /// </summary>
        private void saveButton_Click(object sender, EventArgs e)
            => SaveRequested?.Invoke();

        /// <summary>
        /// Нажатие "Отмена" → сообщаем Presenter'у.
        /// </summary>
        private void cancelButton_Click(object sender, EventArgs e)
            => CancelRequested?.Invoke();

        private void panel2_Paint(object sender, PaintEventArgs e)
        {
        }
    }
}
