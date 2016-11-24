using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace ExcelUtil
{
    public partial class BlockingForm : Form
    {
        public BlockingForm()
        {
            InitializeComponent();
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.ShowInTaskbar = false;
            this.Load += BlockingForm_Load;
        }

        void BlockingForm_Load(object sender, EventArgs e)
        {
            this.Size = new Size(0, 0);
        }
    }
}
