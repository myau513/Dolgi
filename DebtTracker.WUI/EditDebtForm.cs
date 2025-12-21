using DebtTracker._Shared;
using DebtTracker.Entities;
using System;
using System.Windows.Forms;

namespace DebtTracker.WUI
{
    public partial class EditDebtForm : Form, IEditDebtView
    {
        public event Action SaveRequested;
        public event Action CancelRequested;

        public int DebtId { get; }

        public string Subject => subjectTextBox.Text.Trim();
        public string Description => descriptionTextBox.Text.Trim();
        public string Status => statusComboBox.SelectedItem?.ToString();
        public DateTime Deadline => deadlineDateTimePicker.Value;

        public EditDebtForm(int debtId)
        {
            InitializeComponent();
            DebtId = debtId;
            InitializeStatusComboBox();
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
            subjectTextBox.Text = debt.Subject;
            descriptionTextBox.Text = debt.Description;
            statusComboBox.SelectedItem = debt.Status;
            deadlineDateTimePicker.Value = debt.Deadline;
        }

        public void ShowView() => ShowDialog();

        private void saveButton_Click(object sender, EventArgs e)
            => SaveRequested?.Invoke();

        private void cancelButton_Click(object sender, EventArgs e)
            => CancelRequested?.Invoke();

        public void ShowError(string message)
        {
            MessageBox.Show(message, "Ошибка",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}
