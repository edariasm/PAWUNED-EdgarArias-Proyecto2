using System;
using System.Collections.Generic;

namespace PAWUNED_EdgarArias_Proyecto2.Models;

public partial class Usuario
{
    public int IdUsuario { get; set; }

    public string NombreUsuario { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string TipoUsuario { get; set; } = null!;

    public byte[]? FotoUsuario { get; set; }

    public string? NombreArtista { get; set; }

    public string? BiografiaArtista { get; set; }

    public string? EnlacePaginaArtista { get; set; }

    public virtual ICollection<ObrasArte> ObrasArtes { get; set; } = new List<ObrasArte>();

    public virtual ICollection<Oferta> Oferta { get; set; } = new List<Oferta>();

    public virtual ICollection<Subasta> Subasta { get; set; } = new List<Subasta>();

    public virtual ICollection<Transaccione> Transacciones { get; set; } = new List<Transaccione>();
}
