namespace adaptatechwebapibackend.DTOs
    {
    public class DTOTemasMensajes
        {
        public int IdTemaDTO { get; set; }

        public string TituloDTO { get; set; }

        public int IdTemaUsuarioDTO { get; set; }

        public DateTime FechaCreacionDTO { get; set; }

        public List<DTOMensajesItem> MensajesDTO = new List<DTOMensajesItem>();


        }
    }
