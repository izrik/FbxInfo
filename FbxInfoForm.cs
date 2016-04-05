using System;
using System.Windows.Forms;
using System.Drawing;
using FbxSharp;
using System.Linq;

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
            nodeHierarchy.AfterSelect += NodeHierarchy_AfterSelect;

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

            fileOpenItem = new MenuItem("Open", new EventHandler(FileOpenItem_Click));
            fileMenu = new MenuItem("File", new MenuItem[] { fileOpenItem });
            menu = new MainMenu(new MenuItem[] { fileMenu });
            this.Menu = menu;
        }

        void NodeHierarchy_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (e.Node.Tag == null)
            {
                output.Text = e.Node.Text;
            }
            else
            {
                output.Text = e.Node.Text + "\n" + ((FbxObject)e.Node.Tag).ToString();
            }
        }

        protected void FileOpenItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.CheckFileExists = true;
            ofd.Multiselect = false;
            ofd.AddExtension = true;
            ofd.DefaultExt = "fbx";
            var result = ofd.ShowDialog(this);
            if (result != DialogResult.OK) return;

            var filename = ofd.FileName;
            var importer = new Importer();
            var scene = importer.Import(filename);

            var node = GetNodeFromScene(scene);
            nodeHierarchy.Nodes.Clear();
            nodeHierarchy.Nodes.Add(node);
        }

        static TreeNode GetNodeFromScene(Scene scene)
        {
            var node = GetNodeFromFbxObject(scene);
            var root = scene.GetRootNode();
            var nodesnode = GetNodeFromNode(root);
            nodesnode.Text = "Nodes";
            node.Nodes.Insert(0, nodesnode);
            return node;
        }

        static TreeNode GetNodeFromNode(Node obj)
        {
            var tnode = new TreeNode(obj.Name);
            tnode.Tag = obj;
            foreach (var child in obj.ChildNodes)
            {
                tnode.Nodes.Add(
                    GetNodeFromNode(child));
            }
            return tnode;
        }

        static TreeNode GetNodeFromFbxObject(FbxObject fobj)
        {
            var node = new TreeNode(fobj.GetNameWithNameSpacePrefix());
            node.Tag = fobj;
            var srcnode = new TreeNode("SrcObjects");
            srcnode.Tag = fobj.SrcObjects;
            node.Nodes.Add(srcnode);
            srcnode.Nodes.AddRange(
                fobj.SrcObjects
                .Where(sobj => sobj.DstObjects.Count == 1)
                .Select(
                    new Func<FbxObject, TreeNode>(
                        GetNodeFromFbxObject)).ToArray());
            return node;
        }
    }
}

