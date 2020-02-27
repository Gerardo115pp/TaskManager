using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Threading.Tasks;

namespace TaskManager
{
    public enum PROCESS_STATUS
    {
        Active=0,
        Suspended=1,
        Terminated=2
    }
    public class WinTask
    {

        private PerformanceCounter cpu_counter;
        private PerformanceCounter ior_counter; //input output read
        private PerformanceCounter iow_counter;//input output write
        private Process process_instance;
        private int pid;
        private float cpu_last_increase;
        private string process_alias;
        private bool is_accesable;
        private PROCESS_STATUS status;
        
        public WinTask(Process process)
        {
            this.process_instance = process;
            this.is_accesable = true;
            this.pid = process.Id;
            this.status = PROCESS_STATUS.Active;
            this.process_alias = process.ProcessName;
            this.startProcess();
         }

        #region <Getters&Setters>

        public long WorkingPhysicalMemory
        {
            get { return this.process_instance.WorkingSet64; }
        }

        public string UsedRAMMb
        {
            get { return $"{(this.process_instance.WorkingSet64 / (1024 * 1024))} MB"; }
        }

        public float? LastMeasuerdCPUValue
        {
            get { 
                if (this.cpu_last_increase != default(float))
                {
                    return this.cpu_last_increase; 
                }
                return null;
            }
        }

        public int PID
        {
            get { return this.pid;  }
        }

        public float NextCPUValue
        {
            get
            {
                if(!is_accesable)
                {
                    return -1f;
                }

                if (this.Status != PROCESS_STATUS.Terminated)
                {
                    try
                    {
                        this.cpu_last_increase = this.cpu_counter.NextValue();
                    }
                    catch(System.ComponentModel.Win32Exception)
                    {
                        this.is_accesable = false;
                        this.cpu_last_increase = -1f;
                    }
                    return this.cpu_last_increase;
                }
                else
                {
                    return 0f;
                }
            }
        }

        public float NextIOreadValue
        {
            get
            {
                float response;
                if (!is_accesable)
                {
                    return -1f;
                }

                if (this.Status != PROCESS_STATUS.Terminated)
                {
                    try
                    {
                        response = this.ior_counter.NextValue();
                    }
                    catch (System.ComponentModel.Win32Exception)
                    {
                        this.is_accesable = false;
                        response = -1f;
                    }
                    return response;
                }
                else
                {
                    return 0f;
                }
            }
        }

        public float NextIOwriteValue
        {
            get
            {
                float response;
                if (!is_accesable)
                {
                    return -1f;
                }

                if (this.Status != PROCESS_STATUS.Terminated)
                {
                    try
                    {
                        response = this.iow_counter.NextValue();
                    }
                    catch (System.ComponentModel.Win32Exception)
                    {
                        this.is_accesable = false;
                        response = -1f;
                    }
                    return response;
                }
                else
                {
                    return 0f;
                }
            }
        }

        public string Name
        {
            get { return this.process_alias; }
            set { this.process_alias = value;  }
        }

        public string RealName
        {
            get { return this.process_instance.ProcessName;  }
        }

        public PROCESS_STATUS Status
        {
            get {
                if(this.status != PROCESS_STATUS.Terminated)
                {
                    this.refreshProcessState();
                }
                return this.status; 
            }
        }

        public string StatusText
        {
            get { 
                switch(this.status)
                {
                    case PROCESS_STATUS.Terminated:
                        return "Finalizado";
                    case PROCESS_STATUS.Suspended:
                        return "Suspendido";
                    default:
                        return "Activo";
                }
            }
        }

        #endregion </Getters&Setters>


        #region <initializers>

        private void startProcess()
        {
            this.setCounters();
        }

        private void setCounters()
        {
            this.cpu_counter = new PerformanceCounter("Proceso", "% de tiempo de procesador", this.process_instance.ProcessName);
            this.ior_counter = new PerformanceCounter("Proceso", "Operaciones de ES de lectura/s", this.process_instance.ProcessName);
            this.iow_counter = new PerformanceCounter("Proceso", "Operaciones de ES de escritura/s", this.process_instance.ProcessName);
        }

        #endregion </initializer>

        public void refreshProcessState()
        {
            if(this.status != PROCESS_STATUS.Terminated)
            {
                try
                {
                    this.process_instance = Process.GetProcessById(this.pid);
                }
                catch(System.ArgumentException)
                {
                    this.status = PROCESS_STATUS.Terminated;
                }
            }

        }
    }
}
