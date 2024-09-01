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

            //var hello_button = new Button { Text = "Hello..." };
            //hello_button.Click += (sender, e) => OnHelloButton();

            //var child_button = new Button { Text = "Child Dialog..." };
            //child_button.Click += (sender, e) => OnChildButton();

            //var checkBox = new CheckBox { Text = "Check me" };

            var layout = new DynamicLayout { DefaultSpacing = new Size(5, 5), Padding = new Padding(10) };
            var item = new TreeGridItem(
                new object[] { "foo", true, 42 }
                );
            var treeList = new TreeGridItemCollection();
            treeList.Add(item);

            var view = new TreeGridView { DataStore = treeList };
            view.Columns.Add(new GridColumn
            {
                HeaderText = "Name",
                DataCell = new TextBoxCell(0)
            });
            view.Columns.Add(new GridColumn
            {
                HeaderText = "Folder",
                DataCell = new CheckBoxCell(1)
            });
            view.Columns.Add(new GridColumn
            {
                HeaderText = "Size",
                DataCell = new TextBoxCell(2)
            });

            layout.Add(view, true, true);

            //layout.AddSeparateRow(hello_button, null);
            //layout.AddSeparateRow(child_button, null);
            //layout.AddSeparateRow(checkBox, null);
            layout.Add(null);
            Content = layout;
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
