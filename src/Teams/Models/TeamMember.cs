﻿using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Teams.Models
{
    public class TeamMember
    {
        [Column("Id", TypeName = "int"), Key, Required, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Column("TeamId"), ForeignKey(nameof(Team)), Required] 
        public int TeamId { get; set; }
        public virtual Team Team { get; set; }
        [Column("MemberId"), ForeignKey("AspNetUsers"), Required]
        public virtual string MemberId { get; set; }
        public virtual IdentityUser Member { get; set; }
    }
}
