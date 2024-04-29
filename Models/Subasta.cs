using System;
using System.Collections.Generic;

namespace PAWUNED_EdgarArias_Proyecto2.Models;

public partial class Subasta
{
    public int IdSubasta { get; set; }

    public int IdObra { get; set; }

    public decimal? PrecioActual { get; set; }

    public DateTime? FechaHoraCierre { get; set; }

    public int? IdUsuarioGanador { get; set; }

    public virtual Usuario? IdUsuarioGanadorNavigation { get; set; } = null!;

    public virtual ICollection<Oferta> Oferta { get; set; } = new List<Oferta>();
}
