using System;
using System.Windows.Forms;
using System.Drawing;

namespace FbxInfo
{
    public class FbxInfoForm : Form
    {
        MainMenu menu;
        MenuItem fileMenu;
        MenuItem fileOpenItem;
        SplitContainer splitter;
        TreeView nodeHierarchy;
        TextBox output;

        public FbxInfoForm()
        {
            splitter = new SplitContainer();
            splitter.Panel1.SuspendLayout();
            splitter.Panel2.SuspendLayout();
            splitter.SuspendLayout();

            this.SuspendLayout();

            /**************************/

            nodeHierarchy = new TreeView();
            nodeHierarchy.Dock = DockStyle.Fill;

            output = new TextBox();
            output.ReadOnly = true;
            output.Multiline = true;
            output.Dock = DockStyle.Fill;

            splitter.Dock = DockStyle.Fill;
            splitter.Location = new Point(0, 0);
            splitter.Name = "splitter";
            splitter.Orientation = Orientation.Vertical;
            splitter.Panel1.Controls.Add(nodeHierarchy);
            splitter.Panel2.Controls.Add(output);

            this.Controls.Add(splitter);
            this.Name = "FbxInfoForm";
            this.Text = "FbxInfo";

            /**************************/ 

            splitter.Panel1.ResumeLayout(false);
            splitter.Panel1.PerformLayout();
            splitter.Panel2.ResumeLayout(false);
            splitter.Panel2.PerformLayout();
            splitter.ResumeLayout(false);
            splitter.PerformLayout();

            this.ResumeLayout(false);
            this.PerformLayout();

            fileOpenItem = new MenuItem("Open", new EventHandler(File_Open_OnClick));
            fileMenu = new MenuItem("File", new MenuItem[] { fileOpenItem });
            menu = new MainMenu(new MenuItem[] { fileMenu });
            this.Menu = menu;
        }

        protected void File_Open_OnClick(object sender, EventArgs e)
        {
        }
    }
}

