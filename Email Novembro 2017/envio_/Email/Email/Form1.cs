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

namespace Email
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void abrirToolStripMenuItem_Click(object sender, EventArgs e)
        {
                pictureBox1.Visible = true;
            
                //Declaro o StreamReader para o caminho onde se encontra o arquivo
                //StreamReader rd = new StreamReader(@"d:\Pasta1.csv");
                //Criando um DataTable
                DataTable dt = new DataTable();
                
                //Lendo Todas as linhas do arquivo CSV
                string[] Linha = System.IO.File.ReadAllLines(@"C:\Users\TI\Desktop\Envio.csv");
              

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

        }

        private void enviarToolStripMenuItem_Click(object sender, EventArgs e)
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
                            
                                
                            MailMessage mail = new MailMessage();

                            mail.From = new MailAddress("thiago@artchik.com.br");
                            mail.To.Add(dataGridView1.Rows[i].Cells[0].Value.ToString()); // para
                            mail.Subject = "Férias Coletivas Art Chik"; // assunto

                            /*mail.Body = "<!DOCTYPE html>" +
                                        "<html lang='pt-br'>" +
                            "<head>" +                            
                            "<meta name='viewport' content='width=device-width, initial-scale=1'>" +
                            "<link href='https://fonts.googleapis.com/css?family=Basic' rel='stylesheet'>" +
                            "<link href='https://fonts.googleapis.com/css?family=Sans+Serif' rel='stylesheet'>" +
                            "<style>" +                            
                            ".city {" +
                               "float: none;" +
                               "margin: 10px;" +
                               "padding: 15px;" +
                               "max-width: 570px;" +
                               "height: auto;" +
                               "font: medium;" +
                               "background-color: beige;" +      
                            "}" +
                            ".logo{" +
                               "font-size: xx-large;" +
                               "color: green;" +
                               "line-height: 0.8;" +
                               "font-family: Arial, 'Basic' , 'Sans Serif',Helvetica, Verdana, Sans-Serif;" +
                               "}" +
                            ".comunicado{" +
                               "font-size: large;" +
                               "text-decoration: underline;" +
                               "font-family: Arial, 'Basic' ,Helvetica, Verdana, Sans-Serif;" + 
                               "img.seven-logo{" +
                                 "max-width: 100%;" +
                                 "height: auto;" +
                               "}" +
                             "}" +

                            "</style>" +
                            "</head>" +
                            "<body>" +                            
                            "<div class='city'style='float: none;margin: 10px;padding: 15px;max-width: 515px;height: auto;font: medium;background-color: beige;'>" +
                            "<p class='logo' style='font-size: xx-large; color: green; line-height: 0.8;" + 
                            "font-family: Arial, Helvetica,&quot;Basic&quot;, Verdana, Sans-Serif;'>&nbsp;&nbsp;art<br>" +
                            "chik</p>" +

                                 "<p class='logo'>&nbsp;&nbsp;" +    
                                 "</p><p class='comunicado' style='font-size: large; color: black; text-decoration: underline;font-family: Arial, Helvetica,&quot;Basic&quot;, Verdana, Sans-Serif;'><b>COMUNICADO</b></p>" +
                                 "<p style='color: black, font-family: Arial, Helvetica,&quot;Basic&quot;, Verdana, Sans-Serif;'>Comunicamos aos nossos amigos, clientes e fornecedores, que no período de<br /></p>" +
                                 "<p style='color: black, font-family: Arial, Helvetica,&quot;Basic&quot;, Verdana, Sans-Serif;'><b>20 DE DEZEMBRO DE 2019 a 05 DE JANEIRO DE 2020</b><br /></p>" +
                                 "<p style='color: black, font-family: Arial, Helvetica,&quot;Basic&quot;, Verdana, Sans-Serif;'>vamos paralisar as nossas atividades industriais para a necessária manutenção<br />" +
                                 "das máquinas e equipamentos.</p>" +
                                 "<p style='color: black, font-family: Arial, Helvetica,&quot;Basic&quot;, Verdana, Sans-Serif;'><b>IMPORTANTE:</b><br />Os pedidos recebidos até o dia.........10 DE DEZEMBRO<br />serão produzidos e despachados até 19 DE DEZEMBRO<br /></p>" +
                                 "<br />" +
                                 "<p style='color: black, font-family: Arial, Helvetica,&quot;Basic&quot;, Verdana, Sans-Serif;'>Queremos aproveitar esta oportunidade para agradecer a todos que nos distinguiram com sua presença, amizade e confiança neste ano que termina, desejando que possamos continuar juntos na busca do aperfeiçoamento pessoal, profissional e de novas conquistas.</p>" +
                                 "<br />" +
                                 "<p style='color: black, font-family: Arial, Helvetica,&quot;Basic&quot;, Verdana, Sans-Serif;'><b>Boas Festas, Um Feliz Natal e Próspero Ano Novo a todos!</b></p>" +
                                 "<p style='color: black, font-family: Arial, Helvetica,&quot;Basic&quot;, Verdana, Sans-Serif;'>Equipe ART CHIK</p>" +
                            "</div>" +
                            "</body>" +
                            "</html>";*/

                            mail.Body = "<!DOCTYPE html>" +
                            "<html lang='pt-br'>" +
                            "<head>" +
                            "<meta http-equiv='Content-Type' content='text/html; charset=utf-8'>" +
                            "<meta name='viewport' content='width=device-width, initial-scale=1'>" +
    
                            "<link rel='stylesheet'" +
                            "   href='https://fonts.googleapis.com/css?family=Basic'>" +
                            "<link href='https://fonts.googleapis.com/css?family=Open+Sans|Roboto' rel='stylesheet'>" +
                            "<link href='https://fonts.googleapis.com/css?family=Sans+Serif' rel='stylesheet'>" +    
   
                            "<style>" +
                            ".city {" +
                            "  float: none;" +
                            "  margin: 15px;" +
                            "  padding: 15px;" +
                            "  max-width: 500px;" +
                            "  background-color: beige;" +
                            "  font: medium;" +      
                            "}" +
                            ".logo{" +
                            "  font-size: xx-large;" +
                            "  color: green;" +
                            "  line-height: 0.8;" + 
                            "  font-family: Arial, Helvetica, 'Basic', Sans-Serif, Verdana;" +
   
                            "}" +
                            ".comunicado{" +
                            "  font-size: large;" +
                            "  text-decoration: underline;" +
                            "  font-family: Arial, Helvetica, 'Basic', Sans-Serif, Verdana;" + 
                            "  img.seven-logo{" +
                            "  max-width: 100%;" +
                            "  height: auto;" +
                            "  }" + 
                            "}" +

                            "</style>" +
                            "</head>" +
                            "<body>" +
                            "<div class='city' style='float: none; margin: 15px; padding: 15px; max-width: 500px; font: medium; background-color: beige'>" +
                            " <br /><br /><p class='logo' style='font-size: 280%; color: green; line-height: 0.8;" + 
                            " font-family: Arial, Roboto, Open Sans, Helvetica, Basic, Verdana, Sans-Serif; margin: 15px'>&nbsp;&nbsp;art<br />" +
                            "chik</p>" +
                            "<p class='logo'>&nbsp;&nbsp;" +    
                            "<p class='comunicado' style='font-size: 130%; text-decoration: underline;" +
                            "    font-family: Arial,  Roboto, Open Sans, Helvetica, Basic, Verdana, Sans-Serif; color: darkred; margin: 15px'><b>COMUNICADO:</b></p>" +    
                            "    <p style='margin: 15px; font-family: Arial, Roboto, Open Sans , Helvetica, Basic, Sans-Serif, Verdana'>Comunicamos aos nossos amigos, clientes e fornecedores, que no período de<br /></p>" +
                            "    <p style='margin: 15px; font-family: Arial, Roboto, Open Sans, Helvetica, Basic, Sans-Serif, Verdana'><b>20 DE DEZEMBRO DE 2019 a 05 DE JANEIRO DE 2020</b><br /></p>" +
                            "    <p style='margin: 15px; font-family: Arial, Roboto, Open Sans , Helvetica, Basic, Sans-Serif, Verdana'>vamos paralisar as nossas atividades industriais para a necessária manutenção" +
                            "    das máquinas e equipamentos. Retornaremos às atividades dia 06/01/2020.</p>" +
                            "    <p style='margin: 15px; font-family: Arial, Roboto, Open Sans , Helvetica, Basic, Sans-Serif, Verdana'><b>IMPORTANTE:</b><br />Os pedidos recebidos até o dia..........10 DE DEZEMBRO<br />serão produzidos e despachados até 19 DE DEZEMBRO<br /></p>" +
                            "    <br />" +
                            "    <p style='margin: 15px; font-family: Arial, Roboto, Open Sans , Helvetica, Basic, Sans-Serif, Verdana'>Queremos aproveitar esta oportunidade para agradecer a todos que nos distinguiram" +
                            "    com sua presença, amizade e confiança neste ano que termina, desejando que possamos" +
                            "    continuar juntos na busca do aperfeiçoamento pessoal, profissional e de novas conquistas.</p>" +
                            "   <br />" +
                            "   <p style='margin: 15px; font-family: Arial,Roboto, Open Sans , Helvetica, Basic, Sans-Serif, Verdana'><b>Boas Festas, Um Feliz Natal e Próspero Ano Novo a todos!</b></p>" +
                            "   <p style='margin: 15px; font-family: Arial, Roboto, Open Sans , Helvetica, Basic, Sans-Serif, Verdana'>Equipe ART CHIK</p><br /><br /><br /><br /><br /><br />" +
                            "</div>" +
                            "</body>" +
                            "</html>";


                            mail.IsBodyHtml = true;
                                
                            using (var smtp = new SmtpClient("smtp.gmail.com"))
                            {
                                smtp.EnableSsl = true; // GMail requer SSL
                                smtp.Port = 587;       // porta para SSL
                                smtp.DeliveryMethod = SmtpDeliveryMethod.Network; // modo de envio
                                smtp.UseDefaultCredentials = false; // vamos utilizar credencias especificas

                                // seu usuário e senha para autenticação
                                smtp.Credentials = new NetworkCredential("thiago@artchik.com.br", "comercial1982");

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
                            richTextBox1.SaveFile("log.rtf",RichTextBoxStreamType.PlainText);
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
