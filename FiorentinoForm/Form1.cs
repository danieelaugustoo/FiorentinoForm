using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using FiorentinoForm.Components;
using FiorentinoForm.Models;
using FiorentinoForm.Properties;

namespace FiorentinoForm
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            panel1.BackColor = UserData.lightGray;
            label1.ForeColor = UserData.green;
            StartPosition = FormStartPosition.CenterScreen;
            FormBorderStyle = FormBorderStyle.FixedSingle;
            flowLayoutPanel2.AutoScroll = flowLayoutPanel1.AutoScroll = true;
            comboBox1.DropDownStyle = ComboBoxStyle.DropDownList;

            Icon = Icon.FromHandle(Resources.logomarca_2.GetHicon());
        }

        MobileALEntities ctx = new MobileALEntities();
        List<participante> listaParticipantes = new List<participante>();
        private void Form1_Load(object sender, EventArgs e)
        {
            listaParticipantes = ctx.participante.Take(200).ToList();
            foreach (var participante in listaParticipantes)
            {
                participante.pontos = participante.vendas.Count() * 12 + participante.vendas.Sum(x => x.quantidade * x.produto.valor ?? 0) * 24;
            }
            comboBox1.Items.AddRange(ctx.estado.Select(x => x.Sigla).OrderBy(x => x).ToArray());


            LoadLista(listaParticipantes);

        }



        private void LoadLista(List<participante> listaParticipantes)
        {
            listaParticipantes = listaParticipantes.OrderByDescending(x => x.pontos).ToList();


            flowLayoutPanel1.Controls.Clear();

            int posicao = 1;
            foreach (var item in listaParticipantes)
            {
                item.posicao = posicao;
                posicao++;
                flowLayoutPanel1.Controls.Add(new UserControl1(item));
            }
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            var listParticpantes = this.listaParticipantes.ToList();

            if (string.IsNullOrEmpty(textBox1.Text))
            {
                LoadLista(this.listaParticipantes);
                return;
            }

            string pesquisa = "";
            if(textBox1.Text.StartsWith(">"))
            {
                pesquisa = textBox1.Text.Replace(">", "");
                if(int.TryParse(pesquisa, out int valor))
                {
                    listParticpantes = listaParticipantes.Where(x => x.pontos > valor).ToList();
                }
            }else if(textBox1.Text.StartsWith("<"))
            {
                pesquisa = textBox1.Text.Replace("<", "");
                if(int.TryParse(pesquisa, out int valor))
                {
                    listParticpantes = listaParticipantes.Where(x => x.pontos < valor).ToList() ;
                }
            }
            else if (textBox1.Text.StartsWith("+"))
            {
                pesquisa = textBox1.Text.Replace("+", "");
                listParticpantes = listaParticipantes.Where(x => x.cidade.estado.Sigla.ToLower() == pesquisa.ToLower()).ToList();
            }
            else if(textBox1.Text.All(char.IsDigit))
            {
                var pontos = int.Parse(textBox1.Text);
                listParticpantes = listaParticipantes.Where(x => x.pontos == pontos).ToList();
            }
            else if (textBox1.Text.StartsWith("%") && textBox1.Text.EndsWith("%"))
            {
                pesquisa = textBox1.Text.Trim('%');
                if (pesquisa.Length > 0)
                {
                    string termo = pesquisa.ToLower();
                    listParticpantes = listaParticipantes.Where(x => x.nome != null && x.nome.ToLower().Contains(termo)).ToList();
                }
            }

            else if (textBox1.Text.StartsWith("!>") || textBox1.Text.StartsWith("!<"))
{
    bool descending = textBox1.Text.StartsWith("!>");
    pesquisa = textBox1.Text.Substring(2).ToLower(); // Remove os dois primeiros caracteres (!> ou !<)

    if (pesquisa == "po")
    {
        listParticpantes = descending
            ? listaParticipantes.OrderByDescending(x => x.pontos).ToList()
            : listaParticipantes.OrderBy(x => x.pontos).ToList();
    }
    else if (pesquisa == "pa")
    {
        listParticpantes = descending
            ? listaParticipantes.OrderByDescending(x => x.nome).ToList()
            : listaParticipantes.OrderBy(x => x.nome).ToList();
    }
}

            else if (textBox1.Text.StartsWith("!<"))
            {
                pesquisa = textBox1.Text.Substring(2).ToLower();

                if (pesquisa == "po")
                {
                    listParticpantes = listaParticipantes.OrderBy(x => x.pontos).ToList();
                }
                else if (pesquisa == "pa")
                {
                    listParticpantes = listaParticipantes.OrderBy(x => x.nome).ToList();
                }
                
            }
            else
            {
                listParticpantes = listaParticipantes.Where(x => x.nome.ToLower().Contains(textBox1.Text.ToLower())).ToList();
            } 


            LoadLista(listParticpantes);
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox1.SelectedItem == null) return;

            var lista = listaParticipantes.Where(x => x.cidade.estado.Sigla == comboBox1.SelectedItem.ToString()).ToList();

            LoadLista(lista);
        }
    }
}
