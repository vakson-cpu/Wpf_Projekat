﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Projekat
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class BAZA3 : DbContext
    {
        public BAZA3()
            : base("name=BAZA3")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<Administrator> Administrators { get; set; }
        public virtual DbSet<NepotvrdjeniUcenici> NepotvrdjeniUcenicis { get; set; }
        public virtual DbSet<PomocnaSkola> PomocnaSkolas { get; set; }
        public virtual DbSet<Skola> Skolas { get; set; }
        public virtual DbSet<Statistika> Statistikas { get; set; }
        public virtual DbSet<Takmicenje> Takmicenjes { get; set; }
        public virtual DbSet<Tim> Tims { get; set; }
        public virtual DbSet<TrenutnoUlogovani> TrenutnoUlogovanis { get; set; }
        public virtual DbSet<Ucenik> Uceniks { get; set; }
        public virtual DbSet<Pobede> Pobedes { get; set; }
        public virtual DbSet<PobednikSkolskogTakmcienja> PobednikSkolskogTakmcienjas { get; set; }
        public virtual DbSet<RsSkolskog> RsSkolskogs { get; set; }
        public virtual DbSet<OkruznoTakmicenje> OkruznoTakmicenjes { get; set; }
        public virtual DbSet<OkruzniMec> OkruzniMecs { get; set; }
        public virtual DbSet<Pobede2> Pobede2 { get; set; }
        public virtual DbSet<PobednikOkruznogTakmicenja> PobednikOkruznogTakmicenjas { get; set; }
        public virtual DbSet<MVP> MVPs { get; set; }
        public virtual DbSet<Mec> Mecs { get; set; }
        public virtual DbSet<MVPSkolskogTakmicenja> MVPSkolskogTakmicenjas { get; set; }
        public virtual DbSet<MVP1> MVP1 { get; set; }
        public virtual DbSet<MVPOkruznogTakmicenja> MVPOkruznogTakmicenjas { get; set; }
        public virtual DbSet<Arhiva> Arhivas { get; set; }
    }
}