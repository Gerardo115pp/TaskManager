using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using System.Timers;
using System.Runtime.InteropServices;

namespace TaskManager
{
    enum Measureables
    {
        CPU = 0,
        RAM = 1
    }

    [Flags]
    public enum ThreadAccess : int
    {
        TERMINATE = (0x0001),
        SUSPEND_RESUME = (0x0002),
        GET_CONTEXT = (0x0008),
        SET_CONTEXT = (0x0010),
        SET_INFORMATION = (0x0020),
        QUERY_INFORMATION = (0x0040),
        SET_THREAD_TOKEN = (0x0080),
        IMPERSONATE = (0x0100),
        DIRECT_IMPERSONATION = (0x0200)
    }




    public partial class Form1 : Form
    {
        public static List<Process> processes;

        [DllImport("kernel32.dll")]
        static extern IntPtr OpenThread(ThreadAccess dwDesiredAccess, bool bInheritHandle, uint dwThreadId);
        [DllImport("kernel32.dll")]
        static extern uint SuspendThread(IntPtr hThread);
        [DllImport("kernel32.dll")]
        static extern int ResumeThread(IntPtr hThread);
        [DllImport("kernel32", CharSet = CharSet.Auto, SetLastError = true)]
        static extern bool CloseHandle(IntPtr handle);

        private delegate void ChangeLabelText(Label form_label, string text);

        private ulong total_ram = new Microsoft.VisualBasic.Devices.ComputerInfo().TotalPhysicalMemory;
        private Measureables measureing;
        private Bitmap performace_graphic;
        private List<int> performace_data;
        private Process selected_process;
        private System.Timers.Timer process_Info_updater;
        private PerformanceCounter cpu_counter;
        private PerformanceCounter ior_counter; //input output read
        private PerformanceCounter iow_counter; //input output write
        private float cpu_last_increase;
        private int max_height;
        private ChangeLabelText changeLabelText;

        public Form1()
        {
            InitializeComponent();
            Form1.processes = Process.GetProcesses().ToList();
            this.setProcessListToListBox();
            this.measureing = Measureables.CPU;
            this.max_height = 128;
            this.performace_graphic = new Bitmap(250, this.max_height);
            this.performace_data = new List<int>(25);
            this.process_Info_updater = new System.Timers.Timer(500);
            this.process_Info_updater.Elapsed += updateProcesses;
            this.process_Info_updater.AutoReset = true;
            this.measureChange += () => this.resetGraphic(this.getMeasurableName());
            this.measureChange();
            this.total_ram_label.Text = $"RAM total: {this.total_ram / (1024 * 1024)} MB";
            this.changeLabelText = new ChangeLabelText(this.setLabelText);
            this.process_Info_updater.Start();
        }

        #region <Performance Graphic>

        private void resetGraphic(string graphic_name)
        {
            this.current_graphic_name.Text = graphic_name;
            this.performace_graphic = new Bitmap(250, this.max_height);
            this.PerformancePB.Image = (Image)this.performace_graphic;
            this.performace_data.Clear();
            this.performace_data.Add(this.measureing == Measureables.RAM ? this.max_height-18 : this.max_height/2);
        }

        private int getPointToDraw()
        {
            string return_value_string;
            float return_value=0;
            if(this.measureing == Measureables.RAM)
            {
                float process_increase_percentage = (float)this.selected_process.WorkingSet64 / (float)this.total_ram;
                return_value = (this.performace_graphic.Height * process_increase_percentage);
                return_value_string = return_value.ToString("0.##");
                this.current_graphic_name.Invoke(new Action(() => this.current_graphic_name.Text = $"{this.getMeasurableName() + return_value_string}%"));
                return_value = this.max_height - (return_value+18);
                //MessageBox.Show($"{this.selected_process.WorkingSet64} / { this.selected_process.PrivateMemorySize64 } = {process_increase_percentage.ToString("0.##")}");
            }
            else if(this.measureing == Measureables.CPU)
            {
                return_value = (this.performace_graphic.Height / 2) - Convert.ToInt32((this.cpu_last_increase/100) * this.performace_graphic.Height);
                this.current_graphic_name.Invoke(new Action(() => this.current_graphic_name.Text = $"{this.getMeasurableName()+(Convert.ToInt32((this.cpu_last_increase / 100) * this.max_height))}%"));
            }
            return Convert.ToInt32(return_value);
        }

        private void addDrawPointToPerformanceData(int draw_point)
        {
            if(this.performace_data.Count < 25)
            {
                this.performace_data.Add(draw_point);
                return;
            }
            for(int h=1; h < 25; h++)
            {
                this.performace_data[h - 1] = this.performace_data[h];
            }
            this.performace_data[24] = draw_point;
        }

        private void drawInPerformance()
        {
            Graphics graphics = Graphics.FromImage(this.performace_graphic);
            Pen pencil = new Pen(Color.LimeGreen, 1f);
            int draw_point = this.getPointToDraw();
            graphics.FillRectangle(new SolidBrush(Color.Black), 0, 0, 250, 128);
            this.addDrawPointToPerformanceData(draw_point);
            for(int h=0; h < this.performace_data.Count-1; h++)
            {
                graphics.DrawLine(pencil, h * 10, this.performace_data[h], (h + 1) * 10, this.performace_data[h + 1]);
            }
            this.PerformancePB.Image = (Image)this.performace_graphic;
        }

        #endregion </Performance Graphic>

        #region <Timer functions>

        private void updateProcesses(object sender, ElapsedEventArgs e)
        {
            this.updateProcessesListBox();
            if(this.selected_process != null)
            {
                this.updateCurrentProcessInfo();
            }
        }
        
        private void updateProcessesListBox()
        {
            Form1.processes = Process.GetProcesses().ToList();
            Form1.processes.Sort((Process a, Process b) => string.Compare(a.ProcessName, b.ProcessName));
            this.ProcessList.Invoke(new Action(() =>
            {
                for(int h=0; h<Form1.processes.Count; h++)
                {
                     if(h < this.ProcessList.Items.Count)
                    {
                        this.ProcessList.Items[h] = Form1.processes[h].ProcessName;
                    }
                    else
                    {
                        this.ProcessList.Items.Add(Form1.processes[h].ProcessName);
                    }
                }
            }));
        }

        private void updateCurrentProcessInfo()
        {
            int selected_process_pid = this.selected_process.Id;
            string process_ended = this.current_process_ended.Text,
                     process_status, processor_usage;
            try
            {
                this.selected_process = Process.GetProcessById(selected_process_pid);
            }
            catch(System.ArgumentException)
            {
                process_ended = "Esta activo: Inactivo";
            }
            this.current_process_memory_use.Invoke(new Action(() => this.current_process_memory_use.Text = $"Memoria Usada: {(this.selected_process.WorkingSet64 / (1024*1024))} MB"));
            this.current_process_id.Invoke(new Action(() => this.current_process_id.Text = $"PID: {(this.selected_process.Id)}"));
            try
            {  
                this.cpu_last_increase = this.cpu_counter.NextValue();
                processor_usage = $"CPU: {this.cpu_last_increase.ToString("0.##")}%";
                process_status = $"Status: {(this.selected_process.Responding ? "Responde" : "No responde")}";
            }
            catch (System.ComponentModel.Win32Exception)
            {
                process_status = "Status: Acceso denegado";
                process_ended = "Esta activo: Acceso denegado";
            }
            catch (System.InvalidOperationException)
            {
                this.selected_process = null;
                return;
            }
            this.current_process_ended.Invoke(new Action(() => this.current_process_ended.Text = process_ended));
            this.current_process_status.Invoke(new Action(() => this.current_process_status.Text = process_status));
            this.current_process_cpu_usage.Invoke(new Action(() => this.current_process_cpu_usage.Text = processor_usage));
            this.current_process_hddread.Invoke(new Action(() => this.current_process_hddread.Text = $"Operaciones de escritura: {this.ior_counter.NextValue().ToString("0.##")}"));
            this.current_process_hddwrite.Invoke(new Action(() => this.current_process_hddwrite.Text = $"Operaciones de lectura: {this.iow_counter.NextValue().ToString("0.##")}"));


            //Setting the point to draw
            this.drawInPerformance();
        }

        #endregion </Timer Functions>

        #region <Process Info Setters>


        private void setLabelText(Label label, string text)
        {
            label.Text = text;
        }

        private void setProcessListToListBox()
        {
            Form1.processes.Sort((Process a, Process b) => string.Compare(a.ProcessName, b.ProcessName));
            Form1.processes.ForEach(p => this.ProcessList.Items.Add(p.ProcessName));
        }

        private void SetProcessInfo()
        {
            this.resetGraphic(this.getMeasurableName());
            string process_ended, process_status, processor_usage;
            this.current_process_name.Text = this.selected_process.ProcessName;
            this.current_process_memory_use.Text = $"Memoria Usada: {(this.selected_process.WorkingSet64  / (1024 * 1024))} MB";
            this.current_process_id.Text = $"PID: {(this.selected_process.Id)}";
            this.cpu_counter = new PerformanceCounter("Proceso", "% de tiempo de procesador", this.selected_process.ProcessName);
            this.ior_counter = new PerformanceCounter("Proceso", "Operaciones de ES de lectura/s", this.selected_process.ProcessName);
            this.iow_counter = new PerformanceCounter("Proceso", "Operaciones de ES de escritura/s", this.selected_process.ProcessName);
            processor_usage = $"CPU: {this.cpu_counter.NextValue()}%";
            this.ior_counter.NextValue();
            this.iow_counter.NextValue();
            try
            {
                process_ended = $"Esta activo: {(!this.selected_process.HasExited ? "Activo" : "Inactivo")}";
                process_status = $"Status: {(this.selected_process.Responding ? "Responde" : "No responde")}";
            }
            catch(System.ComponentModel.Win32Exception)
            {
                process_status = "Status: Acceso denegado";
                process_ended = "Esta activo: Acceso denegado";
            }
            this.current_process_ended.Text = process_ended;
            this.current_process_status.Text = process_status;
            this.current_process_cpu_usage.Text = processor_usage;
        }

        private void ProcessList_Click(object sender, EventArgs e)
        {
            this.selected_process = Form1.processes[this.ProcessList.SelectedIndex];
            this.SetProcessInfo();
        }
        #endregion </Process Info Setters>

        #region <Process creator>
        private void create_process_btn_Click(object sender, EventArgs e)
        {
            ProcessStartInfo startInfo = new ProcessStartInfo();
            Process process = new Process();
            startInfo.FileName = "python.exe";
            startInfo.UseShellExecute = false;
            startInfo.Arguments = "D:\\ElMar\\Documents\\SoftwareProjects\\Respaldos\\createBackup.py";
            startInfo.WorkingDirectory = "D:\\ElMar\\Documents\\SoftwareProjects\\Respaldos\\";
            process.StartInfo = startInfo;
            process.Start();
            this.selected_process = process;
            this.SetProcessInfo();
        }
        #endregion </Process creator>

        #region <Misc>

        private void CountersBTN_ButtonClick(object sender, EventArgs e)
        {
            CountersListPreviewForm cc = new CountersListPreviewForm();
            if (cc.ShowDialog() != DialogResult.OK)
            {
                return;
            }
        }

        private string getMeasurableName()
        {
            switch (this.measureing)
            {
                case Measureables.CPU:
                    return "Cambios en CPU: ";
                case Measureables.RAM:
                    return "Uso memoria RAM: ";
                default:
                    return "Que raroo";
            }
        }

        private delegate void MeasureChange();

        private event MeasureChange measureChange;

        private void current_process_memory_use_Click(object sender, EventArgs e)
        {
            this.measureing = Measureables.RAM;
            this.measureChange();
        }

        private void current_process_cpu_usage_Click(object sender, EventArgs e)
        {
            this.measureing = Measureables.CPU;
            this.measureChange();
        }

        private static void SuspendProcess(int pid)
        {
            Process process = Process.GetProcessById(pid);
            foreach (ProcessThread pT in process.Threads)
            {
                IntPtr pOpenThread = OpenThread(ThreadAccess.SUSPEND_RESUME, false, (uint)pT.Id);
                if (pOpenThread == IntPtr.Zero)
                {
                    continue;
                }
                SuspendThread(pOpenThread);
                CloseHandle(pOpenThread);
            }
        }

        public static void ResumeProcess(int pid)
        {
            Process process = Process.GetProcessById(pid);
            if (process.ProcessName == string.Empty)
                return;
            foreach (ProcessThread pT in process.Threads)
            {
                IntPtr pOpenThread = OpenThread(ThreadAccess.SUSPEND_RESUME, false, (uint)pT.Id);
                if (pOpenThread == IntPtr.Zero)
                {
                    continue;
                }
                var suspendCount = 0;
                do
                {
                    suspendCount = ResumeThread(pOpenThread);
                } while (suspendCount > 0);
                CloseHandle(pOpenThread);
            }
        }

        #endregion </Misc>

        #region <Controls>

        private void detenerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form1.SuspendProcess(this.selected_process.Id);
        }

        private void resumirToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form1.ResumeProcess(this.selected_process.Id);
        }

        private void terminarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.selected_process != null)
            {
                string process_name = this.selected_process.ProcessName;
                foreach(Process p in Process.GetProcessesByName(process_name))
                {
                    p.CloseMainWindow();
                    p.Close();
                }
                this.selected_process = null;
                this.current_process_status.Invoke(this.changeLabelText, this.current_process_status, "Status: proceso terminado");
                this.current_process_ended.Invoke(this.changeLabelText, this.current_process_ended, "Esta activo: Inactivo");
            }
            else
            {
                MessageBox.Show("Seleccione un proceso no sea imbecil");
            }
        }


        #endregion </Controls>
    }
}
