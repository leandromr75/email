using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Email
{
    public partial class Pausa : Form
    {
        public Pausa()
        {
            InitializeComponent();
        }
        int contador = 0;
        
        private void timer1_Tick(object sender, EventArgs e)
        {
            contador++;
            label2.Text = Convert.ToString(Convert.ToInt32(label2.Text) - 1);
            if (contador == 4)
            {
                contador = 0;
                timer1.Enabled = false;
                label2.Text = "3";
                this.Close();
            }

        }

        private void Pausa_Load(object sender, EventArgs e)
        {
            timer1.Enabled = true;
            label1.Text = Global.cliente.email;
            
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
}
