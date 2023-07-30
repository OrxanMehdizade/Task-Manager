using Microsoft.TeamFoundation.WorkItemTracking.Process.WebApi.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace Task_Manager.Views
{
    /// <summary>
    /// Interaction logic for TaskView.xaml
    /// </summary>
    public partial class TaskView : Window
    {
        public ObservableCollection<ProcessData> GetManager { get; } = new ObservableCollection<ProcessData>();
        private DispatcherTimer timer;
        public TaskView()
        {
            InitializeComponent();
            ListViewManager.ItemsSource = GetManager;
            InitializeTimer();


        }
        private void InitializeTimer()
        {
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(5);
            timer.Tick += Timer_Tick;
            timer.Start();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            RefreshProcesses();
        }
        private void RefreshProcesses()
        {
            GetManager.Clear();
            Process[] processes = Process.GetProcesses();
            int processCount = 0;
            int threadsCount = 0;
            int handleCount = 0;

            foreach (Process process in processes)
            {
                try
                {
                    GetManager.Add(new ProcessData
                    {
                        ProcessName = process.ProcessName,
                        FileName = process.MainModule?.FileName,
                        HandleCount = process.HandleCount,
                        ThreadCount = process.Threads.Count
                    });

                    processCount++;
                    handleCount += process.HandleCount;
                    threadsCount += process.Threads.Count;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            

            TotalProcessCount.Text = $"Total Process\n{processCount.ToString()}";
                TotalHandleCount.Text = $"Total Handle\n{handleCount.ToString()}";
                TotalThreadsCount.Text = $"Total Threads\n{threadsCount.ToString()}";

            }
           // CheckBlacklistedProcesses();
        }







        //private void CheckBlacklistedProcesses()
        //{
        //    List<ProcessData> blacklistedProcesses = GetManager.Where(p => BlackList.GetBlacklistedProcesses().Contains(p.ProcessName)).ToList();
        //    if (blacklistedProcesses.Count > 0)
        //    {
        //        foreach (ProcessData blacklistedProcess in blacklistedProcesses)
        //        {
        //            Process process = Process.GetProcessesByName(blacklistedProcess.ProcessName).FirstOrDefault();
        //            if (process != null)
        //            {
        //                process.Kill();
        //                GetManager.Remove(blacklistedProcess);
        //            }
        //        }
        //    }
        //}








        private void EndTaskMenuItem_Click(object sender, RoutedEventArgs e)
        {
            if (ListViewManager.SelectedItems.Count > 0)
            {
                List<ProcessData> selectedProcesses = new List<ProcessData>();
                foreach (var selectedItem in ListViewManager.SelectedItems)
                {
                    selectedProcesses.Add((ProcessData)selectedItem);
                }

                foreach (ProcessData selectedProcess in selectedProcesses)
                {
                    Process process = Process.GetProcessesByName(selectedProcess.ProcessName).FirstOrDefault();
                    if (process != null)
                    {
                        process.Kill();
                        GetManager.Remove(selectedProcess);
                    }
                }
            }
        }









        private void OpenWindow(int count)
        {
            if (count == 1)
            {
                NewTask newTask = new NewTask();
                Window.GetWindow(this);
                newTask.Show();
            }
            else if (count == 2)
            {
                //BlackList blackList = new BlackList();
                //Window.GetWindow(this);
                //blackList.Show();
                MessageBox.Show("Tamamlanmıyıb");
            }
        }

        private void NewTaskButton_Click(object sender, RoutedEventArgs e)
        {
            OpenWindow(1);
        }

        private void BlackListButton_Click(object sender, RoutedEventArgs e)
        {
            OpenWindow(2);

        }

    }
    public class ProcessData
    {
        public string ProcessName { get; set; }
        public string FileName { get; set; }
        public int HandleCount { get; set; }
        public int ThreadCount { get; set; }

    }
}

