using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace ACS.ConexionBDD
{
    public class Conexion
    {
        private const string strConexion = @"Data Source=DESKTOP-M67E7UU\LEIVA;Initial Catalog=quiniela;Integrated Security=True";
        public const string log = @"‪D:\Documentos\log.txt";
        public SqlConnection abrirConexionBDD()
        {
            try
            {
                SqlConnection conexionBDD = new SqlConnection(strConexion);
                return conexionBDD;
            }
            catch (Exception ex)
            {
                File.AppendAllText(log, string.Concat("Error al abrir la cnexion BDD", ex.ToString()));
                return null;
            }

        }
    }
}
