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




    public partial class TaskManagerForm : Form
    {
        public static Dictionary<int,WinTask> processes;

        public static Dictionary<int,WinTask> GetProcesess()
        {
            Dictionary<int, WinTask> all_processes = new Dictionary<int, WinTask>();
            foreach(Process p in Process.GetProcesses())
            {
                all_processes[p.Id] = new WinTask(p);
            }
            return all_processes;
        }

        [DllImport("kernel32.dll")]
        static extern IntPtr OpenThread(ThreadAccess dwDesiredAccess, bool bInheritHandle, uint dwThreadId);
        [DllImport("kernel32.dll")]
        static extern uint SuspendThread(IntPtr hThread);
        [DllImport("kernel32.dll")]
        static extern int ResumeThread(IntPtr hThread);
        [DllImport("kernel32", CharSet = CharSet.Auto, SetLastError = true)]
        static extern bool CloseHandle(IntPtr handle);

        private delegate void ChangeLabelText(Label form_label, string text);
        private delegate void AlterListBox(ListBox listBox, string thing);

        private ulong total_ram = new Microsoft.VisualBasic.Devices.ComputerInfo().TotalPhysicalMemory;
        private Measureables measureing;
        private Bitmap performace_graphic;
        private List<int> performace_data;
        private WinTask selected_process;
        private System.Timers.Timer process_Info_updater;
        private int max_height;
        private ChangeLabelText changeLabelText;
        private AlterListBox addToListBox;
        private AlterListBox removeFromListBox;
        private Dictionary<int, WinTask> stopped_process;
        //private HashSet<IntPtr> 

        public TaskManagerForm()
        {
            InitializeComponent();
            TaskManagerForm.processes = TaskManagerForm.GetProcesess();
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
            this.addToListBox = new AlterListBox(this.addtoListBox);
            this.removeFromListBox = new AlterListBox(this.removefromListBox);
            this.stopped_process = new Dictionary<int, WinTask>();
            this.process_Info_updater.Start();
        }

        #region <Label functions>

        private string counterValueInerpreter(float counter_value)
        {
            string response = "default";
            if(counter_value >= 0f)
            {
                response = counter_value.ToString("0.##");
            }
            else if(counter_value == -1)
            {
                response = "Acceso denegado";
            }
            return response;
        }

        #endregion </Label functions>

        #region <Performance Graphic>

        private void resetGraphic(string graphic_name)
        {
            this.current_graphic_name.Text = graphic_name;
            this.performace_graphic = new Bitmap(250, this.max_height);
            this.PerformancePB.Image = (Image)this.performace_graphic;
            this.performace_data.Clear();
            this.performace_data.Add(this.max_height - 18);
        }

        private int getPointToDraw()
        {
            string return_value_string;
            float return_value=0;
            if(this.measureing == Measureables.RAM)
            {
                float process_increase_percentage = (float)this.selected_process.WorkingPhysicalMemory / (float)this.total_ram;
                return_value = (this.performace_graphic.Height * process_increase_percentage);
                return_value_string = return_value.ToString("0.##");
                this.current_graphic_name.Invoke(new Action(() => this.current_graphic_name.Text = $"{this.getMeasurableName() + return_value_string}%"));
                return_value = this.max_height - (return_value+18);
                //MessageBox.Show($"{this.selected_process.WorkingSet64} / { this.selected_process.PrivateMemorySize64 } = {process_increase_percentage.ToString("0.##")}");
            }
            else if(this.measureing == Measureables.CPU)
            {
                float last_cpu_measuer = (float)this.selected_process.LastMeasuerdCPUValue;
                return_value = (this.performace_graphic.Height - 18) - Convert.ToInt32((last_cpu_measuer/100) * this.performace_graphic.Height);
                this.current_graphic_name.Invoke(new Action(() => this.current_graphic_name.Text = $"{this.getMeasurableName()+(Convert.ToInt32((last_cpu_measuer / 100) * this.max_height))}%"));
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
            List<Process> current_processes =Process.GetProcesses().ToList();
            current_processes.Sort((Process a, Process b) => string.Compare(a.ProcessName, b.ProcessName));
            for (int h = 0; h <current_processes.Count ; h++)
            {
                if (this.stopped_process.ContainsKey(current_processes[h].Id))
                {
                    continue;
                }

                if (TaskManagerForm.processes.ContainsKey(current_processes[h].Id))
                {
                    //the process already existed
                    if (TaskManagerForm.processes[current_processes[h].Id].Status == PROCESS_STATUS.Terminated)
                    {
                        this.stopped_process[current_processes[h].Id] = TaskManagerForm.processes[current_processes[h].Id];
                        this.stoppedProcess.Invoke(new Action(() => this.stoppedProcess.Items.Add($"{current_processes[h].Id}: {this.stopped_process[current_processes[h].Id].Name}")));
                        this.ProcessList.Invoke(this.removeFromListBox, this.ProcessList, $"{current_processes[h].Id}: {this.stopped_process[current_processes[h].Id].Name}");
                        continue;
                    }
                }
                else
                {
                    //the process is new
                    WinTask winTask = new WinTask(current_processes[h]);
                    TaskManagerForm.processes[winTask.PID] = winTask;
                    this.ProcessList.Invoke(this.addToListBox, this.ProcessList, $"{current_processes[h].Id}: {current_processes[h].ProcessName}");
                }
            }
        }

        #endregion </Timer Functions>

        #region <Process Info Setters>

        private void updateCurrentProcessInfo()
        {
            string memory_label_text = this.selected_process.UsedRAMMb,
                     pid_label_text = $"PID: {(this.selected_process.PID)}",
                     status_label_text = $"Status: {this.selected_process.StatusText}",
                     cpu_label_text = $"CPU: {this.selected_process.NextCPUValue}%",
                     reads_label_text = $"Operaciones de escritura: {this.counterValueInerpreter(this.selected_process.NextIOreadValue)}",
                     writes_label_text = $"Operaciones de lectura: {this.counterValueInerpreter(this.selected_process.NextIOwriteValue)}";
            if (current_process_cpu_usage.InvokeRequired)
            {
                this.current_process_memory_use.Invoke(this.changeLabelText, this.current_process_memory_use, memory_label_text);
                this.current_process_id.Invoke(this.changeLabelText, this.current_process_id, pid_label_text);
                this.current_process_status.Invoke(this.changeLabelText, this.current_process_status, status_label_text);
                this.current_process_cpu_usage.Invoke(this.changeLabelText, this.current_process_cpu_usage, cpu_label_text);
                this.current_process_hddread.Invoke(this.changeLabelText, this.current_process_hddread, reads_label_text);
                this.current_process_hddwrite.Invoke(this.changeLabelText, this.current_process_hddwrite, writes_label_text);
            }
            else
            {
                this.current_process_name.Text = this.selected_process.Name;
                this.current_process_memory_use.Text = memory_label_text;
                this.current_process_id.Text = pid_label_text;
                this.current_process_status.Text = status_label_text;
                this.current_process_cpu_usage.Text = cpu_label_text;
                this.current_process_hddread.Text = reads_label_text;
                this.current_process_hddwrite.Text = writes_label_text;
                this.resetGraphic(this.getMeasurableName());
            }

            //Setting the point to draw
            this.drawInPerformance();
        }

        private void addtoListBox(ListBox listBox, string thing)
        {
            listBox.Items.Add(thing);
        }

        private void removefromListBox(ListBox listBox, string thing)
        {
            listBox.Items.Remove(thing);

        }

        private void setLabelText(Label label, string text)
        {
            label.Text = text;
        }

        private void setProcessListToListBox()
        {
            List<WinTask> current_processes = TaskManagerForm.processes.Values.ToList();
            current_processes.Sort((WinTask a, WinTask b) => string.Compare(a.Name, b.Name));
            current_processes.ForEach(p => this.ProcessList.Items.Add($"{p.PID}: {p.Name}"));
        }

        private void ProcessList_Click(object sender, EventArgs e)
        {
            string string_pid = (string)this.ProcessList.SelectedItem;
            string_pid = string_pid.Split(':')[0];
            this.selected_process = TaskManagerForm.processes[Convert.ToInt32(string_pid)];
            this.updateCurrentProcessInfo();
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
            this.selected_process = new WinTask(process);
            TaskManagerForm.processes[process.Id] = this.selected_process;
            this.updateCurrentProcessInfo();
        }
        #endregion </Process creator>

        #region <Misc>

        private int getPIDFromListBoxName(string name)
        {
            return Convert.ToInt32(name.Split(':')[0]);
        }

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
            TaskManagerForm.SuspendProcess(this.selected_process.PID);
        }

        private void resumirToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TaskManagerForm.ResumeProcess(this.selected_process.PID);
        }

        private void terminarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.selected_process != null)
            {
                string process_name = this.selected_process.RealName;
                foreach(Process p in Process.GetProcessesByName(process_name))
                {
                    this.stopped_process[p.Id] = this.selected_process;
                    p.CloseMainWindow();
                    p.Close();
                }
                this.selected_process = null;
                this.current_process_status.Invoke(this.changeLabelText, this.current_process_status, "Status: proceso terminado");
            }
            else
            {
                MessageBox.Show("Seleccione un proceso no sea especialito :3");
            }
        }


        #endregion </Controls>
    }
}
