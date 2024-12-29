using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace SamiHarun_NET24_Databaser_Indivudellprojekt_Master1.Models;

public partial class Betyg
{
    public int Id { get; set; }

    public int? KursId { get; set; }

    public int? StudentId { get; set; }

    public int? LarareId { get; set; }

    public DateTime? Datum { get; set; }

    public string? Betyg1 { get; set; }

    public virtual Kurser? Kurs { get; set; }

    public virtual Personal? Larare { get; set; }

    public virtual Studenter? Student { get; set; }

    [NotMapped]
    public object Personal { get; internal set; }
}
