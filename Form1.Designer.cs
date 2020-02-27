namespace TaskManager
{
    partial class TaskManagerForm
    {
        /// <summary>
        /// Variable del diseñador necesaria.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Limpiar los recursos que se estén usando.
        /// </summary>
        /// <param name="disposing">true si los recursos administrados se deben desechar; false en caso contrario.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Código generado por el Diseñador de Windows Forms

        /// <summary>
        /// Método necesario para admitir el Diseñador. No se puede modificar
        /// el contenido de este método con el editor de código.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TaskManagerForm));
            this.ProcessList = new System.Windows.Forms.ListBox();
            this.ProcessInfoBar = new System.Windows.Forms.StatusStrip();
            this.current_process_name = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripDropDownButton1 = new System.Windows.Forms.ToolStripDropDownButton();
            this.terminarToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.resumirToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.detenerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.CountersBTN = new System.Windows.Forms.ToolStripSplitButton();
            this.total_ram_label = new System.Windows.Forms.ToolStripStatusLabel();
            this.Process_Info_Group = new System.Windows.Forms.GroupBox();
            this.current_process_hddread = new System.Windows.Forms.Label();
            this.current_process_hddwrite = new System.Windows.Forms.Label();
            this.current_process_cpu_usage = new System.Windows.Forms.Label();
            this.current_process_status = new System.Windows.Forms.Label();
            this.current_process_id = new System.Windows.Forms.Label();
            this.current_process_memory_use = new System.Windows.Forms.Label();
            this.create_process_btn = new System.Windows.Forms.Label();
            this.PerformancePB = new System.Windows.Forms.PictureBox();
            this.current_graphic_name = new System.Windows.Forms.Label();
            this.stoppedProcess = new System.Windows.Forms.ListBox();
            this.ProcessInfoBar.SuspendLayout();
            this.Process_Info_Group.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.PerformancePB)).BeginInit();
            this.SuspendLayout();
            // 
            // ProcessList
            // 
            this.ProcessList.FormattingEnabled = true;
            this.ProcessList.ItemHeight = 16;
            this.ProcessList.Location = new System.Drawing.Point(579, 222);
            this.ProcessList.Name = "ProcessList";
            this.ProcessList.Size = new System.Drawing.Size(333, 228);
            this.ProcessList.TabIndex = 0;
            this.ProcessList.Click += new System.EventHandler(this.ProcessList_Click);
            // 
            // ProcessInfoBar
            // 
            this.ProcessInfoBar.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.ProcessInfoBar.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.current_process_name,
            this.toolStripDropDownButton1,
            this.CountersBTN,
            this.total_ram_label});
            this.ProcessInfoBar.Location = new System.Drawing.Point(0, 589);
            this.ProcessInfoBar.Name = "ProcessInfoBar";
            this.ProcessInfoBar.Size = new System.Drawing.Size(938, 26);
            this.ProcessInfoBar.TabIndex = 1;
            this.ProcessInfoBar.Text = "statusStrip1";
            // 
            // current_process_name
            // 
            this.current_process_name.Name = "current_process_name";
            this.current_process_name.Size = new System.Drawing.Size(66, 20);
            this.current_process_name.Text = "Ninguno";
            // 
            // toolStripDropDownButton1
            // 
            this.toolStripDropDownButton1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(180)))), ((int)(((byte)(255)))), ((int)(((byte)(180)))));
            this.toolStripDropDownButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripDropDownButton1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.terminarToolStripMenuItem,
            this.resumirToolStripMenuItem,
            this.detenerToolStripMenuItem});
            this.toolStripDropDownButton1.Image = ((System.Drawing.Image)(resources.GetObject("toolStripDropDownButton1.Image")));
            this.toolStripDropDownButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripDropDownButton1.Name = "toolStripDropDownButton1";
            this.toolStripDropDownButton1.Size = new System.Drawing.Size(85, 24);
            this.toolStripDropDownButton1.Text = "Opciones";
            // 
            // terminarToolStripMenuItem
            // 
            this.terminarToolStripMenuItem.Name = "terminarToolStripMenuItem";
            this.terminarToolStripMenuItem.Size = new System.Drawing.Size(150, 26);
            this.terminarToolStripMenuItem.Text = "Terminar";
            this.terminarToolStripMenuItem.Click += new System.EventHandler(this.terminarToolStripMenuItem_Click);
            // 
            // resumirToolStripMenuItem
            // 
            this.resumirToolStripMenuItem.Name = "resumirToolStripMenuItem";
            this.resumirToolStripMenuItem.Size = new System.Drawing.Size(150, 26);
            this.resumirToolStripMenuItem.Text = "Resumir";
            this.resumirToolStripMenuItem.Click += new System.EventHandler(this.resumirToolStripMenuItem_Click);
            // 
            // detenerToolStripMenuItem
            // 
            this.detenerToolStripMenuItem.Name = "detenerToolStripMenuItem";
            this.detenerToolStripMenuItem.Size = new System.Drawing.Size(150, 26);
            this.detenerToolStripMenuItem.Text = "Detener";
            this.detenerToolStripMenuItem.Click += new System.EventHandler(this.detenerToolStripMenuItem_Click);
            // 
            // CountersBTN
            // 
            this.CountersBTN.BackColor = System.Drawing.SystemColors.Control;
            this.CountersBTN.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.CountersBTN.Image = ((System.Drawing.Image)(resources.GetObject("CountersBTN.Image")));
            this.CountersBTN.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.CountersBTN.Name = "CountersBTN";
            this.CountersBTN.Size = new System.Drawing.Size(86, 24);
            this.CountersBTN.Text = "Counters";
            this.CountersBTN.ButtonClick += new System.EventHandler(this.CountersBTN_ButtonClick);
            // 
            // total_ram_label
            // 
            this.total_ram_label.Name = "total_ram_label";
            this.total_ram_label.Size = new System.Drawing.Size(79, 20);
            this.total_ram_label.Text = "Ram Total:";
            // 
            // Process_Info_Group
            // 
            this.Process_Info_Group.Controls.Add(this.current_process_hddread);
            this.Process_Info_Group.Controls.Add(this.current_process_hddwrite);
            this.Process_Info_Group.Controls.Add(this.current_process_cpu_usage);
            this.Process_Info_Group.Controls.Add(this.current_process_status);
            this.Process_Info_Group.Controls.Add(this.current_process_id);
            this.Process_Info_Group.Controls.Add(this.current_process_memory_use);
            this.Process_Info_Group.Location = new System.Drawing.Point(84, 30);
            this.Process_Info_Group.Name = "Process_Info_Group";
            this.Process_Info_Group.Size = new System.Drawing.Size(351, 227);
            this.Process_Info_Group.TabIndex = 2;
            this.Process_Info_Group.TabStop = false;
            this.Process_Info_Group.Text = "Infromacion";
            // 
            // current_process_hddread
            // 
            this.current_process_hddread.AutoSize = true;
            this.current_process_hddread.Font = new System.Drawing.Font("Tahoma", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.current_process_hddread.Location = new System.Drawing.Point(34, 163);
            this.current_process_hddread.Name = "current_process_hddread";
            this.current_process_hddread.Size = new System.Drawing.Size(183, 17);
            this.current_process_hddread.TabIndex = 6;
            this.current_process_hddread.Text = "Operaciones de escritura:";
            // 
            // current_process_hddwrite
            // 
            this.current_process_hddwrite.AutoSize = true;
            this.current_process_hddwrite.Font = new System.Drawing.Font("Tahoma", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.current_process_hddwrite.Location = new System.Drawing.Point(34, 192);
            this.current_process_hddwrite.Name = "current_process_hddwrite";
            this.current_process_hddwrite.Size = new System.Drawing.Size(170, 17);
            this.current_process_hddwrite.TabIndex = 5;
            this.current_process_hddwrite.Text = "Operaciones de lectura:";
            // 
            // current_process_cpu_usage
            // 
            this.current_process_cpu_usage.AutoSize = true;
            this.current_process_cpu_usage.Font = new System.Drawing.Font("Tahoma", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.current_process_cpu_usage.Location = new System.Drawing.Point(34, 74);
            this.current_process_cpu_usage.Name = "current_process_cpu_usage";
            this.current_process_cpu_usage.Size = new System.Drawing.Size(46, 17);
            this.current_process_cpu_usage.TabIndex = 4;
            this.current_process_cpu_usage.Text = "CPU: ";
            this.current_process_cpu_usage.Click += new System.EventHandler(this.current_process_cpu_usage_Click);
            // 
            // current_process_status
            // 
            this.current_process_status.AutoSize = true;
            this.current_process_status.Font = new System.Drawing.Font("Tahoma", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.current_process_status.Location = new System.Drawing.Point(34, 134);
            this.current_process_status.Name = "current_process_status";
            this.current_process_status.Size = new System.Drawing.Size(62, 17);
            this.current_process_status.TabIndex = 3;
            this.current_process_status.Text = "Status: ";
            // 
            // current_process_id
            // 
            this.current_process_id.AutoSize = true;
            this.current_process_id.Font = new System.Drawing.Font("Tahoma", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.current_process_id.Location = new System.Drawing.Point(34, 105);
            this.current_process_id.Name = "current_process_id";
            this.current_process_id.Size = new System.Drawing.Size(43, 17);
            this.current_process_id.TabIndex = 1;
            this.current_process_id.Text = "PID: ";
            // 
            // current_process_memory_use
            // 
            this.current_process_memory_use.AutoSize = true;
            this.current_process_memory_use.BackColor = System.Drawing.SystemColors.Control;
            this.current_process_memory_use.Font = new System.Drawing.Font("Tahoma", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.current_process_memory_use.Location = new System.Drawing.Point(34, 42);
            this.current_process_memory_use.Name = "current_process_memory_use";
            this.current_process_memory_use.Size = new System.Drawing.Size(122, 17);
            this.current_process_memory_use.TabIndex = 0;
            this.current_process_memory_use.Text = "Memoria Usada: ";
            this.current_process_memory_use.Click += new System.EventHandler(this.current_process_memory_use_Click);
            // 
            // create_process_btn
            // 
            this.create_process_btn.AutoSize = true;
            this.create_process_btn.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.create_process_btn.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.create_process_btn.Cursor = System.Windows.Forms.Cursors.Hand;
            this.create_process_btn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.create_process_btn.Font = new System.Drawing.Font("Segoe UI Emoji", 16.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.create_process_btn.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(50)))), ((int)(((byte)(50)))));
            this.create_process_btn.Location = new System.Drawing.Point(107, 361);
            this.create_process_btn.Margin = new System.Windows.Forms.Padding(6);
            this.create_process_btn.Name = "create_process_btn";
            this.create_process_btn.Padding = new System.Windows.Forms.Padding(6);
            this.create_process_btn.Size = new System.Drawing.Size(203, 51);
            this.create_process_btn.TabIndex = 3;
            this.create_process_btn.Text = "Crear Proceso";
            this.create_process_btn.Click += new System.EventHandler(this.create_process_btn_Click);
            // 
            // PerformancePB
            // 
            this.PerformancePB.BackColor = System.Drawing.SystemColors.ControlText;
            this.PerformancePB.Location = new System.Drawing.Point(579, 39);
            this.PerformancePB.Name = "PerformancePB";
            this.PerformancePB.Size = new System.Drawing.Size(333, 158);
            this.PerformancePB.TabIndex = 4;
            this.PerformancePB.TabStop = false;
            // 
            // current_graphic_name
            // 
            this.current_graphic_name.AutoSize = true;
            this.current_graphic_name.Font = new System.Drawing.Font("Tahoma", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.current_graphic_name.Location = new System.Drawing.Point(576, 19);
            this.current_graphic_name.Name = "current_graphic_name";
            this.current_graphic_name.Size = new System.Drawing.Size(122, 17);
            this.current_graphic_name.TabIndex = 7;
            this.current_graphic_name.Text = "Memoria Usada: ";
            // 
            // stoppedProcess
            // 
            this.stoppedProcess.FormattingEnabled = true;
            this.stoppedProcess.ItemHeight = 16;
            this.stoppedProcess.Location = new System.Drawing.Point(579, 470);
            this.stoppedProcess.Name = "stoppedProcess";
            this.stoppedProcess.Size = new System.Drawing.Size(333, 116);
            this.stoppedProcess.TabIndex = 8;
            // 
            // TaskManagerForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(938, 615);
            this.Controls.Add(this.stoppedProcess);
            this.Controls.Add(this.current_graphic_name);
            this.Controls.Add(this.PerformancePB);
            this.Controls.Add(this.create_process_btn);
            this.Controls.Add(this.Process_Info_Group);
            this.Controls.Add(this.ProcessInfoBar);
            this.Controls.Add(this.ProcessList);
            this.Name = "TaskManagerForm";
            this.Text = "Form1";
            this.ProcessInfoBar.ResumeLayout(false);
            this.ProcessInfoBar.PerformLayout();
            this.Process_Info_Group.ResumeLayout(false);
            this.Process_Info_Group.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.PerformancePB)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListBox ProcessList;
        private System.Windows.Forms.StatusStrip ProcessInfoBar;
        private System.Windows.Forms.ToolStripStatusLabel current_process_name;
        private System.Windows.Forms.ToolStripDropDownButton toolStripDropDownButton1;
        private System.Windows.Forms.GroupBox Process_Info_Group;
        private System.Windows.Forms.Label current_process_memory_use;
        private System.Windows.Forms.Label current_process_id;
        private System.Windows.Forms.Label current_process_status;
        private System.Windows.Forms.Label create_process_btn;
        private System.Windows.Forms.Label current_process_cpu_usage;
        private System.Windows.Forms.ToolStripSplitButton CountersBTN;
        private System.Windows.Forms.Label current_process_hddread;
        private System.Windows.Forms.Label current_process_hddwrite;
        private System.Windows.Forms.PictureBox PerformancePB;
        private System.Windows.Forms.Label current_graphic_name;
        private System.Windows.Forms.ToolStripMenuItem detenerToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem resumirToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem terminarToolStripMenuItem;
        private System.Windows.Forms.ToolStripStatusLabel total_ram_label;
        private System.Windows.Forms.ListBox stoppedProcess;
    }
}

