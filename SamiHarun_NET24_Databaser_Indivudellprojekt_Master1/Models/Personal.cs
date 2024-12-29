using System;
using System.Collections.Generic;

namespace SamiHarun_NET24_Databaser_Indivudellprojekt_Master1.Models;

public partial class Personal
{
    public int Id { get; set; }

    public string? Namn { get; set; }

    public string? Befattning { get; set; }

    public DateOnly? Anstallningsdatum { get; set; }

    public string? Avdelning { get; set; }

    public decimal? Lon { get; set; }

    public virtual ICollection<Betyg> Betygs { get; set; } = new List<Betyg>();
}
