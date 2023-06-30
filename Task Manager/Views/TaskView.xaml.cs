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
            timer.Interval = TimeSpan.FromSeconds(2);
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
                        ThreadCount = process.Threads.Count,
                        //Images = GetProcessImage(process.Id)
                    });

                    processCount++;
                    handleCount += process.HandleCount;
                    threadsCount += process.Threads.Count;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            

            TotalProcessCount.Text = $"Toplam Süreç\n{processCount.ToString()}";
                TotalHandleCount.Text = $"Toplam Kulp\n{handleCount.ToString()}";
                TotalThreadsCount.Text = $"Toplam İş Parçacığı\n{threadsCount.ToString()}";

            }
        }

        //private BitmapImage GetProcessImage(int processId)
        //{
        //    try
        //    {
        //        Process process = Process.GetProcessById(processId);
        //        using (Icon icon = Icon.ExtractAssociatedIcon(process.MainModule?.FileName))
        //        {
        //            if (icon != null)
        //            {
        //                using (MemoryStream stream = new MemoryStream())
        //                {
        //                    // Convert the Icon to Bitmap
        //                    Bitmap bitmap = icon.ToBitmap();
        //                    bitmap.Save(stream, System.Drawing.Imaging.ImageFormat.Png);
        //                    stream.Seek(0, SeekOrigin.Begin);

        //                    // Create a BitmapImage from the stream
        //                    BitmapImage bitmapImage = new BitmapImage();
        //                    bitmapImage.BeginInit();
        //                    bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
        //                    bitmapImage.StreamSource = stream;
        //                    bitmapImage.EndInit();
        //                    bitmapImage.Freeze();

        //                    return bitmapImage;
        //                }
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        // Handle the error appropriately
        //        Console.WriteLine("Error: " + ex.Message);
        //    }

        //    // Return a default image if no image is found or an error occurs
        //    return new BitmapImage(new Uri("default_image.png", UriKind.Relative));
        //}


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
                BlackList blackList = new BlackList();
                Window.GetWindow(this);
                blackList.Show();

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

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (ListViewManager.SelectedItem is ProcessInfo selectedProcess)
            {
                try
                {
                    Process.GetProcessById(selectedProcess.ProcessId).Kill();
                }
                catch (System.Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
    }
    public class ProcessData
    {
        public string ProcessName { get; set; }
        public string FileName { get; set; }
        public int HandleCount { get; set; }
        public int ThreadCount { get; set; }
        public BitmapImage Images { get; set; }
    }
}

