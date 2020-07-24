﻿using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using xamarin.beacon.Interface;
using xamarin.beacon.Page;

namespace xamarin.beacon
{
    public partial class App : Application
    {

        bool _closeTimer = false;

        public App()
        {
            InitializeComponent();

            DependencyService.Get<IbeaconAndroid>().InitializeService();

            MainPage = new MainPage();

        }

        void startTimer()
        {

            _closeTimer = false;

            Device.StartTimer(TimeSpan.FromSeconds(10), () => {

                if (_closeTimer)
                {

                    System.Diagnostics.Debug.WriteLine("StartTimer: stop repeating");
                    return false;
                }

                Xamarin.Forms.MessagingCenter.Send<App>((App)Xamarin.Forms.Application.Current, "CleanBeacons");

                System.Diagnostics.Debug.WriteLine("StartTimer: end with " + (!_closeTimer ? "repeating" : "stop repeating"));

                return !_closeTimer;
            });
        }

        protected override void OnStart()
        {
            // Handle when your app starts
            DependencyService.Get<IbeaconAndroid>().SetBackgroundMode(false);

            startTimer();
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
            DependencyService.Get<IbeaconAndroid>().SetBackgroundMode(true);
            closeTimer();
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
            DependencyService.Get<IbeaconAndroid>().SetBackgroundMode(false);
            startTimer();
        }

        private void closeTimer()
        {
            _closeTimer = true;
        }
    }
}
