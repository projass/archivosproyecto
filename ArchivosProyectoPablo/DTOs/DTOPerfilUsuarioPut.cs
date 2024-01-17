namespace adaptatechwebapibackend.DTOs
    {
    public class DTOPerfilUsuarioPut
        {

        public int IdPerfilDTO { get; set; }

        public string NombreDTO { get; set; }

        public string ApellidosDTO { get; set; }

        public string TelefonoDTO { get; set; }

        public DateTime FechaNacimientoDTO { get; set; }

        public byte[]? AvatarDTO { get; set; }

        public string? AliasDTO { get; set; }

        }
    }
