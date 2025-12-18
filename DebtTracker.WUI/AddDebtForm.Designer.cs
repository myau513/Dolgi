using System.Drawing;
using System.Drawing.Printing;
using System.Windows.Forms;
using System.Xml.Linq;

namespace DebtTracker.WUI
{
    partial class AddDebtForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            panel1 = new Panel();
            titleLabel = new Label();
            panel2 = new Panel();
            cancelButton = new Button();
            saveButton = new Button();
            statusComboBox = new ComboBox();
            label4 = new Label();
            deadlineDateTimePicker = new DateTimePicker();
            label3 = new Label();
            descriptionTextBox = new TextBox();
            label2 = new Label();
            subjectTextBox = new TextBox();
            label1 = new Label();
            panel1.SuspendLayout();
            panel2.SuspendLayout();
            SuspendLayout();
            // 
            // panel1
            // 
            panel1.BackColor = SystemColors.ControlDark;
            panel1.Controls.Add(titleLabel);
            panel1.Dock = DockStyle.Top;
            panel1.Location = new Point(0, 0);
            panel1.Margin = new Padding(4, 3, 4, 3);
            panel1.Name = "panel1";
            panel1.Size = new Size(450, 60);
            panel1.TabIndex = 0;
            // 
            // titleLabel
            // 
            titleLabel.AutoSize = true;
            titleLabel.Font = new Font("Segoe UI", 14F, FontStyle.Bold);
            titleLabel.Location = new Point(20, 18);
            titleLabel.Margin = new Padding(4, 0, 4, 0);
            titleLabel.Name = "titleLabel";
            titleLabel.Size = new Size(152, 25);
            titleLabel.TabIndex = 0;
            titleLabel.Text = "Добавить долг";
            // 
            // panel2
            // 
            panel2.Controls.Add(cancelButton);
            panel2.Controls.Add(saveButton);
            panel2.Controls.Add(statusComboBox);
            panel2.Controls.Add(label4);
            panel2.Controls.Add(deadlineDateTimePicker);
            panel2.Controls.Add(label3);
            panel2.Controls.Add(descriptionTextBox);
            panel2.Controls.Add(label2);
            panel2.Controls.Add(subjectTextBox);
            panel2.Controls.Add(label1);
            panel2.Dock = DockStyle.Fill;
            panel2.Location = new Point(0, 60);
            panel2.Margin = new Padding(4, 3, 4, 3);
            panel2.Name = "panel2";
            panel2.Padding = new Padding(20);
            panel2.Size = new Size(450, 290);
            panel2.TabIndex = 1;
            panel2.Paint += panel2_Paint;
            // 
            // cancelButton
            // 
            cancelButton.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            cancelButton.Location = new Point(230, 220);
            cancelButton.Margin = new Padding(4, 3, 4, 3);
            cancelButton.Name = "cancelButton";
            cancelButton.Size = new Size(100, 30);
            cancelButton.TabIndex = 9;
            cancelButton.Text = "Отмена";
            cancelButton.UseVisualStyleBackColor = true;
            cancelButton.Click += cancelButton_Click;
            // 
            // saveButton
            // 
            saveButton.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            saveButton.Location = new Point(340, 220);
            saveButton.Margin = new Padding(4, 3, 4, 3);
            saveButton.Name = "saveButton";
            saveButton.Size = new Size(100, 30);
            saveButton.TabIndex = 8;
            saveButton.Text = "Сохранить";
            saveButton.UseVisualStyleBackColor = true;
            saveButton.Click += saveButton_Click;
            // 
            // statusComboBox
            // 
            statusComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            statusComboBox.FormattingEnabled = true;
            statusComboBox.Location = new Point(120, 170);
            statusComboBox.Margin = new Padding(4, 3, 4, 3);
            statusComboBox.Name = "statusComboBox";
            statusComboBox.Size = new Size(320, 23);
            statusComboBox.TabIndex = 7;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(20, 173);
            label4.Margin = new Padding(4, 0, 4, 0);
            label4.Name = "label4";
            label4.Size = new Size(46, 15);
            label4.TabIndex = 6;
            label4.Text = "Статус:";
            // 
            // deadlineDateTimePicker
            // 
            deadlineDateTimePicker.Format = DateTimePickerFormat.Short;
            deadlineDateTimePicker.Location = new Point(120, 130);
            deadlineDateTimePicker.Margin = new Padding(4, 3, 4, 3);
            deadlineDateTimePicker.Name = "deadlineDateTimePicker";
            deadlineDateTimePicker.Size = new Size(320, 23);
            deadlineDateTimePicker.TabIndex = 5;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(20, 136);
            label3.Margin = new Padding(4, 0, 4, 0);
            label3.Name = "label3";
            label3.Size = new Size(57, 15);
            label3.TabIndex = 4;
            label3.Text = "Дедлайн:";
            // 
            // descriptionTextBox
            // 
            descriptionTextBox.Location = new Point(120, 80);
            descriptionTextBox.Margin = new Padding(4, 3, 4, 3);
            descriptionTextBox.Multiline = true;
            descriptionTextBox.Name = "descriptionTextBox";
            descriptionTextBox.ScrollBars = ScrollBars.Vertical;
            descriptionTextBox.Size = new Size(320, 40);
            descriptionTextBox.TabIndex = 3;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(20, 83);
            label2.Margin = new Padding(4, 0, 4, 0);
            label2.Name = "label2";
            label2.Size = new Size(65, 15);
            label2.TabIndex = 2;
            label2.Text = "Описание:";
            // 
            // subjectTextBox
            // 
            subjectTextBox.Location = new Point(120, 30);
            subjectTextBox.Margin = new Padding(4, 3, 4, 3);
            subjectTextBox.Name = "subjectTextBox";
            subjectTextBox.Size = new Size(320, 23);
            subjectTextBox.TabIndex = 1;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(20, 33);
            label1.Margin = new Padding(4, 0, 4, 0);
            label1.Name = "label1";
            label1.Size = new Size(96, 15);
            label1.TabIndex = 0;
            label1.Text = "Название долга:";
            // 
            // AddDebtForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(450, 350);
            Controls.Add(panel2);
            Controls.Add(panel1);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            Margin = new Padding(4, 3, 4, 3);
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "AddDebtForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Добавление долга";
            Load += AddDebtForm_Load;
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            panel2.ResumeLayout(false);
            panel2.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private Panel panel1;
        private Label titleLabel;
        private Panel panel2;
        private Button cancelButton;
        private Button saveButton;
        private ComboBox statusComboBox;
        private Label label4;
        private DateTimePicker deadlineDateTimePicker;
        private Label label3;
        private TextBox descriptionTextBox;
        private Label label2;
        private TextBox subjectTextBox;
        private Label label1;
    }
}