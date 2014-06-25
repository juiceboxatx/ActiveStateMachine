namespace TelephoneUI
{
    partial class Telephone
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
            this.bttn_ReceiverDown = new System.Windows.Forms.Button();
            this.bttn_ReceiverLifted = new System.Windows.Forms.Button();
            this.bttn_Call = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.labelBellValue = new System.Windows.Forms.Label();
            this.labelLineValue = new System.Windows.Forms.Label();
            this.labelReceiverValue = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.labelCurrentViewState = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // bttn_ReceiverDown
            // 
            this.bttn_ReceiverDown.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.bttn_ReceiverDown.Location = new System.Drawing.Point(113, 256);
            this.bttn_ReceiverDown.Name = "bttn_ReceiverDown";
            this.bttn_ReceiverDown.Size = new System.Drawing.Size(121, 74);
            this.bttn_ReceiverDown.TabIndex = 0;
            this.bttn_ReceiverDown.Text = "Receiver Down";
            this.bttn_ReceiverDown.UseVisualStyleBackColor = true;
            this.bttn_ReceiverDown.Click += new System.EventHandler(this.bttn_ReceiverDown_Click);
            // 
            // bttn_ReceiverLifted
            // 
            this.bttn_ReceiverLifted.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.bttn_ReceiverLifted.Location = new System.Drawing.Point(282, 256);
            this.bttn_ReceiverLifted.Name = "bttn_ReceiverLifted";
            this.bttn_ReceiverLifted.Size = new System.Drawing.Size(121, 74);
            this.bttn_ReceiverLifted.TabIndex = 1;
            this.bttn_ReceiverLifted.Text = "Receiver Lifted";
            this.bttn_ReceiverLifted.UseVisualStyleBackColor = true;
            this.bttn_ReceiverLifted.Click += new System.EventHandler(this.bttn_ReceiverLifted_Click);
            // 
            // bttn_Call
            // 
            this.bttn_Call.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.bttn_Call.Location = new System.Drawing.Point(512, 423);
            this.bttn_Call.Name = "bttn_Call";
            this.bttn_Call.Size = new System.Drawing.Size(121, 74);
            this.bttn_Call.TabIndex = 2;
            this.bttn_Call.Text = "Initiate Call";
            this.bttn_Call.UseVisualStyleBackColor = true;
            this.bttn_Call.Click += new System.EventHandler(this.bttn_Call_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(377, 43);
            this.label1.Name = "label1";
            this.label1.Padding = new System.Windows.Forms.Padding(15);
            this.label1.Size = new System.Drawing.Size(71, 54);
            this.label1.TabIndex = 3;
            this.label1.Text = "Bell";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(372, 97);
            this.label2.Name = "label2";
            this.label2.Padding = new System.Windows.Forms.Padding(15);
            this.label2.Size = new System.Drawing.Size(76, 54);
            this.label2.TabIndex = 4;
            this.label2.Text = "Line";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(333, 151);
            this.label3.Name = "label3";
            this.label3.Padding = new System.Windows.Forms.Padding(15);
            this.label3.Size = new System.Drawing.Size(115, 54);
            this.label3.TabIndex = 5;
            this.label3.Text = "Receiver";
            // 
            // labelBellValue
            // 
            this.labelBellValue.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            this.labelBellValue.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelBellValue.Location = new System.Drawing.Point(454, 43);
            this.labelBellValue.Name = "labelBellValue";
            this.labelBellValue.Padding = new System.Windows.Forms.Padding(15, 15, 18, 15);
            this.labelBellValue.Size = new System.Drawing.Size(140, 54);
            this.labelBellValue.TabIndex = 6;
            this.labelBellValue.Text = "Silent";
            // 
            // labelLineValue
            // 
            this.labelLineValue.BackColor = System.Drawing.Color.Red;
            this.labelLineValue.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelLineValue.Location = new System.Drawing.Point(454, 97);
            this.labelLineValue.Name = "labelLineValue";
            this.labelLineValue.Padding = new System.Windows.Forms.Padding(15, 15, 40, 15);
            this.labelLineValue.Size = new System.Drawing.Size(140, 54);
            this.labelLineValue.TabIndex = 7;
            this.labelLineValue.Text = "Off";
            // 
            // labelReceiverValue
            // 
            this.labelReceiverValue.BackColor = System.Drawing.Color.Lime;
            this.labelReceiverValue.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelReceiverValue.Location = new System.Drawing.Point(454, 151);
            this.labelReceiverValue.Name = "labelReceiverValue";
            this.labelReceiverValue.Padding = new System.Windows.Forms.Padding(15);
            this.labelReceiverValue.Size = new System.Drawing.Size(140, 54);
            this.labelReceiverValue.TabIndex = 8;
            this.labelReceiverValue.Text = "Down";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.Location = new System.Drawing.Point(54, 446);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(170, 24);
            this.label7.TabIndex = 9;
            this.label7.Text = "Current View State:";
            // 
            // labelCurrentViewState
            // 
            this.labelCurrentViewState.AutoSize = true;
            this.labelCurrentViewState.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelCurrentViewState.Location = new System.Drawing.Point(230, 446);
            this.labelCurrentViewState.Name = "labelCurrentViewState";
            this.labelCurrentViewState.Size = new System.Drawing.Size(85, 24);
            this.labelCurrentViewState.TabIndex = 10;
            this.labelCurrentViewState.Text = "Receiver";
            // 
            // Telephone
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(645, 509);
            this.Controls.Add(this.labelCurrentViewState);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.labelReceiverValue);
            this.Controls.Add(this.labelLineValue);
            this.Controls.Add(this.labelBellValue);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.bttn_Call);
            this.Controls.Add(this.bttn_ReceiverLifted);
            this.Controls.Add(this.bttn_ReceiverDown);
            this.Name = "Telephone";
            this.Text = "Telephone UI";
            this.Load += new System.EventHandler(this.Telephone_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button bttn_ReceiverDown;
        private System.Windows.Forms.Button bttn_ReceiverLifted;
        private System.Windows.Forms.Button bttn_Call;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label labelBellValue;
        private System.Windows.Forms.Label labelLineValue;
        private System.Windows.Forms.Label labelReceiverValue;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label labelCurrentViewState;
    }
}

