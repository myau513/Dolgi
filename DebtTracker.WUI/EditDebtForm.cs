using System;
using System.Windows.Forms;

namespace DebtTracker.WUI
{
    public partial class EditDebtForm : Form
    {
        private DebtTracker.BusinessLogic.IDebtService _debtService;
        private DebtTracker.Entities.Debt _debt;

        public event EventHandler DebtUpdated;

        public EditDebtForm()
        {
            InitializeComponent();
        }

        public EditDebtForm(DebtTracker.BusinessLogic.IDebtService debtService, DebtTracker.Entities.Debt debt) : this()
        {
            _debtService = debtService;
            _debt = debt;
            InitializeStatusComboBox();
            LoadDebtData();
        }

        private void InitializeStatusComboBox()
        {
            statusComboBox.Items.Clear();
            statusComboBox.Items.Add("Не начат");
            statusComboBox.Items.Add("В процессе");
            statusComboBox.Items.Add("Выполнен");
        }

        private void LoadDebtData()
        {
            if (_debt == null) return;

            subjectTextBox.Text = _debt.Subject;
            descriptionTextBox.Text = _debt.Description;
            deadlineDateTimePicker.Value = _debt.Deadline;

            int statusIndex = (int)_debt.Status;
            if (statusIndex >= 0 && statusIndex < statusComboBox.Items.Count)
            {
                statusComboBox.SelectedIndex = statusIndex;
            }
        }

        private void saveButton_Click(object sender, EventArgs e)
        {
            if (_debtService == null || _debt == null) return;

            if (!ValidateInputs())
                return;

            try
            {
                _debt.Subject = subjectTextBox.Text.Trim();
                _debt.Description = descriptionTextBox.Text.Trim();
                _debt.Status = (DebtTracker.Entities.DebtStatus)statusComboBox.SelectedIndex;
                _debt.Deadline = deadlineDateTimePicker.Value;

                _debtService.UpdateDebt(_debt);
                DebtUpdated?.Invoke(this, EventArgs.Empty);

                MessageBox.Show("Долг успешно обновлен!", "Успех",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private bool ValidateInputs()
        {
            if (string.IsNullOrWhiteSpace(subjectTextBox.Text))
            {
                MessageBox.Show("Название предмета не может быть пустым",
                    "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                subjectTextBox.Focus();
                return false;
            }

            return true;
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void EditDebtForm_Load(object sender, EventArgs e)
        {
            deadlineDateTimePicker.MinDate = DateTime.Today;
        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}