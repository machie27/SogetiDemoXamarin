using DemoXaml.ViewModels;
using DemoXaml.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xamarin.Forms;

namespace DemoXaml
{
    public class App : Application
    {

        private static ViewModelLocator _locator;

        public static ViewModelLocator Locator
        {
            get
            {
                return _locator ?? (_locator = new ViewModelLocator());
            }
        }


        public static Page GetMainPage()
        {
            return new MainPage();
        }
        public App()
        {
            // The root page of your application


            MainPage = GetMainPage();
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
