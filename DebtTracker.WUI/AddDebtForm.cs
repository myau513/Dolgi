using System;
using System.Windows.Forms;
using DebtTracker.BusinessLogic;
using DebtTracker.BusinessLogic.Exceptions;
using DebtTracker.Entities;

namespace DebtTracker.WUI
{
    public partial class AddDebtForm : Form
    {
        private readonly IDebtService _debtService;

        public event EventHandler DebtAdded;

        public AddDebtForm(IDebtService debtService)
        {
            InitializeComponent();
            _debtService = debtService;
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
        {
            deadlineDateTimePicker.MinDate = DateTime.Today;
        }

        private void saveButton_Click(object sender, EventArgs e)
        {
            if (!ValidateInputs())
                return;

            try
            {
                var selectedStatus = (DebtStatus)((dynamic)statusComboBox.SelectedItem).Value;

                var newDebt = new Debt
                {
                    Subject = subjectTextBox.Text.Trim(),
                    Description = descriptionTextBox.Text.Trim(),
                    Status = selectedStatus,
                    Deadline = deadlineDateTimePicker.Value
                };

                _debtService.AddDebt(newDebt);

                DebtAdded?.Invoke(this, EventArgs.Empty);

                MessageBox.Show("Долг успешно добавлен!",
                    "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);

                this.Close();
            }
            catch (DebtValidationException ex)
            {
                MessageBox.Show($"Ошибка валидации: {ex.Message}\nПроверьте введенные данные и попробуйте снова.",
                    "Ошибка валидации", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (DebtOperationException ex)
            {
                MessageBox.Show($"Ошибка при добавлении долга: {ex.Message}",
                    "Ошибка операции", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Неизвестная ошибка: {ex.Message}\nТип ошибки: {ex.GetType().Name}",
                    "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private bool ValidateInputs()
        {
            if (string.IsNullOrWhiteSpace(subjectTextBox.Text))
            {
                MessageBox.Show("Название предмета не может быть пустым",
                    "Ошибка валидации", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                subjectTextBox.Focus();
                return false;
            }

            if (deadlineDateTimePicker.Value < DateTime.Today)
            {
                MessageBox.Show("Дедлайн не может быть в прошлом",
                    "Ошибка валидации", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                deadlineDateTimePicker.Focus();
                return false;
            }

            return true;
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}