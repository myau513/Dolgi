namespace DebtTracker.WUI
{
    partial class MainForm
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.DataGridView debtsDataGridView;
        private System.Windows.Forms.Button addButton;
        private System.Windows.Forms.Button editButton;
        private System.Windows.Forms.Button deleteButton;
        private System.Windows.Forms.Button refreshButton;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label titleLabel;
        private System.Windows.Forms.Button tomorrowButton;


        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            debtsDataGridView = new DataGridView();
            panel1 = new Panel();
            titleLabel = new Label();
            addButton = new Button();
            editButton = new Button();
            deleteButton = new Button();
            refreshButton = new Button();
            tomorrowButton = new Button();
            ((System.ComponentModel.ISupportInitialize)debtsDataGridView).BeginInit();
            panel1.SuspendLayout();
            SuspendLayout();
            // 
            // debtsDataGridView
            // 
            debtsDataGridView.AllowUserToAddRows = false;
            debtsDataGridView.AllowUserToDeleteRows = false;
            debtsDataGridView.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            debtsDataGridView.Dock = DockStyle.Fill;
            debtsDataGridView.Location = new Point(0, 75);
            debtsDataGridView.Margin = new Padding(4);
            debtsDataGridView.Name = "debtsDataGridView";
            debtsDataGridView.ReadOnly = true;
            debtsDataGridView.RowHeadersWidth = 51;
            debtsDataGridView.Size = new Size(788, 394);
            debtsDataGridView.TabIndex = 0;
            // 
            // panel1
            // 
            panel1.Controls.Add(titleLabel);
            panel1.Controls.Add(addButton);
            panel1.Controls.Add(editButton);
            panel1.Controls.Add(deleteButton);
            panel1.Controls.Add(refreshButton);
            panel1.Dock = DockStyle.Top;
            panel1.Location = new Point(0, 0);
            panel1.Margin = new Padding(4);
            panel1.Name = "panel1";
            panel1.Size = new Size(788, 75);
            panel1.TabIndex = 1;
            panel1.Controls.Add(tomorrowButton);
            // 
            // titleLabel
            // 
            titleLabel.AutoSize = true;
            titleLabel.Font = new Font("Microsoft Sans Serif", 14F, FontStyle.Bold);
            titleLabel.Location = new Point(18, 23);
            titleLabel.Margin = new Padding(4, 0, 4, 0);
            titleLabel.Name = "titleLabel";
            titleLabel.Size = new Size(218, 24);
            titleLabel.TabIndex = 4;
            titleLabel.Text = "Управление долгами";
            // 
            // addButton
            // 
            addButton.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            addButton.Location = new Point(472, 19);
            addButton.Margin = new Padding(4);
            addButton.Name = "addButton";
            addButton.Size = new Size(70, 38);
            addButton.TabIndex = 0;
            addButton.Text = "Добавить";
            addButton.UseVisualStyleBackColor = true;
            // 
            // editButton
            // 
            editButton.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            editButton.Location = new Point(551, 19);
            editButton.Margin = new Padding(4);
            editButton.Name = "editButton";
            editButton.Size = new Size(70, 38);
            editButton.TabIndex = 1;
            editButton.Text = "Изменить";
            editButton.UseVisualStyleBackColor = true;
            // 
            // deleteButton
            // 
            deleteButton.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            deleteButton.Location = new Point(630, 19);
            deleteButton.Margin = new Padding(4);
            deleteButton.Name = "deleteButton";
            deleteButton.Size = new Size(70, 38);
            deleteButton.TabIndex = 2;
            deleteButton.Text = "Удалить";
            deleteButton.UseVisualStyleBackColor = true;
            // 
            // refreshButton
            // 
            refreshButton.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            refreshButton.Location = new Point(709, 19);
            refreshButton.Margin = new Padding(4);
            refreshButton.Name = "refreshButton";
            refreshButton.Size = new Size(70, 38);
            refreshButton.TabIndex = 3;
            refreshButton.Text = "Обновить";
            refreshButton.UseVisualStyleBackColor = true;
            //
            // tomorrowButton
            // 
            tomorrowButton.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            tomorrowButton.Location = new Point(709, 19);
            tomorrowButton.Margin = new Padding(4);
            tomorrowButton.Name = "tomorrowButton";
            tomorrowButton.Size = new Size(70, 38);
            tomorrowButton.TabIndex = 4;
            tomorrowButton.Text = "На завтра";
            tomorrowButton.UseVisualStyleBackColor = true;
            // 
            // MainForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(788, 469);
            Controls.Add(debtsDataGridView);
            Controls.Add(panel1);
            Margin = new Padding(4);
            MinimumSize = new Size(527, 377);
            Name = "MainForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Debt Tracker";
            Load += MainForm_Load;
            ((System.ComponentModel.ISupportInitialize)debtsDataGridView).EndInit();
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            ResumeLayout(false);
        }
    }
}