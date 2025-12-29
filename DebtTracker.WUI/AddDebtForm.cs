using System;
using System.Windows.Forms;
using DebtTracker.Presentation.Contracts;

namespace DebtTracker.WUI
{
    /// <summary>
    /// Окно добавления нового долга.
    /// View в архитектуре MVP:
    /// отображает элементы UI,
    /// собирает ввод пользователя,
    /// поднимает события для Presenter'а,
    /// но не содержит бизнес-логики.
    /// </summary>
    public partial class AddDebtForm : Form, IAddDebtView
    {
        /// <summary>
        /// Событие: форма загрузилась (Presenter может подготовить данные).
        /// </summary>
        public event Action LoadView;

        /// <summary>
        /// Событие: пользователь нажал кнопку "Сохранить".
        /// </summary>
        public event Action SaveRequested;

        /// <summary>
        /// Событие: пользователь нажал "Отмена".
        /// </summary>
        public event Action CancelRequested;

        /// <summary>
        /// Тема долга (привязана к полю Subject).
        /// </summary>
        public string Subject
        {
            get => subjectTextBox.Text.Trim();
            set => subjectTextBox.Text = value ?? string.Empty;
        }

        /// <summary>
        /// Описание долга.
        /// </summary>
        public string Description
        {
            get => descriptionTextBox.Text.Trim();
            set => descriptionTextBox.Text = value ?? string.Empty;
        }

        /// <summary>
        /// Код статуса (NotStarted / InProgress / Completed).
        /// Значение берётся из выбранного элемента ComboBox.
        /// </summary>
        public string Status
        {
            get => ((StatusItem)statusComboBox.SelectedItem).Value;
            set
            {
                foreach (StatusItem item in statusComboBox.Items)
                {
                    if (item.Value == value)
                    {
                        statusComboBox.SelectedItem = item;
                        break;
                    }
                }
            }
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
        /// Создаёт форму добавления долга и инициализирует элементы интерфейса.
        /// </summary>
        public AddDebtForm()
        {
            InitializeComponent();
            InitializeStatusComboBox();

            this.Load += AddDebtForm_Load;
        }

        /// <summary>
        /// При загрузке формы ограничиваем дату и уведомляем Presenter.
        /// </summary>
        private void AddDebtForm_Load(object sender, EventArgs e)
        {
            deadlineDateTimePicker.MinDate = DateTime.Today;
            LoadView?.Invoke();
        }

        /// <summary>
        /// Заполняет ComboBox статусами долга и задаёт значения по умолчанию.
        /// </summary>
        private void InitializeStatusComboBox()
        {
            statusComboBox.Items.AddRange(new object[]
            {
                new StatusItem("Не начат",   "NotStarted"),
                new StatusItem("В процессе","InProgress"),
                new StatusItem("Выполнен",  "Completed")
            });

            statusComboBox.DisplayMember = nameof(StatusItem.Text);
            statusComboBox.ValueMember = nameof(StatusItem.Value);
            statusComboBox.SelectedIndex = 0;
        }

        /// <summary>
        /// Открывает форму как модальное окно.
        /// </summary>
        public new void Show() => ShowDialog();

        /// <summary>
        /// Закрывает форму.
        /// </summary>
        public new void Close() => base.Close();

        /// <summary>
        /// Показывает информационное сообщение.
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
        /// Нажатие кнопки "Сохранить".
        /// </summary>
        private void saveButton_Click(object sender, EventArgs e)
            => SaveRequested?.Invoke();

        /// <summary>
        /// Нажатие кнопки "Отмена".
        /// </summary>
        private void cancelButton_Click(object sender, EventArgs e)
            => CancelRequested?.Invoke();

        /// <summary>
        /// Элемент статуса для ComboBox:
        /// Text — что видит пользователь,
        /// Value — служебное значение для модели.
        /// </summary>
        private class StatusItem
        {
            public string Text { get; }
            public string Value { get; }

            public StatusItem(string text, string value)
            {
                Text = text;
                Value = value;
            }

            public override string ToString() => Text;
        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {
        }
    }
}
