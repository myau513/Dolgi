using System;
using System.Windows.Forms;
using DebtTracker.Presentation.Contracts;

namespace DebtTracker.WUI
{
    public partial class AddDebtForm : Form, IAddDebtView
    {
        public event Action LoadView;
        public event Action SaveRequested;
        public event Action CancelRequested;

        public string Subject
        {
            get => subjectTextBox.Text.Trim();
            set => subjectTextBox.Text = value ?? string.Empty;
        }

        public string Description
        {
            get => descriptionTextBox.Text.Trim();
            set => descriptionTextBox.Text = value ?? string.Empty;
        }

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

        public DateTime Deadline
        {
            get => deadlineDateTimePicker.Value;
            set => deadlineDateTimePicker.Value = value;
        }

        public AddDebtForm()
        {
            InitializeComponent();
            InitializeStatusComboBox();
            this.Load += AddDebtForm_Load;

        }

        private void AddDebtForm_Load(object sender, EventArgs e)
        {
            deadlineDateTimePicker.MinDate = DateTime.Today;
            LoadView?.Invoke();
        }

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

        public new void Show() => ShowDialog();
        public new void Close() => base.Close();

        public void ShowMessage(string message)
            => MessageBox.Show(message);

        public void ShowError(string message)
            => MessageBox.Show(message, "Ошибка",
                MessageBoxButtons.OK, MessageBoxIcon.Error);

        private void saveButton_Click(object sender, EventArgs e)
            => SaveRequested?.Invoke();

        private void cancelButton_Click(object sender, EventArgs e)
            => CancelRequested?.Invoke();

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
