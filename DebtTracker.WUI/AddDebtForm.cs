using DebtTracker._Shared;
using DebtTracker.BusinessLogic;
using DebtTracker.BusinessLogic.Exceptions;
using DebtTracker.Entities;
using System;
using System.Windows.Forms;

namespace DebtTracker.WUI
{
    public partial class AddDebtForm : Form, IAddDebtView
    {
        public event Action SaveRequested;
        public event Action CancelRequested;

        public string Subject => subjectTextBox.Text.Trim();
        public string Description => descriptionTextBox.Text.Trim();
        public string Status =>
            ((dynamic)statusComboBox.SelectedItem).Value.ToString();
        public DateTime Deadline => deadlineDateTimePicker.Value;

        public void ShowView() => ShowDialog();
        public AddDebtForm()
        {
            InitializeComponent();
            InitializeStatusComboBox();
        }

        private void InitializeStatusComboBox()
        {
            statusComboBox.Items.AddRange(new object[]
            {
            new { Text = "Не начат", Value = DebtStatus.NotStarted },
            new { Text = "В процессе", Value = DebtStatus.InProgress },
            new { Text = "Выполнен", Value = DebtStatus.Completed }
            });

            statusComboBox.DisplayMember = "Text";
            statusComboBox.ValueMember = "Value";
            statusComboBox.SelectedIndex = 0;
        }

        private void AddDebtForm_Load(object sender, EventArgs e)
            => deadlineDateTimePicker.MinDate = DateTime.Today;

        private void saveButton_Click(object sender, EventArgs e)
            => SaveRequested?.Invoke();

        private void cancelButton_Click(object sender, EventArgs e)
            => CancelRequested?.Invoke();

        public void ShowError(string message)
        {
            MessageBox.Show(message, "Ошибка",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        public void Close() => base.Close();


        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }
    }

}