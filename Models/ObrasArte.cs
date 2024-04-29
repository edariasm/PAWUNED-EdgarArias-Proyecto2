using System;
using System.Collections.Generic;

namespace PAWUNED_EdgarArias_Proyecto2.Models;

public partial class ObrasArte
{
    public int IdObra { get; set; }

    public int IdUsuario { get; set; }

    public string? TituloObra { get; set; }

    public string? DescripcionObra { get; set; }

    public string CategoriaObra { get; set; } = null!;

    public string? DimensionesObra { get; set; }

    public decimal PrecioInicial { get; set; }

    public DateTime? FechaInicioSubasta { get; set; }

    public int? DuracionSubasta { get; set; }

    public String? ImagenesObra { get; set; }

    public string? EstadoObra { get; set; }

    public virtual Usuario? IdUsuarioNavigation { get; set; } = null!;

    public virtual ICollection<Transaccione> Transacciones { get; set; } = new List<Transaccione>();
}
