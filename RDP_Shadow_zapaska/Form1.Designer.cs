namespace RDP_Shadow_zapaska
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.sessionListView = new System.Windows.Forms.ListView();
            this.Username = new System.Windows.Forms.ColumnHeader();
            this.SessionID = new System.Windows.Forms.ColumnHeader();
            this.refreshButton = new System.Windows.Forms.Button();
            this.shadowButton = new System.Windows.Forms.Button();
            this.domainComboBox = new System.Windows.Forms.ComboBox();
            this.domain = new System.Windows.Forms.Label();
            this.serverall = new System.Windows.Forms.Label();
            this.serverComboBox = new System.Windows.Forms.ComboBox();
            this.AutoUpdate = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // sessionListView
            // 
            this.sessionListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.Username,
            this.SessionID});
            this.sessionListView.Dock = System.Windows.Forms.DockStyle.Left;
            this.sessionListView.Location = new System.Drawing.Point(0, 0);
            this.sessionListView.Name = "sessionListView";
            this.sessionListView.Size = new System.Drawing.Size(352, 660);
            this.sessionListView.Sorting = System.Windows.Forms.SortOrder.Ascending;
            this.sessionListView.TabIndex = 9;
            this.sessionListView.UseCompatibleStateImageBehavior = false;
            this.sessionListView.View = System.Windows.Forms.View.Details;
            this.sessionListView.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.sessionListView_MouseDoubleClick);
            // 
            // Username
            // 
            this.Username.DisplayIndex = 1;
            this.Username.Text = "Имя пользователя";
            this.Username.Width = 100;
            // 
            // SessionID
            // 
            this.SessionID.DisplayIndex = 0;
            this.SessionID.Text = "ID";
            this.SessionID.Width = 100;
            // 
            // refreshButton
            // 
            this.refreshButton.AutoSize = true;
            this.refreshButton.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.refreshButton.Location = new System.Drawing.Point(352, 629);
            this.refreshButton.Name = "refreshButton";
            this.refreshButton.Size = new System.Drawing.Size(307, 31);
            this.refreshButton.TabIndex = 10;
            this.refreshButton.Text = "Обновить (F5)";
            this.refreshButton.UseVisualStyleBackColor = true;
            this.refreshButton.Click += new System.EventHandler(this.refreshButton_Click_1);
            // 
            // shadowButton
            // 
            this.shadowButton.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.shadowButton.Location = new System.Drawing.Point(352, 599);
            this.shadowButton.Name = "shadowButton";
            this.shadowButton.Size = new System.Drawing.Size(307, 30);
            this.shadowButton.TabIndex = 11;
            this.shadowButton.Text = "Подключится";
            this.shadowButton.UseVisualStyleBackColor = true;
            this.shadowButton.Click += new System.EventHandler(this.shadowButton_Click_1);
            // 
            // domainComboBox
            // 
            this.domainComboBox.FormattingEnabled = true;
            this.domainComboBox.Location = new System.Drawing.Point(526, 12);
            this.domainComboBox.Name = "domainComboBox";
            this.domainComboBox.Size = new System.Drawing.Size(121, 23);
            this.domainComboBox.TabIndex = 12;
            // 
            // domain
            // 
            this.domain.AutoSize = true;
            this.domain.Location = new System.Drawing.Point(470, 15);
            this.domain.Name = "domain";
            this.domain.Size = new System.Drawing.Size(47, 15);
            this.domain.TabIndex = 13;
            this.domain.Text = "Домен:";
            // 
            // serverall
            // 
            this.serverall.AutoSize = true;
            this.serverall.Location = new System.Drawing.Point(470, 44);
            this.serverall.Name = "serverall";
            this.serverall.Size = new System.Drawing.Size(50, 15);
            this.serverall.TabIndex = 15;
            this.serverall.Text = "Сервер:";
            // 
            // serverComboBox
            // 
            this.serverComboBox.FormattingEnabled = true;
            this.serverComboBox.Location = new System.Drawing.Point(526, 41);
            this.serverComboBox.Name = "serverComboBox";
            this.serverComboBox.Size = new System.Drawing.Size(121, 23);
            this.serverComboBox.TabIndex = 14;
            // 
            // AutoUpdate
            // 
            this.AutoUpdate.AutoSize = true;
            this.AutoUpdate.Location = new System.Drawing.Point(358, 574);
            this.AutoUpdate.Name = "AutoUpdate";
            this.AutoUpdate.Size = new System.Drawing.Size(119, 19);
            this.AutoUpdate.TabIndex = 16;
            this.AutoUpdate.Text = "Автообновление";
            this.AutoUpdate.UseVisualStyleBackColor = true;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(659, 660);
            this.Controls.Add(this.AutoUpdate);
            this.Controls.Add(this.serverall);
            this.Controls.Add(this.serverComboBox);
            this.Controls.Add(this.domain);
            this.Controls.Add(this.domainComboBox);
            this.Controls.Add(this.shadowButton);
            this.Controls.Add(this.refreshButton);
            this.Controls.Add(this.sessionListView);
            this.KeyPreview = true;
            this.Name = "Form1";
            this.Text = "Запаска RDP";
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Form1_KeyDown);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private ListView sessionListView;
        private Button refreshButton;
        private Button shadowButton;
        private ColumnHeader SessionID;
        private ColumnHeader Username;
        private ComboBox domainComboBox;
        private Label domain;
        private Label serverall;
        private ComboBox serverComboBox;
        private CheckBox AutoUpdate;
    }
}