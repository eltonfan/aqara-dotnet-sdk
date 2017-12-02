namespace Elton.Aqara.Win
{
    partial class MainForm
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
            this.label1 = new System.Windows.Forms.Label();
            this.labelGatewayIP = new System.Windows.Forms.Label();
            this.labelTimestamp = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.labelToken = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.columnId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.columnModel = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.columnName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.columnShortId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.columnData = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.columnTimestamp = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.columnDateUpdated = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(13, 9);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(133, 26);
            this.label1.TabIndex = 0;
            this.label1.Text = "网关IP";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // labelGatewayIP
            // 
            this.labelGatewayIP.Location = new System.Drawing.Point(154, 9);
            this.labelGatewayIP.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelGatewayIP.Name = "labelGatewayIP";
            this.labelGatewayIP.Size = new System.Drawing.Size(181, 26);
            this.labelGatewayIP.TabIndex = 1;
            this.labelGatewayIP.Text = "<IP>";
            this.labelGatewayIP.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // labelTimestamp
            // 
            this.labelTimestamp.Location = new System.Drawing.Point(154, 35);
            this.labelTimestamp.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelTimestamp.Name = "labelTimestamp";
            this.labelTimestamp.Size = new System.Drawing.Size(181, 26);
            this.labelTimestamp.TabIndex = 3;
            this.labelTimestamp.Text = "<Timestamp>";
            this.labelTimestamp.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(13, 35);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(133, 26);
            this.label3.TabIndex = 2;
            this.label3.Text = "心跳时间";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.AllowUserToResizeRows = false;
            this.dataGridView1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridView1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dataGridView1.ColumnHeadersHeight = 32;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.columnId,
            this.columnModel,
            this.columnName,
            this.columnShortId,
            this.columnData,
            this.columnTimestamp,
            this.columnDateUpdated});
            this.dataGridView1.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.dataGridView1.Location = new System.Drawing.Point(12, 90);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowTemplate.Height = 23;
            this.dataGridView1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView1.ShowEditingIcon = false;
            this.dataGridView1.Size = new System.Drawing.Size(1760, 859);
            this.dataGridView1.TabIndex = 4;
            // 
            // labelToken
            // 
            this.labelToken.Location = new System.Drawing.Point(154, 61);
            this.labelToken.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelToken.Name = "labelToken";
            this.labelToken.Size = new System.Drawing.Size(181, 26);
            this.labelToken.TabIndex = 6;
            this.labelToken.Text = "<Token>";
            this.labelToken.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(13, 61);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(133, 26);
            this.label4.TabIndex = 5;
            this.label4.Text = "令牌";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // columnId
            // 
            this.columnId.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.columnId.DataPropertyName = "sid";
            this.columnId.FillWeight = 83.45177F;
            this.columnId.HeaderText = "ID";
            this.columnId.Name = "columnId";
            this.columnId.ReadOnly = true;
            this.columnId.Width = 48;
            // 
            // columnModel
            // 
            this.columnModel.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.columnModel.DataPropertyName = "model";
            this.columnModel.FillWeight = 93.03745F;
            this.columnModel.HeaderText = "Model";
            this.columnModel.Name = "columnModel";
            this.columnModel.ReadOnly = true;
            this.columnModel.Width = 73;
            // 
            // columnName
            // 
            this.columnName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.columnName.DataPropertyName = "name";
            this.columnName.FillWeight = 198.4799F;
            this.columnName.HeaderText = "Name";
            this.columnName.Name = "columnName";
            this.columnName.ReadOnly = true;
            this.columnName.Width = 70;
            // 
            // columnShortId
            // 
            this.columnShortId.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.columnShortId.DataPropertyName = "short_id";
            this.columnShortId.FillWeight = 182.7411F;
            this.columnShortId.HeaderText = "ShortId";
            this.columnShortId.Name = "columnShortId";
            this.columnShortId.ReadOnly = true;
            this.columnShortId.Width = 79;
            // 
            // columnData
            // 
            this.columnData.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.columnData.DataPropertyName = "data";
            this.columnData.FillWeight = 21.14487F;
            this.columnData.HeaderText = "Data";
            this.columnData.Name = "columnData";
            this.columnData.ReadOnly = true;
            // 
            // columnTimestamp
            // 
            this.columnTimestamp.DataPropertyName = "timestamp";
            this.columnTimestamp.FillWeight = 21.14487F;
            this.columnTimestamp.HeaderText = "Timestamp";
            this.columnTimestamp.Name = "columnTimestamp";
            this.columnTimestamp.ReadOnly = true;
            this.columnTimestamp.Width = 200;
            // 
            // columnDateUpdated
            // 
            this.columnDateUpdated.DataPropertyName = "DateUpdated";
            this.columnDateUpdated.HeaderText = "DateUpdated";
            this.columnDateUpdated.Name = "columnDateUpdated";
            this.columnDateUpdated.ReadOnly = true;
            this.columnDateUpdated.Visible = false;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 19F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1784, 961);
            this.Controls.Add(this.labelToken);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.labelTimestamp);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.labelGatewayIP);
            this.Controls.Add(this.label1);
            this.Font = new System.Drawing.Font("Segoe UI", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "DEMO";
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label labelGatewayIP;
        private System.Windows.Forms.Label labelTimestamp;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Label labelToken;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.DataGridViewTextBoxColumn columnId;
        private System.Windows.Forms.DataGridViewTextBoxColumn columnModel;
        private System.Windows.Forms.DataGridViewTextBoxColumn columnName;
        private System.Windows.Forms.DataGridViewTextBoxColumn columnShortId;
        private System.Windows.Forms.DataGridViewTextBoxColumn columnData;
        private System.Windows.Forms.DataGridViewTextBoxColumn columnTimestamp;
        private System.Windows.Forms.DataGridViewTextBoxColumn columnDateUpdated;
    }
}

