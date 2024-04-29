using System;
using System.Collections.Generic;

namespace PAWUNED_EdgarArias_Proyecto2.Models;

public partial class Oferta
{
    public int IdOferta { get; set; }

    public int IdSubasta { get; set; }

    public int IdUsuarioComprador { get; set; }

    public decimal MontoOferta { get; set; }

    public virtual Subasta? IdSubastaNavigation { get; set; } = null!;

    public virtual Usuario? IdUsuarioCompradorNavigation { get; set; } = null!;
}
