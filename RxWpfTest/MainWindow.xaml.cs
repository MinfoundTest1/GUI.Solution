using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reactive.Concurrency;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace RxWpfTest
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private MemberSearchViewModel vm; // = new MemberSearchViewModel();

        private IDisposable SearchTask;

        public MainWindow(MemberSearchViewModel vm)
        {
            InitializeComponent();
            this.vm = vm;
            this.DataContext = vm;

            vm.OnPropertyChanges(o => o.SearchText)
                .Throttle(TimeSpan.FromSeconds(0.5))
                .Subscribe(Out);

            //vm.FocalSpots.Add("First");
            //vm.FocalSpots.Add("Second");

            vm.SelectedFocalSpot = "First";

            //Observable.FromEventPattern(MyButton, "Click")
            //    .Subscribe(a => MessageBox.Show(a.EventArgs.ToString()));

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            SearchTask?.Dispose();
            vm.Clear();
            MessageBox.Show(vm.FocalSpots.Count().ToString());
        }

        private void Out(string text)
        {
      //       textBox.AppendText(text + "\t");
            Dispatcher.Invoke(() => textBox.AppendText(text + "\t"));
        }

        private async void Button_Click_1(object sender, RoutedEventArgs e)
        {
            await Task.Delay(10);
            var list = GetFocalSpots();
            SearchTask = list.ToObservable()
                             .SubscribeOn(Scheduler.Default)
                             .Subscribe(f => vm.Add(f));
            //foreach (var item in list)
            //{
            //    vm.Add(item);
            //}

            //        MessageBox.Show(vm.FocalSpots.Count().ToString());

        }

        private IEnumerable<string> GetFocalSpots()
        {
            yield return "A";
            Thread.Sleep(1000);
            yield return "B";
            Thread.Sleep(1000);
            yield return "C";
            Thread.Sleep(1000);
            yield return "D";
        }
    }
}
