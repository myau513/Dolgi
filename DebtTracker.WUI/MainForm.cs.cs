using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using DebtTracker.BusinessLogic;
using DebtTracker.Entities;

namespace DebtTracker.WUI
{
    public partial class MainForm : Form
    {
        private readonly IDebtService _debtService;
        private List<Debt> _debts;

        public MainForm(IDebtService debtService)
        {
            InitializeComponent();
            _debtService = debtService;
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            LoadTomorrowDeadlines();
            LoadAllDebts();
        }

        private void LoadTomorrowDeadlines()
        {
            try
            {
                var tomorrowDebts = _debtService.GetDebtsWithTomorrowDeadline();
                if (tomorrowDebts.Any())
                {
                    var warningText = "=== ВНИМАНИЕ: Завтра дедлайн! ===\n\n";
                    foreach (var debt in tomorrowDebts)
                    {
                        warningText += $"• {debt.Subject} - {debt.Description} (Дедлайн: {debt.Deadline:yyyy-MM-dd})\n";
                    }

                    MessageBox.Show(warningText, "Предупреждение",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при загрузке завтрашних дедлайнов: {ex.Message}",
                    "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadAllDebts()
        {
            try
            {
                _debts = _debtService.GetAllDebtsSorted();
                debtsDataGridView.DataSource = null;
                debtsDataGridView.DataSource = _debts.Select(d => new
                {
                    d.Id,
                    Предмет = d.Subject,
                    Описание = d.Description,
                    Дедлайн = d.Deadline.ToString("yyyy-MM-dd"),
                    Статус = GetStatusText(d.Status)
                }).ToList();

                ConfigureDataGridView();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при загрузке долгов: {ex.Message}",
                    "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ConfigureDataGridView()
        {
            debtsDataGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            debtsDataGridView.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            debtsDataGridView.ReadOnly = true;
            debtsDataGridView.AllowUserToAddRows = false;
            debtsDataGridView.RowHeadersVisible = false;

            if (debtsDataGridView.Columns.Count > 0)
            {
                debtsDataGridView.Columns[0].Visible = false; 
                debtsDataGridView.Columns["Статус"].DefaultCellStyle.Alignment =
                    DataGridViewContentAlignment.MiddleCenter;
            }
        }

        private string GetStatusText(DebtStatus status)
        {
            switch (status)
            {
                case DebtStatus.NotStarted:
                    return "Не начат";
                case DebtStatus.InProgress:
                    return "В процессе";
                case DebtStatus.Completed:
                    return "Выполнен";
                default:
                    return "Неизвестно";
            }
        }

        private void addButton_Click(object sender, EventArgs e)
        {
            var addForm = new AddDebtForm(_debtService);
            addForm.DebtAdded += (s, args) =>
            {
                LoadAllDebts();
            };
            addForm.ShowDialog();
        }

        private void editButton_Click(object sender, EventArgs e)
        {
            if (debtsDataGridView.SelectedRows.Count == 0)
            {
                MessageBox.Show("Пожалуйста, выберите долг для редактирования",
                    "Информация", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var selectedRow = debtsDataGridView.SelectedRows[0];
            var debtId = (int)selectedRow.Cells["Id"].Value; 

            try
            {
                var debt = _debtService.GetDebtById(debtId);
                var editForm = new EditDebtForm(_debtService, debt);
                editForm.DebtUpdated += (s, args) =>
                {
                    LoadAllDebts(); 
                };
                editForm.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при получении данных: {ex.Message}",
                    "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void deleteButton_Click(object sender, EventArgs e)
        {
            if (debtsDataGridView.SelectedRows.Count == 0)
            {
                MessageBox.Show("Пожалуйста, выберите долг для удаления",
                    "Информация", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var selectedRow = debtsDataGridView.SelectedRows[0];
            var debtId = (int)selectedRow.Cells[0].Value;
            var subject = selectedRow.Cells[1].Value.ToString();

            var result = MessageBox.Show($"Вы уверены, что хотите удалить долг '{subject}'?",
                "Подтверждение удаления",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                try
                {
                    _debtService.DeleteDebt(debtId);
                    LoadAllDebts();
                    MessageBox.Show("Долг успешно удален",
                        "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка при удалении: {ex.Message}",
                        "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void refreshButton_Click(object sender, EventArgs e)
        {
            LoadAllDebts();
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            var result = MessageBox.Show("Вы уверены, что хотите выйти?",
                "Подтверждение выхода",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.No)
            {
                e.Cancel = true;
            }
        }


        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}