using System;
using System.Collections.Generic;

namespace SamiHarun_NET24_Databaser_Indivudellprojekt_Master1.Models;

public partial class Studenter
{
    public int Id { get; set; }

    public string? Namn { get; set; }

    public string? Personnummer { get; set; }

    public string? Klass { get; set; }

    public virtual ICollection<Betyg> Betygs { get; set; } = new List<Betyg>();
}
