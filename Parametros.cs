using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;

namespace Port_COM_to_LAN
{
    internal class ConfiguracaoDosParametros
    {
        public List<DadosDosParametros> DadosParametros = new List<DadosDosParametros>();
        public ConfiguracaoDosParametros()
        {
            CarregaDados();
        }

        private void CarregaDados()
        {
            NpgsqlConnection lanConexão = new NpgsqlConnection(BancoPostGres.StringConexao);
            string SQL = "select id, nome, valor, tipo, descricao from lanparametros";
            NpgsqlCommand cmd = new NpgsqlCommand(SQL, lanConexão);
            NpgsqlDataAdapter adapter = new NpgsqlDataAdapter(cmd);

            DataTable dt = new DataTable();

            try
            {
                lanConexão.Open();

                adapter.Fill(dt);

                lanConexão.Dispose();

                if (dt.Rows.Count != 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        DadosParametros.Add(new DadosDosParametros(Convert.ToInt32(dr["id"]), dr["nome"].ToString(), dr["valor"].ToString(), dr["tipo"].ToString(), dr["descricao"].ToString()));
                    }
                }

            }
            catch (Exception Ex)
            {
                Log.Logger(Ex.ToString());
            }
        }

    }
    
    internal class DadosDosParametros
    {
        public int Id { get; set; }        
        public string Nome { get; set; }        
        public string Valor { get; set; }
        public string Tipo { get; set; }
        public string Descricao { get; set; }

        public DadosDosParametros(int Id, string Nome, string Valor, string Tipo, string Descricao)
        {
            this.Id = Id;
            this.Nome = Nome;
            this.Valor = Valor;
            this.Tipo = Tipo;
            this.Descricao = Descricao;
        }
    }
}
