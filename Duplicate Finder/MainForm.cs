using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.IO;

namespace DuplicateFinder   
{
    public partial class MainForm : Form
    {
        private string fileName = "TestFile.csv";
        private List<string> Column1 = new List<string>();
        private List<string> Column2 = new List<string>();
        public MainForm()
        {
            InitializeComponent();
        }

        private void MainForm_Paint(object sender, PaintEventArgs e)
        {
            var border = this.ClientRectangle;
            border.Inflate(-5, -5);
            ControlPaint.DrawBorder3D(e.Graphics, border, Border3DStyle.Bump);

        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            var path = Directory.GetCurrentDirectory();
            if (!File.Exists(string.Format("{0}\\{1}", path, fileName)))
                throw new FileNotFoundException(string.Format("File {0} not found", fileName));

            using (var sr = new StreamReader(fileName))
            {
                if (sr.ReadLine() == null)
                    return;

                string line;
                while ((line = sr.ReadLine()) != null)
                {
                    var values = line.Split(',');
                    if (values.Length != 2)
                        throw new Exception("File input is not in expected format");
                    Column1.Add(values[0]);
                    Column2.Add(values[1]);
                }
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            textBox1.Clear();
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            List<int> indeces;
            if (radioColumnOne.Checked)
                indeces = FindDuplicateEntries(Column1);
            else
                indeces = FindDuplicateEntries(Column2);

            foreach(var index in indeces)
            {
                textBox1.AppendText(string.Format("{0}, {1}\n", Column1[index], Column2[index]));
            }
        }

        private List<int> FindDuplicateEntries(List<string> column)
        {
            List<int> indeces = new List<int>();

            var groups = column.GroupBy(i => i);
            foreach(var group in groups)
            {
                if(group.Count() > 1)
                {
                    for(int i = 0; i<column.Count;i++)
                    {
                        if (column[i] == group.Key)
                            indeces.Add(i);
                    }
                }
            }
            return indeces;
        }
    }
}
