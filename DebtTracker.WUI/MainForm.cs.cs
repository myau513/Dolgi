using DebtTracker._Shared;
using DebtTracker.BusinessLogic;
using DebtTracker.Entities;
using Presenter;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Windows.Forms;

namespace DebtTracker.WUI
{
    public partial class MainForm : Form, IMainDebtView
    {
        public event Action LoadView;
        public event Action AddRequested;
        public event Action EditRequested;
        public event Action DeleteRequested;
        public event Action RefreshRequested;

        public int SelectedDebtId =>
            debtsDataGridView.SelectedRows.Count == 0
                ? -1
                : (int)debtsDataGridView.SelectedRows[0].Cells["Id"].Value;

        public MainForm()
        {
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
            => LoadView?.Invoke();

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
                }).ToList();
        }

        public void ShowTomorrowWarning(IEnumerable<DebtDto> debts)
        {
            if (!debts.Any()) return;

            var text = "=== Завтра дедлайн ===\n\n" +
                       string.Join("\n", debts.Select(d =>
                           $"{d.Subject} ({d.Deadline:yyyy-MM-dd})"));

            MessageBox.Show(text, "Внимание",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }


        public void ShowMessage(string message)
        => MessageBox.Show(message);

        public void ShowError(string message)
            => MessageBox.Show(message, "Ошибка",
                MessageBoxButtons.OK, MessageBoxIcon.Error);

        private void addButton_Click(object sender, EventArgs e)
            => AddRequested?.Invoke();

        private void editButton_Click(object sender, EventArgs e)
            => EditRequested?.Invoke();

        private void deleteButton_Click(object sender, EventArgs e)
            => DeleteRequested?.Invoke();

        private void refreshButton_Click(object sender, EventArgs e)
            => RefreshRequested?.Invoke();
    }

}