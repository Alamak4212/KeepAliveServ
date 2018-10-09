using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;

namespace keepAliveServ
{

    public partial class keepAliveService : ServiceBase
    {
        public enum ServiceState
        {
            SERVICE_STOPPED = 0x00000001,
            SERVICE_START_PENDING = 0x00000002,
            SERVICE_STOP_PENDING = 0x00000003,
            SERVICE_RUNNING = 0x00000004,
            SERVICE_CONTINUE_PENDING = 0x00000005,
            SERVICE_PAUSE_PENDING = 0x00000006,
            SERVICE_PAUSED = 0x00000007,
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct ServiceStatus
        {
            public long dwServiceType;
            public ServiceState dwCurrentState;
            public long dwControlsAccepted;
            public long dwWin32ExitCode;
            public long dwServiceSpecificExitCode;
            public long dwCheckPoint;
            public long dwWaitHint;
        };
        struct ProcData
        {
            public string szappname;
            public string szapppath;
            public string szargument;
            public int imonitorport;
        }
        private ProcData[] DataMonitorProc = new ProcData[4];
        
        private int counter;
        private int NumProc = 0;
        System.Timers.Timer timer = new System.Timers.Timer();

        public keepAliveService()
        {
            InitializeComponent();
            NumProc = GetProcName();
            counter = 0;
        }
        public void RunAsConsole(string[] args)
        {
            OnStart(args);
            OnElapsedTime(this, null);
            Console.WriteLine("Press any key to exit...");
            Console.ReadLine();
            OnStop();
        }
        private int GetProcName()
        {
            int used = 0;
            string patch = "";
            try
            {
                if (Properties.Settings.Default.AppName1st == null || Properties.Settings.Default.AppName1st.Length <= 0)
                {
                    TraceService("Process Init : " + "AppName1st Parameter Not Set " + DateTime.Now);
                    return used;
                }
                else
                {
                    if (Properties.Settings.Default.AppPath1st != null)
                    {
                        if (!File.Exists(Properties.Settings.Default.AppPath1st + "//" + Properties.Settings.Default.AppName1st + ".exe"))
                        {
                            TraceService("Process Init : " + "1st App File Not Found In Path Folder" + DateTime.Now);
                            if (!File.Exists(Properties.Settings.Default.AppName1st + ".exe"))
                            {
                                TraceService("Process Init : " + "1st App File Not Found In Current Folder" + DateTime.Now);
                                return used;
                            }
                            patch = Properties.Settings.Default.AppName1st + ".exe";

                        }
                        else
                            patch = Properties.Settings.Default.AppPath1st + "//" + Properties.Settings.Default.AppName1st + ".exe";
                    }
                    else
                    {
                        if (!File.Exists(Properties.Settings.Default.AppName1st + ".exe"))
                        {
                            TraceService("Process Init : " + "1st App File Not Found In Current Folder" + DateTime.Now);
                            return used;
                        }
                        patch = Properties.Settings.Default.AppName1st + ".exe";
                    }
                    DataMonitorProc[used].szappname = Properties.Settings.Default.AppName1st;
                    DataMonitorProc[used].szapppath = patch;
                    DataMonitorProc[used].szargument = Properties.Settings.Default.AppArg1;
                    DataMonitorProc[used].imonitorport = Properties.Settings.Default.MonitorPort_App1;
                    used++;
                }
                //2

                if (Properties.Settings.Default.AppName2nd == null || Properties.Settings.Default.AppName2nd.Length <= 0)
                {
                    TraceService("Process Init : " + "AppName2nd Parameter Not Set " + DateTime.Now);
                    return used;
                }
                else
                {
                    if (Properties.Settings.Default.AppPath2nd != null)
                    {
                        if (!File.Exists(Properties.Settings.Default.AppPath2nd + "//" + Properties.Settings.Default.AppName2nd + ".exe"))
                        {
                            TraceService("Process Init : " + "2nd App File Not Found In Path Folder" + DateTime.Now);
                            if (!File.Exists(Properties.Settings.Default.AppName2nd + ".exe"))
                            {
                                TraceService("Process Init : " + "2nd App File Not Found In Current Folder" + DateTime.Now);
                                return used;
                            }
                            patch = Properties.Settings.Default.AppName2nd + ".exe";
                        }
                        else
                            patch = Properties.Settings.Default.AppPath2nd + "//" + Properties.Settings.Default.AppName2nd + ".exe";
                    }
                    else
                    {
                        if (!File.Exists(Properties.Settings.Default.AppName2nd + ".exe"))
                        {
                            TraceService("Process Init : " + "2nd App File Not Found In Current Folder" + DateTime.Now);
                            return used;
                        }
                        patch = Properties.Settings.Default.AppName2nd + ".exe";
                    }
                    DataMonitorProc[used].szappname = Properties.Settings.Default.AppName2nd;
                    DataMonitorProc[used].szapppath = patch;
                    DataMonitorProc[used].szargument = Properties.Settings.Default.AppArg2;
                    DataMonitorProc[used].imonitorport = Properties.Settings.Default.MonitorPort_App2;
                    used++;
                }
                //3

                if (Properties.Settings.Default.AppName3rd == null || Properties.Settings.Default.AppName3rd.Length <= 0)
                {
                    TraceService("Process Init : " + "AppName3rd Parameter Not Set " + DateTime.Now);
                    return used;
                }
                else
                {
                    if (Properties.Settings.Default.AppPath3rd != null)
                    {
                        if (!File.Exists(Properties.Settings.Default.AppPath3rd + "//" + Properties.Settings.Default.AppName3rd + ".exe"))
                        {
                            TraceService("Process Init : " + "1st App File Not Found In Path Folder" + DateTime.Now);
                            if (!File.Exists(Properties.Settings.Default.AppName3rd + ".exe"))
                            {
                                TraceService("Process Init : " + "1st App File Not Found In Current Folder" + DateTime.Now);
                                return used;
                            }
                            patch = Properties.Settings.Default.AppName3rd + ".exe";
                        }
                        else
                            patch = Properties.Settings.Default.AppPath3rd + "//" + Properties.Settings.Default.AppName3rd + ".exe";
                    }
                    else
                    {
                        if (!File.Exists(Properties.Settings.Default.AppName3rd + ".exe"))
                        {
                            TraceService("Process Init : " + "1st App File Not Found In Current Folder" + DateTime.Now);
                            return used;
                        }
                        patch = Properties.Settings.Default.AppName3rd + ".exe";
                    }
                    DataMonitorProc[used].szappname = Properties.Settings.Default.AppName3rd;
                    DataMonitorProc[used].szapppath = patch;
                    DataMonitorProc[used].szargument = Properties.Settings.Default.AppArg3;
                    DataMonitorProc[used].imonitorport = Properties.Settings.Default.MonitorPort_App3;
                    used++;
                }
                //4
#if false
                if (Properties.Settings.Default.PSName == null || Properties.Settings.Default.PSName.Length <= 0)
                {
                    TraceService("Process Init : " + "PinPad Server Parameter Not Set " + DateTime.Now);
                    return used;
                }
                else
                {
                    if (Properties.Settings.Default.PSPath != null)
                    {
                        if (!File.Exists(Properties.Settings.Default.PSPath + "//" + Properties.Settings.Default.PSName + ".exe"))
                        {
                            TraceService("Process Init : " + "PinPad Server App File Not Found In Path Folder" + DateTime.Now);
                            if (!File.Exists(Properties.Settings.Default.PSName + ".exe"))
                            {
                                TraceService("Process Init : " + "PinPad Server App File Not Found In Current Folder" + DateTime.Now);
                                return used;
                            }
                            patch = Properties.Settings.Default.PSName + ".exe";
                        }
                        else
                            patch = Properties.Settings.Default.PSPath + "//" + Properties.Settings.Default.PSName + ".exe";
                    }
                    else
                    {
                        if (!File.Exists(Properties.Settings.Default.PSName + ".exe"))
                        {
                            TraceService("Process Init : " + "PinPad Server App File Not Found In Current Folder" + DateTime.Now);
                            return used;
                        }
                        patch = Properties.Settings.Default.PSName + ".exe";
                    }
                    DataMonitorProc[used].szappname = Properties.Settings.Default.PSName;
                    DataMonitorProc[used].szapppath = patch;
                    DataMonitorProc[used].szargument = Properties.Settings.Default.PSArg;
                    DataMonitorProc[used].imonitorport = Properties.Settings.Default.MonitorPort_PS;
                    used++;
                }
                //5

                if (Properties.Settings.Default.SOTrigerName == null || Properties.Settings.Default.SOTrigerName.Length <= 0)
                {
                    TraceService("Process Init : " + "Sign On App Parameter Not Set " + DateTime.Now);
                    return used;
                }
                else
                {
                    if (Properties.Settings.Default.SOTrigerPath != null)
                    {
                        if (!File.Exists(Properties.Settings.Default.SOTrigerPath + "//" + Properties.Settings.Default.SOTrigerName + ".exe"))
                        {
                            TraceService("Process Init : " + "Sign On App File Not Found In Path Folder" + DateTime.Now);
                            if (!File.Exists(Properties.Settings.Default.SOTrigerName + ".exe"))
                            {
                                TraceService("Process Init : " + "Sign On App File Not Found In Current Folder" + DateTime.Now);
                                return used;
                            }
                            patch = Properties.Settings.Default.SOTrigerName + ".exe";
                        }
                        else
                            patch = Properties.Settings.Default.SOTrigerPath + "//" + Properties.Settings.Default.SOTrigerName + ".exe";
                    }
                    else
                    {
                        if (!File.Exists(Properties.Settings.Default.SOTrigerName + ".exe"))
                        {
                            TraceService("Process Init : " + "Sign On App File Not Found In Current Folder" + DateTime.Now);
                            return used;
                        }
                        patch = Properties.Settings.Default.SOTrigerName + ".exe";
                    }
                    DataMonitorProc[used].szappname = Properties.Settings.Default.SOTrigerName;
                    DataMonitorProc[used].szapppath = patch;
                    DataMonitorProc[used].szargument = Properties.Settings.Default.SOTrigerArg;
                    DataMonitorProc[used].imonitorport = Properties.Settings.Default.MonitorPort_SignOn;
                    used++;
                }
#endif
            }
            catch (Exception)
            {
                return used;
            }
            return used;
        }//4212
        private int checkAppOpened(string applname)//asli dari pk dani
        {
            int ret = -1;

            Process[] procs = Process.GetProcesses();
            foreach (Process proc in procs)
            {
                if (proc.ProcessName == applname)
                {
                    /* get process id */
                    ret = proc.Id;
                    return ret;
                }
            }

            return ret;
        }
        private int checkAppOpened(ProcData[] ProcDataColl)
        {
            int idx = 0;
            foreach (ProcData Data in ProcDataColl)
            {
                if (idx >= NumProc)
                {
                    break;
                }
                if (checkAppOpened(Data.szappname) >= 0)
                {
                    idx++;
                }
                else
                {
                    break;
                }
            }
            return idx;
        }
        private int ScanPort(int portno)
        {
            if (portno <= 0)
                return 0;
            string hostname = "localhost";
            IPAddress ipa = (IPAddress)Dns.GetHostAddresses(hostname)[0];
            try
            {
                System.Net.Sockets.Socket sock =
                        new System.Net.Sockets.Socket(System.Net.Sockets.AddressFamily.InterNetwork,
                                                      System.Net.Sockets.SocketType.Stream,
                                                      System.Net.Sockets.ProtocolType.Tcp);
                sock.Connect(ipa, portno);
                if (sock.Connected == true) // Port is in use and connection is successful
                    return 0;
                sock.Close();
            }
            catch (System.Net.Sockets.SocketException ex)
            {
                if (ex.ErrorCode == 10061) // Port is unused and could not establish connection 
                {
                    TraceService("Port " + portno + " Is Unused and could not establish connection");
                    return -1;
                }
                else
                {
                    TraceService("Port " + portno + " With Error " + ex.Message);
                    return -2;
                }
            }
            TraceService("Port " + portno + " Client connection Unsuccesful");
            return -3;
        }
        private int checkAppRunningOk(ProcData[] ProcDataColl)
        {
            int idx = 0;
            foreach (ProcData Data in ProcDataColl)
            {
                if (idx >= NumProc)
                {
                    break;
                }
                if (checkAppRespond(Data.szappname) >= 0 && ScanPort(Data.imonitorport) >= 0)
                {
                    idx++;
                }
                else
                {
                    break;
                }
            }
            return idx;
        }
        private int checkAppRespond(string applname)//4212 edited from appNotResponding
        {
            int ret = -1;

            Process[] procs = Process.GetProcesses();
            foreach (Process proc in procs)
            {
                if (proc.ProcessName == applname && proc.Responding)
                {
                    ret = proc.Id;
                    return ret;
                }
            }

            return ret;
        }
        private bool processStart(ProcData ProcesToStart)
        {
            string errorMessage = "";
            string outputMessage = "";
            if(checkAppOpened(ProcesToStart.szappname) > 0)
            {
                TraceService("Process Start, Set infofilename = " + ProcesToStart.szappname + " Is Ready");
                return true;
            }
            try
            {
                using (Process myProcess = new Process())
                {
                    TraceService("Process Start, Set infofilename = " + ProcesToStart.szapppath);
                    myProcess.StartInfo.FileName = ProcesToStart.szapppath;
                    FileInfo FI = new FileInfo(ProcesToStart.szapppath);
                    myProcess.StartInfo.WorkingDirectory = FI.Directory.ToString();
                    //if (ProcesToStart.szappname.Contains("_CON"))
                    //{
                    //    myProcess.StartInfo.RedirectStandardError = false;
                    //    myProcess.StartInfo.RedirectStandardOutput = false;
                    //    myProcess.StartInfo.UseShellExecute = true;
                    //}
                    //else
                    //{
                    //    myProcess.StartInfo.RedirectStandardError = true;
                    //    myProcess.StartInfo.RedirectStandardOutput = true;
                    //    myProcess.StartInfo.UseShellExecute = false;
                    //}
                    

                    myProcess.StartInfo.WindowStyle = ProcessWindowStyle.Normal;
                    myProcess.StartInfo.Arguments = ProcesToStart.szargument;
                    //myProcess.StartInfo.UserName = Properties.Settings.Default.User;
                    //myProcess.StartInfo.Password. = Properties.Settings.Default.User_Password as System.Security.SecureString;

                    counter = 0;
                    myProcess.Start();


                    //errorMessage = myProcess.StandardError.ReadToEnd();
                    //myProcess.WaitForExit();
                    //outputMessage = myProcess.StandardOutput.ReadToEnd();
                    //myProcess.WaitForExit();

                    myProcess.Close();
                    TraceService("Process " + ProcesToStart.szappname + " Started by KeepAlive");
                }
            }
            catch (InvalidOperationException inEx)
            {
                TraceService("Process Start InvalidOperationException : " + inEx.Message);
                return false;
            }
            catch (Win32Exception winEx)
            {
                TraceService("Process Start Win32Exception : " + winEx.Message);
                return false;
            }
            catch (Exception ex)
            {
                TraceService("Process Start Exception : " + ex.Message);
                return false;
            }
            finally
            {
                //TraceService("Process Start, StandardError : " + errorMessage);
                //TraceService("Process Start, StandardOutput : " + outputMessage);
            }
            return true;

        }//4212 edited
        private int processStart(ProcData[] ProcDataColl)
        {
            int idx = 0;
            foreach(ProcData Data in ProcDataColl)
            {
                if(idx >= NumProc)
                {
                    break;
                }
                if(processStart(Data))
                {
                    idx++;
                    Thread.Sleep(TimeSpan.FromSeconds(10));
                }
                else
                {
                    break;
                }
            }
            return idx;
        }
        private int processKill(string applname)
        {
            int ret = 0;
            try
            {
                Process[] procs = Process.GetProcesses();
                foreach (Process proc in procs)
                {
                    if (proc.ProcessName == applname)
                    {
                        ret = proc.Id;
                        proc.Kill();
                        TraceService("Process " + applname + " Killed by KeepAlive");
                        return ret;
                    }
                }
            }
            catch(Exception)
            {
                return -1;
            }

            return ret;//ret 0 if app doesnt run
        }
        private int processKill(ProcData[] ProcDataColl)
        {
            int idx = 0;
            foreach (ProcData Data in ProcDataColl)
            {
                if (idx >= NumProc)
                {
                    break;
                }
                if (processKill(Data.szappname) >= 0)
                {
                    idx++;
                }
                else
                {
                    break;
                }
            }
            return idx;
        }
        private int processKill(ProcData[] ProcDataColl,int idxStart)
        {
            int idx = 0;
            foreach (ProcData Data in ProcDataColl)
            {
                if(idx < idxStart)
                {
                    idx++;
                    continue;
                }
                if (idx >= NumProc)
                {
                    break;
                }
                if (processKill(Data.szappname) >= 0)
                {
                    idx++;
                }
                else
                {
                    break;
                }
            }
            return idx;
        }
        private int StopAllProcs()
        {
            return processKill(DataMonitorProc, 0);
        }
        private int MonitoringProcs()
        {
            int appRun = 0;
            int appRunOk = 0;
            string tracemsg = "";
            appRun = checkAppOpened(DataMonitorProc);
            appRunOk = checkAppRunningOk(DataMonitorProc);
            if (appRunOk >= NumProc)
            {
                return appRunOk;
            }
            if(appRun < NumProc)
            {
                tracemsg = "Application No : " + (appRun+1).ToString() + " Process Not Found";
                TraceService(tracemsg);
            }
            else if (appRunOk < NumProc)
            {
                tracemsg = "Application No : " + (appRunOk+1).ToString() + " Process Not Responding";
                TraceService(tracemsg);
            }
            int i = 0;
            i = processKill(DataMonitorProc, appRunOk);
            i = processStart(DataMonitorProc);
            return 0;
        }
        protected override void OnStart(string[] args)
        {
            // Update the service state to Start Pending.
            ServiceStatus serviceStatus = new ServiceStatus();
            serviceStatus.dwCurrentState = ServiceState.SERVICE_START_PENDING;
            serviceStatus.dwWaitHint = 100000;
            SetServiceStatus(this.ServiceHandle, ref serviceStatus);

            //add this line to text file during start of service

            TraceService("Start service " + ServiceName);//4212
            //handle Elapsed event
            timer.Elapsed += new ElapsedEventHandler(OnElapsedTime);

            //This statement is used to set interval to 1 minute (= 60,000 milliseconds)
            timer.Interval = 15000;

            //enabling the timer
            timer.Enabled = true;

            // Update the service state to Running.
            serviceStatus.dwCurrentState = ServiceState.SERVICE_RUNNING;
            SetServiceStatus(this.ServiceHandle, ref serviceStatus);

        }
        protected override void OnStop()
        {
            timer.Enabled = false;
            StopAllProcs();
            TraceService("stopping service" + DateTime.Now);
            counter = 0;
        }
        private DateTime LastMonitorDate = DateTime.Now;
        private void OnElapsedTime(object source, ElapsedEventArgs e)
        {
            TraceService("OnElapsedTime " + DateTime.Now);
            if (DateTime.Now.Day != LastMonitorDate.Day || 
                (DateTime.Now.Hour  == Properties.Settings.Default.TimeRestart_1 && DateTime.Now.Hour != LastMonitorDate.Hour) ||
                (DateTime.Now.Hour  == Properties.Settings.Default.TimeRestart_2 && DateTime.Now.Hour != LastMonitorDate.Hour) ||
                (DateTime.Now.Hour  == Properties.Settings.Default.TimeRestart_3 && DateTime.Now.Hour != LastMonitorDate.Hour)
                )//untuk restart program 12 / 17
            {
                TraceService("OnElapsedTime Start : Stop All Proccess (EOD)");
                StopAllProcs();
                LastMonitorDate = DateTime.Now;
            }
            if (MonitoringProcs() > 0)
            {
                TraceService("OnElapsedTime Ended : No Problem Detected");
            }
            else
            {
                TraceService("OnElapsedTime Ended : Problem Solve");
                counter++;
            }
        }
        private void TraceService(string content)
        {
            try
            {
                if (Environment.UserInteractive)
                {
                    Console.WriteLine(content);
                }
                if (Properties.Settings.Default.Trace==false)
                {
                    return;
                }
                //set up a filestream
                DateTime time = DateTime.Now;
                string filenamepath = Directory.GetCurrentDirectory() + "\\" + time.ToString("yyMMdd") + ".keepalive.log";
                if (Properties.Settings.Default.BatchFileFolder != null || Properties.Settings.Default.BatchFileFolder.Length > 0 || Directory.Exists(Properties.Settings.Default.BatchFileFolder))
                {
                    filenamepath = Properties.Settings.Default.BatchFileFolder + "\\" + time.ToString("yyMMdd") + ".keepalive.log";
                }

                FileStream fs = new FileStream(@filenamepath, FileMode.OpenOrCreate, FileAccess.Write);

                //set up a streamwriter for adding text
                StreamWriter sw = new StreamWriter(fs);

                //find the end of the underlying filestream
                sw.BaseStream.Seek(0, SeekOrigin.End);

                //add the text
                sw.WriteLine(content);
                //add the text to the underlying filestream

                sw.Flush();

                //close the writer
                sw.Close();
            }
            catch(Exception e)
            {

            }
            finally
            {

            }
        }
        [DllImport("advapi32.dll", SetLastError = true)]
        private static extern bool SetServiceStatus(IntPtr handle, ref ServiceStatus serviceStatus);


    }
}
