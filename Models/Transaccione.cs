using System;
using System.Collections.Generic;

namespace PAWUNED_EdgarArias_Proyecto2.Models;

public partial class Transaccione
{
    public int IdTransaccion { get; set; }

    public int IdUsuarioComprador { get; set; }

    public int IdUsuarioArtista { get; set; }

    public int IdObra { get; set; }

    public decimal MontoTransaccion { get; set; }

    public DateOnly? FechaTransaccion { get; set; }

    public virtual ObrasArte? IdObraNavigation { get; set; } = null!;

    public virtual Usuario? IdUsuarioCompradorNavigation { get; set; } = null!;
}
