namespace ACS.Constantes
{
    static class Constantes
    {
        // Error - API Response 
        public const string ERROR_mensaje = "Error en el servicio";
        public const int ERROR_error = 1;

        // Exito - GET API Response
        public const string OK_mensaje = "Información obtenida con éxito";
        public const int OK_error = 0;

        // Usuario sin Ligas
        public const string LIGAS_UsuarioSinLigas = "El usuario no pertenece a ninguna Liga";

        // Procedimientos - BDD
        public const string SP_AutenticarUsuario = "SP_AutenticarUsuario";
        public const string SP_Ligas_x_Usuario = "SP_Ligas_x_Usuario";
    }

}