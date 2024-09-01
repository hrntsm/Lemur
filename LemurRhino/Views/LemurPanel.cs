using System;

using Eto.Drawing;
using Eto.Forms;

using Rhino.UI;

namespace LemurRhino.Views
{
    [System.Runtime.InteropServices.Guid("2e0b6d91-ff66-423d-8e85-c3f600ffdf50")]
    public class LemurPanel : Panel, IPanel
    {
        public string Title { get; }
        public static Guid PanelId => typeof(LemurPanel).GUID;
        readonly uint _document_sn;

        public LemurPanel(uint documentSerialNumber)
        {
            _document_sn = documentSerialNumber;

            Title = GetType().Name;

            var layout = new DynamicLayout { DefaultSpacing = new Size(5, 5), Padding = new Padding(10) };

            var view = new TreeGridView();
            view.Columns.Add(new GridColumn
            {
                DataCell = new TextBoxCell { Binding = Binding.Property<MyTreeGridItem, string>(item => item.Name) },
                HeaderText = "Name"
            });

            var treeCollection = new TreeGridItemCollection();
            var model = new MyTreeGridItem { Name = "Model" };
            var analysis = new MyTreeGridItem { Name = "Analysis" };
            var result = new MyTreeGridItem { Name = "Result" };
            result.Children.Add(new MyTreeGridItem { Name = "Mises Stress" });
            treeCollection.Add(model);
            treeCollection.Add(analysis);
            treeCollection.Add(result);
            view.DataStore = treeCollection;


            layout.AddRow(new Label { Text = "Model Tree" });
            layout.AddRow(view);

            view.CellClick += (sender, e) =>
            {
                if (e.Buttons == MouseButtons.Alternate && e.Item is MyTreeGridItem item)
                {
                    ContextMenu contextMenu = CreateContextMenu(item.Name);
                    contextMenu.Show(view);
                }
            };

            Content = layout;
        }

        private static ContextMenu CreateContextMenu(string name)
        {
            var contextMenu = new ContextMenu();

            switch (name)
            {
                case "Model":
                    var openItem = new ButtonMenuItem { Text = "Open Fistr File" };
                    openItem.Click += (sender, e) => MessageBox.Show("Load Fistr File");
                    contextMenu.Items.Add(openItem);
                    break;
                case "Analysis":
                    var editItem = new ButtonMenuItem { Text = "Run" };
                    editItem.Click += (sender, e) => MessageBox.Show("Run Analysis");
                    contextMenu.Items.Add(editItem);
                    break;
                case "Result":
                    var loadItem = new ButtonMenuItem { Text = "Load" };
                    loadItem.Click += (sender, e) => MessageBox.Show("Load Result");
                    contextMenu.Items.Add(loadItem);
                    break;
                case "Mises Stress":
                    var shoeItem = new ButtonMenuItem { Text = "Show" };
                    shoeItem.Click += (sender, e) => MessageBox.Show("Show Result");
                    contextMenu.Items.Add(shoeItem);
                    break;
                default:
                    break;
            }

            var propertiesItem = new ButtonMenuItem { Text = "Property" };
            propertiesItem.Click += (sender, e) => MessageBox.Show($"Show {name} properties");
            contextMenu.Items.Add(propertiesItem);

            return contextMenu;
        }

        public void PanelShown(uint documentSerialNumber, ShowPanelReason reason)
        {
        }

        public void PanelHidden(uint documentSerialNumber, ShowPanelReason reason)
        {
        }

        public void PanelClosing(uint documentSerialNumber, bool onCloseDocument)
        {
        }
    }

    public class MyTreeGridItem : TreeGridItem
    {
        public string Name { get; set; }
    }
}
