using System;
using System.Windows.Forms;
using DebtTracker.Dto;
using DebtTracker.Presentation.Contracts;

namespace DebtTracker.WUI
{
    public partial class EditDebtForm : Form, IEditDebtView
    {
        public event Action LoadView;
        public event Action SaveRequested;
        public event Action CancelRequested;

        public int DebtId { get; }

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
            get => statusComboBox.SelectedItem?.ToString();
            set => statusComboBox.SelectedItem = value;
        }

        public DateTime Deadline
        {
            get => deadlineDateTimePicker.Value;
            set => deadlineDateTimePicker.Value = value;
        }

        public EditDebtForm(int debtId)
        {
            InitializeComponent();
            DebtId = debtId;
            InitializeStatusComboBox();
            this.Load += EditDebtForm_Load;
        }

        private void EditDebtForm_Load(object sender, EventArgs e)
        {
            LoadView?.Invoke();
        }

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

        public void Fill(DebtDto debt)
        {
            Subject = debt.Subject;
            Description = debt.Description;
            Status = debt.Status;
            Deadline = debt.Deadline;
        }

        public new void Show() => ShowDialog();

        public new void Close() => base.Close();

        public void ShowMessage(string message)
            => MessageBox.Show(message);

        public void ShowError(string message)
        {
            MessageBox.Show(message, "Ошибка",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void saveButton_Click(object sender, EventArgs e)
            => SaveRequested?.Invoke();

        private void cancelButton_Click(object sender, EventArgs e)
            => CancelRequested?.Invoke();
    }
}
