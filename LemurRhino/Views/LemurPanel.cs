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
            var analysis = new TreeGridItem(
                new object[] { "Analysis" }
                );
            var aa = new Label { Text = "Analysis" };
            TreeGridItem result = CreateResultTree();
            var treeList = new TreeGridItemCollection
            {
                analysis,
                result
            };

            var view = new TreeGridView { DataStore = treeList };
            view.Columns.Add(new GridColumn
            {
                HeaderText = "Name",
                DataCell = new TextBoxCell(0)
            });
            layout.AddRow(new Label { Text = "Model Tree" });
            layout.AddRow(view);

            var tree = new TreeGridView();
            tree.Columns.Add(new GridColumn
            {
                HeaderText = "Name",
                DataCell = new TextBoxCell(0)
            });
            layout.AddRow(new Label { Text = "Property" });
            layout.AddRow(tree);

            var contextMenu = new ContextMenu();
            var editItem = new ButtonMenuItem { Text = "編集" };
            editItem.Click += (sender, e) => MessageBox.Show("編集が選択されました");
            var deleteItem = new ButtonMenuItem { Text = "削除" };
            deleteItem.Click += (sender, e) => MessageBox.Show("削除が選択されました");
            contextMenu.Items.Add(editItem);
            contextMenu.Items.Add(deleteItem);

            view.CellClick += (sender, e) =>
            {
                if (e.Buttons == MouseButtons.Alternate)
                {
                    contextMenu.Show(view);
                }
            };


            Content = layout;
        }

        private static TreeGridItem CreateResultTree()
        {
            var stress = new TreeGridItem(
                new object[] { "Stress" }
                );
            stress.Children.Add(new TreeGridItem(
                new object[] { "Mises" }
                ));
            stress.Children.Add(new TreeGridItem(
                new object[] { "XX" }
                ));

            var displacement = new TreeGridItem(
                new object[] { "Displacement" }
                );
            displacement.Children.Add(new TreeGridItem(
                new object[] { "Total" }
                ));
            displacement.Children.Add(new TreeGridItem(
                new object[] { "X" }
                ));
            displacement.Children.Add(new TreeGridItem(
                new object[] { "Y" }
                ));
            displacement.Children.Add(new TreeGridItem(
                new object[] { "Z" }
                ));


            var result = new TreeGridItem(new object[] { "Result" });
            result.Children.Add(stress);
            result.Children.Add(displacement);
            return result;
        }

        protected void OnHelloButton()
        {
            Dialogs.ShowMessage("Hello Rhino!", Title);
        }

        /// <summary>
        /// Sample of how to display a child Eto dialog
        /// </summary>
        protected void OnChildButton()
        {
            var dialog = new Dialog
            {
                Title = "Child Dialog",
                Content = new Label { Text = "This is a child dialog" },
                ClientSize = new Size(200, 100)
            };
            dialog.ShowModal(Application.Instance.MainForm);
        }

        public void PanelShown(uint documentSerialNumber, ShowPanelReason reason)
        {
            Rhino.RhinoApp.WriteLine("Show Lemur panel");
        }

        public void PanelHidden(uint documentSerialNumber, ShowPanelReason reason)
        {
            Rhino.RhinoApp.WriteLine("Close Lemur panel");
        }

        public void PanelClosing(uint documentSerialNumber, bool onCloseDocument)
        {
            // Called when the document or panel container is closed/destroyed
            Rhino.RhinoApp.WriteLine($"Panel closing for document {documentSerialNumber}, this serial number {_document_sn} should be the same");
        }
    }
}
