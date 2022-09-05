using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using ACS.ConexionBDD;

namespace ACS.OperacionBDD
{
    public class Operacion
    {

        public SqlConnection ObjBDD;

        public Conexion ConexionBDD;
        public const string log = "D:/Documentos/log.txt";

        public DataTable getDataSp(string nombreSP, List<SqlParameter> parametros = null)
        {
            DataTable resultado = new DataTable();
            SqlCommand cmd = new SqlCommand();

            try
            {
                ConexionBDD = new Conexion();
                ObjBDD = this.ConexionBDD.abrirConexionBDD();
                ObjBDD.Open();

                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = nombreSP;
                if (parametros != null)
                {
                    cmd.Parameters.AddRange(parametros.ToArray());
                }
                cmd.Connection = ObjBDD;

                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.HasRows)
                {
                    resultado.Load(reader);
                }
                else
                {
                    this.ObjBDD.Close();
                    return null;
                }
                File.AppendAllText(log, "Consulta exitosa");
            }
            catch (Exception ex)
            {
                this.ObjBDD.Close();
                File.WriteAllText("D:/Documentos/prueba.txt", "hola");
                File.AppendAllText(log, string.Concat("Error al abrir la cnexion BDD", ex.ToString()));
                return null;
            }
            return resultado;
        }

        public int updateDataSp(string nombreSP, List<SqlParameter> parametros)
        {
            int filas = -1;

            try
            {
                SqlCommand cmd = new SqlCommand();
                ConexionBDD = new Conexion();
                ObjBDD = this.ConexionBDD.abrirConexionBDD();
                ObjBDD.Open();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = nombreSP;
                cmd.Parameters.AddRange(parametros.ToArray());
                cmd.Connection = ObjBDD;



                filas = cmd.ExecuteNonQuery();
                this.ObjBDD.Close();
                return filas;

            }
            catch (Exception ex)
            {
                this.ObjBDD.Close();
                File.AppendAllText(log, string.Concat("Error al abrir la cnexion BDD", ex.ToString()));
                return -1;
            }
        }

    }
}
