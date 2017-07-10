using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace dragdoptree
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void btnLoad_Click(object sender, EventArgs e)
        {
            // test to get the type from values and print the name
            object[] values = { "word", true, 120, 136.34, 'a', new Form1() };
            foreach (var value in values)
                Console.WriteLine("{0} - type {1}", value,
                                  value.GetType().Name);

            foreach (var value in values[5].GetType().GetMethods())
            {
                Console.WriteLine("{0} - type {1}", value.Name,
                          value.ReturnType);
            }
            
           
        }

        private void btnsave_Click(object sender, EventArgs e)
        {

        }


           
    }
}
