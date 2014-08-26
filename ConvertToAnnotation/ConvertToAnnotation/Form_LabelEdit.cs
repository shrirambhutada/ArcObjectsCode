using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ESRI.ArcGIS.Carto;

namespace ConvertToAnnotation
{
    public partial class Form_LabelEdit : Form
    {
        public Form_LabelEdit()
        {
            InitializeComponent();
        }

        public ITextElement m_TextElement;
        public IActiveView m_ActiveView;
        private void button1_Click(object sender, EventArgs e)
        {
            m_TextElement.Text = this.textBox1.Text.Trim();
            this.m_ActiveView.Refresh();
            this.Visible = false;
            this.m_TextElement = null;
            this.Close();
        }

        private void Form_LabelEdit_Load(object sender, EventArgs e)
        {
            if (this.m_TextElement != null)
            {
                this.textBox1.Text = this.m_TextElement.Text;
                
            }
        }
    }
}
