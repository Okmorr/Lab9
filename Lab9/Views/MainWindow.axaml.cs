using Avalonia.Controls;
using Lab9.Models;
using Lab9.ViewModels;
using System.Linq;
using System.IO;
using Avalonia.Controls.Primitives;

namespace Lab9.Views{
    public partial class MainWindow : Window{
        private Carousel _Slider;
            private Button _Back;
                private Button _Next;

        private void Init(){
            _Slider = this.FindControl<Carousel>("Slider");
                _Back = this.FindControl<Button>("Back");
                    _Next = this.FindControl<Button>("Next");}
        public MainWindow(){
            InitializeComponent();
            Init();
            _Back.Click += (s, e) => _Slider.Previous();
                _Next.Click += (s, e) => _Slider.Next();
        }

        private void ChangedSelectedNode(object sender, SelectionChangedEventArgs e){
            string[] allowedExtensions = new[] { ".jpg", ".jpeg", ".png" };
            TreeView treeView = sender as TreeView;
            var context = this.DataContext as MainWindowViewModel;

            Node selectedNode = treeView.SelectedItems[0] as Node;

            if (allowedExtensions.Any(selectedNode.NodeName.ToLower().EndsWith)){
                string path = selectedNode.FullPath.Substring(0, selectedNode.FullPath.IndexOf(selectedNode.NodeName));
                var files = Directory.EnumerateFiles(path)
                    .Where(file => allowedExtensions.Any(file.EndsWith))
                    .ToList();


                if (files.Count > 1){
                    _Next.IsEnabled = true;
                        _Back.IsEnabled = true;
                }
                else{ 
                    _Next.IsEnabled = false;
                        _Back.IsEnabled = false;
                }

                var id = files.IndexOf(selectedNode.FullPath);

                context.RefreshImageList(files);
                _Slider.SelectedIndex = id;
            }
        }

        private void ClickForLoadNodes(object sender, TemplateAppliedEventArgs e)
        {
            ContentControl treeViewItem = sender as ContentControl;
            Node selectedNode = treeViewItem.DataContext as Node;
            selectedNode.GetFilesAndFolders();
        }
    }
}