/**
 * Copyright (c) 2014 Microsoft Mobile. All rights reserved.
 *
 * Nokia and Nokia Connecting People are registered trademarks of Nokia Corporation.
 * Other product and company names mentioned herein may be trademarks
 * or trade names of their respective owners.
 *
 * See the license text file for license information.
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;

namespace OpticalReaderApp
{
    public partial class MainPage : PhoneApplicationPage
    {
        private OpticalReaderLib.IProcessor _processor = null;
        private OpticalReaderLib.OpticalReaderTask _task = null;
        private OpticalReaderLib.OpticalReaderResult _taskResult = null;
        private int _size = 3;

        public MainPage()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            

            if (_taskResult != null)
            {
                if (_taskResult.TaskResult == Microsoft.Phone.Tasks.TaskResult.OK)
                {
                    System.Diagnostics.Debug.WriteLine("Code read successfully");

                    var byteCount = _taskResult.Data != null ? _taskResult.Data.Length : 0;

                  
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine("Code reading aborted");
                }

                _taskResult = null;
            }

            ReadCodeButton_Click();

          
        }

        private void ReadCodeButton_Click()
        {
            

            if (_task != null)
            {
                _task.Completed -= OpticalReaderTask_Completed;
                _task.Dispose();
                _task = null;
            }

            var showDebugInformation =false;
            var useCustomProcessor = false;
            var focusInterval = new TimeSpan(0, 0, 0, 0, 2500);
            var objectSize = _size <= 10 ? new Windows.Foundation.Size(_size * 10, _size * 10) : new Windows.Foundation.Size(0, 0);
            var requireConfirmation = true;

            if (useCustomProcessor)
            {
                _processor = new CustomProcessor();

                focusInterval = new TimeSpan(0, 0, 0, 0, 4000);
            }
            else
            {
                _processor = new OpticalReaderLib.ZxingProcessor();
            }

            _task = new OpticalReaderLib.OpticalReaderTask()
            {
                Processor = _processor,
                ShowDebugInformation = showDebugInformation,
                FocusInterval = focusInterval,
                ObjectSize = objectSize,
                RequireConfirmation = false,

            };

            _task.Completed += OpticalReaderTask_Completed;
            _task.Show();
            
        }

        private void OpticalReaderTask_Completed(object sender, OpticalReaderLib.OpticalReaderResult e)
        {
            _taskResult = e;
            if (_taskResult.Text != null)
            {
                String thing = _taskResult.Text;
                System.Windows.MessageBox.Show(thing);
            }
        }

  

        

        
    }
}