namespace ACS.Constantes
{
    static class Constantes
    {
        // =============================== Mensajes API ==================================================
        // Error - API Response 
        public const string ERROR_mensaje = "Error en el servicio";
        public const int ERROR_error = 1;

        public const string ERROR_ASIGNARmensaje = "No fue posible asignarse a esta liga.";
        public const int ERROR_ASGINARerror = 1;

        // Exito - GET API Response
        public const string OK_mensaje = "Información obtenida con éxito";
        public const int OK_error = 0;

        // Exito - POST API Response
        public const string OK_mensaje_POST = "Información almacenada con éxito";
        public const int OK_error_POST = 0;

        // Usuario sin Ligas
        public const string LIGAS_UsuarioSinLigas = "El usuario no pertenece a ninguna Liga";

        // Sin ligas existentes
        public const string LIGAS_No_Existen = "No se ha creado ninguna liga todavía.";

        // Liga no existente
        public const string LIGAS_No_Existente = "No se encontró la liga seleccionada";

        // Sin grupos existentes
        public const string GRUPOS_No_Existen = "No se ha creado ningun grupo todavía.";


        // Sin sedes existentes
        public const string SEDES_No_Existen = "No se ha creado ninguna sede todavía.";

        // Sin equipos existentes
        public const string EQUIPOS_No_Existen = "No se ha creado ningun equipo todavía.";

        // ============================== Procedimientos - BDD ==============================================

        // Usuarios
        public const string SP_Crear_Usuario = "SP_Crear_Usuario";
        public const string SP_Actualizar_Usuario = "SP_Actualizar_Usuario";
        public const string SP_AutenticarUsuario = "SP_AutenticarUsuario";
        public const string SP_Ligas_x_Usuario = "SP_Ligas_x_Usuario";
        
        // Liga
        public const string SP_Obtener_Ligas = "SP_Obtener_Ligas";
        public const string SP_Obtener_Liga = "SP_Obtener_Liga";
        public const string SP_Crear_Liga = "SP_Crear_Liga";
        public const string SP_Actualizar_Liga = "SP_Actualizar_Liga";
        public const string SP_Asignar_Liga = "SP_Asignar_Liga";
        public const string SP_Obtener_Detalle_Liga = "SP_Obtener_Detalle_Liga";
        public const string SP_Actualizar_Asignacion = "SP_Actualizar_Asignacion";

        // Grupo
        public const string SP_Obtener_Grupos = "SP_Obtener_Grupos";
        public const string SP_Crear_Grupo = "SP_Crear_Grupo";

        // Sede
        // Liga
        public const string SP_Obtener_Sedes = "SP_Obtener_Sedes";
        public const string SP_Obtener_Sede = "SP_Obtener_Sede";
        public const string SP_Obtener_Partidos_x_Sede = "SP_Obtener_Partidos_x_Sede";
        public const string SP_Obtener_Detalle_Equipo = "SP_Obtener_Detalle_Equipo";
        public const string SP_Crear_Sede = "SP_Crear_Sede";
        public const string SP_Actualizar_Sede = "SP_Actualizar_Sede";

        public const string SP_Obtener_Equipos = "SP_Obtener_Equipos";
        public const string SP_Crear_Equipo = "SP_Crear_Equipo";
        
    }

}