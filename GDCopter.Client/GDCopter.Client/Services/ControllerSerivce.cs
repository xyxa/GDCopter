﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;
using GDCopter.Client.Models;

namespace GDCopter.Client.Services
{
    public class ControllerSerivce : ServiceBase
    {
        private readonly DispatcherTimer _timer = new DispatcherTimer();

        public ControllerSerivce(CommunicationModule communicationModule, ControllerModel dataModel)
            : base(communicationModule, dataModel)
        {
            _timer.Interval = TimeSpan.FromMilliseconds(50);
            _timer.Tick += TimerTick;
        }

        private void TimerTick(object sender, EventArgs e)
        {
            if (IsRunning && CommunicationModule.LastMessage != null)
            {
                var message = CommunicationModule.LastMessage;
                var dataModel = (ControllerModel)Model;
                CommunicationModule.OutputMessage.Rotor1 = (float)dataModel.CurrentThrottle;
                CommunicationModule.OutputMessage.Rotor2 = (float)dataModel.CurrentThrottle;
                CommunicationModule.OutputMessage.Rotor3 = (float)dataModel.CurrentThrottle;
                CommunicationModule.OutputMessage.Rotor4 = (float)dataModel.CurrentThrottle;
                dataModel.Throttle.Add(message.Pressure);
                dataModel.Rotor1.Add(message.Rotor1);
                dataModel.Rotor2.Add(message.Rotor2);
                dataModel.Rotor3.Add(message.Rotor3);
                dataModel.Rotor4.Add(message.Rotor4);

                if (dataModel.Rotor1.Count > 50)
                    dataModel.Rotor1.RemoveAt(0);
                if (dataModel.Rotor2.Count > 50)
                    dataModel.Rotor2.RemoveAt(0);
                if (dataModel.Rotor3.Count > 50)
                    dataModel.Rotor3.RemoveAt(0);
                if (dataModel.Rotor4.Count > 50)
                    dataModel.Rotor4.RemoveAt(0);
                if(dataModel.Throttle.Count > 50)
                    dataModel.Throttle.RemoveAt(0);
                dataModel.Update();
            }
        }

        public override void Run()
        {
            base.Run();
            //var dataModel = (ControllerModel)Model;
            _timer.Start();
        }

        public override void Stop()
        {
            base.Stop();
            _timer.Stop();
        }

    }
}
