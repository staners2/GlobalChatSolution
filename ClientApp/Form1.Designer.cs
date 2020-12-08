﻿using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace ClientApp
{
    partial class Form1
    {
        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
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
            this.NickNameBox = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.ConnectButton = new System.Windows.Forms.Button();
            this.SendButton = new System.Windows.Forms.Button();
            this.SendMessageBox = new System.Windows.Forms.TextBox();
            this.ChatBox = new System.Windows.Forms.ListBox();
            this.UserBox = new System.Windows.Forms.ListBox();
            this.SuspendLayout();
            // 
            // NickNameBox
            // 
            this.NickNameBox.Font = new System.Drawing.Font("Trebuchet MS", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.NickNameBox.Location = new System.Drawing.Point(105, 9);
            this.NickNameBox.Multiline = true;
            this.NickNameBox.Name = "NickNameBox";
            this.NickNameBox.Size = new System.Drawing.Size(338, 35);
            this.NickNameBox.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Trebuchet MS", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label1.Location = new System.Drawing.Point(0, 14);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(99, 24);
            this.label1.TabIndex = 1;
            this.label1.Text = "NickName";
            // 
            // ConnectButton
            // 
            this.ConnectButton.Font = new System.Drawing.Font("Trebuchet MS", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.ConnectButton.Location = new System.Drawing.Point(449, 9);
            this.ConnectButton.Name = "ConnectButton";
            this.ConnectButton.Size = new System.Drawing.Size(174, 35);
            this.ConnectButton.TabIndex = 2;
            this.ConnectButton.Text = "Connect";
            this.ConnectButton.UseVisualStyleBackColor = true;
            this.ConnectButton.Click += new System.EventHandler(this.ConnectButton_Click);
            // 
            // SendButton
            // 
            this.SendButton.Enabled = false;
            this.SendButton.Font = new System.Drawing.Font("Trebuchet MS", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.SendButton.Location = new System.Drawing.Point(504, 390);
            this.SendButton.Name = "SendButton";
            this.SendButton.Size = new System.Drawing.Size(119, 35);
            this.SendButton.TabIndex = 3;
            this.SendButton.Text = "Send";
            this.SendButton.UseVisualStyleBackColor = true;
            this.SendButton.Click += new System.EventHandler(this.SendButton_Click);
            // 
            // SendMessageBox
            // 
            this.SendMessageBox.Enabled = false;
            this.SendMessageBox.Font = new System.Drawing.Font("Trebuchet MS", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.SendMessageBox.Location = new System.Drawing.Point(4, 390);
            this.SendMessageBox.Multiline = true;
            this.SendMessageBox.Name = "SendMessageBox";
            this.SendMessageBox.Size = new System.Drawing.Size(494, 35);
            this.SendMessageBox.TabIndex = 4;
            // 
            // ChatBox
            // 
            this.ChatBox.Font = new System.Drawing.Font("Trebuchet MS", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.ChatBox.FormattingEnabled = true;
            this.ChatBox.ItemHeight = 22;
            this.ChatBox.Location = new System.Drawing.Point(4, 50);
            this.ChatBox.Name = "ChatBox";
            this.ChatBox.Size = new System.Drawing.Size(619, 334);
            this.ChatBox.TabIndex = 7;
            // 
            // UserBox
            // 
            this.UserBox.Font = new System.Drawing.Font("Trebuchet MS", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.UserBox.FormattingEnabled = true;
            this.UserBox.ItemHeight = 22;
            this.UserBox.Location = new System.Drawing.Point(629, 25);
            this.UserBox.Name = "UserBox";
            this.UserBox.Size = new System.Drawing.Size(171, 400);
            this.UserBox.TabIndex = 8;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(812, 431);
            this.Controls.Add(this.UserBox);
            this.Controls.Add(this.ChatBox);
            this.Controls.Add(this.SendMessageBox);
            this.Controls.Add(this.SendButton);
            this.Controls.Add(this.ConnectButton);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.NickNameBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private TextBox NickNameBox;
        private Label label1;
        private Button ConnectButton;
        private Button SendButton;
        private TextBox SendMessageBox;
        public ListBox ChatBox;
        public ListBox UserBox;
    }
}

