namespace PAWUNED_EdgarArias_Proyecto2.Models
{
    public class HistorialViewModel
    {

        public string TituloObra { get; set; }
        public int idobra { get; set; }
        public decimal Precio { get; set; }
        public DateTime FechaVentaCompra { get; set; }
        public List<OfertaViewModel> Movimientos { get; set; }

    }

    public class OfertaViewModel
    {
        public int IdUsuarioComprador { get; set; }
        public decimal MontoOferta { get; set; }
    }
}
