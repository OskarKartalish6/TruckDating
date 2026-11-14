using System;

namespace TruckDating.Backend.Models.Entities;

public class Match
{
    public Guid Id { get; set; }

    public Guid UserOneId { get; set; }

    public Guid UserTwoId { get; set; }

    public DateTime CreatedAt { get; set; }

    public User UserOne { get; set; } = null!;

    public User UserTwo { get; set; } = null!;

    public Chat? Chat { get; set; }
}