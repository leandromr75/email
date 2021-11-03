using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading;

namespace Email
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            CheckForIllegalCrossThreadCalls = false;
        }

        private void abrirToolStripMenuItem_Click(object sender, EventArgs e)
        {
            

            //Declaro o StreamReader para o caminho onde se encontra o arquivo
            //StreamReader rd = new StreamReader(@"d:\Pasta1.csv");
            //Criando um DataTable
            DataTable dt = new DataTable();
            string path = "";
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                path = openFileDialog1.FileName;
            }
            else
            {
                return;
            }

            string[] Linha = System.IO.File.ReadAllLines(path);
            //Lendo Todas as linhas do arquivo CSV
            //string[] Linha = System.IO.File.ReadAllLines(@"C:\Users\TI\Desktop\500.csv");


            //Neste For, vamos percorrer todas as linhas que foram lidas do arquivo CSV
            for (Int32 i = 0; i < Linha.Length; i++)
            {
                //Aqui Estamos pegando a linha atual, e separando os campos
                //Por exemplo, ele vai lendo um texto, e quando achar um ponto e virgula
                //ele pega o texto e joga na outra posição do array temp, e assim por diante
                //até chegar no final da linha

                string[] campos = Linha[i].Split(Convert.ToChar(";"));


                //Um datable precisa de colunas
                //Como a função acima jogou cada campo em uma posição do array de acordo com
                //o ponto e virgula, eu estou pegando quantos campos ele achou e criando a mesma
                //quantidade de colunas no datatable para corresponder a cada campo
                if (i == 0)
                {
                    for (Int32 i2 = 0; i2 < campos.Length; i2++)
                    {
                        //Criando uma coluna
                        DataColumn col = new DataColumn();
                        //Adicionando a coluna criada ao datatable
                        dt.Columns.Add(col);

                    }
                }

                //Inserindo uma linha(row) no datable (Vai fazer isso para cada linha encontrada
                //no arquivo CSV
                dt.Rows.Add(campos);



            }

            dataGridView1.DataSource = dt;       
                
        }

        private void listarLogToolStripMenuItem_Click(object sender, EventArgs e)
        {
            richTextBox1.LoadFile("log.rtf",RichTextBoxStreamType.PlainText);
        }

        private void limparCsvToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dataGridView1.Rows.Count > 0)
            {
                richTextBox1.Text = "";
                if (dataGridView1.Rows.Count > 0)
                {
                    for (int j = 0; j < dataGridView1.Rows.Count; j++)
                    {
                       // richTextBox1.Text += dataGridView1.Rows[j].Cells[0].Value.ToString().Trim(new Char[] { ',', '*', '.', ' ', '|' }) + "\n";
                        dataGridView1.Rows[j].Cells[0].Value = dataGridView1.Rows[j].Cells[0].Value.ToString().Trim(new Char[] { ',', '*', '.' ,' ','|'});
                    }
                }
            }
        }

        private void limparRestanteCSVToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dataGridView1.Rows.Count > 0)
            {
                richTextBox1.Text = "";
                for (int k = 0; k < dataGridView1.Rows.Count; k++)
                {
                    //richTextBox1.Text += dataGridView1.Rows[k].Cells[0].Value.ToString().Replace("Normal", "") + "\n";

                    dataGridView1.Rows[k].Cells[0].Value = dataGridView1.Rows[k].Cells[0].Value.ToString().Replace("Normal", "");
                    //richTextBox1.Text += dataGridView1.Rows[k].Cells[0].Value.ToString().Replace(",", "") + "\n";
                    //richTextBox1.Text += dataGridView1.Rows[k].Cells[0].Value.ToString().Replace(" ", "") + "\n";
                    dataGridView1.Rows[k].Cells[0].Value = dataGridView1.Rows[k].Cells[0].Value.ToString().Replace(",", "");
                    dataGridView1.Rows[k].Cells[0].Value = dataGridView1.Rows[k].Cells[0].Value.ToString().Replace(" ", "");


                }
            }
        }

        private void validarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dataGridView1.Rows.Count > 0)
            {
                for (int l = 0; l < dataGridView1.Rows.Count; l++)
                {
                    if (String.IsNullOrEmpty(dataGridView1.Rows[l].Cells[0].Value.ToString()) == false)
                    {
                        string strModelo = "^([0-9a-zA-Z]([-.\\w]*[0-9a-zA-Z])*@([0-9a-zA-Z][-\\w]*[0-9a-zA-Z]\\.)+[a-zA-Z]{2,9})$";
                        if (System.Text.RegularExpressions.Regex.IsMatch(dataGridView1.Rows[l].Cells[0].Value.ToString(), strModelo))
                        {
                            
                        }
                        else
                        {                            
                            dataGridView1.CurrentCell = dataGridView1.Rows[l].Cells[0];
                            Global.cliente.clientes = dataGridView1.Rows[l].Cells[0].Value.ToString();
                            Form for2 = new Form2();
                            for2.ShowDialog();
                            if (Global.cliente.clientes == "exit")
                            {
                                 Application.Exit();
                                 return;
                            }
                            dataGridView1.Rows[l].Cells[0].Value = Global.cliente.clientes;
                            Global.cliente.clientes = "";                       
                        }     
                    }
                }
            }
           
        }

        private void visualizarHTMLToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form htm = new HTML();
            htm.ShowDialog();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            comboBox1.Text = "true";            
        }

        private void enviarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Thread thread = new Thread(() => { enviar(); });
            thread.Start();
            pictureBox1.Visible = true;
            
        }

        public void enviar()
        {
            string temp1 = "1";
            int cont = 0;
            if (dataGridView1.Rows.Count > 0)
            {

                for (int i = 0; i < dataGridView1.Rows.Count; i++)
                {

                    dataGridView1.CurrentCell = dataGridView1.Rows[i].Cells[0];

                    if (String.IsNullOrEmpty(dataGridView1.Rows[i].Cells[0].Value.ToString()) == false && temp1 != dataGridView1.Rows[i].Cells[0].Value.ToString())
                    {

                        if (string.IsNullOrEmpty(textBox1.Text) == true ||
                            string.IsNullOrEmpty(textBox2.Text) == true ||
                            string.IsNullOrEmpty(textBox3.Text) == true ||
                            string.IsNullOrEmpty(textBox4.Text) == true ||
                            string.IsNullOrEmpty(textBox6.Text) == true ||
                            string.IsNullOrEmpty(richTextBox2.Text) == true )
                        {
                            MessageBox.Show("Favor preencher campos  do email, senha, assunto, HTML");
                            return;
                        }
                        if (!textBox4.Text.All(char.IsDigit))
                        {
                            MessageBox.Show("Porta SMTP precisa ser numérica");
                            return;
                        }

                        MailMessage mail = new MailMessage();

                        mail.From = new MailAddress(textBox1.Text);
                        mail.To.Add(dataGridView1.Rows[i].Cells[0].Value.ToString()); // para
                        mail.Subject = textBox3.Text; // assunto

                        mail.Body = richTextBox2.Text;
                        /*"<!DOCTYPE html>" +
                        "<html>" +
                        "<head>" +
                        "<style>" +
                        "table, td, th {" +
                            "border: 1px solid #ddd;" +
                            "text-align: center;" +
                        "}" +

                            "table {" +
                                "border-collapse: collapse;" +
                                "width: 100%;" +
                            "}" +

                            "th, td {" +
                                "padding: 15px;" +
                            "}" +
                            "</style>" +
                            "</head>" +
                            "<body>" +


                            "<p style='text-align: left;font-size:normal;padding: 15px;'>Compartilhamos com você os horários de expediente que a Art Chik realizará durante os jogos do Brasil na Copa do Mundo 2018:</p><br />" +

                            "<table style='border: 2px solid #ddd;text-align: center;'>" +
                              "<tr>" +
                                "<th style='border: 2px solid #ddd;text-align: center;font-size:large;color:red;padding: 15px;'>Jogo</th>" +
                                "<th style='border: 2px solid #ddd;text-align: center;font-size:large;color:red;padding: 15px;'>Data</th>" +
                                "<th style='border: 2px solid #ddd;text-align: center;font-size:large;color:red;padding: 15px;'>Horário de Expediente</th>" +
                              "</tr>" +
                              "<tr>" +
                            "<td style='border: 2px solid #ddd;text-align: center;font-size:large;color:blue;padding: 15px;'>Brasil X Costa Rica</td>" +
                                "<td style='border: 2px solid #ddd;text-align: center;font-size:large;color:green;padding: 15px;'>22 de Junho - Sexta-Feira 9h00</td>" +
                                "<td style='border: 2px solid #ddd;text-align: center;font-size:large;padding: 15px;'>Estaremos em recesso somente durante o período da manhã. Retornaremos com nossas atividades às <strong>12h00</strong></td>" +
                              "</tr>" +
                              "<tr>" +
                                "<td style='border: 2px solid #ddd;text-align: center;font-size:large;color:blue;padding: 15px;'>Brasil X Sérvia</td>" +
                                "<td style='border: 2px solid #ddd;text-align: center;font-size:large;color:green;padding: 15px;'>27 de Junho - Quarta-Feira 15h00</td>" +
                                "<td style='border: 2px solid #ddd;text-align: center;font-size:large;padding: 15px;'>O expediente será encerrado às <strong>14h00</strong>. Retornaremos com nossas atividades normais na quinta-feira, 28 de Junho.</td>" +
                              "</tr>" +
                            "</table>" +
                            "<br />" +
                            "<p style='text-align: left;font-size:normal;padding: 15px;'>Aproveitamos a oportunidade para solicitar que, se possível, antecipem seus pedidos. Isso contribuirá para uma melhor programação das entregas.</p>" +
                            "<p style='text-align: left;margin: 0;font-size:normal;padding: 15px;'>Em caso de dúvidas, entre em contato com o vendedor de sua região ou pelo telefone: (16)2111-1900.</p>" +
                            "<p style='text-align: left;font-size:normal;padding: 15px;'>Atenciosamente,</p>" +
                            "<p style='text-align: left;margin: 0;font-weight:bold;font-size:normal;padding: 15px;'>Edson Bianchi</p>" +
                            "<p style='text-align: left;margin: 0;font-size:normal;padding: 15px;'>Diretor Presidente</p>" +
                            "<p style='text-align: left;margin: 0;font-size:normal;padding: 15px;'>Art Chik Produtos Publicitários</p>" +
                            "</body>" +
                            "</html>";*/

                        mail.IsBodyHtml = true;

                        using (var smtp = new SmtpClient(textBox6.Text))
                        {
                            smtp.EnableSsl = Convert.ToBoolean(comboBox1.Text); // GMail requer SSL
                            smtp.Port = Convert.ToInt32(textBox4.Text);       // porta para SSL
                            smtp.DeliveryMethod = SmtpDeliveryMethod.Network; // modo de envio
                            smtp.UseDefaultCredentials = false; // vamos utilizar credencias especificas

                            // seu usuário e senha para autenticação
                            smtp.Credentials = new NetworkCredential(textBox1.Text, textBox2.Text);

                            // envia o e-mail
                            smtp.Send(mail);
                            smtp.Dispose();
                            richTextBox1.Text += "[Enviado com sucesso para E-mail: " + dataGridView1.Rows[i].Cells[0].Value.ToString() + " ]\n";

                        }



                        temp1 = dataGridView1.Rows[i].Cells[0].Value.ToString();
                        cont++;
                        label3.Text = Convert.ToString(Convert.ToInt32(label3.Text) + 1);

                    }
                    else
                    {
                        richTextBox1.Text += "[Email Inválido ou Duplicado]\n";

                    }

                    if (cont == 20)
                    {
                        Form con = new Pausa();
                        Global.cliente.email = "E-mail enviados: " + label3.Text;
                        con.ShowDialog();

                        cont = 0;
                        richTextBox1.SaveFile("log.rtf", RichTextBoxStreamType.PlainText);
                        dataGridView1.Focus();



                    }
                    if (cont == 5)
                    {
                        richTextBox1.SaveFile("log.rtf", RichTextBoxStreamType.PlainText);
                    }
                    if (cont == 10)
                    {
                        richTextBox1.SaveFile("log.rtf", RichTextBoxStreamType.PlainText);
                    }
                    if (cont == 15)
                    {
                        richTextBox1.SaveFile("log.rtf", RichTextBoxStreamType.PlainText);
                    }

                }
                richTextBox1.SaveFile("log.rtf", RichTextBoxStreamType.PlainText);
                pictureBox1.Visible = false;
            }
        }

        
    }
}
