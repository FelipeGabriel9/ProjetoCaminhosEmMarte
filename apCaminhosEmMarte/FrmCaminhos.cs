using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace apCaminhosEmMarte
{
  public partial class FrmCaminhos : Form
  {
    public FrmCaminhos()
    {
      InitializeComponent();
    }

    IHashing<Cidade> tabelaDeCidades;

    private void Form1_Load(object sender, EventArgs e)
    {

    }

    private void btnAbrirArquivo_Click(object sender, EventArgs e)
    {
      if (dlgAbrir.ShowDialog() == DialogResult.OK)
      {
        // verificamos qual a técnica de Hash escolhida
        // pelo usuário e criamos uma tabela de hash de
        // acordo com essa escolha
        if (rbBucketHash.Checked) 
          tabelaDeCidades = new BucketHash<Cidade>();

        else if (rbSondagemLinear.Checked)
          tabelaDeCidades = new HashLinear<Cidade>();

        else if (rbSondagemQuadratica.Checked)
           tabelaDeCidades = new HashQuadratico<Cidade>();

        else
          tabelaDeCidades = new HashDuplo<Cidade>();

        // abrimos o arquivo escolhido
        var asCidades = new StreamReader(dlgAbrir.FileName);
        // ler registros do arquivo aberto
        while (!asCidades.EndOfStream)
        {
          // instanciar um objeto cidade
          // lê-lo do arquivo para preencher seus atributos
          // armazenar esse objeto na tabela de Hash
          // de acordo com a técnica de hash escolhida
          // pelo usuário
            Cidade umaCidade = new Cidade().LerRegistro(asCidades);
            if (umaCidade != null)
                tabelaDeCidades.Incluiu(umaCidade);

        }
        pbMapa.Invalidate();
        // Desenhar os nomes das cidades no mapa de Marte
        asCidades.Close();  // deixar arquivo fechado
      }
    }

    private void FrmCaminhos_FormClosing(object sender, FormClosingEventArgs e)
    {
      // aqui, a tabela de hash deve ser percorrida e os 
      // registros armazenados devem ser gravados no arquivo
      // agora, aberto para saída (StreamWriter).
    }

        private void pbMapa_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            if (tabelaDeCidades != null)

            {
                var listaCidades = tabelaDeCidades.Conteudo();
                foreach (var cidade in listaCidades)
                {
                    // Se as coordenadas no arquivo forem de 0 a 1:
                    int xPixel = (int)(cidade.X * pbMapa.Width);
                    int yPixel = (int)(cidade.Y * pbMapa.Height);

                    g.FillEllipse(Brushes.Red, xPixel - 3, yPixel - 3, 6, 6);

                    g.DrawString(cidade.Chave, this.Font, Brushes.Black, xPixel - 10, yPixel + 5);
                }
            }
        }

        private void btnInserir_Click(object sender, EventArgs e)
        {
            if (tabelaDeCidades == null)
            {
                MessageBox.Show("Abra o arquivo primeiro!");
                return;
            }
            else
            {
                string nome = txtNome.Text.Trim();
                double x = (double)udX.Value;
                double y = (double)udY.Value;

                if (string.IsNullOrEmpty(nome))
                {
                    MessageBox.Show("Digite um nome para a cidade!");
                }
                else
                {

                    var novaCidade = new Cidade(nome, x, y);

                    if (tabelaDeCidades.Incluiu(novaCidade))
                    {
                        pbMapa.Invalidate();
                        MessageBox.Show("Incluido com sucesso!");
                    }
                    else
                    {
                        pbMapa.Invalidate();
                        MessageBox.Show("Erro ao incluir!");
                    }
                }
            }
        }

        private void btnRemover_Click(object sender, EventArgs e)
        {
            if (tabelaDeCidades == null)
            {
                MessageBox.Show("Abra o arquivo primeiro!");
                return;
            }
            var cidadeAExcluir = new Cidade(txtNome.Text.Trim(), 0, 0);

            if (tabelaDeCidades.Excluiu(cidadeAExcluir))
            {
                pbMapa.Invalidate();
                MessageBox.Show("Cidade excluída com sucesso!");
            }
            else
            {
                MessageBox.Show("Cidade não encontrada!");
            }
        }

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            if (txtNome.Text == "")
            {
                MessageBox.Show("Preencha o nome da cidade!");
                txtNome.Focus();
                pbMapa.Invalidate();
            }

            var cidadeProcurada = new Cidade(txtNome.Text.Trim(), 0, 0);

            if (tabelaDeCidades.Existe(cidadeProcurada, out int onde))
            {
                var cidadeEncontrada = tabelaDeCidades.Conteudo().Find(c => c.Chave == cidadeProcurada.Chave);

                udX.Value = (decimal)cidadeEncontrada.X;
                udY.Value = (decimal)cidadeEncontrada.Y;

                pbMapa.Invalidate();

                MessageBox.Show("Busca realizada com sucesso! ");
            }
            else
            {
                pbMapa.Invalidate();
                MessageBox.Show("Erro ao buscar cidade!");
            }
        }
    }
}
